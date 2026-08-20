import { useNavigate } from "react-router-dom";

function Header() {
    const navigate = useNavigate();
    const nombreUsuario = (() => {
        try {
            const usuario = JSON.parse(sessionStorage.getItem("user") ?? "{}");
            return usuario.usuario || "Usuario";
        } catch {
            return "Usuario";
        }
    })();

    const cerrarSesion = () => {
        sessionStorage.removeItem("token");
        sessionStorage.removeItem("user");
        navigate("/login", { replace: true });
    };

    return (
        <nav className="navbar navbar-light app-topbar shadow-sm">
            <div className="container-fluid d-flex align-items-center justify-content-between">
                <button className="btn btn-outline-secondary d-lg-none sidebar-toggle" type="button" data-bs-toggle="offcanvas" data-bs-target="#sidebarOffcanvas" aria-controls="sidebarOffcanvas">
                    <i className="bi bi-list"></i>
                </button>
                <div className="d-flex align-items-center gap-3 ms-auto user-session-area">
                    <div className="user-avatar-pill">
                        <span className="user-avatar-badge" aria-hidden="true"><i className="bi bi-person-fill"></i></span>
                        <span className="text-truncate text-dark fw-semibold">{nombreUsuario}</span>
                    </div>
                    <button type="button" className="btn btn-link btn-sm text-decoration-none" onClick={cerrarSesion}>Cerrar sesión</button>
                </div>
            </div>
        </nav>
    );
}

export default Header;
