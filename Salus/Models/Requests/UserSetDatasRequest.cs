namespace Salus.Models.Requests
{
    public class UserSetDatasRequest
    {
        public double weight { get; set; }
        public double height { get; set; }
        public DateTime birthDate { get; set; } = default(DateTime);
        public genderEnum gender { get; set; }
        public double goalWeight { get; set; }
    }
}
