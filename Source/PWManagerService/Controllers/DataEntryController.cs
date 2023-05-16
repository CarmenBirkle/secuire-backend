using Microsoft.AspNetCore.Mvc;
using PWManagerService.Model;
using PWManagerServiceModelEF;
using Newtonsoft.Json;
using PWManagerService.Factory;
using System.Net;

//using System.Text.Json;

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

        [HttpPost]
        public async Task<ActionResult<PostResponseBody<DataEntry>>> PostDataEntry([FromBody] DataEntryClientRequest clientData)
        {
            DataEntry entry;
            PostResponseBody<DataEntry> postBody;
            IDataHandler<DataEntry> dataHandler = new SqlDataHandler<DataEntry>();

            try
            {
                entry = DataEntryFactory.InitEntry(clientData, out postBody, logger);

                if (!postBody.IsSuccess)
                {
                    Response.StatusCode = (int)postBody.StatusCode;

                    return BadRequest(postBody);
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            try
            {
                entry.InsertEntry(out postBody, logger, dataHandler);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

            return CreatedAtAction(nameof(GetId), new { id = entry.Id }, entry);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetId(int id)
        {
            GetResponseBody<DataEntry> response = new GetResponseBody<DataEntry>();
            SafeNoteEntry entry = new SafeNoteEntry()
            {
                Id = id,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName ="Name", FieldValue="Value"} },
                SafeNote = "Z8Qf62joUrQ1e6hoRo/btXp6j0QyjoG3vp37iuUyjRm1WJITZNVIhAUjArt9720D",//Hier steht eine ganz sichere Notiz
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs="//Hier steht das Thema
            };

            response.Data = entry;
            return Ok(response.Data);

        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> GetAll()
        {
            GetResponseBody<List<DataEntry>> responseBody = new GetResponseBody<List<DataEntry>>();

            responseBody.Data = new List<DataEntry>();

            SafeNoteEntry noteEntry = new SafeNoteEntry()
            {
                Id = 1,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
                SafeNote = "Z8Qf62joUrQ1e6hoRo/btXp6j0QyjoG3vp37iuUyjRm1WJITZNVIhAUjArt9720D",//Hier steht eine ganz sichere Notiz
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs="//Hier steht das Thema
            }; SafeNoteEntry noteEntry2 = new SafeNoteEntry()
            {
                Id = 2,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
                SafeNote = "Z8Qf62joUrQ1e6hoRo/btXp6j0QyjoG3vp37iuUyjRm1WJITZNVIhAUjArt9720D",//Hier steht eine ganz sichere Notiz
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs="//Hier steht das Thema
            };
            LoginEntry loginEntry = new LoginEntry()
            {
                Id = 1,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                Password = "SehrsicheresPasswort",
                Url = "https://sichereurl.com",
                Username = "kreativerUsername",
            }; LoginEntry loginEntry2 = new LoginEntry()
            {
                Id = 2,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
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
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                CardType = "Kartentyp",
                ExpirationDate = "Datum",
                Cardnumber = "Nummer",
                Owner = "Max Mustermann",
                Pin = "1234"
            }; PaymentCardEntry paymentCard2 = new PaymentCardEntry()
            {
                Id = 2,
                Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
                Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                CardType = "Kartentyp",
                ExpirationDate = "Datum",
                Cardnumber = "Nummer",
                Owner = "Max Mustermann",
                Pin = "1234"
            };


            responseBody.Data.Add(noteEntry);
            responseBody.Data.Add(noteEntry2);
            responseBody.Data.Add(loginEntry);
            responseBody.Data.Add(loginEntry2);
            responseBody.Data.Add(paymentCard);
            responseBody.Data.Add(paymentCard2);
            return Ok(responseBody.Data);
        }
    }
}
