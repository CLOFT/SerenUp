
using CLOFT.SerenUp.WebApp.Services.Models;

namespace CLOFT.SerenUp.WebApp.Services
{
    public interface IBraceletsService
    {
        Task<IEnumerable<Bracelet>> GetUnlinkedBracelets();

        Task<HttpResponseMessage> AssociateBracialetToUser(Bracelet bracelet, User username);

        Task<HttpResponseMessage> InsertUser(User user);
        Task<Bracelet> GetUserIdBracelet(string username);
        Task<HttpResponseMessage> InsertUserSecureContact(UserSecureContact userSC);
    }
}