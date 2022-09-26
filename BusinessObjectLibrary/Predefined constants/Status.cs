using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLibrary.Predefined_constants
{
    public enum Status
    {
        Pending = 0,
        Unclaimed = 1,
        Claimed = 2,
        Confirmed = 3,
        Denied = 4,
        Active = 5,
        Deactivated = 6
    }
}
