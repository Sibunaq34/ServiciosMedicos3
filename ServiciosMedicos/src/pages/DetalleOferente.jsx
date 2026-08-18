import { useEffect, useState } from 'react'
import { useNavigate, useParams, useSearchParams } from 'react-router-dom'
import { api, esErrorAxios } from '../services/apiService'

function normalizarLista(valor) {
  if (!valor) return []
  if (Array.isArray(valor)) return valor
  if (typeof valor === 'string') return [valor]
  if (typeof valor === 'object') {
    if (Array.isArray(valor.items)) return valor.items
    const valores = Object.values(valor)
    return valores.filter((item) => item !== null && item !== undefined)
  }
  return []
}

function extraerRespuestaData(responseData) {
  if (!responseData || typeof responseData !== 'object') {
    return {}
  }

  if ('datos' in responseData && responseData.datos && typeof responseData.datos === 'object') {
    return responseData.datos
  }

  if ('data' in responseData && responseData.data && typeof responseData.data === 'object') {
    return responseData.data
  }

  if ('oferente' in responseData && responseData.oferente && typeof responseData.oferente === 'object') {
    return responseData.oferente
  }

  return responseData
}

function normalizarDetalleCrudo(rawDetalle) {
  const detalle = rawDetalle && typeof rawDetalle === 'object' ? rawDetalle : {}
  const puesto = detalle.puesto ?? detalle.Puesto ?? {}
  const curriculum = detalle.curriculum ?? detalle.Curriculum ?? {}

  const nombreCompleto = detalle.nombreCompleto ?? detalle.nombre_completo ?? detalle.NombreCompleto ?? ''
  const identificacion = detalle.identificacion ?? detalle.Identificacion ?? ''
  const tipoIdentificacion = detalle.tipoIdentificacion ?? detalle.tipo_identificacion ?? detalle.TipoIdentificacion ?? ''
  const fechaNacimiento = detalle.fechaNacimiento ?? detalle.fecha_nacimiento ?? detalle.FechaNacimiento ?? ''

  return {
    idOferente: detalle.idOferente ?? detalle.id_oferente ?? detalle.IdOferente ?? '',
    nombreCompleto,
    identificacion,
    tipoIdentificacion,
    fechaNacimiento,
    correos: normalizarLista(detalle.correos ?? detalle.Correos ?? detalle.correo ?? detalle.Correo),
    telefonos: normalizarLista(detalle.telefonos ?? detalle.Telefonos ?? detalle.telefono ?? detalle.Telefono),
    puesto: {
      codigoPuesto: puesto.codigoPuesto ?? puesto.codigo_puesto ?? puesto.CodigoPuesto ?? '',
      nombrePuesto: puesto.nombrePuesto ?? puesto.nombre_puesto ?? puesto.NombrePuesto ?? '',
    },
    curriculum: {
      nombreArchivo: curriculum.nombreArchivo ?? curriculum.nombre_archivo ?? curriculum.NombreArchivo ?? '',
      mime: curriculum.mime ?? curriculum.Mime ?? '',
      tamanioFormateado: curriculum.tamanioFormateado ?? curriculum.tamanio_formateado ?? curriculum.TamanioFormateado ?? '',
    },
  }
}

function extraerDetalleDesdeListado(payload, idBuscado) {
  if (!payload || typeof payload !== 'object') {
    return null
  }

  const candidatos = Array.isArray(payload)
    ? payload
    : Array.isArray(payload.data)
      ? payload.data
      : Array.isArray(payload.oferentes)
        ? payload.oferentes
        : []

  if (candidatos.length > 0) {
    const encontrado = candidatos.find((item) => {
      const rawId = item?.idOferente ?? item?.id ?? item?.codigoOferente ?? item?.id_oferente ?? item?.IdOferente ?? ''
      return String(rawId) === String(idBuscado)
    })

    if (encontrado) {
      return normalizarDetalleCrudo(encontrado)
    }

    const primerItem = candidatos[0]
    if (primerItem && typeof primerItem === 'object') {
      return normalizarDetalleCrudo(primerItem)
    }
  }

  const detalleBase = extraerRespuestaData(payload)
  const detalleNormalizado = normalizarDetalleCrudo(detalleBase)
  return detalleNormalizado && (detalleNormalizado.nombreCompleto || detalleNormalizado.identificacion || detalleNormalizado.idOferente)
    ? detalleNormalizado
    : null
}

