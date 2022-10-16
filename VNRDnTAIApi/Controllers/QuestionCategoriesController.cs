using DataAccessLibrary.Business_Entity;
using DataAccessLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VNRDnTAIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]

    public class QuestionCategoriesController : ControllerBase
    {
        private readonly QuestionCategoryBusinessEntity _entity;

        public QuestionCategoriesController(IUnitOfWork work)
        {
            _entity = new QuestionCategoryBusinessEntity(work);
        }

    }
}
