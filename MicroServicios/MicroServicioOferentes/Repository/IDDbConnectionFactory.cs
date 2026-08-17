using System.Data;

namespace MicroServicioOferentes.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
