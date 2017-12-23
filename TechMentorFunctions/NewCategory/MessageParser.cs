using System;
using System.Linq;
using Microsoft.Azure.WebJobs.Host;

namespace TechMentorFunctions.NewCategory
{
    public class MessageParser : IMessageParser
    {
        public Message Parse(string message, TraceWriter log)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                log.Warning("Queue message is empty, ignoring");

                return null;
            }

            var separators = new[] { Environment.NewLine, "\r", "\n" };
            var parts = message.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                log.Warning($"Queue message has the wrong number of lines {parts.Length}, ignoring - {message}");

                return null;
            }

            var group = parts[0].Trim();
            var validGroups = new[]
            {
                "GENDER",
                "SKILL",
                "LANGUAGE"
            };
            var checkGroup = group.ToUpperInvariant();

            if (validGroups.Contains(checkGroup) == false)
            {
                log.Warning($"Queue message does not indicate a correct category group, ignoring - {message}");

                return null;
            }

            var name = parts[1].Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                log.Warning($"Queue message does not indicate a valid category name, ignoring - {message}");
                
                return null;
            }

            var data = new Message
            {
                Group = group,
                Name = name
            };

            return data;
        }
    }
}
