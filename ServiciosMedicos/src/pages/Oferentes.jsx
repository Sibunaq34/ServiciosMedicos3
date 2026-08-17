import { Link, useNavigate, useParams, useSearchParams } from 'react-router-dom'
import { useEffect, useMemo, useState } from 'react'
import { obtenerOferentesPorPuesto } from '../services/oferenteService'

const PAGE_SIZE = 10

function normalizarPagina(valor) {
  const pagina = Number.parseInt(valor ?? '1', 10)
  return Number.isNaN(pagina) || pagina < 1 ? 1 : pagina
}

export default function Oferentes() {
  const { codigoPuesto } = useParams()
  const navigate = useNavigate()
  const [searchParams, setSearchParams] = useSearchParams()
  const page = useMemo(() => normalizarPagina(searchParams.get('page')), [searchParams])

  const [oferentes, setOferentes] = useState([])
  const [meta, setMeta] = useState({
    page: 1,
    pageSize: PAGE_SIZE,
    total: 0,
    totalPages: 0,
  })
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    let activo = true

    async function cargarOferentes() {
      if (!codigoPuesto) {
        setError('No se recibio el codigo del puesto.')
        setLoading(false)
        return
      }

      setLoading(true)
      setError('')

      try {
        const response = await obtenerOferentesPorPuesto(codigoPuesto, page, PAGE_SIZE)

        if (!activo) {
          return
        }

        setOferentes(response.data ?? [])
        setMeta(
          response.meta ?? {
            page,
            pageSize: PAGE_SIZE,
            total: 0,
            totalPages: 0,
          },
        )
      } catch (requestError) {
        if (!activo) {
          return
        }

        const message =
          requestError.response?.data?.error?.message ??
          'No fue posible consultar los oferentes del puesto.'
        setError(message)
        setOferentes([])
        setMeta({
          page,
          pageSize: PAGE_SIZE,
          total: 0,
          totalPages: 0,
        })
      } finally {
        if (activo) {
          setLoading(false)
        }
      }
    }

    cargarOferentes()

    return () => {
      activo = false
    }
  }, [codigoPuesto, page])

  function cambiarPagina(nuevaPagina) {
    setSearchParams({ page: String(nuevaPagina) })
  }

  return (
    <section className="core7-page" aria-labelledby="core7-title">
      <header className="core7-page-header card">
        <div className="card-body p-4 p-lg-5 d-flex align-items-start gap-3 gap-md-4">
          <span className="core7-page-header-icon" aria-hidden="true">
            <i className="bi bi-people-fill"></i>
          </span>
          <div>
            <p className="core7-page-kicker mb-1">Puesto {codigoPuesto}</p>
            <h1 id="core7-title" className="h3 mb-2">
              Oferentes registrados para el puesto
            </h1>
          </div>
        </div>
      </header>

      {error && (
        <div className="alert alert-warning core7-alert d-flex gap-3 align-items-start" role="alert">
          <i className="bi bi-exclamation-triangle-fill" aria-hidden="true"></i>
          <div>
            <strong className="d-block mb-1">No se pudo cargar la informacion</strong>
            {error}
          </div>
        </div>
      )}

      <div className="card overflow-hidden">
        {loading ? (
          <div className="card-body text-center py-5">
            <div className="spinner-border text-primary" role="status">
              <span className="visually-hidden">Cargando oferentes...</span>
            </div>
            <p className="mt-3 mb-0 text-secondary">Cargando oferentes...</p>
          </div>
        ) : !error && oferentes.length === 0 ? (
          <div className="core7-empty-state">
            <span className="core7-empty-icon" aria-hidden="true">
              <i className="bi bi-person-x"></i>
            </span>
            <h2 className="h5 mb-2">No hay oferentes registrados para este puesto</h2>
            <p className="text-secondary mb-0">
              Cuando AUT3 registre postulantes para este puesto, apareceran aqui.
            </p>
          </div>
        ) : (
          !error && (
            <>
              <div className="table-responsive core7-table-wrap">
                <table className="table table-hover core7-table mb-0">
                  <caption className="visually-hidden">Listado de oferentes por puesto</caption>
                  <thead>
                    <tr>
                      <th scope="col">Nombre completo</th>
                      <th scope="col">Identificacion</th>
                    </tr>
                  </thead>
                  <tbody>
                    {oferentes.map((oferente) => (
                      <tr key={oferente.idOferente}>
                        <td data-label="Nombre completo">
                          <Link
                            className="core7-oferente-link"
                            to={`/oferentes/${oferente.idOferente}?codigoPuesto=${encodeURIComponent(
                              codigoPuesto,
                            )}`}
                          >
                            {oferente.nombreCompleto}
                          </Link>
                        </td>
                        <td data-label="Identificacion">{oferente.identificacion}</td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>

              <footer className="core7-list-footer">
                <p className="mb-0 text-secondary">
                  Pagina {meta.page} de {meta.totalPages || 1}
                </p>
                <nav className="core7-pagination" aria-label="Paginacion de oferentes">
                  <ul className="pagination mb-0">
                    <li className={`page-item ${page <= 1 ? 'disabled' : ''}`}>
                      <button
                        className="page-link"
                        type="button"
                        onClick={() => cambiarPagina(page - 1)}
                        disabled={page <= 1}
                      >
                        Anterior
                      </button>
                    </li>
                    <li className="page-item active" aria-current="page">
                      <span className="page-link">{meta.page}</span>
                    </li>
                    <li
                      className={`page-item ${
                        meta.totalPages === 0 || page >= meta.totalPages ? 'disabled' : ''
                      }`}
                    >
                      <button
                        className="page-link"
                        type="button"
                        onClick={() => cambiarPagina(page + 1)}
                        disabled={meta.totalPages === 0 || page >= meta.totalPages}
                      >
                        Siguiente
                      </button>
                    </li>
                  </ul>
                </nav>
              </footer>
            </>
          )
        )}
      </div>

      <button type="button" className="btn btn-outline-primary core7-back-button" onClick={() => navigate('/puestos')}>
        <i className="bi bi-arrow-left me-2" aria-hidden="true"></i>
        Regresar
      </button>
    </section>
  )
}
