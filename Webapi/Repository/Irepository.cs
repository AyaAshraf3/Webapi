﻿
using Webapi.theModel;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;


//A repository is an abstraction layer between our application and data access layer which is in this case the APIcontext.cs
//This interface will represent the operations that will be performed on the database.

namespace Webapi.Repository
{
    public interface Irepository
    {
        //This will retrieve all Clients consume's entries
        Task<IEnumerable<webapiDTO>> Get();

        //This will retrieve a particular Client entry.
        Task<webapiDTO> Get(Guid Clordid);

        //This will create a client entry
        Task<webapiDTO> Create(webapiDTO ClientConsume);

        //This will update a client entry
        Task Update(webapiDTO ClientConsume);

        //This will delete a client entry
        Task Delete(Guid Clordid);


    }
}

