using BusinessObjectLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOsLibrary
{
    public class TestAttempDTO
    {
        public string imageUrl { get; set; }
        public string questionContent { get; set; }
        public string chosenAnswerContent { get; set; }
        public string correctAnswerContent { get; set; }
        public bool isCorrect { get; set; }
    }
}
