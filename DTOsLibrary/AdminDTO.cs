using System;

namespace DTOsLibrary
{
    public class AdminDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }

        public string PendingRequests { get; set; }
    }
}
