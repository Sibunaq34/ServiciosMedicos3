using MicroServicioPuestos.Entities;
using MicroServicioPuestos.Repository;

namespace MicroServicioPuestos.Services
{
    public class PuestosService : IPuestos
    {
        private readonly PuestosRepository _puestosRepository;

        public PuestosService(PuestosRepository puestosRepository)
        {
            _puestosRepository = puestosRepository;
        }

        public async Task<IEnumerable<Puestos>> ListarPuestos(int pagina)
        {
            return await _puestosRepository.ListarPuestos(pagina);
        }


    }
}