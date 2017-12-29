using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TechMentorFunctions.UpdatedProfile
{
    public class UpdatedProfileRunner
    {
        [FunctionName("UpdatedProfile")]
        public static async Task Run([QueueTrigger("updatedprofile", Connection = "QueueConnectionString")]
            Profile profile, TraceWriter log)
        {
            if (profile == null)
            {
                log.Info("No updated profile found, ignoring queue message");

                return;
            }

            var websiteUri = Environment.GetEnvironmentVariable("WebsiteUri");
            var apiUri = Environment.GetEnvironmentVariable("ApiUri");
            var apiKey = Environment.GetEnvironmentVariable("MailGunApiKey");
            var toAddress = Environment.GetEnvironmentVariable("ToAddress");

            var builder = new MessageBuilder();

            var html = builder.Build(profile, websiteUri, apiUri);

            var fields = new Dictionary<string, string>
            {
                {"from", "Tech Mentors <noreply@mail.techmentors.info>"},
                {"to", toAddress},
                {"subject", "Updated Tech Mentors Profile"},
                {"html", html}
            };

            var content = new FormUrlEncodedContent(fields);

            log.Info($"Updating profile at {websiteUri}/profiles/{profile.Id}");

            var handler = new HttpClientHandler
                {Credentials = new NetworkCredential("api", apiKey)};

            var client = new HttpClient(handler);

            await client.PostAsync("https://api.mailgun.net/v3/mail.techmentors.info/messages", content)
                .ConfigureAwait(false);
        }
    }
}