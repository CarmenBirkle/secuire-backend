using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using PWManagerService.Model;
using System.Net;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableRateLimiting("fixed")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Article>>> GetAll()
        {
            try
            {

                _logger.LogTrace("Hier wird eine Trace geloggt");
                _logger.LogDebug("Hier wird gedebuged");
                _logger.LogInformation($"Hier wird eine Information geloggt");
                _logger.LogWarning("Hier wird eine Warnung geloggt");
                _logger.LogError("Hier wird ein Error geloggt");
                _logger.LogCritical("Und hier passieren gerade kritische sachen");

                List<Article> list = new List<Article>
                {
                    new Article { ArticleCode = "1", Description = "Beschreibungstext1", Thickness = 0.4d },
                    new Article { ArticleCode = "2", Description = "Beschreibungstext2", Thickness = 0.3d },
                    new Article { ArticleCode = "3", Description = "Beschreibungstext3", Thickness = 0.3d },
                    new Article { ArticleCode = "4", Description = "Beschreibungstext4", Thickness = 0.3d }
                };

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet]
        [Route("{id}")]
        [DisableRateLimiting]
        public async Task<ActionResult<Article>> Get(int id)
        {
            _logger.LogInformation($"Getaufruf mit Id {id}");

            if (id < 0)
            {
                return BadRequest();
            }

            string testValue = Appsettings.Instance.TestValue;

            Article article = new Article { ArticleCode = id.ToString(), Description = testValue, Thickness = 0.4d };

            return article;
        }


        [HttpPut]
        public async Task<ActionResult> PostArticle(Article article)
        {
            // Hier Maßnahmen zum Anlegen des Artikels
            if (article == null)
            {
                return BadRequest("Article is Null");
            }

            article.ArticleCode = Guid.NewGuid().ToString();

            return CreatedAtAction(nameof(Get), new { id = article.ArticleCode }, article);
        }
    }
}
