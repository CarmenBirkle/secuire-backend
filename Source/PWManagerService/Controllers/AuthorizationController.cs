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

        public AuthorizationController(UserManager<IdentityUser> userManager, ILogger<AuthorizationController> logger)
        {
            this.logger = logger;
            this.userManager = userManager;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string mail, string password, string salt, string passwordHint, string username, DateTime agbAcceptedAt)
        {
            User user = new User()
            {
                Email = mail,
                UserName = username,
                Password = password,
                PasswordHint = passwordHint,
                AgbAcceptedAt = agbAcceptedAt,
                FailedLogins = 0,
                LockedLogin = false,
                Salt = salt
            };

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            IdentityResult result = await userManager.CreateAsync(
                user, user.Password);


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
