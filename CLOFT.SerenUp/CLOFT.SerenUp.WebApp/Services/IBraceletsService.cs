
namespace CLOFT.SerenUp.WebApp.Services
{
    public interface IBraceletsService
    {
        Task<IEnumerable<Bracelet>> GetUnlinkedBracelets();

        Task AssociateBracialetToUser(Bracelet bracelet, User username);

        Task<HttpResponseMessage> InsertUser(User user);
        Task<Bracelet> GetUserIdBracelet(string username);
    }
}