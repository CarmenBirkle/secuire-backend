using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using PWManagerServiceModelEF;
using System.Text.Json;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private ILogger<AuthorizationController> logger;
        private readonly UserManager<IdentityUser> userManager;
        private DataContext dataContext;
        private readonly TokenService tokenService;

        public AuthorizationController(UserManager<IdentityUser> userManager, ILogger<AuthorizationController> logger, DataContext dataContext, TokenService tokenService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.dataContext = dataContext;
            this.tokenService = tokenService;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(string mail, string password, string salt, string passwordHint, string username, DateTime agbAcceptedAt)
        {
            IdentityUser identUser = new IdentityUser();
            identUser.Email = mail;
            identUser.PasswordHash = password;
            identUser.UserName = username;

            User user = new User();
            //user.IdentUser = identUser;
            user.PasswordHint = passwordHint;
            user.AgbAcceptedAt = agbAcceptedAt;
            user.Salt = salt;
            user.FailedLogins = 0;
            user.LockedLogin = false;


            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            IdentityResult result = await userManager.CreateAsync(identUser, identUser.PasswordHash);


            if (result.Succeeded)
            {
                user.IdentityUser = identUser;
                await dataContext.User.AddAsync(user);
                dataContext.SaveChanges();
                return CreatedAtAction(nameof(Register), new { email = identUser.Email }, user);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<object>> Authenticate( string mail, string passwordHash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await userManager.FindByEmailAsync(mail);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(managedUser, passwordHash);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }
            var userInDb = dataContext.Users.FirstOrDefault(u => u.Email == mail);
            if (userInDb is null)
                return Unauthorized();
            string accessToken = tokenService.CreateToken(userInDb);
            await dataContext.SaveChangesAsync();
            
            return Ok(accessToken);
        }

    }
}
