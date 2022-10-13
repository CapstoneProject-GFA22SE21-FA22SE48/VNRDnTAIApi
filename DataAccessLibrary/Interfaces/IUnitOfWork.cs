using BusinessObjectLibrary;
using System;
using System.Threading.Tasks;

namespace DataAccessLibrary.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Repository
        IGenericRepository<Answer> Answers { get; }
        IGenericRepository<AssignedColumn> AssignedColumns { get; }
        IGenericRepository<Column> Columns { get; }
        IGenericRepository<Comment> Comments { get; }
        IGenericRepository<Decree> Decrees { get; }
        IGenericRepository<Gpssign> Gpssigns { get; }
        IGenericRepository<Keyword> Keywords { get; }
        IGenericRepository<KeywordParagraph> KeywordParagraphs { get; }
        IGenericRepository<Paragraph> Paragraphs { get; }
        IGenericRepository<LawModificationRequest> LawModificationRequests { get; }
        IGenericRepository<Question> Questions { get; }
        IGenericRepository<QuestionModificationRequest> QuestionModificationRequests { get; }
        IGenericRepository<Reference> References { get; }
        IGenericRepository<Section> Sections { get; }
        IGenericRepository<Sign> Signs { get; }
        IGenericRepository<SignCategory> SignCategories { get; }
        IGenericRepository<SignModificationRequest> SignModificationRequests { get; }
        IGenericRepository<SignParagraph> SignParagraphs { get; }
        IGenericRepository<Statue> Statues { get; }
        IGenericRepository<TestCategory> TestCategories { get; }
        IGenericRepository<TestResult> TestResults { get; }
        IGenericRepository<TestResultDetail> TestResultDetails { get; }

        IGenericRepository<User> Users { get; }
        IGenericRepository<UserModificationRequest> UserModificationRequests { get; }
        IGenericRepository<VehicleCategory> VehicleCategories { get; }
        #endregion

        /// <summary>
        /// Save changes to database
        /// </summary>
        /// <returns></returns>
        Task<int> Save();
    }
}
