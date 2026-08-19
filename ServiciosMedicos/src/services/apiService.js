import axios from 'axios'

const API_BASE_URL = typeof import.meta.env.VITE_API_BASE_URL === 'string' && import.meta.env.VITE_API_BASE_URL.trim()
  ? import.meta.env.VITE_API_BASE_URL.trim()
  : ''

export const api = axios.create({
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

export const esErrorAxios = axios.isAxiosError