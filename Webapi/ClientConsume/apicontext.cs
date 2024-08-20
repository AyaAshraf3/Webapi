using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore; //This namespace provides Entity Framework Core (EF Core) functionalities, which is an ORM (Object-Relational Mapper) for .NET. EF Core allows you to work with a database using .NET objects.

//setting up an Entity Framework Core (EF Core) DbContext in a .NET application to interact with DB

namespace Webapi.ClientConsume
{
    public class APIcontext : DbContext //inherits from DbContext, a core class in EF Core that manages database connections
    {
        //this object is configuration of the context.

        public APIcontext(DbContextOptions<APIcontext> options) //This constructor takes a DbContextOptions object as a parameter, which contains configuration information (like the connection string)

            : base(options)

        {

            
            //  This ensures that the database is created if it doesn't already exist.
            Database.EnsureCreated();
           

        }

        //DbSet<T> is a way of working with entities in the database. In this case, ClientConsume will map to a table in the database where each row corresponds to an ClientConsumeAPI object.
        public DbSet<ClientConsumeAPI> ClientConsume { get; set; } 
    }
}




