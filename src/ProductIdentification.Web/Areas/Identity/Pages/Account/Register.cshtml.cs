using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ProductIdentification.Core.Models.Roles;

namespace ProductIdentification.Web.Areas.Identity.Pages.Account
{
    [Authorize(Roles = CustomRoles.ManagerOrAbove)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        
        [TempData]
        public string RegisteredStatusMessage { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required]
            [Display(Name = "Role")]
            public string RoleName { get; set; }
        }

        public void OnGet()
        {
            SetRolesList();
        }

        private void SetRolesList()
        {
            if (User.IsInRole(Role.Admin))
            {
                RolesList = RoleNameDictionary.All.Select(x => new SelectListItem(x.Value, x.Key));
            }

            if (User.IsInRole(Role.Manager))
            {
                RolesList = RoleNameDictionary.BelowManager.Select(x => new SelectListItem(x.Value, x.Key));
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {HttpContext.User.Identity.Name} created a new account without password.");
                    
                    await _userManager.AddToRoleAsync(user, Input.RoleName);

                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/SetPassword",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Create your account in Product Identification system!",
                        $"Please finish setting up your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    RegisteredStatusMessage = $"User: {user.UserName} has been created in role: {RoleNameDictionary.All[Input.RoleName]}.";
                    
                    return Redirect("./UserRegisterConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            SetRolesList();
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
