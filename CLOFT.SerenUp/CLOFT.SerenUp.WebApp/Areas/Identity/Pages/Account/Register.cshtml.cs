// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using CLOFT.SerenUp.WebApp.Services;
using CLOFT.SerenUp.WebApp.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CLOFT.SerenUp.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly CognitoUserPool _pool;
        private readonly IBraceletsService _braceletService;

        public RegisterModel(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            ILogger<RegisterModel> logger,
            CognitoUserPool pool,
            IBraceletsService braceletsService)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _logger = logger;
            _pool = pool;
            _braceletService = braceletsService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public List<string> usernames { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Display(Name = "Phone number")]
            [Phone]
            public string PhoneNumber { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Birth date")]
            [DataType(DataType.Date)]
            public DateTime BirthDate { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public Guid BraceletId { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Emergency Email")]
            public string EmergencyEmail1 { get; set; }

            public string Role { get; set; } = "user";
        }

        public IEnumerable<Bracelet> bracelets  { get; set; }

        public async Task OnGet()
        {
            bracelets = await _braceletService.GetUnlinkedBracelets();
            //var users = await _userManager.GetUsersAsync();
            //usernames = users.Select(x => x.Username).ToList();
            //ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            bracelets = await _braceletService.GetUnlinkedBracelets();


            
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(Input.Username);
                user.Attributes.Add(CognitoAttribute.Email.AttributeName, Input.Email);
                user.Attributes.Add(CognitoAttribute.Name.AttributeName, Input.Name);
                user.Attributes.Add(CognitoAttribute.FamilyName.AttributeName, Input.LastName);
                user.Attributes.Add(CognitoAttribute.PhoneNumber.AttributeName, Input.PhoneNumber);
                user.Attributes.Add(CognitoAttribute.BirthDate.AttributeName, Input.BirthDate.ToString("yyyy-MM-dd"));
                
                var result = await _userManager.CreateAsync(user, Input.Password);
                Console.WriteLine($"create user: {result}");

                var addRoleResp = await _userManager.AddToRoleAsync(user, Input.Role);
                Console.WriteLine($"add role to user: {addRoleResp}");


                var confirmUser = await _userManager.AdminConfirmSignUpAsync(user);
                Console.WriteLine($"confirm user: {confirmUser}");


                if (result.Succeeded)
                {
                    var bracelet = bracelets.Where(x => x.SerialNumber == Input.BraceletId).FirstOrDefault();
                    bracelet.Username = Input.Email;
                    var userToRegister = new User { Username =  Input.Email, Role = Input.Role, Birth = Input.BirthDate.ToString("yyyy-MM-dd") };
                    var braceletToUserResp = await _braceletService.AssociateBracialetToUser(bracelet, userToRegister);
                    Console.WriteLine($"bracelet associate with user: {braceletToUserResp.ReasonPhrase}");
                    
                    var resp = await _braceletService.InsertUserSecureContact(new UserSecureContact { Username = Input.Email, ContactEmail = Input.EmergencyEmail1 });
                    Console.WriteLine($"insert secure contant: {resp.ReasonPhrase}");


                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToPage("./ConfirmEmail");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                       .Where(y => y.Count > 0)
                       .ToList();
            }
            // If we got this far, something failed, redisplay form
            Console.WriteLine("omething failed");
            return RedirectToPage("/Account/Register");
        }
    }
}
