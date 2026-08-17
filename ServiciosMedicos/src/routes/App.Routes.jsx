import { Navigate, Route, Routes, useLocation } from 'react-router-dom'
import Login from '../pages/Login'
import Inicio from '../pages/Index'
import Puestos from "../pages/Puestos";
import DetalleOferente from '../pages/DetalleOferente'

const POST_LOGIN_ROUTE = '/'
const LOGIN_ROUTE = '/login'

function isAuthenticated() {
  return Boolean(sessionStorage.getItem('token'))
}

function RequireAuth({ children }) {
  const location = useLocation()

  return isAuthenticated()
    ? children
    : <Navigate to={LOGIN_ROUTE} replace state={{ from: location }} />
}

function RedirectIfAuthenticated() {
  return isAuthenticated()
    ? <Navigate to={POST_LOGIN_ROUTE} replace />
    : <Login />
}

function AppRoutes() {
  return (
    <Routes>
      <Route path={LOGIN_ROUTE} element={<RedirectIfAuthenticated />} />
      <Route
        path={POST_LOGIN_ROUTE}
        element={<RequireAuth><Inicio /></RequireAuth>}
      />
      <Route
        path="/puestos"
        element={(
          <RequireAuth>
            <Puestos />
          </RequireAuth>
        )}
      />
      <Route
        path="/puestos/:codigoPuesto/oferentes/:codigoOferente"
        element={<RequireAuth><DetalleOferente /></RequireAuth>}
      />
      <Route path="*" element={<Navigate to={LOGIN_ROUTE} replace />} />
    </Routes>
  )
}

export default AppRoutes
