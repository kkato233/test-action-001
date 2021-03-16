using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp1.Service;
using Microsoft.Extensions.Configuration;

namespace FunctionApp1
{
    public class Function1
    {
        GitHubIssueService _github;
        public static IConfiguration Configuration { get; set; }
        public Function1(GitHubIssueService github, IConfiguration configuration)
        {
            this._github = github;
            Configuration = configuration;
        }

        [FunctionName("Contact")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string email = req.Query["email"];
            string address = req.Query["address"];
            string message = req.Query["message"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                ContactMessage data = JsonConvert.DeserializeObject<ContactMessage>(requestBody);

                if (data != null)
                {
                    string title = "WEBÉyÅ[ÉWÇ©ÇÁñ‚Ç¢çáÇÌÇπ";

                    string[] lines = new string[]
                    {
                        $"ñºëO: {data.Name}",
                        $"Email: {data.Email}",
                        $"èZèä: {data.Address}",
                        $"Ç®ñ‚Ç¢çáÇÌÇπì‡óe:",
                        data.Message,
                    };

                    string body = string.Join("\r\n", lines);

                    // GitHub Ç… ÉfÅ[É^ìoò^
                    await _github.AddIssue(title, body);

                    // ìoò^ê≥èÌÇï‘Ç∑
                    return new JsonResult(new { success = true });
                }
                else
                {
                    // ìoò^ÉGÉâÅ[Çï‘Ç∑
                    return new JsonResult(new { success = false });
                }
            }
            catch(Exception exp)
            {
                // ó·äOÇãLò^Ç∑ÇÈ
                log.LogError(exp, exp.Message);
                throw;
            }
        }

        class ContactMessage
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Message { get; set; }
        }
    }
}
