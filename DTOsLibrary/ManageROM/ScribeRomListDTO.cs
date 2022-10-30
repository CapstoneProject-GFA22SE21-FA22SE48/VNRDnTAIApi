using System.Collections.Generic;

namespace DTOsLibrary.ManageROM
{
    public class ScribeRomListDTO
    {
        public List<LawRomDTO> LawRoms { get; set; }
        public List<SignRomDTO> SignRoms { get; set; }
        public List<QuestionRomDTO> QuestionRoms { get; set; }
    }
}
