using Microsoft.AspNetCore.Mvc;
using PWManagerServiceModelEF;
using Newtonsoft.Json;
using PWManagerService.Factory;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using PWManagerService.Model;

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

        #region NewMethods

        [HttpGet]
        public async Task<ActionResult<GetResponseBody<List<object>>>> GetAllDataEntries()
        {
            List<object> dataEntries = new List<object>();

            List<PaymentCard> paymentCards = dataContext.GetPaymentCard();
            List<SafeNote> safeNotes = dataContext.GetSafeNote();
            List<Login> logins = dataContext.GetLogin();

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

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetDataEntryById(int id)
        {
            PaymentCard paymentCard = dataContext.GetPaymentCard(id);
            SafeNote safeNote = dataContext.GetSafeNote(id);
            Login login = dataContext.GetLogin(id);

            if (!(paymentCard == null))
            {
                return Ok(paymentCard);
            }
            else if (!(login == null))
            {
                return Ok(login);
            }
            else if (!(safeNote == null))
            {
                return Ok(safeNote);
            }
            else
            {
                return NotFound("Data Entry with the requested id not found.");
            }

        }

        [HttpPost]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> CreateDataEntry([FromBody] DataEntryClientRequest dataEntryClientRequest)
        {
            string category = dataEntryClientRequest.Category;

            if (category == null | (!category.Equals("login") & !category.Equals("safenote") & !category.Equals("paymentcard")))
            {
                return BadRequest("Invalid category. Data Entry could not be created.");
            }

            DataEntry dataEntry = new DataEntry
            {
                UserId = 1,
                Comment = dataEntryClientRequest.Comment,
                Favourite = dataEntryClientRequest.Favourite,
                Subject = dataEntryClientRequest.Subject,
                CustomTopics = dataEntryClientRequest.CustomTopics,
                SelectedIcon = dataEntryClientRequest.SelectedIcon
            };

            await dataContext.DataEntry.AddAsync(dataEntry);
            dataContext.SaveChanges();

            switch (dataEntryClientRequest.Category)
            {
                case "paymentcard":
                    PaymentCard paymentCard = new PaymentCard
                    {
                        DataEntryId = dataEntry.Id,
                        Owner = dataEntryClientRequest.Owner,
                        Number = dataEntryClientRequest.CardNumber,
                        ExpirationDate = dataEntryClientRequest.ExpirationDate,
                        Pin = dataEntryClientRequest.Pin,
                        Cvv = dataEntryClientRequest.Cvv
                    };
                    await dataContext.PaymentCard.AddAsync(paymentCard);
                    dataContext.SaveChanges();

                    return CreatedAtAction(nameof(GetAllDataEntries), new { id = paymentCard.DataEntryId }, paymentCard);

                case "login":
                    Login login = new Login
                    {
                        DataEntryId = dataEntry.Id,
                        Username = dataEntryClientRequest.Username,
                        Password = dataEntryClientRequest.Password,
                        Url = dataEntryClientRequest.Url
                    };
                    await dataContext.Login.AddAsync(login);
                    dataContext.SaveChanges();

                    return CreatedAtAction(nameof(GetAllDataEntries), new { id = login.DataEntryId }, login);

                case "safenote":
                    SafeNote safeNote = new SafeNote
                    {
                        DataEntryId = dataEntry.Id,
                        Note = dataEntryClientRequest.Note,
                    };
                    await dataContext.SafeNote.AddAsync(safeNote);
                    dataContext.SaveChanges();
                    return CreatedAtAction(nameof(GetAllDataEntries), new { id = safeNote.DataEntryId }, safeNote);
                default:
                    dataContext.DataEntry.Remove(dataEntry);
                    dataContext.SaveChanges();

                    return BadRequest("Invalid request. Data Entry could not be created.");
            }

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> AlterDataEntry([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            string category = dataEntryClientRequest.Category;

            if (category == null | (!category.Equals("login") & !category.Equals("safenote") & !category.Equals("paymentcard")))
            {
                return BadRequest("Invalid category. Data Entry could not be updated.");
            }

            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if (dataEntry == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            switch (category)
            {
                case "login":
                    Login login = dataContext.GetLogin(id);

                    if (login == null)
                    {
                        string errorMessage = "No Login with the requested Id could be found.";
                        return NotFound(errorMessage);
                    }

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

                    Login alteredLogin = dataContext.GetLogin(id);

                    if (alteredLogin == null)
                    {
                        string errorMessage = "No Login with the requested Id could be found.";
                        return NotFound(errorMessage);
                    }

                    return Ok(alteredLogin);

                case "paymentcard":
                    PaymentCard paymentCard = dataContext.GetPaymentCard(id);

                    if (paymentCard == null)
                    {
                        string errorMessage = "No Payment Card with the requested Id could be found.";
                        return NotFound(errorMessage);
                    }

                    // Alter Data Entry:
                    dataEntry.UserId = 1;
                    dataEntry.Comment = dataEntryClientRequest.Comment;
                    dataEntry.Favourite = dataEntryClientRequest.Favourite;
                    dataEntry.Subject = dataEntryClientRequest.Subject;

                    // Alter PaymentCard:
                    paymentCard.Owner = dataEntryClientRequest.Owner;
                    paymentCard.Number = dataEntryClientRequest.CardNumber;
                    paymentCard.ExpirationDate = dataEntryClientRequest.ExpirationDate;
                    paymentCard.Pin = dataEntryClientRequest.Pin;
                    paymentCard.Cvv = dataEntryClientRequest.Cvv;

                    dataContext.SaveChanges();

                    PaymentCard alteredPaymentCard = dataContext.GetPaymentCard(id);

                    if (alteredPaymentCard == null)
                    {
                        string errorMessage = "No Payment Card with the requested Id could be found.";
                        return NotFound(errorMessage);
                    }

                    return Ok(alteredPaymentCard);

                case "safenote":
                    SafeNote safenote = dataContext.GetSafeNote(id);

                    if (safenote == null)
                    {
                        string errorMessage = "No Safe Note with the requested Id could be found.";
                        return NotFound(errorMessage);
                    }

                    // Alter Data Entry:
                    dataEntry.UserId = 1;
                    dataEntry.Comment = dataEntryClientRequest.Comment;
                    dataEntry.Favourite = dataEntryClientRequest.Favourite;
                    dataEntry.Subject = dataEntryClientRequest.Subject;

                    // Alter SafeNote:
                    safenote.Note = dataEntryClientRequest.Note;

                    dataContext.SaveChanges();

                    SafeNote alteredSafeNote = dataContext.GetSafeNote(id);

                    if (alteredSafeNote == null)
                    {
                        string errorMessage = "No Safe Note with the requested Id could be found.";
                        return NotFound(errorMessage);
                    }

                    return Ok(alteredSafeNote);

                default:
                    return BadRequest("Invalid request. Data Entry could not be updated.");

            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> DeleteDataEntryById(int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if (dataEntry == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            PaymentCard paymentCard = dataContext.GetPaymentCard(id);
            SafeNote safeNote = dataContext.GetSafeNote(id);
            Login login = dataContext.GetLogin(id);

            if (!(paymentCard == null))
            {
                dataContext.DataEntry.Remove(dataEntry);
                dataContext.PaymentCard.Remove(paymentCard);

                dataContext.SaveChanges();

                return Ok("Payment Card got deleted successfully");
            }
            else if (!(login == null))
            {
                dataContext.DataEntry.Remove(dataEntry);
                dataContext.Login.Remove(login);

                dataContext.SaveChanges();

                return Ok("Login got deleted successfully");
            }
            else if (!(safeNote == null))
            {
                dataContext.DataEntry.Remove(dataEntry);
                dataContext.SafeNote.Remove(safeNote);

                dataContext.SaveChanges();

                return Ok("Safe Note got deleted successfully");
            }
            else
            {
                return NotFound("Data Entry with the requested id not found.");
            }

        }
        #endregion

        //[HttpPost]
        //[Route("/addRandomSafeNote")]
        //public async Task<ActionResult<GetResponseBody<DataEntry>>> AddRandomSafeNote()
        //{

        //    List<object> data = TestData.CreateRandomSafeNote();

        //    DataEntry dataEntry = (DataEntry)data[0];

        //    await dataContext.DataEntry.AddAsync(dataEntry);
        //    dataContext.SaveChanges();

        //    SafeNote safeNote = (SafeNote)data[1];

        //    safeNote.DataEntryId = dataEntry.Id;

        //    await dataContext.SafeNote.AddAsync(safeNote);
        //    dataContext.SaveChanges();

        //    return CreatedAtAction(nameof(GetAllDataEntries), new { id = safeNote.DataEntryId }, safeNote);

        //}



        /*
        #region PaymentCard Methods
            // GetAllPaymentCards
            [HttpGet]
        [Route("paymentcard/all")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> GetAllPaymentCards()
        {

            List<PaymentCard> paymentCards = dataContext.GetPaymentCard();

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

                PaymentCard paymentCard = dataContext.GetPaymentCard(id);

                if(paymentCard == null)
                {
                    string errorMessage = "No Payment Card with the requested Id could be found.";
                    return NotFound(errorMessage);
                }
                return Ok(paymentCard);


        }

        // PostNewPaymentCard
        [HttpPost]
        [Route("paymentcard/new")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> CreatePaymentCard([FromBody] DataEntryClientRequest dataEntryClientRequest)
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
        public async Task<ActionResult<GetResponseBody<DataEntry>>> AlterPaymentCard([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if (dataEntry == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            PaymentCard paymentCard = dataContext.GetPaymentCard(id);

            if(paymentCard == null)
            {
                string errorMessage = "No Payment Card with the requested Id could be found.";
                return NotFound(errorMessage);
            }

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

            PaymentCard alteredPaymentCard = dataContext.GetPaymentCard(id);

            if(alteredPaymentCard == null)
            {
                string errorMessage = "No Payment Card with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            return Ok(alteredPaymentCard);
        }

        // Delete PaymentCard
        [HttpDelete]
        [Route("paymentcard/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> DeletePaymentCard(int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if (dataEntry == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            PaymentCard paymentCard = dataContext.GetPaymentCard(id);

            if (paymentCard == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

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

            List<Login> logins = dataContext.GetLogin();

            if(logins == null || logins.Count == 0)
            {
                string errorMessage = "No Logins could be found.";
                return NotFound(errorMessage);
            }

            return Ok(logins);
        }

        // GetLoginById
        [HttpGet]
        [Route("login/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetLoginById(int id)
        {
            Login login = dataContext.GetLogin(id);

            return Ok(login);

        }

        // PostNewLogin
        [HttpPost]
        [Route("login/new")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> CreateLogin([FromBody] DataEntryClientRequest dataEntryClientRequest)
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
        public async Task<ActionResult<GetResponseBody<DataEntry>>> AlterLoginCard([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            Login login = dataContext.GetLogin(id);

            if (login == null)
            {
                string errorMessage = "No Login with the requested Id could be found.";
                return NotFound(errorMessage);
            }

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

            Login alteredLogin = dataContext.GetLogin(id);

            if (alteredLogin == null)
            {
                string errorMessage = "No Login with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            return Ok(alteredLogin);
        }

        // Delete Login
        [HttpDelete]
        [Route("login/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> DeleteLogin(int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if(dataEntry == null)
            {

                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            Login login = dataContext.GetLogin(id);

            if (login == null)
            {
                string errorMessage = "No Login with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            dataContext.DataEntry.Remove(dataEntry);
            dataContext.Login.Remove(login);

            dataContext.SaveChanges();

            return Ok("Login got deleted successfully");
        }
        #endregion

        #region SafeNote Methods
        // GetAllSafeNotes
        [HttpGet]
        [Route("safenote/all")]
        public async Task<ActionResult<GetResponseBody<List<DataEntry>>>> GetAllSafeNotes()
        {

            List<SafeNote> safeNotes = dataContext.GetSafeNote();

            if (safeNotes == null || safeNotes.Count == 0)
            {
                string ErrorMessage = "No Safe Notes found.";
                return NotFound(ErrorMessage);
            }

            return Ok(safeNotes);
        }

        // GetSafeNoteById
        [HttpGet]
        [Route("safenote/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> GetSafeNoteById(int id)
        {         
            SafeNote safeNote = dataContext.GetSafeNote(id);

            if(safeNote == null)
            {
                string errorMessage = "No Safe Note with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            return Ok(safeNote);

        }

        // PostNewPaymentCard
        [HttpPost]
        [Route("safenote/new")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> CreateSafeNote([FromBody] DataEntryClientRequest dataEntryClientRequest)
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

            SafeNote safeNote = new SafeNote
            {
                DataEntryId = dataEntry.Id,
                Note = dataEntryClientRequest.Note,
            };

            await dataContext.SafeNote.AddAsync(safeNote);

            dataContext.SaveChanges();

            return CreatedAtAction(nameof(GetAllPaymentCards), new { id = safeNote.DataEntryId }, safeNote);

        }

        // Alter SafeNote
        [HttpPut]
        [Route("safenote/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> AlterSafeNote([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if(dataEntry == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            SafeNote safenote = dataContext.GetSafeNote(id);

            if(safenote == null)
            {
                string errorMessage = "No Safe Note with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            // Alter Data Entry:
            dataEntry.UserId = 1;
            dataEntry.Comment = dataEntryClientRequest.Comment;
            dataEntry.Favourite = dataEntryClientRequest.Favourite;
            dataEntry.Subject = dataEntryClientRequest.Subject;

            // Alter SafeNote:
            safenote.Note = dataEntryClientRequest.Note;

            dataContext.SaveChanges();

            SafeNote alteredSafeNote = dataContext.GetSafeNote(id);

            if(alteredSafeNote == null)
            {
                string errorMessage = "No Safe Note with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            return Ok(alteredSafeNote);
        }

        // Delete SafeNote
        [HttpDelete]
        [Route("safenote/{id:int}")]
        public async Task<ActionResult<GetResponseBody<DataEntry>>> DeleteSafeNote(int id)
        {
            DataEntry dataEntry = dataContext.GetDataEntry(id);

            if(dataEntry == null)
            {
                string errorMessage = "No Data Entry with the requested Id could be found.";
                return NotFound(errorMessage);
            }

            SafeNote safeNote = dataContext.GetSafeNote(id);

            if(safeNote == null)
            {
                string errorMessage = "No Safe Note with the requested Id could be found.";
                return NotFound(errorMessage);
            }


            dataContext.DataEntry.Remove(dataEntry);
            dataContext.SafeNote.Remove(safeNote);

            dataContext.SaveChanges();

            return Ok("Safe Note got deleted successfully");
        }
        #endregion
        */

        #region User Methods

        #endregion



    }
}
