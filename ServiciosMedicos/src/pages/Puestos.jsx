import { useEffect, useState } from "react";

import { getPuestosByPage } from "../services/PuestosServices";

function Puestos() {
    const [puestos, setPuestos] = useState([]);
    const [paginaActual, setPaginaActual] = useState(1);
    const [totalPaginas, setTotalPaginas] = useState(1);
    const [totalRegistros, setTotalRegistros] = useState(0);
    const [cantidadMostrada, setCantidadMostrada] = useState(0);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const cargarPuestos = async () => {
            setIsLoading(true);
            setError("");

            try {
                const response = await getPuestosByPage(paginaActual);
                setPuestos(response.puestos || []);
                setTotalRegistros(response.totalRegistros || 0);
                setTotalPaginas(response.totalPaginas || 1);
                setCantidadMostrada(response.cantidadMostrada || 0);
            } catch (err) {
                setError(err.message || "No se pudieron cargar los puestos.");
                setPuestos([]);
            } finally {
                setIsLoading(false);
            }
        };

        cargarPuestos();
    }, [paginaActual]);

    const inicio = cantidadMostrada > 0 ? ((paginaActual - 1) * 10) + 1 : 0;
    const fin = cantidadMostrada > 0 ? inicio + cantidadMostrada - 1 : 0;

    const formatearMonto = (monto) => {
        if (monto === null || monto === "" || Number.isNaN(Number(monto))) {
            return "No disponible";
        }

        return Number(monto).toLocaleString("es-AR", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2,
        });
    };

    const renderContent = () => {
        if (isLoading) {
            return (
                <div className="card mt-4">
                    <div className="card-body text-center py-5">
                        <div className="spinner-border text-primary" role="status">
                            <span className="visually-hidden">Cargando puestos...</span>
                        </div>
                        <p className="mt-3 mb-0 text-secondary">Cargando puestos...</p>
                    </div>
                </div>
            );
        }

        if (error) {
            return (
                <div className="alert alert-warning core6-puestos-alert d-flex gap-3 align-items-start mt-4" role="alert">
                    <i className="bi bi-exclamation-triangle-fill" aria-hidden="true"></i>
                    <div>
                        <strong className="d-block mb-1">No se pudo cargar la información</strong>
                        {error}
                    </div>
                </div>
            );
        }

        if (puestos.length === 0) {
            return (
                <div className="card mt-4">
                    <div className="core6-puestos-empty empty-state">
                        <span className="empty-icon" aria-hidden="true"><i className="bi bi-briefcase"></i></span>
                        <h2 className="h5 mb-2">No hay puestos activos disponibles</h2>
                        <p className="text-secondary mb-0">En este momento no se encontraron puestos activos para mostrar.</p>
                    </div>
                </div>
            );
        }

        return (
            <div className="card mt-4 overflow-hidden">
                <div className="table-responsive core6-puestos-table-wrap">
                    <table className="table table-hover core6-puestos-table mb-0">
                        <caption className="visually-hidden">Listado de puestos activos</caption>
                        <thead>
                            <tr>
                                <th scope="col">Código</th>
                                <th scope="col">Puesto</th>
                                <th scope="col">Jefatura</th>
                                <th scope="col" className="text-end">Monto salarial</th>
                                <th scope="col">Estado</th>
                            </tr>
                        </thead>
                        <tbody>
                            {puestos.map((puesto) => (
                                <tr key={puesto.codigoPuesto || puesto.id || puesto.nombrePuesto}>
                                    <td data-label="Código"><span className="core6-puesto-code">{puesto.codigoPuesto || "-"}</span></td>
                                    <td data-label="Puesto">
                                        <a className="core6-puesto-link" href="#">
                                            <span>{puesto.nombrePuesto || "Sin nombre"}</span>
                                            <i className="bi bi-arrow-right" aria-hidden="true"></i>
                                        </a>
                                    </td>
                                    <td data-label="Jefatura">
                                        {puesto.jefatura ? puesto.jefatura : <span className="text-secondary">Sin jefatura asignada</span>}
                                    </td>
                                    <td data-label="Monto salarial" className="text-end core6-puesto-salary">{formatearMonto(puesto.montoSalario)}</td>
                                    <td data-label="Estado"><span className="core6-puesto-status"><span aria-hidden="true"></span>Activo</span></td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

                <div className="core6-puestos-list-footer">
                    <p className="mb-0 text-secondary">Mostrando {inicio}&ndash;{fin} de {totalRegistros} puestos activos</p>
                    {totalPaginas > 1 ? (
                        <nav className="core6-puestos-pagination" aria-label="Paginación de puestos">
                            <ul className="pagination mb-0">
                                <li className={`page-item ${paginaActual === 1 ? "disabled" : ""}`}>
                                    <button className="page-link" type="button" onClick={() => setPaginaActual((prev) => Math.max(1, prev - 1))} disabled={paginaActual === 1}>
                                        Anterior
                                    </button>
                                </li>
                                {Array.from({ length: totalPaginas }, (_, index) => index + 1).map((pagina) => (
                                    <li key={pagina} className={`page-item ${pagina === paginaActual ? "active" : ""}`}>
                                        <button className="page-link" type="button" onClick={() => setPaginaActual(pagina)}>
                                            {pagina}
                                        </button>
                                    </li>
                                ))}
                                <li className={`page-item ${paginaActual === totalPaginas ? "disabled" : ""}`}>
                                    <button className="page-link" type="button" onClick={() => setPaginaActual((prev) => Math.min(totalPaginas, prev + 1))} disabled={paginaActual === totalPaginas}>
                                        Siguiente
                                    </button>
                                </li>
                            </ul>
                        </nav>
                    ) : null}
                </div>
            </div>
        );
    };

    return (
        <section className="core6-puestos-page" aria-labelledby="core6-puestos-title">
            <header className="core6-puestos-header card">
                <div className="card-body p-4 p-lg-5 d-flex align-items-start gap-3 gap-md-4">
                    <span className="core6-puestos-header-icon" aria-hidden="true">
                        <i className="bi bi-briefcase-fill"></i>
                    </span>
                    <div>
                        <h1 id="core6-puestos-title" className="h3 mb-2">Puestos activos</h1>
                        <p className="text-secondary mb-0">Seleccione un puesto para consultar los oferentes que cumplen sus requisitos.</p>
                    </div>
                </div>
            </header>

            <div className="core6-puestos-summary mt-4" aria-label="Resumen del listado">
                <div className="core6-summary-item">
                    <span className="core6-summary-icon" aria-hidden="true"><i className="bi bi-briefcase"></i></span>
                    <div><strong>{totalRegistros}</strong><span>puestos activos</span></div>
                </div>
                <div className="core6-summary-item">
                    <span className="core6-summary-icon" aria-hidden="true"><i className="bi bi-files"></i></span>
                    <div><strong>{paginaActual} de {totalPaginas}</strong><span>página actual</span></div>
                </div>
                <div className="core6-summary-item">
                    <span className="core6-summary-icon" aria-hidden="true"><i className="bi bi-list-check"></i></span>
                    <div><strong>{cantidadMostrada}</strong><span>mostrados en esta página</span></div>
                </div>
            </div>

            {renderContent()}
        </section>
    );
}

export default Puestos;
