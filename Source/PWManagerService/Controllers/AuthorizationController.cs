using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using PWManagerServiceModelEF;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private ILogger<AuthorizationController> logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly DataContext context;
        private readonly TokenService tokenService;


        public AuthorizationController(UserManager<IdentityUser> userManager, ILogger<AuthorizationController> logger, DataContext context, TokenService tokenService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ResponseBody<object>>> Authenticate(string password, string mail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityUser managedUser = await userManager.FindByEmailAsync(mail);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }
            bool isPasswordValid = await userManager.CheckPasswordAsync(managedUser, password);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }
            
            IdentityUser userInDb = context.Users.FirstOrDefault(u => u.Email == mail);
            if (userInDb is null)
                return Unauthorized();

            string accessToken = tokenService.CreateToken(userInDb);
            await context.SaveChangesAsync();

            return Ok(accessToken);
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
