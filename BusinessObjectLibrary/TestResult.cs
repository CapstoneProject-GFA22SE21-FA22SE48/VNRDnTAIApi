using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class TestResult
    {
        public TestResult()
        {
            TestResultDetails = new HashSet<TestResultDetail>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TestCategoryId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TestCategory TestCategory { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<TestResultDetail> TestResultDetails { get; set; }
    }
}
