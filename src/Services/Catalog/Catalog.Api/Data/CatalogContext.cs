using Catalog.Api.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IConfiguration _config;

        public CatalogContext(IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(_config["DatabaseSettings:ConnectionString"]);
            var database = client.GetDatabase(_config["DatabaseSettings:DatabaseName"]);

            Products = database.GetCollection<Product>(_config["DatabaseSettings:CollectionName"]);

            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; }
    }
}
