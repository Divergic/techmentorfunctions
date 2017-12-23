using FluentAssertions;
using TechMentorFunctions.NewCategory;
using Xunit;
using Xunit.Abstractions;

namespace TechMentorFunctions.UnitTests.NewCategory
{
    public class MessageParserTests
    {
        private readonly ITestOutputHelper _output;
        private readonly TestTraceWriter _writer;

        public MessageParserTests(ITestOutputHelper output)
        {
            _output = output;
            _writer = new TestTraceWriter(output);
        }

        [Theory]
        [InlineData(@"skill
Azure", "skill", "Azure")]
        [InlineData(@"Skill
Azure", "Skill", "Azure")]
        [InlineData(@"SKILL
Azure", "SKILL", "Azure")]
        [InlineData(@"language
English", "language", "English")]
        [InlineData(@"Language
English", "Language", "English")]
        [InlineData(@"LANGUAGE
English", "LANGUAGE", "English")]
        [InlineData(@"gender
Female", "gender", "Female")]
        [InlineData(@"Gender
Female", "Gender", "Female")]
        [InlineData(@"GENDER
Female", "GENDER", "Female")]
        public void ParseReturnsDataTest(string message, string group, string name)
        {
            var sut = new MessageParser();

            var actual = sut.Parse(message, _writer);

            actual.Should().NotBeNull();
            actual.Group.Should().Be(group);
            actual.Name.Should().Be(name);
        }

        [Theory]
        [InlineData(@"skill
")]
        [InlineData(@"skill
 ")]
        public void ParseReturnsNullWithEmptyNameTest(string message)
        {
            var sut = new MessageParser();

            var actual = sut.Parse(message, _writer);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ParseReturnsNullWithEmptyValueTest(string message)
        {
            var sut = new MessageParser();

            var actual = sut.Parse(message, _writer);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData("single")]
        [InlineData(@"first
second
third")]
        public void ParseReturnsNullWithIncorrectLineNumbersTest(string message)
        {
            var sut = new MessageParser();

            var actual = sut.Parse(message, _writer);

            actual.Should().BeNull();
        }

        [Theory]
        [InlineData(@"something
other")]
        [InlineData(@"someskill
here")]
        [InlineData(@"skillsome
here")]
        public void ParseReturnsNullWithUnsupportedGroupTest(string message)
        {
            var sut = new MessageParser();

            var actual = sut.Parse(message, _writer);

            actual.Should().BeNull();
        }
    }
}