using Dapper;
using MicroServicioPuestos.Entities;
using System.Data;

namespace MicroServicioPuestos.Repository
{
    public class PuestosRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public PuestosRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<IEnumerable<Puestos>> ListarPuestos(int pagina)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var puestos = await connection.QueryAsync<Puestos>(
                    "SP_ListarPuestos",
                    new
                    {
                        p_pagina = pagina
                    },
                    commandType: CommandType.StoredProcedure
                );

                return puestos;
            }
        }

    }
}
