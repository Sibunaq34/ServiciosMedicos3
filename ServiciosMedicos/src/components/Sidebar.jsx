import { Link } from "react-router-dom";

function Sidebar() {
    return (
        <>
            <aside className="app-sidebar d-none d-lg-flex flex-column">
                <div className="sidebar-brand p-3">
                    <h5 className="mb-0">Servicios Médicos</h5>
                </div>
                <nav className="sidebar-nav nav nav-pills flex-column px-3 py-3">
                    <Link className="sidebar-link nav-link" to="/"> <i className="bi bi-house-door-fill me-2"></i>Inicio</Link>
                    <Link className="sidebar-link nav-link" to="/puestos"><i className="bi bi-clipboard-data me-2"></i>Puestos</Link>
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
                    </nav>
                </div>
            </div>
        </>
    );
}

export default Sidebar;
