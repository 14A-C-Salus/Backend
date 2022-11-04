
using System.Diagnostics.CodeAnalysis;

namespace Authentication.Controllers.Models.UserProfileModels
{
    public enum genderEnum
    {
        nondefined,
        male,
        female,
        other
    }
    public class UserProfile
    {
        public int id { get; set; }
        [AllowNull]
        public double weight { get; set; }
        [AllowNull]
        public double height { get; set; }
        [AllowNull]
        public string birthDate { get; set; } = default(DateTime).ToString("yyyy.MM.dd");
        [AllowNull]
        public genderEnum gender { get; set; }
        public bool isAdmin { get; set; } = false;
        [AllowNull]
        public double goalWeight { get; set; }
        public virtual Auth auth { get; set; }
        public int authOfProfileId { get; set; }
    }
}
