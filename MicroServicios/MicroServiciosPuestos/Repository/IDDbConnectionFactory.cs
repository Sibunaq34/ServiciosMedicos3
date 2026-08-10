using System.Data;

namespace MicroServicioPuestos.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}