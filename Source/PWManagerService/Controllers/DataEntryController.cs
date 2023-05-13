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
        public async Task<ActionResult<ResponseBody<DataEntry>>> GetId(int id)
        {
            ResponseBody<DataEntry> response = new ResponseBody<DataEntry>();
            SafeNoteEntry entry = new SafeNoteEntry()
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
        public async Task<ActionResult<ResponseBody<DataEntryLists>>> GetAll()
        {
            ResponseBody<DataEntryLists> responseBody = new ResponseBody<DataEntryLists>();
            DataEntryLists entries = new DataEntryLists();

            responseBody.Data = entries;


            SafeNoteEntry noteEntry = new SafeNoteEntry()
            {
                Id = 1,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                SafeNote = "Z8Qf62joUrQ1e6hoRo/btXp6j0QyjoG3vp37iuUyjRm1WJITZNVIhAUjArt9720D",//Hier steht eine ganz sichere Notiz
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs="//Hier steht das Thema
            }; SafeNoteEntry noteEntry2 = new SafeNoteEntry()
            {
                Id = 2,
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
            }; LoginEntry loginEntry2 = new LoginEntry()
            {
                Id = 2,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                Password = "SehrsicheresPasswort",
                Url = "https://sichereurl.com",
                Username = "kreativerUsername",
            };
            PaymentCardEntry paymentCard = new PaymentCardEntry()
            {
                Id = 1,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                CardType = "Kartentyp",
                ExpirationDate = "Datum",
                Number = "Nummer",
                Owner = "Max Mustermann",
                Pin = "1234"
            }; PaymentCardEntry paymentCard2 = new PaymentCardEntry()
            {
                Id = 2,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new Dictionary<string, string>(),
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                CardType = "Kartentyp",
                ExpirationDate = "Datum",
                Number = "Nummer",
                Owner = "Max Mustermann",
                Pin = "1234"
            };

            entries.SafeNoteEntryList.Add(noteEntry);
            entries.SafeNoteEntryList.Add(noteEntry2);
            entries.LoginEntryList.Add(loginEntry);
            entries.LoginEntryList.Add(loginEntry2);
            entries.PaymentCardEntryList.Add(paymentCard);
            entries.PaymentCardEntryList.Add(paymentCard2);
            string jsonObject = JsonSerializer.Serialize<object>(responseBody);
            return Ok(jsonObject);
            //return responseBody;
        }
    }
}
