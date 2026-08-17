import { Navigate, Route, Routes } from 'react-router-dom'
import Login from '../pages/Login'
import Inicio from '../pages/Index'
import Oferentes from '../pages/Oferentes'
import Puestos from "../pages/Puestos";

const POST_LOGIN_ROUTE = '/'
const LOGIN_ROUTE = '/login'

function RequireAuth({ children }) {
  return sessionStorage.getItem('token')
    ? children
    : <Navigate to={LOGIN_ROUTE} replace />
}

function AppRoutes() {
  return (
    <Routes>
      <Route path={LOGIN_ROUTE} element={<Login />} />
      <Route
        path={POST_LOGIN_ROUTE}
        element={(
          <RequireAuth>
            <Inicio />
          </RequireAuth>
        )}
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
        path="/puestos/:codigoPuesto/oferentes"
        element={(
          <RequireAuth>
            <Oferentes />
          </RequireAuth>
        )}
      />
      <Route path="*" element={<Navigate to={LOGIN_ROUTE} replace />} />
    </Routes>
  )
}

export default AppRoutes
