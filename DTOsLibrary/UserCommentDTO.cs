using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary
{
    public class UserCommentDTO
    {
        public Guid UserId { get; set; }
        public string Avatar { get; set; }
        public string DisplayName { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