export async function obtenerOferentesPorPuesto(codigoPuesto, idOferente = null) {
  const id = Number(idOferente ?? 0)

  if (!Number.isFinite(id) || id <= 0) {
    throw new Error('No se recibió un identificador de oferente válido.')
  }

  try {
    const response = await api.get(`/Oferentes/${encodeURIComponent(id)}/detalle`)

    const payload = response.data ?? {}
    
    // El endpoint devuelve { exito, mensaje, datos }
    if (payload.exito && payload.datos) {
      const datos = payload.datos
      
      // Normalizar el tamanio del curriculum si es necesario
      const curriculum = datos.curriculum ?? {}
      const tamanioFormateado = curriculum.tamanioFormateado ?? formatearTamanio(curriculum.tamanio ?? 0)
      
      const detalleNormalizado = {
        idOferente: datos.idOferente ?? '',
        nombreCompleto: datos.nombreCompleto ?? '',
        identificacion: datos.identificacion ?? '',
        tipoIdentificacion: datos.tipoIdentificacion ?? '',
        fechaNacimiento: datos.fechaNacimiento ?? '',
        correos: Array.isArray(datos.correos) ? datos.correos : [],
        telefonos: Array.isArray(datos.telefonos) ? datos.telefonos : [],
        puesto: {
          codigoPuesto: datos.puesto?.codigoPuesto ?? '',
          nombrePuesto: datos.puesto?.nombrePuesto ?? '',
        },
        curriculum: {
          nombreArchivo: curriculum.nombreArchivo ?? '',
          mime: curriculum.mime ?? '',
          tamanioFormateado: tamanioFormateado,
        },
      }

      return {
        ok: true,
        status: response.status,
        data: detalleNormalizado,
        mensaje: payload.mensaje ?? 'Oferente cargado correctamente.',
      }
    }

    throw new Error(payload.mensaje ?? 'No fue posible cargar el oferente.')
  } catch (error) {
    if (esErrorAxios(error)) {
      const data = error.response?.data ?? {}
      const mensaje =
        data?.mensaje ??
        data?.message ??
        data?.detail ??
        data?.title ??
        'No fue posible cargar los oferentes del puesto.'

      const errorServicio = new Error(mensaje)
      errorServicio.status = error.response?.status ?? 500
      throw errorServicio
    }

    const errorServicio = new Error('No fue posible cargar los oferentes del puesto.')
    errorServicio.status = 500
    throw errorServicio
  }
}

function formatearTamanio(bytes) {
  if (!bytes || bytes === 0) return '0 B'
  const k = 1024
  const sizes = ['B', 'KB', 'MB', 'GB']
  const i = Math.floor(Math.log(bytes) / Math.log(k))
  return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i]
}

export async function obtenerDetalleOferente(idOferente, codigoPuesto) {
  const id = Number(idOferente ?? 0)

  if (!Number.isFinite(id) || id <= 0) {
    throw new Error('No se recibió un identificador de oferente válido.')
  }

  const urls = []

  if (codigoPuesto) {
    urls.push(`/Oferentes/${encodeURIComponent(id)}/detalle`)
  }

  urls.push(...[
    `/Oferentes/${encodeURIComponent(id)}/detalle`,
  ])

  let ultimoError = null

  for (const url of urls) {
    try {
      const response = await api.get(url)

      const payload = response.data ?? {}
      const detalle = extraerDetalleDesdeListado(payload, id)

      if (detalle && (detalle.nombreCompleto || detalle.identificacion || detalle.idOferente)) {
        return {
          ok: true,
          status: response.status,
          data: detalle,
          mensaje: payload?.mensaje ?? payload?.message ?? 'Detalle del oferente obtenido correctamente.',
        }
      }
    } catch (error) {
      ultimoError = error
    }
  }

  if (esErrorAxios(ultimoError)) {
    const data = ultimoError.response?.data ?? {}
    const mensaje =
      data?.mensaje ??
      data?.message ??
      data?.detail ??
      data?.title ??
      'No fue posible cargar el detalle del oferente.'

    const errorServicio = new Error(mensaje)
    errorServicio.status = ultimoError.response?.status ?? 500
    throw errorServicio
  }

  const errorServicio = new Error('No fue posible cargar el detalle del oferente.')
  errorServicio.status = 500
  throw errorServicio
}

