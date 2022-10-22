using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary
{
    public class QuestionCategoryDTO
    {
        public Guid Id { get; set; }
        public Guid TestCategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int NoOfQuestion { get; set; }
    }
}
