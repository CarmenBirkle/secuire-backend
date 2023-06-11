using Microsoft.AspNetCore.Mvc;
using PWManagerServiceModelEF;
using Newtonsoft.Json;
using PWManagerService.Factory;
using System.Net;
using Microsoft.EntityFrameworkCore;
using PWManagerServiceModelEF.Model;
using PWManagerService.Model;
using System;

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

        #region DataEntries Methods
        // Get All DataEntries
        [HttpGet]
        [Route("dataentry/all")]
        public async Task<ActionResult<GetResponseBody<List<object>>>> GetAllDataEntries()
        {
            List<object> dataEntries = new List<object>();

            List<PaymentCard> paymentCards = await dataContext.PaymentCard.Include(d => d.DataEntry).ToListAsync();
            List<SafeNote> safeNotes = await dataContext.SafeNote.Include(d => d.DataEntry).ToListAsync();
            List<Login> logins = await dataContext.Login.Include(d => d.DataEntry).ToListAsync();

            dataEntries.AddRange(paymentCards);
            dataEntries.AddRange(safeNotes);
            dataEntries.AddRange(logins);

            if (dataEntries == null || dataEntries.Count == 0)
            {
                string ErrorMessage = "No Data Entries found.";
                return NotFound(ErrorMessage);
            }

            return Ok(dataEntries);
        }

        #endregion

        #region PaymentCard Methods
        // GetAllPaymentCards
        [HttpGet]
        [Route("paymentcard/all")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> GetAllPaymentCards()
        {

            List<PaymentCard> paymentCards = await dataContext.PaymentCard
                .Include(p => p.DataEntry)
                .ToListAsync();

            if (paymentCards == null || paymentCards.Count == 0)
            {
                string ErrorMessage = "No Payment Cards found.";
                return NotFound(ErrorMessage);
            }

            return Ok(paymentCards);
        }

        // GetPaymentCardById
        [HttpGet]
        [Route("paymentcard/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetPaymentCardById(int id)
        {
            try
            {
                PaymentCard paymentCard = dataContext.PaymentCard
                .Where(p => p.DataEntryId == id)
                .Include(p => p.DataEntry)
                .ToList()
                .Single();

                return Ok(paymentCard);
            }
            catch (ArgumentNullException)
            {
                string ErrorMessage = "No Payment Card with the requested Id could be found.";
                return NotFound(ErrorMessage);
            }

        }

        // PostNewPaymentCard
        [HttpPost]
        [Route("paymentcard/new")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> CreatePaymentCard([FromBody] DataEntryClientRequest dataEntryClientRequest)
        {
            DataEntry dataEntry = new DataEntry
            {
                UserId = 1,
                Comment = dataEntryClientRequest.Comment,
                Favourite = dataEntryClientRequest.Favourite,
                Subject = dataEntryClientRequest.Subject
            };

            await dataContext.DataEntry.AddAsync(dataEntry);


            dataContext.SaveChanges();


            PaymentCard paymentCard = new PaymentCard
            {
                DataEntryId = dataEntry.Id,
                Owner = dataEntryClientRequest.Owner,
                Number = dataEntryClientRequest.CardNumber,
                CardTypeId = (int)dataEntryClientRequest.CardTypeId,
                ExpirationDate = (DateTime)dataEntryClientRequest.ExpirationDate,
                Pin = dataEntryClientRequest.Pin,
                Cvv = dataEntryClientRequest.Cvv
            };

            await dataContext.PaymentCard.AddAsync(paymentCard);

            dataContext.SaveChanges();

            //return Ok("Created");
            return CreatedAtAction(nameof(GetAllPaymentCards), new { id = paymentCard.DataEntryId }, paymentCard);

        }

        // Alter PaymentCard
        [HttpPut]
        [Route("paymentcard/{id:int}")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> AlterPaymentCard([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            DataEntry dataEntry = dataContext.DataEntry
                .Where(d => d.Id == id)
                .ToList()
                .Single();

            PaymentCard paymentCard = dataContext.PaymentCard
                .Where(p => p.DataEntryId == dataEntry.Id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            // Alter Data Entry:
            dataEntry.UserId = 1;
            dataEntry.Comment = dataEntryClientRequest.Comment;
            dataEntry.Favourite = dataEntryClientRequest.Favourite;
            dataEntry.Subject = dataEntryClientRequest.Subject;

            // Alter PaymentCard:
            paymentCard.Owner = dataEntryClientRequest.Owner;
            paymentCard.Number = dataEntryClientRequest.CardNumber;
            paymentCard.CardTypeId = 1;//(int)dataEntryClientRequest.CardTypeId;
            paymentCard.ExpirationDate = (DateTime)dataEntryClientRequest.ExpirationDate;
            paymentCard.Pin = dataEntryClientRequest.Pin;
            paymentCard.Cvv = dataEntryClientRequest.Cvv;

            dataContext.SaveChanges();

            PaymentCard alteredPaymentCard = dataContext.PaymentCard
                .Where(p => p.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            return Ok(alteredPaymentCard);
        }

        // Delete PaymentCard
        [HttpDelete]
        [Route("paymentcard/{id:int}")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> DeletePaymentCard(int id)
        {
            DataEntry dataEntry = dataContext.DataEntry
                .Where(d => d.Id == id)
                .ToList()
                .Single();

            PaymentCard paymentCard = dataContext.PaymentCard
                .Where(p => p.DataEntryId == dataEntry.Id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            dataContext.DataEntry.Remove(dataEntry);
            dataContext.PaymentCard.Remove(paymentCard);

            dataContext.SaveChanges();

            return Ok("Payment Card got deleted successfully");
        }
        #endregion

        #region Login Methods
        // GetAllLogins
        [HttpGet]
        [Route("login/all")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> GetAllLogins()
        {

            var result = await dataContext.Login
                .Include(d => d.DataEntry)
                .ToListAsync();

            return Ok(result);
        }

        // GetLoginById
        [HttpGet]
        [Route("login/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetLoginById(int id)
        {
            var result = dataContext.Login
                .Where(l => l.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            return Ok(result);

        }

        // PostNewLogin
        [HttpPost]
        [Route("login/new")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> CreateLogin([FromBody] DataEntryClientRequest dataEntryClientRequest)
        {
            DataEntry dataEntry = new DataEntry
            {
                UserId = 1,
                Comment = dataEntryClientRequest.Comment,
                Favourite = dataEntryClientRequest.Favourite,
                Subject = dataEntryClientRequest.Subject
            };

            await dataContext.DataEntry.AddAsync(dataEntry);
            dataContext.SaveChanges();


            Login login = new Login
            {
                DataEntryId = dataEntry.Id,
                Username = dataEntryClientRequest.Username,
                Password = dataEntryClientRequest.Password,
                Url = dataEntryClientRequest.Url
            };

            await dataContext.Login.AddAsync(login);

            dataContext.SaveChanges();

            //return Ok("Created");
            return CreatedAtAction(nameof(GetAllLogins), new { id = login.DataEntryId }, login);

        }

        // Alter Login
        [HttpPut]
        [Route("login/{id:int}")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> AlterLoginCard([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            DataEntry dataEntry = dataContext.DataEntry
                .Where(d => d.Id == id)
                .ToList()
                .Single();

            Login login = dataContext.Login
                .Where(l => l.DataEntryId == dataEntry.Id)
                .Include(l => l.DataEntry)
                .ToList()
                .Single();

            // Alter Data Entry:
            dataEntry.UserId = 1;
            dataEntry.Comment = dataEntryClientRequest.Comment;
            dataEntry.Favourite = dataEntryClientRequest.Favourite;
            dataEntry.Subject = dataEntryClientRequest.Subject;

            // Alter Login:
            login.Username = dataEntryClientRequest.Username;
            login.Password = dataEntryClientRequest.Password;
            login.Url = dataEntryClientRequest.Url;

            dataContext.SaveChanges();

            Login alteredLogin = dataContext.Login
                .Where(l => l.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            return Ok(alteredLogin);
        }

        // Delete Login
        [HttpDelete]
        [Route("login/{id:int}")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> DeleteLogin(int id)
        {
            DataEntry dataEntry = dataContext.DataEntry
                .Where(d => d.Id == id)
                .ToList()
                .Single();

            Login login = dataContext.Login
                .Where(l => l.DataEntryId == dataEntry.Id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

            dataContext.DataEntry.Remove(dataEntry);
            dataContext.Login.Remove(login);

            dataContext.SaveChanges();

            return Ok("Payment Card got deleted successfully");
        }
        #endregion

        #region SafeNote Methods



        #endregion



    }
}
