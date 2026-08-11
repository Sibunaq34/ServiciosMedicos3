import axios from "axios";

// Cambia esta URL cuando el Gateway se publique.
// En desarrollo, Vite redirige /gateway al backend configurado en vite.config.js.
const gatewayBaseUrl = "/gateway";

const normalizeGatewayResponse = (payload) => {
    const isSinglePuesto = payload && typeof payload === "object" && !Array.isArray(payload) && (
        Object.prototype.hasOwnProperty.call(payload, "idPuesto") ||
        Object.prototype.hasOwnProperty.call(payload, "codigoPuesto") ||
        Object.prototype.hasOwnProperty.call(payload, "nombrePuesto")
    );

    const puestos = Array.isArray(payload)
        ? payload
        : Array.isArray(payload?.puestos)
            ? payload.puestos
            : Array.isArray(payload?.data)
                ? payload.data
                : Array.isArray(payload?.items)
                    ? payload.items
                    : isSinglePuesto
                        ? [payload]
                        : [];

    return {
        puestos,
        totalRegistros: Number(payload?.totalRegistros ?? payload?.total ?? puestos.length ?? 0),
        totalPaginas: Number(payload?.totalPaginas ?? payload?.pages ?? 1),
        paginaActual: Number(payload?.paginaActual ?? payload?.page ?? 1),
        cantidadMostrada: Number(payload?.cantidadMostrada ?? puestos.length ?? 0),
    };
};

export const getPuestosByPage = async (page = 1) => {
    try {
        const response = await axios.get(`${gatewayBaseUrl}/Puestos/ListaPuestos/${page}`, {
            headers: { Accept: "application/json" },
        });

        return normalizeGatewayResponse(response.data);
    } catch (error) {
        if (error.response) {
            throw new Error(`El Gateway respondió con error ${error.response.status}.`);
        }

        if (error.request) {
            throw new Error("No se pudo conectar con el Gateway. Verifica que el servicio esté disponible.");
        }

        throw new Error(error.message || "No se pudo cargar la información.");
    }
};
