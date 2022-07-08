﻿namespace CLOFT.SerenUp.WebApp.Pages
{
    using Amazon.Extensions.CognitoAuthentication;
    using CLOFT.SerenUp.WebApp.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.ComponentModel.DataAnnotations;

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IBraceletsService _braceletService;
        
        public IndexModel(IBraceletsService braceletService)
        {
            _braceletService = braceletService;
        }

        [BindProperty]
        [DisplayFormat(DataFormatString = "{0: MM/dd/yyyy}")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [BindProperty]
        public Bracelet UserBracelet { get; set; }



        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            var email = User.Claims.ToList()[9];
            UserBracelet = await _braceletService.GetUserIdBracelet(email.Value.ToString());
            return Page();

        }
    }
}