using Newtonsoft.Json;

namespace CLOFT.SerenUp.WebApp.Services
{
    public class BraceletsService : IBraceletsService
    {
        HttpClient client = new HttpClient();

        public async Task AssociateBracialetToUser(Bracelet bracelet)
        {
            string url = "https://hepj2fzca6.execute-api.eu-west-1.amazonaws.com/api/Bracelets";
            string request = JsonConvert.SerializeObject(bracelet);
            HttpContent content = new StringContent(request);
            await client.PostAsync(url, content);
        }

        public async Task<List<Bracelet>> GetBracelets()
        {
            string url = "https://hepj2fzca6.execute-api.eu-west-1.amazonaws.com/api/Bracelets";
            var responseBody = await client.GetStringAsync(url);
            List<Bracelet> bracelets = JsonConvert.DeserializeObject<List<Bracelet>>(responseBody);
            return bracelets.ToList();
        }
    }
}
