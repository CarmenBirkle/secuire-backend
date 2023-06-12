using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using PWManagerServiceModelEF;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private ILogger<AuthorizationController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public AuthorizationController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string mail, string password, string salt, string passwordHint, string username, DateTime agbAcceptedAt)
        {
            User user = new User()
            {
                Email = mail,
                Username = username,
                Password = password,
                PasswordHint = passwordHint,
                AgbAcceptedAt = agbAcceptedAt,
                FailedLogins = 0,
                LockedLogin = false,
                Salt = salt
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userManager.CreateAsync(
                new IdentityUser { Email = user.Email },
                user.Password
            );
            if (result.Succeeded)
            {
                user.Password = "";
                return CreatedAtAction(nameof(Register), new { email = user.Email }, user);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

    }
}
