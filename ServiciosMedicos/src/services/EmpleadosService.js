import axios from 'axios'
import api from './apiClient'

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
