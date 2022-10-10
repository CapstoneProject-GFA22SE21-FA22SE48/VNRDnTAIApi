using System;
using System.Collections.Generic;

#nullable disable

namespace BusinessObjectLibrary
{
    public partial class Column
    {
        public Column()
        {
            Statues = new HashSet<Statue>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DecreeId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Decree Decree { get; set; }
        public virtual ICollection<Statue> Statues { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
