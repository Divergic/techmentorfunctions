using System;
using System.Text;
using HeyRed.MarkdownSharp;

namespace TechMentorFunctions.UpdatedProfile
{
    public interface IMessageBuilder
    {
        string Build(Profile profile, string websiteUri, string apiUri);
    }

    public class MessageBuilder : IMessageBuilder
    {
        public string Build(Profile profile, string websiteUri, string apiUri)
        {
            var builder = new StringBuilder();

            builder.AppendLine("<html><body>");
            builder.AppendLine(
                $"<p>A profile has been updated in <a href=\"{websiteUri}profiles/{profile.Id}\">Tech Mentors ({websiteUri})</a>.</p>");
            builder.AppendLine("<h3>Profile</h3>");
            builder.AppendLine($"<p>Status: {profile.Status}</p>");
            builder.AppendLine($"<p>Email: {profile.Email}</p>");
            builder.AppendLine("<h3>Personal</h3>");
            builder.AppendLine($"<p>Name: {profile.FirstName} {profile.LastName}</p>");

            if (profile.BirthYear.HasValue)
            {
                builder.AppendLine($"<p>Born: {profile.BirthYear}</p>");
            }

            if (string.IsNullOrWhiteSpace(profile.Gender) == false)
            {
                builder.AppendLine($"<p>Gender: {profile.Gender}</p>");
            }

            if (string.IsNullOrWhiteSpace(profile.TimeZone) == false)
            {
                builder.AppendLine($"<p>TimeZone: {profile.TimeZone}</p>");
            }

            if (profile.Languages.Count > 0)
            {
                var languages = string.Join(", ", profile.Languages);

                builder.AppendLine($"<p>Languages: {languages}</p>");
            }

            if (profile.PhotoId.HasValue)
            {
                builder.AppendLine($"<p><img src\"{apiUri}profiles/{profile.Id}/photos/{profile.PhotoId}\" /></p>");
            }

            if (string.IsNullOrWhiteSpace(profile.About) == false)
            {
                var markdown = new Markdown();

                var about = markdown.Transform(profile.About);

                builder.AppendLine("<p>About:</p>");
                builder.AppendLine(about);
            }

            builder.AppendLine("<h3>Technical</h3>");

            if (profile.YearStartedInTech.HasValue)
            {
                builder.AppendLine($"<p>Year started in tech: {profile.YearStartedInTech}</p>");
            }

            if (string.IsNullOrWhiteSpace(profile.Website) == false)
            {
                builder.AppendLine($"<p>Website: <a href=\"{profile.Website}\">{profile.Website}</a></p>");
            }

            if (string.IsNullOrWhiteSpace(profile.GitHubUsername) == false)
            {
                builder.AppendLine(
                    $"<p>GitHub: <a href=\"https://github.com/{profile.GitHubUsername}\">{profile.GitHubUsername}</a></p>");
            }

            if (string.IsNullOrWhiteSpace(profile.TwitterUsername) == false)
            {
                builder.AppendLine(
                    $"<p>Twitter: <a href=\"https://twitter.com/{profile.TwitterUsername}\">{profile.TwitterUsername}</a></p>");
            }

            if (profile.Skills.Count > 0)
            {
                builder.AppendLine("<h3>Skills</h3>");

                foreach (var skill in profile.Skills)
                {
                    builder.Append($"<p>Skill: {skill.Name}, {skill.Level}");

                    var yearRange = DisplayYearRange(skill);

                    if (string.IsNullOrWhiteSpace(yearRange))
                    {
                        builder.Append($", {yearRange}");
                    }

                    builder.AppendLine("</p>");
                }
            }

            builder.AppendLine("</body></html>");

            var html = builder.ToString();

            return html;
        }

        private static string DisplayYearRange(Skill skill)
        {
            if (skill == null)
            {
                return "";
            }

            if (!skill.YearStarted.HasValue)
            {
                if (skill.YearLastUsed.HasValue)
                {
                    return "up to " + skill.YearLastUsed;
                }

                // Both year values are not supplied
                return "";
            }

            var endYear = DateTime.Now.Year;

            if (skill.YearLastUsed.HasValue)
            {
                endYear = skill.YearLastUsed.Value;
            }

            var totalYears = endYear - skill.YearStarted.Value;

            if (totalYears < 1)
            {
                // Other than a bug where the start is after the end, this is likely to be that the years are the same
                // It doesn't make sense to say the user has no experience, so we will use a year as a minimum
                totalYears = 1;
            }

            var yearLabel = "years";

            if (totalYears == 1)
            {
                yearLabel = "year";
            }

            // We have a year started
            if (skill.YearLastUsed.HasValue)
            {
                // We have a value for both years
                return "over " + totalYears + " " + yearLabel + " until " + skill.YearLastUsed.Value;
            }

            return "over " + totalYears + " " + yearLabel;
        }
    }
}