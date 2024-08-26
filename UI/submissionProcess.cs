using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS
{
    public class submissionProcess
    {
        public async Task<string> submit(HttpClient client, SubmissionData sbmt)
        {
            client.BaseAddress = new Uri("https://localhost:7254/");

            // Use asynchronous methods for HTTP requests
            var response = await client.PostAsJsonAsync("api/Values", sbmt);
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}

