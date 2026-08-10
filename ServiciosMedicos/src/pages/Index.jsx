function Inicio() {

    const nombreCompleto = "Usuario";

    return (
        <div className="container mt-4">

            <div className="row justify-content-center">

                <div className="col-lg-8">

                    <div className="card shadow-sm border-0">

                        <div className="card-body p-4 p-lg-5">

                            <h1 className="h3 mb-3">
                                Bienvenido, {nombreCompleto}
                            </h1>

                            <p className="text-muted mb-0">
                            </p>

                            <div className="text-center mt-4">

                                <img
                                    src="/assets/images/simbolo.png"
                                    alt="Símbolo de Servicios Médicos"
                                    className="img-fluid"
                                    style={{
                                        maxWidth: "320px",
                                        height: "auto"
                                    }}
                                />

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </div>
    );
}

export default Inicio;

