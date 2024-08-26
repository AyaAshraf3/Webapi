using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore; //This namespace provides Entity Framework Core (EF Core) functionalities, which is an ORM (Object-Relational Mapper) for .NET. EF Core allows you to work with a database using .NET objects.

using Microsoft.Extensions.Logging;

//setting up an Entity Framework Core (EF Core) DbContext in a .NET application to interact with DB

namespace Webapi.theModel
{

    public class APIcontext : DbContext //inherits from DbContext, a core class in EF Core that manages database connections
    {
        private readonly ILogger<APIcontext> _logger;


        //this object is configuration of the context.

         //This constructor takes a DbContextOptions object as a parameter, which contains configuration information (like the connection string)
        public APIcontext(DbContextOptions<APIcontext> options, ILogger<APIcontext> logger)
            : base(options)
        {
            _logger = logger;

            // Log that the database is being checked for creation
            _logger.LogInformation("Ensuring database is created...");

            // This ensures that the database is created if it doesn't already exist.
            Database.EnsureCreated();

            _logger.LogInformation("Database created or already exists.");
        }

        //DbSet<T> is a way of working with entities in the database. In this case, ClientConsume will map to a table in the database where each row corresponds to an ClientConsumeAPI object.
        public DbSet<webapiDTO> ClientConsume { get; set; } 
    }
}




