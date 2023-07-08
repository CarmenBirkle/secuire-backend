using Microsoft.AspNetCore.Authorization;
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
        private AuthorizationFactory factory;

        public AuthorizationController(UserManager<IdentityUser> userManager, ILogger<AuthorizationController> logger, DataContext dataContext, TokenService tokenService)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.dataContext = dataContext;
            this.tokenService = tokenService;
            this.factory = new AuthorizationFactory(dataContext, userManager, logger, tokenService);
        }

        /// <summary>
        /// Usererstellung
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] AccountPostPutData userData)
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

        /// <summary>
        /// Account loeschen. Account wird anhand JWT erkannt, keine weiteren Parameter noetig
        /// </summary>
        /// <returns></returns>
        [HttpDelete, Authorize]
        public async Task<ActionResult> DeleteAccount()
        {
            int statusCode = await factory.DeleteAccount(TokenService.ReadToken(Request.Headers));
            return StatusCode(statusCode);
        }

        /// <summary>
        /// Anhand EMail Adresse wird entsprechendes Salt zurueckgegeben
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Salt")]
        public async Task<ActionResult<object>> GetSalt(string email)
        {
            User user = await dataContext.GetUser(email);
            if (user == null)
                return NotFound();

            return Ok(user.Salt);
        }

        /// <summary>
        /// Anhand EMail Adresse wird entsprechender Passwordhinweis zurueckgegeben
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Hint")]
        public async Task<ActionResult<string>> GetHint(string email)
        {
            User user = await dataContext.GetUser(email);
            if (user == null)
                return NotFound();

            return Ok(user.PasswordHint);
        }

        /// <summary>
        /// Login-Route
        /// Benutzerdaten und JWT wird zurueckgegeben
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<User>> Authenticate([FromBody] AuthentificationData loginData)
        {
            (User?, int) factoryResult = new();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                factoryResult = await factory.Login(loginData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            if (factoryResult.Item2 == 400)
                return BadRequest("Bad Credentials");
            else if (factoryResult.Item2 == 403)
                return StatusCode(403, "User is blocked, try again later ");
            else if (factoryResult.Item2 == 200)
                return Ok(factoryResult.Item1);

            return StatusCode(500);
        }

        /// <summary>
        /// Fuer Aenderung an User, die vorgenommen werden sollen
        /// IST User wird anhand JWT ermittelt
        /// </summary>
        /// <param name="updatedUser"></param>
        /// <returns></returns>
        [HttpPut, Authorize]
        public async Task<ActionResult<User>> PutUser([FromBody] AccountPostPutData updatedUser)
        {
            string jwtToken = TokenService.ReadToken(Request.Headers);
            (User?, int) updatedResult = (new User(), 0);
            try
            {
                updatedResult = await factory.UpdateAccount(jwtToken, updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            if (updatedResult.Item2 == 400)
            {
                return BadRequest();
            }
            else if (updatedResult.Item1 != null && updatedResult.Item2 == 200)
                return Ok(updatedResult.Item1);
            else
                return StatusCode(500);
        }

    }
}
