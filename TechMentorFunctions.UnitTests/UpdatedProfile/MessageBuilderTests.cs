namespace TechMentorFunctions.UnitTests.UpdatedProfile
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using TechMentorFunctions.UpdatedProfile;
    using Xunit;
    using Xunit.Abstractions;

    public class MessageBuilderTests
    {
        private const string ApiUri = "https://testapi.techmentors.info/";
        private const string WebsiteUri = "https://test.techmentors.info/";

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
            {
                About = "**" + aboutContent + "**"
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain("<strong>" + aboutContent + "</strong>");
        }

        [Fact]
        public void BuildReturnsMessageWithBannedProfileIndicatorTest()
        {
            var expected = new Profile
            {
                BannedAt = DateTimeOffset.Now
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain("This profile was banned at " + expected.BannedAt.Value.ToString("D"));
        }

        [Fact]
        public void BuildReturnsMessageWithCurrentSkillStartedLastYearTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearStarted = DateTime.Now.Year - 1
            };
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level + " over 1 year");
        }

        [Fact]
        public void BuildReturnsMessageWithCurrentSkillStartedThisYearTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearStarted = DateTime.Now.Year
            };
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level + " over 1 year");
        }

        [Fact]
        public void BuildReturnsMessageWithCurrentSkillTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearStarted = 1999
            };
            var yearsUsed = DateTime.Now.Year - skill.YearStarted;
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level + " over " + yearsUsed + " years");
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

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(WebsiteUri + "profiles/" + expected.Id);
        }

        [Fact]
        public void BuildReturnsMessageWithNullSkillsAndLanguagesTest()
        {
            var expected = new Profile
            {
                Skills = null,
                Languages = null
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().NotContain("Skills");
            actual.Should().NotContain("Languages");
        }

        [Fact]
        public void BuildReturnsMessageWithPastSkillWithNoSpecifiedStartTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearLastUsed = 1999
            };
            var yearsUsed = DateTime.Now.Year - skill.YearStarted;
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level + " up to " + skill.YearLastUsed);
        }

        [Fact]
        public void BuildReturnsMessageWithPastSkillWithSpecifiedStartEndingNextYearTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearStarted = 1999,
                YearLastUsed = 2000
            };
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level + " over 1 year until " + skill.YearLastUsed);
        }

        [Fact]
        public void BuildReturnsMessageWithPastSkillWithSpecifiedStartEndingSameYearTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearStarted = 1999,
                YearLastUsed = 1999
            };
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level + " over 1 year until " + skill.YearLastUsed);
        }

        [Fact]
        public void BuildReturnsMessageWithPastSkillWithSpecifiedStartTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString(),
                YearStarted = 1994,
                YearLastUsed = 1999
            };
            var yearsUsed = skill.YearLastUsed - skill.YearStarted;
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(
                skill.Name + ", " + skill.Level + " over " + yearsUsed + " years until " + skill.YearLastUsed);
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

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

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

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(expected.Status.ToString());
            actual.Should().Contain(expected.Email);
        }

        [Fact]
        public void BuildReturnsMessageWithSkillsTest()
        {
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    new Skill
                    {
                        Level = SkillLevel.Beginner,
                        Name = Guid.NewGuid().ToString(),
                        YearLastUsed = 1999,
                        YearStarted = 1998
                    },
                    new Skill
                    {
                        Level = SkillLevel.Expert,
                        Name = Guid.NewGuid().ToString(),
                        YearLastUsed = 1999,
                        YearStarted = 1998
                    }
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(expected.Skills.First().Name);
            actual.Should().Contain(expected.Skills.Skip(1).First().Name);
        }

        [Fact]
        public void BuildReturnsMessageWithSkillWithNoYearsTest()
        {
            var skill = new Skill
            {
                Level = SkillLevel.Beginner,
                Name = Guid.NewGuid().ToString()
            };
            var expected = new Profile
            {
                Skills = new List<Skill>
                {
                    skill
                }
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(skill.Name + ", " + skill.Level);
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

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(expected.YearStartedInTech.ToString());
            actual.Should().Contain("<a href=\"" + expected.Website + "\">" + expected.Website + "</a>");
            actual.Should().Contain(
                "<a href=\"https://github.com/" + expected.GitHubUsername + "\">" + expected.GitHubUsername + "</a>");
            actual.Should().Contain(
                "<a href=\"https://twitter.com/" + expected.TwitterUsername + "\">" + expected.TwitterUsername +
                "</a>");
        }

        [Fact]
        public void BuildReturnsProfileImageTest()
        {
            var expected = new Profile
            {
                Id = Guid.NewGuid(),
                PhotoId = Guid.NewGuid(),
                PhotoHash = Guid.NewGuid().ToString()
            };

            var sut = new MessageBuilder();

            var actual = sut.Build(expected, WebsiteUri, ApiUri);

            _output.WriteLine(actual);

            actual.Should().Contain(
                "<img src\"" + ApiUri + "profiles/" + expected.Id + "/photos/" + expected.PhotoId + "?hash=" +
                expected.PhotoHash + "\" />");
        }
    }
}