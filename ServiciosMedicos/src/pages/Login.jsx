import { useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { login as loginService } from '../services/authService'
import '../components/Login.css'

const POST_LOGIN_ROUTE = '/'

function Login() {
  const [usuario, setUsuario] = useState('')
  const [contrasena, setContrasena] = useState('')
  const [mensaje, setMensaje] = useState('')
  const [cargando, setCargando] = useState(false)
  const navigate = useNavigate()
  const location = useLocation()

  const handleSubmit = async (event) => {
    event.preventDefault()
    if (cargando) {
      return
    }

    const usuarioTrim = usuario.trim()
    if (!usuarioTrim || usuarioTrim.length > 50 || !contrasena) {
      setMensaje('Usuario y/o contraseña incorrectos.')
      return
    }

    setMensaje('')
    setCargando(true)

    try {
      const result = await loginService(usuarioTrim, contrasena)

      if (!result.success) {
        if (result.status === 403) {
          setMensaje('El usuario no tiene acceso al sistema.')
        } else if (result.status === 500) {
          setMensaje('No fue posible procesar la solicitud. Intente nuevamente.')
        } else if (result.status === null) {
          setMensaje('No fue posible comunicarse con el servidor.')
        } else {
          setMensaje('Usuario y/o contraseña incorrectos.')
        }
        return
      }

      const { token, usuario: datosUsuario } = result
      if (!token) {
        setMensaje('No fue posible procesar la solicitud. Intente nuevamente.')
        return
      }

      sessionStorage.setItem('token', token)

      if (datosUsuario && typeof datosUsuario === 'object') {
        const safeUser = {
          idUsuario: datosUsuario.IdUsuario ?? datosUsuario.idUsuario ?? null,
          usuario: datosUsuario.Usuario ?? datosUsuario.usuario ?? '',
          nombreCompleto: datosUsuario.NombreCompleto ?? datosUsuario.nombreCompleto ?? '',
          idRol: datosUsuario.IdRol ?? datosUsuario.idRol ?? null,
          nombreRol: datosUsuario.NombreRol ?? datosUsuario.nombreRol ?? '',
        }

        sessionStorage.setItem('user', JSON.stringify(safeUser))
      }

      const destination = location.state?.from?.pathname || POST_LOGIN_ROUTE
      navigate(destination, { replace: true })
    } finally {
      setCargando(false)
    }
  }

  return (
    <main className="login-page">
      <div className="login-shell">
        <div className="login-card card shadow-sm">
          <div className="card-body p-4">
            <div className="login-heading text-center">
              <img
                className="login-symbol"
                src="/simbolo.png"
                alt="Símbolo de Servicios Médicos"
              />
              <h1 className="login-title">Servicios Médicos</h1>
              <p className="login-subtitle">Ingrese al sistema</p>
            </div>

            {mensaje && (
              <div className="alert alert-danger login-alert" role="alert" aria-live="polite">
                {mensaje}
              </div>
            )}

            <form onSubmit={handleSubmit} noValidate>
              <div className="mb-3">
                <label htmlFor="usuario" className="form-label">
                  Usuario
                </label>
                <input
                  id="usuario"
                  name="usuario"
                  type="text"
                  className="form-control"
                  value={usuario}
                  onChange={(event) => setUsuario(event.target.value)}
                  autoComplete="username"
                  maxLength={50}
                  disabled={cargando}
                />
              </div>

              <div className="mb-4">
                <label htmlFor="contrasena" className="form-label">
                  Contraseña
                </label>
                <input
                  id="contrasena"
                  name="contrasena"
                  type="password"
                  className="form-control"
                  value={contrasena}
                  onChange={(event) => setContrasena(event.target.value)}
                  autoComplete="current-password"
                  disabled={cargando}
                />
              </div>

              <button type="submit" className="btn login-submit w-100" disabled={cargando}>
                {cargando ? 'Ingresando...' : 'Aceptar'}
              </button>
            </form>
          </div>
        </div>

        <footer className="login-footer">
          © 2026 Servicios Médicos S.A. <span>Aviso de privacidad</span>
        </footer>
      </div>
    </main>
  )
}

export default Login
