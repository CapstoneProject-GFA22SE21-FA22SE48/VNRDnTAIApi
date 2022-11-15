using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MailsController : ControllerBase
    {
        private readonly MailBusinessEntity _entity;

        public MailsController(IConfiguration configuration)
        {
            _entity = new MailBusinessEntity(configuration);
        }

        //POST: api/Mails
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult SendRetrainEmail([FromBody] string body)
        {
            try
            {
               _entity.SendRetrainEmail(body);
               return StatusCode(200);
            } catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
