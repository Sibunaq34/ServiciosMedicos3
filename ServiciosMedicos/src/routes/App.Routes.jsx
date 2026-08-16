import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import Login from '../pages/Login'

const POST_LOGIN_ROUTE = '/'
const LOGIN_ROUTE = '/login'

function RequireAuth({ children }) {
  return sessionStorage.getItem('token')
    ? children
    : <Navigate to={LOGIN_ROUTE} replace />
}

function Bienvenida() {
  const nombreCompleto = (() => {
    try {
      const usuario = JSON.parse(sessionStorage.getItem('user') ?? '{}')
      return usuario.nombreCompleto || usuario.usuario || ''
    } catch {
      return ''
    }
  })()

  return (
    <main className="container py-5">
      <h1>Bienvenido{nombreCompleto ? `, ${nombreCompleto}` : ''}</h1>
    </main>
  )
}

function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path={LOGIN_ROUTE} element={<Login />} />
        <Route
          path={POST_LOGIN_ROUTE}
          element={(
            <RequireAuth>
              <Bienvenida />
            </RequireAuth>
          )}
        />
        <Route path="*" element={<Navigate to={LOGIN_ROUTE} replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default AppRoutes
