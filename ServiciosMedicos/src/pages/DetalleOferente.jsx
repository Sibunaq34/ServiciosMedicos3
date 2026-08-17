import { useEffect, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import { crearEmpleado } from '../services/EmpleadosService'
import { obtenerDetalleOferente } from '../services/OferentesService'

const NOMBRES_CAMPOS = {
  idOferente: 'Identificador de oferente',
  idJefatura: 'Identificador de jefatura',
}

function obtenerValorId(datos, nombre) {
  const valor = datos?.[nombre]
  return Number.isInteger(Number(valor)) && Number(valor) > 0 ? Number(valor) : null
}

function formatearEtiqueta(nombre) {
  return NOMBRES_CAMPOS[nombre]
    ?? nombre.replace(/([A-Z])/g, ' $1').replace(/^./, (letra) => letra.toUpperCase())
}

function formatearValor(valor) {
  if (valor === null || valor === undefined || valor === '') {
    return 'No registrado'
  }

  if (typeof valor === 'boolean') {
    return valor ? 'Sí' : 'No'
  }

  if (typeof valor === 'object') {
    return JSON.stringify(valor)
  }

  return String(valor)
}

function DetalleOferente() {
  const { codigoPuesto, codigoOferente } = useParams()
  const navigate = useNavigate()
  const [oferente, setOferente] = useState(null)
  const [cargando, setCargando] = useState(true)
  const [creando, setCreando] = useState(false)
  const [errorCarga, setErrorCarga] = useState('')
  const [errorCreacion, setErrorCreacion] = useState('')
  const [mensajeExito, setMensajeExito] = useState('')

  useEffect(() => {
    let activo = true

    const cargarDetalle = async () => {
      setCargando(true)
      setErrorCarga('')

      try {
        const detalle = await obtenerDetalleOferente(codigoOferente)
        if (activo) {
          setOferente(detalle)
        }
      } catch (error) {
        if (activo) {
          setOferente(null)
          setErrorCarga(error.message || 'No se pudo cargar el detalle del oferente.')
        }
      } finally {
        if (activo) {
          setCargando(false)
        }
      }
    }

    cargarDetalle()

    return () => {
      activo = false
    }
  }, [codigoOferente])

  const volverAOferentes = () => {
    navigate(`/puestos/${encodeURIComponent(codigoPuesto)}/oferentes`)
  }

  const crear = async () => {
    if (!oferente || creando) {
      return
    }

    const idOferente = obtenerValorId(oferente, 'idOferente')
    const idJefatura = obtenerValorId(oferente, 'idJefatura')
    const idPuesto = Number(codigoPuesto)

    if (!idOferente || !idJefatura || !Number.isInteger(idPuesto) || idPuesto <= 0) {
      setErrorCreacion('CORE8 debe proporcionar idOferente e idJefatura válidos para crear el empleado.')
      return
    }

    const datosEmpleado = { idOferente, idPuesto, idJefatura }

    setCreando(true)
    setErrorCreacion('')
    setMensajeExito('')

    try {
      const empleado = await crearEmpleado(datosEmpleado)
      const numeroEmpleado = empleado.numeroEmpleado ?? empleado.NumeroEmpleado
      setMensajeExito(numeroEmpleado
        ? `Empleado creado correctamente. Número de empleado: ${numeroEmpleado}.`
        : 'Empleado creado correctamente.')
    } catch (error) {
      const mensajesPorEstado = {
        400: 'Los datos del oferente no cumplen las validaciones requeridas.',
        404: 'No se encontró el oferente, puesto o jefatura requeridos para crear el empleado.',
        409: 'Este oferente ya fue convertido en empleado.',
        500: 'Ocurrió un error al crear el empleado. Intente nuevamente.',
      }
      setErrorCreacion(mensajesPorEstado[error.status] ?? error.message ?? 'No se pudo crear el empleado.')
    } finally {
      setCreando(false)
    }
  }

  return (
    <section className="core9-detalle-page" aria-labelledby="core9-detalle-title">
      <header className="core9-detalle-header card">
        <div className="card-body p-4 p-lg-5 d-flex align-items-start gap-3 gap-md-4">
          <span className="core9-detalle-header-icon" aria-hidden="true"><i className="bi bi-person-vcard-fill" /></span>
          <div>
            <h1 id="core9-detalle-title" className="h3 mb-2">Detalle del oferente</h1>
            <p className="mb-0">Información registrada para el oferente seleccionado.</p>
          </div>
        </div>
      </header>

      {cargando && (
        <div className="card">
          <div className="card-body text-center py-5">
            <div className="spinner-border text-primary" role="status"><span className="visually-hidden">Cargando detalle del oferente...</span></div>
            <p className="mt-3 mb-0 text-secondary">Cargando detalle del oferente...</p>
          </div>
        </div>
      )}

      {!cargando && errorCarga && (
        <div className="alert alert-warning" role="alert">
          <strong className="d-block mb-1">No se pudo cargar la información</strong>
          {errorCarga}
        </div>
      )}

      {!cargando && oferente && (
        <>
          <div className="card overflow-hidden">
            <div className="card-header px-4 py-3">Información registrada</div>
            <dl className="core9-detalle-list mb-0">
              {Object.entries(oferente).map(([campo, valor]) => (
                <div className="core9-detalle-item" key={campo}>
                  <dt>{formatearEtiqueta(campo)}</dt>
                  <dd>{formatearValor(valor)}</dd>
                </div>
              ))}
            </dl>
          </div>

          {mensajeExito && <div className="alert alert-success mb-0" role="status">{mensajeExito}</div>}
          {errorCreacion && <div className="alert alert-danger mb-0" role="alert">{errorCreacion}</div>}
        </>
      )}

      <div className="core9-detalle-actions">
        <button className="btn btn-outline-primary" type="button" onClick={volverAOferentes} disabled={creando}>Cancelar</button>
        <button className="btn btn-primary" type="button" onClick={crear} disabled={!oferente || cargando || creando || Boolean(mensajeExito)}>
          {creando ? 'Creando empleado...' : 'Crear empleado'}
        </button>
      </div>
    </section>
  )
}

export default DetalleOferente
