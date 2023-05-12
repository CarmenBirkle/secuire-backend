using Microsoft.AspNetCore.Mvc;
using PWManagerService.Model;
using PWManagerServiceModelEF;
using System.Text.Json;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataEntryController : ControllerBase
    {
        private ILogger<DataEntryController> logger;
        public DataEntryController(ILogger<DataEntryController> logger)
        {
            this.logger = logger;
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ResponseBody<SafeNoteEntry>>> GetId(int id)
        {
            ResponseBody<DataEntry> response = new ResponseBody<DataEntry>();   
            DataEntry entry =   new SafeNoteEntry()
            {
                Id = 1,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                SafeNote = "Z8Qf62joUrQ1e6hoRo/btXp6j0QyjoG3vp37iuUyjRm1WJITZNVIhAUjArt9720D",//Hier steht eine ganz sichere Notiz
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs="//Hier steht das Thema
            };

            response.Data = entry;
            return Ok(response);

        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<ResponseBody<List<DataEntry>>>> GetAll()
        {
            ResponseBody<List<DataEntry>> responseBody = new ResponseBody<List<DataEntry>>();
            responseBody.Data = new List<DataEntry>();
            

            SafeNoteEntry noteEntry = new SafeNoteEntry()
            {
                Id = 1,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                SafeNote = "Z8Qf62joUrQ1e6hoRo/btXp6j0QyjoG3vp37iuUyjRm1WJITZNVIhAUjArt9720D",//Hier steht eine ganz sichere Notiz
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs="//Hier steht das Thema
            };
            LoginEntry loginEntry = new LoginEntry()
            {
                Id = 1,
                
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                Password = "SehrsicheresPasswort",
                Url = "https://sichereurl.com",
                Username = "kreativerUsername",
            };

            responseBody.Data.Add(noteEntry);
            responseBody.Data.Add(loginEntry);
            string jsonObject = JsonSerializer.Serialize<object>(responseBody);
            return Ok(jsonObject);
            //return responseBody;
        }
    }
}
