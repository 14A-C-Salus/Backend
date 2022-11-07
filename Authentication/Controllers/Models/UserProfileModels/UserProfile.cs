namespace Authentication.Controllers.Models.UserProfileModels
{
    public enum genderEnum
    {
        nondefined,
        male,
        female,
        other
    }
    public enum hairEnum
    {
        nondefined,
        blond,
        brown,
        ginger,
        black,
        white
    }
    public enum skinEnum
    {
        nondefined,
        darkest,
        dark,
        light,
        lightest
    }
    public enum eyesEnum
    {
        nondefined,
        blue,
        green,
        brown
    }
    public enum mouthEnum
    {
        nondefined,
        happy,
        neutral,
        sad
    }

    public class UserProfile
    {
        public int id { get; set; }
        public double weight { get; set; }
        public double height { get; set; }
        public string birthDate { get; set; } = default(DateTime).ToString("yyyy.MM.dd");
        public genderEnum gender { get; set; }
        public bool isAdmin { get; set; } = false;
        public double goalWeight { get; set; }

        //Profilepic data
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }

        //Connection with auth
        public virtual Auth auth { get; set; }
        public int authOfProfileId { get; set; }
    }
}
