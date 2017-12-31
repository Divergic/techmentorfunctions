using System;

namespace TechMentorFunctions.UpdatedProfile
{
    public class Profile : UpdatableProfile
    {
        public DateTimeOffset? BannedAt { get; set; }

        public Guid Id { get; set; }
    }
}