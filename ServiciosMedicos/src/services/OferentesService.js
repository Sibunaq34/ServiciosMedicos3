import axios from 'axios'

const API_BASE_URL = typeof import.meta.env.VITE_API_BASE_URL === 'string' && import.meta.env.VITE_API_BASE_URL.trim()
  ? import.meta.env.VITE_API_BASE_URL.trim()
  : 'http://localhost:5220'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
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

const DETALLE_OFERENTE_PATH =
  typeof import.meta.env.VITE_CORE8_DETALLE_OFERENTE_PATH === 'string' &&
    import.meta.env.VITE_CORE8_DETALLE_OFERENTE_PATH.includes(':codigoOferente')
    ? import.meta.env.VITE_CORE8_DETALLE_OFERENTE_PATH
    : '/Oferentes/:codigoOferente'

const DETALLE_OFERENTE_URL = `${API_BASE_URL}${DETALLE_OFERENTE_PATH}`

export async function obtenerOferentesPorPuesto(codigoPuesto, page, pageSize) {
  try {
    const response = await api.get(
      `/Puestos/${encodeURIComponent(codigoPuesto)}/oferentes`,
      {
        params: {
          page,
          pageSize,
        },
      },
    )

    return response?.data ?? { data: [], meta: {} }
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const mensaje = error.response?.data?.mensaje
        ?? error.response?.data?.detail
        ?? error.response?.data?.message
        ?? error.message

      const errorServicio = new Error(mensaje || 'No se pudo consultar los oferentes del puesto.', { cause: error })
      errorServicio.status = error.response?.status ?? null
      throw errorServicio
    }

    throw new Error('No se pudo consultar los oferentes del puesto.', { cause: error })
  }
}

export async function obtenerDetalleOferente(codigoPuesto, codigoOferente) {
  try {
    const id = Number(codigoOferente ?? 0)
    if (!Number.isFinite(id) || id <= 0) {
      throw new Error('No se recibió un identificador de oferente válido.')
    }

    const rutas = [
      `/Puestos/${encodeURIComponent(codigoPuesto)}/oferentes/${encodeURIComponent(id)}`,
      `/Puestos/${encodeURIComponent(codigoPuesto)}/oferentes`,
    ]

    let ultimoError = null

    for (const ruta of rutas) {
      try {
        const response = await api.get(ruta, {
          params: ruta.endsWith('/oferentes') ? { idOferente: id, page: 1, pageSize: 100 } : {},
        })

        const payload = response?.data ?? {}
        const items = Array.isArray(payload) ? payload : payload.data ?? payload.oferentes ?? []
        const encontrado = Array.isArray(items)
          ? items.find((item) => String(item.idOferente ?? item.id ?? item.codigoOferente ?? item.id_oferente ?? '') === String(id))
          : payload

        if (encontrado) {
          return encontrado
        }
      } catch (error) {
        ultimoError = error
      }
    }

    if (ultimoError) {
      throw ultimoError
    }

    return {}
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const mensaje = error.response?.data?.mensaje
        ?? error.response?.data?.detail
        ?? error.response?.data?.message
        ?? error.message

      const errorServicio = new Error(mensaje || 'No se pudo cargar el detalle del oferente.', { cause: error })
      errorServicio.status = error.response?.status ?? null
      throw errorServicio
    }

    throw new Error('No se pudo cargar el detalle del oferente.', { cause: error })
  }
}
