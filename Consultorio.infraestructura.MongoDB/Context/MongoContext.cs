using MongoDB.Driver;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Consultorio.Dominio.Entidades;

namespace Consultorio.infraestructura.MongoDB.Context
{
    public class MongoContext
    {

        private readonly MongoClient mongoClient;
        public readonly IMongoDatabase database;
        public IConfigurationRoot Configuration { get; }

        public MongoContext()
        {
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            mongoClient = new MongoClient(Configuration["MongoDBDatabase:ConnectionString"]);
            database = mongoClient.GetDatabase(Configuration["MongoDBDatabase:DatabaseName"]);
        }

        //public IMongoCollection<Cliente> Cliente
        //{
        //    get
        //    {
        //        return database.GetCollection<Cliente>("Clientes");
        //    }
        //}
        //public IMongoCollection<User> User
        //{
        //    get
        //    {
        //        return database.GetCollection<User>("Users");
        //    }
        //}
        //public IMongoCollection<Doctor> Doctor
        //{
        //    get
        //    {
        //        return database.GetCollection<Doctor>("Doctores");
        //    }
        //}
        //public IMongoCollection<Consulta> Consulta
        //{
        //    get
        //    {
        //        return database.GetCollection<Consulta>("Consultas");
        //    }
        //}
    }
}
