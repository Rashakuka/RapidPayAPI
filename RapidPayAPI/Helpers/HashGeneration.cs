namespace RapidPayAPI.EncryptionLibrary
{
    public class HashGeneration
    {
        private readonly IConfiguration _configuration;

        public HashGeneration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateHash(string text)
        {
            int workfactor = Convert.ToInt32(_configuration["AppSettings:Workfactor"]);

            string salt = BCrypt.Net.BCrypt.GenerateSalt(workfactor);
            string hash = BCrypt.Net.BCrypt.HashPassword(text, salt);

            return hash;
        }
    }
}
