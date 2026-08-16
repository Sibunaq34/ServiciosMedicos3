import axios from "axios";

const getGatewayBaseUrl = () => {
    const baseUrl = import.meta.env.VITE_API_BASE_URL;

    if (!baseUrl) {
        throw new Error("No se configuró la URL del Gateway en VITE_API_BASE_URL.");
    }

    return baseUrl.replace(/\/$/, "");
};

export const getPuestosByPage = async (pagina = 1) => {
    const gatewayBaseUrl = getGatewayBaseUrl();
    const url = `${gatewayBaseUrl}/Puestos/ListaPuestos/${pagina}`;

    try {
        const response = await axios.get(url);
        const payload = response?.data ?? {};

        return {
            puestos: payload.puestos || [],
            totalRegistros: payload.totalRegistros || 0,
            totalPaginas: payload.totalPaginas || 1,
            cantidadMostrada: payload.cantidadMostrada || 0,
        };
    } catch (error) {
        const message = error?.response?.data?.message || error?.message || "No se pudieron cargar los puestos.";
        throw new Error(message);
    }
};
