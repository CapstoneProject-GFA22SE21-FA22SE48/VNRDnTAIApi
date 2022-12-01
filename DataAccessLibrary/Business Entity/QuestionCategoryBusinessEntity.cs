using BusinessObjectLibrary;
using BusinessObjectLibrary.Predefined_constants;
using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class QuestionCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public QuestionCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }

        public async Task<IEnumerable<QuestionCategoryDTO>> GetQuestionCategoriesByTestCategoryId(Guid testCategoryId)
        {
            List<QuestionCategoryDTO> res = new List<QuestionCategoryDTO>();
            await work.Questions.GetAllAsync();
            List<QuestionCategory> qcs = (await work.QuestionCategories.GetAllAsync()).Where(q => !q.IsDeleted && q.TestCategoryId == testCategoryId).ToList();
            foreach (var qc in qcs)
            {
                QuestionCategoryDTO dto = new QuestionCategoryDTO();
                dto.Id = qc.Id;
                dto.Name = qc.Name;
                dto.TestCategoryId = qc.TestCategoryId;
                dto.NoOfQuestion = qc.Questions.Where(q => !q.IsDeleted && q.Status == (int)Status.Active).ToList().Count;
                res.Add(dto);
            }
            return res;
        }
    }
}
