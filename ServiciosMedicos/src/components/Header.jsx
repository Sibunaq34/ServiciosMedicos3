import { Link, useNavigate } from "react-router-dom";

function Header({ children }) {
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
        <div className="app-layout">
            <aside className="app-sidebar d-none d-lg-flex flex-column">
                <div className="sidebar-brand p-3">
                    <h5 className="mb-0">Servicios Médicos</h5>
                </div>
                <nav className="sidebar-nav nav nav-pills flex-column px-3 py-3">
                    <Link className="sidebar-link nav-link" to="/"> <i className="bi bi-house-door-fill me-2"></i>Inicio</Link>
                    <Link className="sidebar-link nav-link" to="/puestos"><i className="bi bi-clipboard-data me-2"></i>Puestos</Link>
                    <Link className="sidebar-link nav-link" to="/oferentes-puesto"><i className="bi bi-people me-2"></i>Oferentes por Puesto</Link>
                    <Link className="sidebar-link nav-link" to="/oferentes"><i className="bi bi-card-list me-2"></i>Listado de Oferentes</Link>
                </nav>
            </aside>

            <div className="offcanvas offcanvas-start app-sidebar offcanvas-sidebar" tabIndex="-1" id="sidebarOffcanvas" aria-labelledby="sidebarOffcanvasLabel">
                <div className="offcanvas-header">
                    <h5 className="offcanvas-title" id="sidebarOffcanvasLabel">Servicios Médicos</h5>
                    <button type="button" className="btn-close" data-bs-dismiss="offcanvas" aria-label="Cerrar"></button>
                </div>
                <div className="offcanvas-body p-0">
                    <nav className="sidebar-nav nav nav-pills flex-column px-3 py-3">
                        <Link className="sidebar-link nav-link" to="/"> <i className="bi bi-house-door-fill me-2"></i>Inicio</Link>
                        <Link className="sidebar-link nav-link" to="/puestos"><i className="bi bi-clipboard-data me-2"></i>Puestos</Link>
                        <Link className="sidebar-link nav-link" to="/oferentes-puesto"><i className="bi bi-people me-2"></i>Oferentes por Puesto</Link>
                        <Link className="sidebar-link nav-link" to="/oferentes"><i className="bi bi-card-list me-2"></i>Listado de Oferentes</Link>
                    </nav>
                </div>
            </div>

            <div className="app-content">
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

                <main className="app-main">
                    {children}
                </main>
            </div>
        </div>
    );
}

export default Header;
