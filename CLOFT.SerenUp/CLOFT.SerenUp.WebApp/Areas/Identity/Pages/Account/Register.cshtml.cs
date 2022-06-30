// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CLOFT.SerenUp.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly CognitoUserPool _pool;

        public RegisterModel(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            ILogger<RegisterModel> logger,
            CognitoUserPool pool)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _logger = logger;
            _pool = pool;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Display(Name = "Phone number")]
            [Phone]
            public string PhoneNumber { get; set; } = string.Empty;

            //[Display(Name = "Picture")]
            //public int Picture { get; set; }

            [Required]
            [Display(Name = "Birth date")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
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
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Emergency Email")]
            public string EmergencyEmail1 { get; set; }

            [EmailAddress]
            [Display(Name = "Emergency Email2")]
            public string EmergencyEmail2 { get; set; } = "email@mail.com";
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(Input.UserName);
                user.Attributes.Add(CognitoAttribute.Email.AttributeName, Input.Email);
                user.Attributes.Add(CognitoAttribute.Name.AttributeName, Input.Name);
                user.Attributes.Add(CognitoAttribute.MiddleName.AttributeName, Input.LastName);
                user.Attributes.Add(CognitoAttribute.PhoneNumber.AttributeName, Input.PhoneNumber);
                user.Attributes.Add(CognitoAttribute.BirthDate.AttributeName, Input.BirthDate.ToString("d"));
                user.Attributes.Add(CognitoAttribute.Picture.AttributeName, "");
                user.Attributes.Add("custom:emergencyContact1", Input.EmergencyEmail1);
                user.Attributes.Add("custom:emergencyContact2", Input.EmergencyEmail2);

                

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToPage("./ConfirmEmail");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
