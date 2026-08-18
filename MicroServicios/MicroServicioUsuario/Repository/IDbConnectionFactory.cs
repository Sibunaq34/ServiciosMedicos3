using System.Data.Common;

namespace MicroServicioUsuario.Repository;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}
