using Consultorio.Dominio.Entidades;
using Consultorio.Dominio.Repositorios;
using Consultorio.infraestructura.MongoDB.Context;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Consultorio.infraestructura.MongoDB.Repositorios
{
    public class ClienteRepository : Repository<Cliente>, IClienteRepository
    {
        private readonly MongoContext _mongoContext;

        public ClienteRepository()
        {
            _mongoContext = new MongoContext();
        }

        public Task<List<Cliente>> GetClientesByAge(int age)
        {
            throw new NotImplementedException();
        }
    }
}