export async function registrarEmpleado(datos) {
  try {
    const payload = {
      idOferente: Number(datos?.idOferente ?? 0),
      codigoPuesto: String(datos?.codigoPuesto ?? '').trim(),
      idUsuario: Number(datos?.idUsuario ?? 0),
      idJefatura: datos?.idJefatura === '' || datos?.idJefatura == null ? null : Number(datos.idJefatura),
    }

    const response = await api.post('/api/Empleados', payload)

    return {
      ok: true,
      status: response.status,
      data: response.data ?? {},
      mensaje: response.data?.mensaje ?? 'Empleado registrado correctamente.',
    }
  } catch (error) {
    if (esErrorAxios(error)) {
      const data = error.response?.data ?? {}
      const mensaje =
        data?.mensaje ??
        data?.message ??
        data?.detail ??
        data?.title ??
        'No fue posible registrar el empleado.'

      const errorServicio = new Error(mensaje)
      errorServicio.status = error.response?.status ?? 500
      throw errorServicio
    }

    const errorServicio = new Error('No fue posible registrar el empleado.')
    errorServicio.status = 500
    throw errorServicio
  }
}

export default function DetalleOferente() {
  const { codigoOferente } = useParams()
  const navigate = useNavigate()
  const [searchParams] = useSearchParams()
  const [detalle, setDetalle] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [cargandoRegistro, setCargandoRegistro] = useState(false)
  const [mensajeExito, setMensajeExito] = useState('')

  const idOferente = searchParams.get('idOferente') ?? codigoOferente ?? ''
  const codigoPuesto = searchParams.get('codigoPuesto') ?? ''
  const volver = codigoPuesto ? `/puestos/${encodeURIComponent(codigoPuesto)}/oferentes` : '/puestos'

  useEffect(() => {
    let activo = true

    async function cargarDetalle() {
      setLoading(true)
      setError('')

      try {
        let response

        // Si hay codigoPuesto, usar el nuevo endpoint que devuelve datos normalizados
        if (codigoPuesto) {
          response = await obtenerOferentesPorPuesto(codigoPuesto, idOferente)
        } else {
          // Si no hay codigoPuesto, usar el endpoint genérico
          response = await obtenerDetalleOferente(idOferente)
        }

        if (!activo) {
          return
        }

        setDetalle(response?.data ?? null)
      } catch (requestError) {
        if (!activo) {
          return
        }

        setError(requestError?.message || 'No fue posible cargar el detalle del oferente.')
      } finally {
        if (activo) {
          setLoading(false)
        }
      }
    }

    if (idOferente) {
      cargarDetalle()
    } else {
      setError('No se recibió un identificador del oferente.')
      setLoading(false)
    }

    return () => {
      activo = false
    }
  }, [idOferente, codigoPuesto])

  const manejarCrearEmpleado = async () => {
    setCargandoRegistro(true)
    setError('')
    setMensajeExito('')

    try {
      // Obtener idUsuario de la sesión
      const userData = sessionStorage.getItem('user')
      const usuario = userData ? JSON.parse(userData) : null
      const idUsuario = Number(usuario?.idUsuario ?? 0)

      if (idUsuario <= 0) {
        throw new Error('No se encontró el ID del usuario autenticado. Por favor inicia sesión nuevamente.')
      }

      const resultado = await registrarEmpleado({
        idOferente: Number(detalle.idOferente),
        codigoPuesto: codigoPuesto || detalle.puesto?.codigoPuesto,
        idUsuario,
        idJefatura: null,
      })

      if (resultado.ok) {
        setMensajeExito(resultado.mensaje || 'Empleado registrado correctamente.')
        // Redirigir después de 2 segundos
        setTimeout(() => {
          navigate(volver)
        }, 2000)
      }
    } catch (registroError) {
      setError(registroError?.message || 'No fue posible registrar el empleado.')
    } finally {
      setCargandoRegistro(false)
    }
  }

  const datos = detalle ?? {}
  const puesto = datos.puesto ?? {}
  const curriculum = datos.curriculum ?? {}
  const correos = Array.isArray(datos.correos) ? datos.correos : []
  const telefonos = Array.isArray(datos.telefonos) ? datos.telefonos : []

  return (
    <section className="oferente-detalle">
      <div className="d-flex flex-column flex-md-row justify-content-between gap-3 align-items-md-center mb-4">
        <div>
          <p className="section-kicker mb-2">Contratación</p>
          <h1 className="h3 mb-1">Detalle de oferente</h1>
        </div>
        <button className="btn btn-outline-secondary" type="button" onClick={() => navigate(volver)}>
          Cancelar
        </button>
      </div>

      {error && (
        <div className="alert alert-danger" role="alert">{error}</div>
      )}

      {!error && !loading && !datos.nombreCompleto && !datos.identificacion && !datos.idOferente && (
        <div className="alert alert-warning">No se encontró el oferente solicitado.</div>
      )}

      {!error && !loading && (datos.nombreCompleto || datos.identificacion || datos.idOferente) && (
        <>
          <article className="card mb-4">
            <div className="card-body p-4">
              <h2 className="h4 mb-3">{datos.nombreCompleto || 'Oferente'}</h2>
              <dl className="row mb-0">
                <dt className="col-sm-4">Identificación</dt>
                <dd className="col-sm-8">{datos.identificacion || 'No disponible'}</dd>
                <dt className="col-sm-4">Tipo</dt>
                <dd className="col-sm-8">{datos.tipoIdentificacion || 'No disponible'}</dd>
                <dt className="col-sm-4">Fecha de nacimiento</dt>
                <dd className="col-sm-8">{datos.fechaNacimiento || 'No disponible'}</dd>
              </dl>
            </div>
          </article>

          <div className="row g-4 mb-4">
            <div className="col-lg-6">
              <article className="card h-100">
                <div className="card-header">Correos</div>
                <ul className="list-group list-group-flush">
                  {correos.length > 0 ? (
                    correos.map((correo, index) => (
                      <li key={`${correo}-${index}`} className="list-group-item">{correo}</li>
                    ))
                  ) : (
                    <li className="list-group-item text-secondary">Sin información registrada.</li>
                  )}
                </ul>
              </article>
            </div>
            <div className="col-lg-6">
              <article className="card h-100">
                <div className="card-header">Teléfonos</div>
                <ul className="list-group list-group-flush">
                  {telefonos.length > 0 ? (
                    telefonos.map((telefono, index) => (
                      <li key={`${telefono}-${index}`} className="list-group-item">{telefono}</li>
                    ))
                  ) : (
                    <li className="list-group-item text-secondary">Sin información registrada.</li>
                  )}
                </ul>
              </article>
            </div>
          </div>

          <div className="row g-4 mb-4">
            <div className="col-lg-6">
              <article className="card h-100">
                <div className="card-header">Puesto seleccionado</div>
                <div className="card-body">
                  {puesto && (puesto.codigoPuesto || puesto.nombrePuesto) ? (
                    <dl className="row mb-0">
                      <dt className="col-sm-4">Código</dt>
                      <dd className="col-sm-8">{puesto.codigoPuesto || 'No disponible'}</dd>
                      <dt className="col-sm-4">Nombre</dt>
                      <dd className="col-sm-8 mb-0">{puesto.nombrePuesto || 'No disponible'}</dd>
                    </dl>
                  ) : (
                    <p className="text-secondary mb-0">Sin puesto asociado.</p>
                  )}
                </div>
              </article>
            </div>
            <div className="col-lg-6">
              <article className="card h-100">
                <div className="card-header">Currículo</div>
                <div className="card-body">
                  {curriculum && (curriculum.nombreArchivo || curriculum.mime || curriculum.tamanioFormateado) ? (
                    <dl className="row mb-0">
                      <dt className="col-sm-4">Archivo</dt>
                      <dd className="col-sm-8">{curriculum.nombreArchivo || 'No disponible'}</dd>
                      <dt className="col-sm-4">Tipo</dt>
                      <dd className="col-sm-8">{curriculum.mime || 'No disponible'}</dd>
                      <dt className="col-sm-4">Tamaño</dt>
                      <dd className="col-sm-8 mb-0">{curriculum.tamanioFormateado || 'No disponible'}</dd>
                    </dl>
                  ) : (
                    <p className="text-secondary mb-0">Sin currículo registrado.</p>
                  )}
                </div>
              </article>
            </div>
          </div>
        </>
      )}

      {mensajeExito && (
        <div className="alert alert-success" role="status">{mensajeExito}</div>
      )}

      {!error && !loading && (datos.nombreCompleto || datos.identificacion || datos.idOferente) && codigoPuesto && (
        <div className="d-flex gap-2 mb-4">
          <button 
            className="btn btn-primary" 
            type="button" 
            onClick={manejarCrearEmpleado}
            disabled={cargandoRegistro}
          >
            {cargandoRegistro ? (
              <>
                <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                Creando empleado...
              </>
            ) : (
              'Crear empleado'
            )}
          </button>
          <button className="btn btn-outline-secondary" type="button" onClick={() => navigate(volver)}>
            Cancelar
          </button>
        </div>
      )}

      {loading && (
        <div className="card mt-3">
          <div className="card-body text-center py-5">
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">Cargando detalle...</span>
            </div>
            <p className="mt-3 mb-0 text-secondary">Cargando detalle del oferente...</p>
          </div>
        </div>
      )}

      <button type="button" className="btn btn-outline-primary core7-back-button mt-3" onClick={() => navigate(volver)}>
        <i className="bi bi-arrow-left me-2" aria-hidden="true"></i>
        Regresar
      </button>
    </section>
  )
}
