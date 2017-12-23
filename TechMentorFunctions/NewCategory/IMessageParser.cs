using Microsoft.Azure.WebJobs.Host;

namespace TechMentorFunctions.NewCategory
{
    public interface IMessageParser
    {
        Message Parse(string message, TraceWriter log);
    }
}