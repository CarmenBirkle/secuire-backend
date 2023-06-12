using Microsoft.AspNetCore.Mvc;
using PWManagerServiceModelEF;
using Newtonsoft.Json;
using PWManagerService.Factory;
using System.Net;
using Microsoft.EntityFrameworkCore;
using PWManagerServiceModelEF.Model;
using PWManagerService.Model;
using System;
using Microsoft.AspNetCore.Authorization;

//using System.Text.Json;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataEntryController : ControllerBase
    {
        private ILogger<DataEntryController> logger;
        private DataContext dataContext;
        public DataEntryController(ILogger<DataEntryController> logger, DataContext dataContext)
        {
            this.logger = logger;

            this.dataContext = dataContext;
        }

        /*
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

                        dataContext.Add(entry);
                        dataContext.SaveChanges();
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
                    SafeNote safeNote = new SafeNote()
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
                    Login loginEntry = new Login()
                    {
                        Id = 1,
                        Comment = "nckUc+JbmNJK/7M9i8l5668vvMLYtdojJIqQk4nXc8A=", // verschlüsselterKommentar
                        Favourite = "lcOBKz4fRxFtmHcQd/nn4Q==",//true
                        CustomTopics = new List<CustomTopic>() { new CustomTopic() { FieldName = "Name", FieldValue = "Value" } },
                        Subject = "WF0e1dHSkl4En+FMLl5Hs/xRRUGGPNgmNpwmGsUrENs=", //Hier steht das Thema
                        Password = "SehrsicheresPasswort",
                        Url = "https://sichereurl.com",
                        Username = "kreativerUsername",
                    }; Login loginEntry2 = new Login()
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
                    PaymentCard paymentCard = new PaymentCard()
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
                    }; PaymentCard paymentCard2 = new PaymentCard()
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
                }*/

        // Get All DataEntries
        [HttpGet, Authorize]
        [Route("all")]
        public async Task<ActionResult<GetResponseBody<List<object>>>> GetAllDataEntries()
        {
            List<object> dataEntries = new List<object>();

            var paymentCards = await dataContext.PaymentCard.Include(d => d.DataEntry).ToListAsync();
            var safeNotes = await dataContext.SafeNote.Include(d => d.DataEntry).ToListAsync();
            var logins = await dataContext.Login.Include(d => d.DataEntry).ToListAsync();

            dataEntries.AddRange(paymentCards);
            dataEntries.AddRange(safeNotes);
            dataEntries.AddRange(logins);

            //var result = await dataContext.DataEntry.ToListAsync();


            return Ok(dataEntries);
        }

        // GetAllPaymentCards
        [HttpGet]
        [Route("paymentcard/all")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> GetAllPaymentCards()
        {

            var result = await dataContext.PaymentCard
                .Include(d => d.DataEntry)
                .ToListAsync();

            return Ok(result);
        }

        // GetPaymentCardById
        [HttpGet]
        [Route("paymentcard/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetPaymentCardById(int id)
        {
            var result = dataContext.PaymentCard
                .Where(p => p.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            return Ok(result);

        }
            // PostNewDataEntry
            [HttpPost]
        [Route("paymentcard/new")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> CreatePaymentCard([FromBody] PaymentCardClientRequest paymentCardClientRequest)
        {
            DataEntry dataEntry = new DataEntry
            {
                UserId = 1,
                Comment = paymentCardClientRequest.Comment,
                Favourite = paymentCardClientRequest.Favourite,
                Subject = paymentCardClientRequest.Subject
            };

            await dataContext.DataEntry.AddAsync(dataEntry);
            dataContext.SaveChanges();


            PaymentCard paymentCard = new PaymentCard
            {
                DataEntryId = dataEntry.Id,
                Owner = paymentCardClientRequest.Owner,
                Number = paymentCardClientRequest.Number,
                CardTypeId = paymentCardClientRequest.CardTypeId,
                ExpirationDate = paymentCardClientRequest.ExpirationDate,
                Pin = paymentCardClientRequest.Pin,
                Cvv = paymentCardClientRequest.Cvv
            };
            
            await dataContext.PaymentCard.AddAsync(paymentCard);
            
            dataContext.SaveChanges();

            return Ok("Created");
            //return CreatedAtAction(nameof(GetAllPaymentCards), new { id = entry.Id }, entry);

        }



    }
}
