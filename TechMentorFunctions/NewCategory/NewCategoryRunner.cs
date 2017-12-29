using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TechMentorFunctions.NewCategory
{
    public static class NewCategoryRunner
    {
        [FunctionName("NewCategory")]
        public static async Task Run([QueueTrigger("newcategories", Connection = "QueueConnectionString")]string message, TraceWriter log)
        {
            var parser = new MessageParser();

            var data = parser.Parse(message, log);

            if (data == null)
            {
                return;
            }

            var websiteUri = Environment.GetEnvironmentVariable("WebsiteUri");
            var apiKey = Environment.GetEnvironmentVariable("MailGunApiKey");
            var toAddress = Environment.GetEnvironmentVariable("ToAddress");

            var encodedName = HttpUtility.UrlEncode(data.Name);

            var html = $"<html><body><p>A new category has been added to <a href=\"{websiteUri}\">Tech Mentors ({websiteUri})</a>.</p><p><a href=\"{websiteUri}categories/approve?group={data.Group}&name={encodedName}\">Approve {data.Group} {data.Name}</a></p></body></html>";
            var fields = new Dictionary<string, string>
            {
                {"from", "Tech Mentors <noreply@mail.techmentors.info>"},
                {"to", toAddress},
                {"subject", "New Tech Mentors Category"},
                {"html", html}
            };

            var content = new FormUrlEncodedContent(fields);

            log.Info($"Approving {data.Group} {data.Name} using {websiteUri}");

            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential("api", apiKey)
            };

            var client = new HttpClient(handler);

            await client.PostAsync("https://api.mailgun.net/v3/mail.techmentors.info/messages", content).ConfigureAwait(false);
        }
    }
}
