using System.Data;

namespace MicroServiciosEmpleados.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}