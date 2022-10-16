using DataAccessLibrary.Interfaces;

namespace DataAccessLibrary.Business_Entity
{
    public class QuestionCategoryBusinessEntity
    {
        private IUnitOfWork work;
        public QuestionCategoryBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
    }
}
