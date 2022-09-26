using BusinessObjectLibrary;
using DataAccessLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly vnrdntaiContext context;
        public IGenericRepository<Answer> Answers { get; }

        public IGenericRepository<Column> Columns { get; }

        public IGenericRepository<Decree> Decrees { get; }

        public IGenericRepository<Gpssign> Gpssigns { get; }

        public IGenericRepository<Keyword> Keywords { get; }

        public IGenericRepository<Paragraph> Paragraphs { get; }

        public IGenericRepository<ParagraphModificationRequest> ParagraphModificationRequests { get; }

        public IGenericRepository<Question> Questions { get; }
        public IGenericRepository<QuestionModificationRequest> QuestionModificationRequests { get; }

        public IGenericRepository<Reference> References { get; }

        public IGenericRepository<Section> Sections { get; }

        public IGenericRepository<Sign> Signs { get; }

        public IGenericRepository<SignCategory> SignCategories { get; }

        public IGenericRepository<SignModificationRequest> SignModificationRequests { get; }

        public IGenericRepository<SignParagraph> SignParagraphs { get; }

        public IGenericRepository<Statue> Statues { get; }

        public IGenericRepository<TestCategory> TestCategories { get; }

        public IGenericRepository<User> Users { get; }
        public IGenericRepository<UserModificationRequest> UserModificationRequests { get; }


        public IGenericRepository<VehicleCategory> VehicleCategories { get; }

        public UnitOfWork(vnrdntaiContext context, 
            IGenericRepository<Answer> answers, 
            IGenericRepository<Column> columns, 
            IGenericRepository<Decree> decrees, 
            IGenericRepository<Gpssign> gpssigns, 
            IGenericRepository<Keyword> keywords, 
            IGenericRepository<Paragraph> paragraphs, 
            IGenericRepository<ParagraphModificationRequest> paragraphModificationRequests, 
            IGenericRepository<Question> questions,
            IGenericRepository<QuestionModificationRequest> questionModificationRequests,
            IGenericRepository<Reference> references, 
            IGenericRepository<Section> sections, 
            IGenericRepository<Sign> signs, 
            IGenericRepository<SignCategory> signCategories, 
            IGenericRepository<SignModificationRequest> signModificationRequests,
            IGenericRepository<SignParagraph> signParagraphs, 
            IGenericRepository<Statue> statues, 
            IGenericRepository<TestCategory> testCategories, 
            IGenericRepository<User> users,
            IGenericRepository<UserModificationRequest> userModificationRequests,
            IGenericRepository<VehicleCategory> vehicleCategories)
        {
            this.context = context;
            Answers = answers;
            Columns = columns;
            Decrees = decrees;
            Gpssigns = gpssigns;
            Keywords = keywords;
            Paragraphs = paragraphs;
            ParagraphModificationRequests = paragraphModificationRequests;
            Questions = questions;
            QuestionModificationRequests = questionModificationRequests;
            References = references;
            Sections = sections;
            Signs = signs;
            SignCategories = signCategories;
            SignModificationRequests = signModificationRequests;
            SignParagraphs = signParagraphs;
            Statues = statues;
            TestCategories = testCategories;
            Users = users;
            UserModificationRequests = userModificationRequests;
            VehicleCategories = vehicleCategories;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }
    }
}
