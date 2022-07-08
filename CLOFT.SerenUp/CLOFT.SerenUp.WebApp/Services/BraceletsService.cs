using Newtonsoft.Json;
using System.Text;

namespace CLOFT.SerenUp.WebApp.Services
{
    public class BraceletsService : IBraceletsService
    {
        HttpClient client = new HttpClient();

        public async Task AssociateBracialetToUser(Bracelet bracelet, User user)
        {
            var res = await InsertUser(user);
            if (res.IsSuccessStatusCode)
            {
                var response = await client.PutAsJsonAsync("https://hepj2fzca6.execute-api.eu-west-1.amazonaws.com/api/Bracelets", bracelet);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Table user/bracialets not updated");
                }
            }
            else
            {
                Console.WriteLine("errore ");
            }

        }

        public async Task<IEnumerable<Bracelet>> GetUnlinkedBracelets()
        {
            string url = "https://hepj2fzca6.execute-api.eu-west-1.amazonaws.com/api/Bracelets";
            var responseBody = await client.GetStringAsync(url);
            List<Bracelet> bracelets = JsonConvert.DeserializeObject<List<Bracelet>>(responseBody);
            bracelets.ToList().FindAll(b => b.Username == null);
            return bracelets;
        }


        public async Task<HttpResponseMessage> InsertUser(User user)
        {

            var response = await client.PostAsJsonAsync("https://hepj2fzca6.execute-api.eu-west-1.amazonaws.com/api/Users", user);

            return response;
        }

        public async Task<Bracelet> GetUserIdBracelet(string username)
        {
            string url = $"https://hepj2fzca6.execute-api.eu-west-1.amazonaws.com/api/Bracelets/GetByUsername/{username}";
            var responseBody = await client.GetStringAsync(url);
            var idBracelet = JsonConvert.DeserializeObject<Bracelet>(responseBody);
            return idBracelet;
        }
    }
}
