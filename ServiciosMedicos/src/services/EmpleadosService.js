import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5220'

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

export async function crearEmpleado(datosEmpleado) {
  try {
    const response = await api.post('/Empleados/', datosEmpleado)
    return response?.data ?? {}
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const mensaje = error.response?.data?.mensaje
        ?? error.response?.data?.detail
        ?? error.message

      const errorServicio = new Error(mensaje || 'No se pudo crear el empleado.', { cause: error })
      errorServicio.status = error.response?.status ?? null
      throw errorServicio
    }

    throw new Error('No se pudo crear el empleado.', { cause: error })
  }
}
