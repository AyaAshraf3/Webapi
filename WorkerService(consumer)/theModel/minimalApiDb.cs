using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerService_consumer_.theModel
{
    public class minimalApiDb : DbContext
    {
        private readonly ILogger<minimalApiDb> _logger;


        //this object is configuration of the context.

        //This constructor takes a DbContextOptions object as a parameter, which contains configuration information (like the connection string)
        public minimalApiDb(DbContextOptions<minimalApiDb> options, ILogger<minimalApiDb> logger)
            : base(options)
        {
            _logger = logger;


            _logger.LogInformation("Database created or already exists.");
        }

        //DbSet<T> is a way of working with entities in the database. In this case, ClientConsume will map to a table in the database where each row corresponds to an ClientConsumeAPI object.
        public DbSet<submitContent> ClientConsume { get; set; }
    }
}


