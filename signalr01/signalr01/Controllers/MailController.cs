using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.ComponentModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace signalr01.Controllers
{
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        private bool mailSent = false;
        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                //Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                //Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                //Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }

        // GET api/values/5
        [HttpGet("{email}")]
        public async Task<string> Get(int email)
        {
            await SendMail();
            return "value";
        }

        //api/Mail/1234
        public async Task SendMail()
        { 
            SmtpClient mySmtpClient = new SmtpClient("smtp.gmail.com");

            // set smtp-client with basicAuthentication
            mySmtpClient.UseDefaultCredentials = false;
            System.Net.NetworkCredential basicAuthenticationInfo = new
               System.Net.NetworkCredential("lapadol@builk.com", "0814668757ll");
            mySmtpClient.Credentials = basicAuthenticationInfo;

            // add from,to mailaddresses
            MailAddress from = new MailAddress("lapadol@msn.com", "TestFromName");
            MailAddress to = new MailAddress("lapadol6109@gmail.com", "TestToName");
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            // add ReplyTo
            MailAddress replyto = new MailAddress("lapadoljp@gmail.com");
            myMail.ReplyToList.Add(replyto);

            // set subject and encoding
            myMail.Subject = "Test message";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            // set body-message and encoding
            myMail.Body = "<b>Test Mail</b><br>using <b>HTML</b>.";
            myMail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            myMail.IsBodyHtml = true;

            mySmtpClient.Send(myMail);
        }

    }
}
