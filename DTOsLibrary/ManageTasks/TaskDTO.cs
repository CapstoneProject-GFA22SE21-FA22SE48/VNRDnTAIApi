using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary.ManageTasks
{
    public class TaskDTO
    {
        public Guid TaskId { get; set; }
        public string TaskName { get; set; }
        public Guid? ScribeId { get; set; }
        public string ScribeName { get; set; }
        public bool IsAssigned { get; set; }

    }
}
