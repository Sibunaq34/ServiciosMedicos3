import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5220'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

export async function obtenerOferentesPorPuesto(codigoPuesto, page, pageSize) {
  const response = await api.get(
    `/api/v1/puestos/${encodeURIComponent(codigoPuesto)}/oferentes`,
    {
      params: {
        page,
        pageSize,
      },
    },
  )

  return response.data
}
