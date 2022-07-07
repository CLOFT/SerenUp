namespace CLOFT.SerenUp.WebApp.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        

        [BindProperty]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            return Page();

        }
    }
}