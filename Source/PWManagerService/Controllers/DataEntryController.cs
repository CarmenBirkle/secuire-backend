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
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

//using System.Text.Json;

namespace PWManagerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataEntryController : ControllerBase
    {
        private ILogger<DataEntryController> logger;
        private DataContext dataContext;
        private UserManager<IdentityUser> userManager;
        private DataEntryFactory factory;
        public DataEntryController(ILogger<DataEntryController> logger, DataContext dataContext, UserManager<IdentityUser> userManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.dataContext = dataContext;
            this.factory = new DataEntryFactory(dataContext, userManager, logger);
        }

        #region NewMethods

        [HttpGet, Authorize]
        [Route("all")]
        public async Task<ActionResult<List<object>>> GetAllDataEntries()
        {
            try
            {
                string token = factory.ReadToken(Request.Headers);
                List<object> dataEntries = await factory.GetAllDataEntries(token, userManager);
                return Ok(dataEntries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet, Authorize]
        //[Route("{id:int}")]
        //public async Task<ActionResult<DataEntry>> GetDataEntryById(int id)
        //{


        //    PaymentCard paymentCard = dataContext.GetPaymentCard(id);
        //    SafeNote safeNote = dataContext.GetSafeNote(id);
        //    Login login = dataContext.GetLogin(id);

        //    if (!(paymentCard == null))
        //    {
        //        return Ok(paymentCard);
        //    }
        //    else if (!(login == null))
        //    {
        //        return Ok(login);
        //    }
        //    else if (!(safeNote == null))
        //    {
        //        return Ok(safeNote);
        //    }
        //    else
        //    {
        //        return NotFound("Data Entry with the requested id not found.");
        //    }

        //}


        [HttpPost, Authorize]
        public async Task<ActionResult<(string, object)>> PostDataEntry([FromBody] DataEntryClientRequest dataEntryClientRequest)
        {
            string token = factory.ReadToken(Request.Headers);

            (int, object?) dataTuple = await factory.CreateDataEntry(token, dataEntryClientRequest);
            if (dataTuple.Item1 != 201)
            {
                return StatusCode(dataTuple.Item1);
            }
            else
                return CreatedAtAction(nameof(GetAllDataEntries), dataTuple.Item2);
        }

        [HttpPut, Authorize]
        [Route("{id:int}")]
        public async Task<ActionResult<object>> AlterDataEntry([FromBody] DataEntryClientRequest dataEntryClientRequest, int id)
        {
            string token = factory.ReadToken(Request.Headers);
            (int, object?) updatedDataset;
            try
            {
                updatedDataset = await factory.UpdateDataEntry(id, dataEntryClientRequest, token);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            if (updatedDataset.Item1 != 200)
                return StatusCode(updatedDataset.Item1);
            else
                return Ok(updatedDataset.Item2);

            //string category = dataEntryClientRequest.Category;

            //if (category == null | (!category.Equals("login") & !category.Equals("safenote") & !category.Equals("paymentcard")))
            //{
            //    return BadRequest("Invalid category. Data Entry could not be updated.");
            //}

            //DataEntry dataEntry = dataContext.GetDataEntry(id);

            //if (dataEntry == null)
            //{
            //    string errorMessage = "No Data Entry with the requested Id could be found.";
            //    return NotFound(errorMessage);
            //}

            //switch (category)
            //{
            //    case "login":
            //        Login login = dataContext.GetLogin(id);

            //        if (login == null)
            //        {
            //            string errorMessage = "No Login with the requested Id could be found.";
            //            return NotFound(errorMessage);
            //        }

            //        // Alter Data Entry:
            //        dataEntry.UserId = "1"; //ToDo: Echten User
            //        dataEntry.Comment = dataEntryClientRequest.Comment;
            //        dataEntry.Favourite = dataEntryClientRequest.Favourite;
            //        dataEntry.Subject = dataEntryClientRequest.Subject;

            //        // Alter Login:
            //        login.Username = dataEntryClientRequest.Username;
            //        login.Password = dataEntryClientRequest.Password;
            //        login.Url = dataEntryClientRequest.Url;

            //        dataContext.SaveChanges();

            //        Login alteredLogin = dataContext.GetLogin(id);

            //        if (alteredLogin == null)
            //        {
            //            string errorMessage = "No Login with the requested Id could be found.";
            //            return NotFound(errorMessage);
            //        }

            //        return Ok(alteredLogin);

            //    case "paymentcard":
            //        PaymentCard paymentCard = dataContext.GetPaymentCard(id);

            //        if (paymentCard == null)
            //        {
            //            string errorMessage = "No Payment Card with the requested Id could be found.";
            //            return NotFound(errorMessage);
            //        }

            //        // Alter Data Entry:
            //        dataEntry.UserId = "1"; //ToDo: Echten User
            //        dataEntry.Comment = dataEntryClientRequest.Comment;
            //        dataEntry.Favourite = dataEntryClientRequest.Favourite;
            //        dataEntry.Subject = dataEntryClientRequest.Subject;

            //        // Alter PaymentCard:
            //        paymentCard.Owner = dataEntryClientRequest.Owner;
            //        paymentCard.Number = dataEntryClientRequest.CardNumber;
            //        paymentCard.ExpirationDate = dataEntryClientRequest.ExpirationDate;
            //        paymentCard.Pin = dataEntryClientRequest.Pin;
            //        paymentCard.Cvv = dataEntryClientRequest.Cvv;

            //        dataContext.SaveChanges();

            //        PaymentCard alteredPaymentCard = dataContext.GetPaymentCard(id);

            //        if (alteredPaymentCard == null)
            //        {
            //            string errorMessage = "No Payment Card with the requested Id could be found.";
            //            return NotFound(errorMessage);
            //        }

            //        return Ok(alteredPaymentCard);

            //    case "safenote":
            //        SafeNote safenote = dataContext.GetSafeNote(id);

            //        if (safenote == null)
            //        {
            //            string errorMessage = "No Safe Note with the requested Id could be found.";
            //            return NotFound(errorMessage);
            //        }

            //        // Alter Data Entry:
            //        dataEntry.UserId = "1"; //ToDo: Echten User
            //        dataEntry.Comment = dataEntryClientRequest.Comment;
            //        dataEntry.Favourite = dataEntryClientRequest.Favourite;
            //        dataEntry.Subject = dataEntryClientRequest.Subject;

            //        // Alter SafeNote:
            //        safenote.Note = dataEntryClientRequest.Note;

            //        dataContext.SaveChanges();

            //        SafeNote alteredSafeNote = dataContext.GetSafeNote(id);

            //        if (alteredSafeNote == null)
            //        {
            //            string errorMessage = "No Safe Note with the requested Id could be found.";
            //            return NotFound(errorMessage);
            //        }

            //        return Ok(alteredSafeNote);

            //    default:
            //        return BadRequest("Invalid request. Data Entry could not be updated.");

            //}
        }

        [HttpDelete, Authorize]
        [Route("{id:int}")]
        public async Task<ActionResult<DataEntry>> DeleteDataEntryById(int id)
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
    }
}
