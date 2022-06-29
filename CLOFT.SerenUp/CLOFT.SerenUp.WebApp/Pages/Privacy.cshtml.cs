namespace CLOFT.SerenUp.WebApp.Pages
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Identity/Account/Login");
            //}
            //return Page();
        }
    }
}