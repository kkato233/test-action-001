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

        ContactMailSendService _supportMail;

        public static IConfiguration Configuration { get; set; }
        public Function1(GitHubIssueService github, ContactMailSendService supportMail, IConfiguration configuration)
        {
            this._github = github;
            this._supportMail = supportMail;
            Configuration = configuration;
        }

        [FunctionName("Contact")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                ContactMessage data = JsonConvert.DeserializeObject<ContactMessage>(requestBody);

                if (data != null)
                {
                    string title = "WEBページから問い合わせ";

                    string[] lines = new string[]
                    {
                        $"名前: {data.Name}",
                        $"Email: {data.Email}",
                        $"住所: {data.Address}",
                        $"お問い合わせ内容:",
                        data.Message,
                    };

                    string body = string.Join("\r\n", lines);

                    if (_github.CheckEnv())
                    {
                        // GitHub に データ登録
                        await _github.AddIssue(title, body);

                        // 正常を返す
                        return new JsonResult(new { success = true });
                    } 
                    else if (_supportMail.CheckEnv())
                    {
                        // メール送信
                        await _supportMail.SendMailAsync(title, body);

                        // 正常を返す
                        return new JsonResult(new { success = true });
                    }
                    else
                    {
                        // 登録エラーを返す
                        return new JsonResult(new { success = false });
                    }
                }
                else
                {
                    // 登録エラーを返す
                    return new JsonResult(new { success = false });
                }
            }
            catch(Exception exp)
            {
                // 例外を記録する
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
