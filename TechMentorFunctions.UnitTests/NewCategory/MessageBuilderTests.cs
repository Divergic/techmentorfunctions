using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TechMentorFunctions.UpdatedProfile;
using Xunit;
using Xunit.Abstractions;

namespace TechMentorFunctions.UnitTests.NewCategory
{
    public class MessageBuilderTests
    {
        private const string _apiUri = "https://testapi.techmentors.info/";
        private const string _websiteUri = "https://test.techmentors.info/";

        private readonly ITestOutputHelper _output;

        public MessageBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BuildReturnsAboutCompiledFromMarkdownTest()
        {
            var aboutContent = Guid.NewGuid();
            var expected = new Profile
                {About = "**" + aboutContent + "**"};

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, _websiteUri, _apiUri);

            _output.WriteLine(actual);

            actual.Should().Contain("<strong>" + aboutContent + "</strong>");
        }

        [Fact]
        public void BuildReturnsMessageWithLinkToProfileTest()
        {
            var expected = new Profile
            {
                Id = Guid.NewGuid(),
                Status = ProfileStatus.Available,
                Email = Guid.NewGuid().ToString()
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, _websiteUri, _apiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(_websiteUri + "profiles/" + expected.Id);
        }

        [Fact]
        public void BuildReturnsMessageWithPersonalInformationTest()
        {
            var expected = new Profile
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                BirthYear = 1900,
                Gender = "Female",
                TimeZone = "Australia/Sydney",
                Languages = new List<string>
                {
                    "English",
                    "Spanish"
                },
                About = Guid.NewGuid().ToString()
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, _websiteUri, _apiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(expected.FirstName + " " + expected.LastName);
            actual.Should().Contain(expected.BirthYear.ToString());
            actual.Should().Contain(expected.Gender);
            actual.Should().Contain(expected.TimeZone);
            actual.Should().Contain(expected.Languages.First() + ", " + expected.Languages.Skip(1).First());
            actual.Should().Contain(expected.About);
        }

        [Fact]
        public void BuildReturnsMessageWithProfileInformationTest()
        {
            var expected = new Profile
            {
                Id = Guid.NewGuid(),
                Status = ProfileStatus.Available,
                Email = Guid.NewGuid().ToString()
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, _websiteUri, _apiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(expected.Status.ToString());
            actual.Should().Contain(expected.Email);
        }

        [Fact]
        public void BuildReturnsMessageWithTechnicalInformationTest()
        {
            var expected = new Profile
            {
                YearStartedInTech = 1900,
                Website = "https://www.test.com",
                GitHubUsername = Guid.NewGuid().ToString(),
                TwitterUsername = Guid.NewGuid().ToString()
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, _websiteUri, _apiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(expected.YearStartedInTech.ToString());
            actual.Should().Contain("<a href=\"" + expected.Website + "\">" + expected.Website + "</a>");
            actual.Should().Contain("<a href=\"https://github.com/" + expected.GitHubUsername + "\">" +
                                    expected.GitHubUsername + "</a>");
            actual.Should().Contain("<a href=\"https://twitter.com/" + expected.TwitterUsername + "\">" +
                                    expected.TwitterUsername + "</a>");
        }

        [Fact]
        public void BuildReturnsProfileImageTest()
        {
            var expected = new Profile
            {
                Id = Guid.NewGuid(),
                PhotoId = Guid.NewGuid()
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, _websiteUri, _apiUri);

            _output.WriteLine(actual);

            actual.Should().Contain("<img src\"" + _apiUri + "profiles/" + expected.Id + "/photos/" + expected.PhotoId +
                                    "\" />");
        }
    }
}