namespace SAS.DTO
{
    public class AuthenticationResultDTO
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email{ get; set; }

        public int Age { get; set; }

    }
}
