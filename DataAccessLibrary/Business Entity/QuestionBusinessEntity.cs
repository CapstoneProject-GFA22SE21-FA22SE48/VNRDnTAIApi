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
    public class QuestionBusinessEntity
    {
        private IUnitOfWork work;
        public QuestionBusinessEntity(IUnitOfWork work)
        {
            this.work = work;
        }
        public async Task<IEnumerable<QuestionDTO>> GetAssigedQuestionsAsync(Guid scribeId)
        {
            //return (await work.Questions.GetAllAsync(nameof(Question.Answers)))
            //    .Where(question => !question.IsDeleted && question.Status == (int)Status.Active)
            //    .OrderBy(u => int.Parse(u.Name.Split(" ")[1]));
            IEnumerable<QuestionDTO> questionDTOs = from assignedQuestionCategory in (await work.AssignedQuestionCategories.GetAllAsync())
                                                    .Where(aqc => !aqc.IsDeleted && aqc.ScribeId == scribeId)
                                                    join questionCategory in (await work.QuestionCategories.GetAllAsync())
                                                    .Where(qc => !qc.IsDeleted)
                                                    on assignedQuestionCategory.QuestionCategoryId equals questionCategory.Id
                                                    join question in (await work.Questions.GetAllAsync())
                                                    .Where(question => !question.IsDeleted && question.Status == (int)Status.Active)
                                                    on questionCategory.Id equals question.QuestionCategoryId
                                                    join testCategory in (await work.TestCategories.GetAllAsync())
                                                    .Where(tc => !tc.IsDeleted)
                                                    on question.TestCategoryId equals testCategory.Id

                                                    select new QuestionDTO
                                                    {
                                                        Id = question.Id,
                                                        Name = question.Name,
                                                        Content = question.Content,
                                                        ImageUrl = question.ImageUrl,
                                                        TestCategoryId = question.TestCategoryId,
                                                        TestCategoryName = testCategory.Name,
                                                        QuestionCategoryId = question.QuestionCategoryId,
                                                        QuestionCategoryName = questionCategory.Name
                                                    };
            return questionDTOs.OrderBy(u => int.Parse(u.Name.Split(" ")[1]));
        }

        //public async Task<IEnumerable<Question> GetAllStudySets(string testCatId) //questioncate
        //{
        //    var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
        //        .Where(question => !question.IsDeleted
        //                && question.Status == (int)Status.Active
        //                && question.TestCategoryId.ToString()
        //                .Equals(testCatId.ToString()))
        //        .OrderBy(u => int.Parse(u.Name.Split(" ")[1]));
        //    return res;
        //}

        public async Task<IEnumerable<Question>> GetStudySetByCategoryAndSeparator(string testCatId, int separator)
        {
            var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted
                        && question.Status == (int)Status.Active
                        && question.TestCategoryId.ToString().Equals(testCatId.ToString())).OrderBy(u => int.Parse(u.Name.Split(" ")[1])).Skip(25 * separator).Take(25);
            return res;
        }

        //***
        public async Task<IEnumerable<Question>> GetRandomTestSetByCategory(string testCatId)
        {
            var res = (await work.Questions.GetAllAsync(nameof(Question.Answers)))
                .Where(question => !question.IsDeleted
                        && question.Status == (int)Status.Active
                        && question.TestCategoryId.ToString().Equals(testCatId.ToString())).OrderBy(r => Guid.NewGuid()).Take(25);
            return res;
        }
        public async Task<Question> GetQuestionAsync(Guid id)
        {
            return (await work.Questions.GetAllAsync())
                .Where(question => !question.IsDeleted
                        && question.Status == (int)Status.Active
                        && question.Id.Equals(id))
                .FirstOrDefault();
        }
        public async Task<Question> AddQuestion(Question question)
        {
            question.Id = Guid.NewGuid();
            question.IsDeleted = false;
            await work.Questions.AddAsync(question);
            await work.Save();
            return question;
        }

        public async Task<Question> UpdateQuestion(Question question)
        {
            work.Questions.Update(question);
            await work.Save();
            return question;
        }
        public async Task RemoveQuestion(Guid id)
        {
            Question question = await work.Questions.GetAsync(id);
            question.IsDeleted = true;
            work.Questions.Update(question);
            await work.Save();
        }

        //----------------------------------------------------
        public async Task<Question> AddQuestionForROM(Question question)
        {
            //Add new deactivated question
            question.TestCategory = null;
            question.Id = Guid.NewGuid();


            //If the question is for Delete ROM, then keep IsDeleted = true
            if (question.IsDeleted == true) { }
            else
            {
                question.IsDeleted = false;
            }
            question.Status = (int)Status.Deactivated;

            //Add new question answers
            foreach (Answer answer in question.Answers)
            {
                answer.Id = Guid.NewGuid();
                answer.QuestionId = question.Id;
                //If the answer is for Delete ROM, then keep IsDeleted = true
                if (answer.IsDeleted == true) { }
                else
                {
                    answer.IsDeleted = false;
                }
            }

            await work.Questions.AddAsync(question);
            await work.Save();
            return question;
        }
    }
}
