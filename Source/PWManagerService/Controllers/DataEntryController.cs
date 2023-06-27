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

        [HttpGet, Authorize]
        [Route("all")]
        public async Task<ActionResult<List<object>>> GetAllDataEntries()
        {
            try
            {
                string token = TokenService.ReadToken(Request.Headers);
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
            string token = TokenService.ReadToken(Request.Headers);

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
            string token = TokenService.ReadToken(Request.Headers);
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
        }

        [HttpDelete, Authorize]
        [Route("{id:int}")]
        public async Task<ActionResult<int>> DeleteDataEntryById(int id)
        {
            string token = TokenService.ReadToken(Request.Headers);
            try
            {
                return StatusCode(await factory.DeleteDataEntry(token, id));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
