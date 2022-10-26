using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary.SearchSign
{
    public class SignCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<SearchSignDTO> SearchSignDTOs { get; set; }
    }
}
