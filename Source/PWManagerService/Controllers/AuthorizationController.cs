using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using PWManagerService.Model;
using PWManagerServiceModelEF;
using System.Text.Json;
using System.Text.Json.Nodes;

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
        public async Task<IActionResult> Register([FromBody] RegistrationData userData)
        {
            IdentityUser identUser = new IdentityUser();
            identUser.Email = userData.Email;
            identUser.PasswordHash = userData.HashedPassword;
            identUser.UserName = userData.Username;

            User user = new User();
            user.PasswordHint = userData.PasswordHint;
            user.AgbAcceptedAt = userData.AgbAcceptedAt;
            user.Salt = userData.Salt;
            user.FailedLogins = 0;
            user.LockedLogin = false;

            //Ueberpruefung ob Email schon vorhanden ist
            User takenUser = await dataContext.GetUser(userData.Email, userManager);
            if (takenUser != null)
                return Conflict("EMail Adress already taken");

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

        [HttpGet]
        [Route("Salt")]
        public async Task<ActionResult<object>> GetSalt(string email)
        {
            User user = await dataContext.GetUser(email);
            if (user == null)
                return NotFound();

            return Ok(user.Salt);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<object>> Authenticate([FromBody] AuthentificationData loginData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await dataContext.GetUser(loginData.Email, userManager);

            if (user == null)
            {
                return BadRequest("Bad credentials");
            }
            bool isPasswordValid = await userManager.CheckPasswordAsync(user.IdentityUser, loginData.HashedPassword);
            if (!isPasswordValid)
            {
                return Unauthorized("Password invalide");
            }
            
            user.JwtToken = tokenService.CreateToken(user.IdentityUser);
            await dataContext.SaveChangesAsync();
  
            return Ok(user);
        }

    }
}
