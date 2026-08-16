import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import Login from '../pages/Login'

const POST_LOGIN_ROUTE = '/'
const LOGIN_ROUTE = '/login'

function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path={LOGIN_ROUTE} element={<Login />} />
        <Route path={POST_LOGIN_ROUTE} element={<Login />} />
        <Route path="*" element={<Navigate to={LOGIN_ROUTE} replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default AppRoutes
export { POST_LOGIN_ROUTE }
