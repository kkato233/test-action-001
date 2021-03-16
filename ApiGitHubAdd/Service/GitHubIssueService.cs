using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionApp1.Service
{
    /// <summary>
    /// GitHub に Issue 登録するサービス
    /// </summary>
    public class GitHubIssueService
    {
        ILogger _log;
        public static IConfiguration Configuration { get; set; }

        public GitHubIssueService(
            IConfiguration configuration,
            ILogger<GitHubIssueService> log)
        {
            Configuration = configuration;
            this._log = log;
        }

        public async Task AddIssue(string title, string body)
        {
            var client = new GitHubClient(new ProductHeaderValue("WebContactFormApp"));
            string token = Configuration["GitHub_Token"];
            var tokenAuth = new Credentials(token);
            client.Credentials = tokenAuth;

            string repOwner = Configuration["GitHub_Owner"]; // "kkato233";
            string repName = Configuration["GitHub_RepName"];  // "test-action-001";

            var issue = new NewIssue(title);
            issue.Body = body;
            
            // 正常に登録できない場合は 例外が発生する。
            // Octokit.AuthorizationException: Bad credentials
            // Octokit.NotFoundException: Not Found
            // など
            var result = await client.Issue.Create(repOwner, repName, issue);

            Console.WriteLine(result.Id);
        }
    }
}
