using MicroServicioPuestos.Entities;

namespace MicroServicioPuestos.Services
{
    public interface IPuestos
    {
        Task<IEnumerable<Puestos>> ListarPuestos(int pagina);

    }
}
