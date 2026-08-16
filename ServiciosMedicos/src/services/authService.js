import axios from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5220'

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
})

export async function login(usuario, contrasena) {
  try {
    const response = await api.post('/api/autenticacion/login', {
      Usuario: usuario,
      Contrasena: contrasena,
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
