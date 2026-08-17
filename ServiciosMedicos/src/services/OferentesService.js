import axios from 'axios'
import api from './apiClient'

const DETALLE_OFERENTE_PATH = import.meta.env.VITE_CORE8_DETALLE_OFERENTE_PATH

function obtenerMensajeError(error, mensajePredeterminado) {
  if (!axios.isAxiosError(error)) {
    return mensajePredeterminado
  }

  return error.response?.data?.mensaje
    ?? error.response?.data?.detail
    ?? error.message
    ?? mensajePredeterminado
}

/**
 * CORE8 debe configurar VITE_CORE8_DETALLE_OFERENTE_PATH con un patrón que
 * incluya :codigoOferente, por ejemplo: /ruta/:codigoOferente. Su respuesta
 * debe incluir idOferente e idJefatura numéricos para la contratación; los
 * demás campos se presentan sin transformarlos en la pantalla.
 */
export async function obtenerDetalleOferente(codigoOferente) {
  if (!DETALLE_OFERENTE_PATH?.includes(':codigoOferente')) {
    throw new Error('La ruta de detalle de oferente de CORE8 aún no está configurada.')
  }

  try {
    const ruta = DETALLE_OFERENTE_PATH.replace(
      ':codigoOferente',
      encodeURIComponent(codigoOferente),
    )
    const response = await api.get(ruta)
    return response?.data ?? {}
  } catch (error) {
    throw new Error(
      obtenerMensajeError(error, 'No se pudo cargar el detalle del oferente.'),
      { cause: error },
    )
  }
}
