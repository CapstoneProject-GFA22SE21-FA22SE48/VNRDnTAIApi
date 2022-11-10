using BusinessObjectLibrary;
using System.Collections.Generic;

namespace DTOsLibrary.ManageROM
{
    public class AdminRomListDTO
    {
        public List<LawRomDTO> LawRoms { get; set; }
        public List<SignRomDTO> SignRoms { get; set; }
        public List<QuestionRomDTO> QuestionRoms { get; set; }
        public List<UserRomDTO> UserRoms { get; set; }
        public List<SignModificationRequest> GPSSignRoms { get; set; }

    }
}
