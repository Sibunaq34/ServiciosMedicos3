import axios from 'axios'
import api from './apiClient'

export async function login(usuario, contrasena) {
  try {
    const response = await api.post('/api/autenticacion/login', {
      usuario,
      contrasena,
    })

    const data = response?.data ?? {}
    return {
      success: true,
      status: response.status,
      token: data.token,
      usuario: data.usuario,
    }
  } catch (error) {
    if (axios.isAxiosError(error)) {
      if (error.response) {
        return {
          success: false,
          status: error.response.status,
          message: error.response.data?.mensaje ?? 'Error de autenticación.',
        }
      }

      return {
        success: false,
        status: null,
        message: 'No fue posible comunicarse con el servidor.',
      }
    }

    return {
      success: false,
      status: null,
      message: 'No fue posible procesar la solicitud. Intente nuevamente.',
    }
  }
}
