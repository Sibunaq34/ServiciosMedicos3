import axios from 'axios'

const API_BASE_URL = typeof import.meta.env.VITE_API_BASE_URL === 'string' && import.meta.env.VITE_API_BASE_URL.trim()
  ? import.meta.env.VITE_API_BASE_URL.trim()
  : 'https://keen-hoover.138-59-135-33.plesk.page/Gateway'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  const token = sessionStorage.getItem('token')

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})

api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      sessionStorage.removeItem('token')
      sessionStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  },
)

function extraerRespuestaData(responseData) {
  if (!responseData || typeof responseData !== 'object') {
    return {}
  }

  if ('data' in responseData && responseData.data && typeof responseData.data === 'object') {
    return responseData.data
  }

  if ('oferente' in responseData && responseData.oferente && typeof responseData.oferente === 'object') {
    return responseData.oferente
  }

  return responseData
}

export async function obtenerDetalleOferente(idOferente) {
  const id = Number(idOferente ?? 0)

  if (!Number.isFinite(id) || id <= 0) {
    throw new Error('No se recibió un identificador de oferente válido.')
  }

  const urls = [
    `/api/Oferentes/${encodeURIComponent(id)}/detalle`,
    `/api/Oferentes/detalle?idOferente=${encodeURIComponent(id)}`,
    `/api/Oferentes/${encodeURIComponent(id)}`,
    `/api/OferenteDetalle?idOferente=${encodeURIComponent(id)}`,
  ]

  let ultimoError = null

  for (const url of urls) {
    try {
      const response = await api.get(url)
      const detalle = extraerRespuestaData(response.data)

      return {
        ok: true,
        status: response.status,
        data: detalle,
        mensaje: response.data?.mensaje ?? 'Detalle del oferente obtenido correctamente.',
      }
    } catch (error) {
      ultimoError = error
    }
  }

  if (axios.isAxiosError(ultimoError)) {
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
    if (axios.isAxiosError(error)) {
      const data = error.response?.data ?? {}
      const mensaje =
        data?.mensaje ??
        data?.message ??
        data?.detail ??
        data?.title ??
        'No fue posible cargar el oferente del puesto.'

      const errorServicio = new Error(mensaje)
      errorServicio.status = error.response?.status ?? 500
      throw errorServicio
    }

    const errorServicio = new Error('No fue posible cargar el oferente del puesto.')
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
    if (axios.isAxiosError(error)) {
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
