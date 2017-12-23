using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;
using Xunit.Abstractions;

namespace TechMentorFunctions.UnitTests
{
    public class TestTraceWriter : TraceWriter
    {
        private readonly ITestOutputHelper _output;

        public TestTraceWriter(ITestOutputHelper output)
            : base(TraceLevel.Verbose)
        {
            _output = output;
        }

        public TestTraceWriter(TraceLevel level)
            : base(level)
        {
        }

        public override void Trace(TraceEvent traceEvent)
        {
            if (traceEvent.Exception != null)
            {
                _output.WriteLine(traceEvent.Exception.ToString());
            }

            if (string.IsNullOrWhiteSpace(traceEvent.Message) == false)
            { 
                _output.WriteLine(traceEvent.Message);
            }
        }
    }
}
