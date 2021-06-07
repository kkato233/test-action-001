using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;


namespace FunctionApp1.Service
{
    public class ContactMailSendService
    {
        ILogger _log;
        public static IConfiguration Configuration { get; set; }

        public ContactMailSendService(
            IConfiguration configuration,
            ILogger<ContactMailSendService> log)
        {
            Configuration = configuration;
            this._log = log;
        }
        /// <summary>
        /// 環境変数チェック
        /// </summary>
        /// <returns></returns>
        public bool CheckEnv()
        {
            if (Configuration["SupportMail:SMTPServer"]?.Length > 0 &&
                Configuration["SupportMail:SMTPLoginUserName"]?.Length > 0 &&
                Configuration["SupportMail:SMTPLoginPassword"]?.Length > 0 &&
                Configuration["SupportMail:ToMailAddress"]?.Length > 0)
            {
                // 環境変数設定されている
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> SendMailAsync(string title, string body)
        {
            // サポートメール送信設定
            string SMTPServer = Configuration["SupportMail:SMTPServer"];
            string SMTPLoginUserName = Configuration["SupportMail:SMTPLoginUserName"];
            string SMTPLoginPassword = Configuration["SupportMail:SMTPLoginPassword"];
            string ToMailAddress = Configuration["SupportMail:ToMailAddress"];

            Trace.TraceInformation("メール送信");

            string subject = title;

            try
            {
                // 参考： --- Office 365の SMTP 送信設定 --
                // SMTP 設定
                // サーバー名：smtp.office365.com
                // ポート 587
                // 暗号化 TLS

                System.Net.Mail.SmtpClient cl = new System.Net.Mail.SmtpClient();
                cl.Credentials = new System.Net.NetworkCredential(SMTPLoginUserName, SMTPLoginPassword);
                cl.Host = SMTPServer;
                cl.Port = 587;
                cl.EnableSsl = true;

                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage()
                {
                    Subject = subject,
                    Body = body
                };
                mailMsg.To.Add(ToMailAddress);
                mailMsg.From = new MailAddress(SMTPLoginUserName);
                await cl.SendMailAsync(mailMsg);

                return true;
            }
            catch (SmtpException exp)
            {
                // SMTP 設定等のエラー？ TO アドレスがおかしい？
                Trace.TraceError(exp.ToString());

                return false;
            }
            catch (Exception exp)
            {
                Trace.TraceError(exp.ToString());

                throw;
            }
        }
    }
}
