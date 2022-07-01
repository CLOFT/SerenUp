
namespace CLOFT.SerenUp.WebApp.Services
{
    public interface IBraceletsService
    {
        Task<List<Bracelet>> GetBracelets();

        Task AssociateBracialetToUser(Bracelet bracelet);
    }
}