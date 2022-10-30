using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary
{
    public class TestCategoryCountDTO
    {
        public Guid TestCategoryId { get; set; }
        public string TestCategoryName { get; set; }
        public int QuestionsCount { get; set; }
    }
}
