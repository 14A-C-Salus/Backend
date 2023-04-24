namespace Salus.Models
{
    public class UserProfileGetResponse
    {
        public int id { get; set; }
        public double weight { get; set; }
        public double height { get; set; }
        public string birthDate { get; set; } = string.Empty;
        public genderEnum gender { get; set; }
        public double goalWeight { get; set; }


        //Profile picture data
        public hairEnum hairIndex { get; set; }
        public skinEnum skinIndex { get; set; }
        public eyesEnum eyesIndex { get; set; }
        public mouthEnum mouthIndex { get; set; }
    }
}
