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

export async function getPuestosByPage(pagina = 1) {
  try {
    const response = await api.get(`/Puestos/ListaPuestos/${pagina}`)
    const data = response?.data ?? {}

    if (Array.isArray(data)) {
      return {
        puestos: data,
        totalRegistros: data.length,
        totalPaginas: 1,
        cantidadMostrada: data.length,
      }
    }

    const puestos = Array.isArray(data.puestos)
      ? data.puestos
      : Array.isArray(data.resultado)
        ? data.resultado
        : Array.isArray(data.items)
          ? data.items
          : []

    return {
      puestos,
      totalRegistros: Number(data.totalRegistros ?? data.total ?? puestos.length ?? 0),
      totalPaginas: Number(data.totalPaginas ?? data.totalPages ?? 1),
      cantidadMostrada: Number(data.cantidadMostrada ?? data.count ?? puestos.length ?? 0),
    }
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const mensaje = error.response?.data?.mensaje ?? error.message
      throw new Error(mensaje || 'No se pudieron cargar los puestos.', { cause: error })
    }

    throw new Error('No se pudieron cargar los puestos.', { cause: error })
  }
}
