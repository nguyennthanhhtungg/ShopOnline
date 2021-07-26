using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnlineAPI.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ShopOnlineAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error()
        {
            try
            {
                throw new CustomException(HttpStatusCode.BadRequest, "An error occured...!");
                //throw new Exception("An error occured...!");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
