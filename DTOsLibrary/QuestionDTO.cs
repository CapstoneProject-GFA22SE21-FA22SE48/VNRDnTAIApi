using System;

namespace DTOsLibrary
{
    public class QuestionDTO
    {
        public Guid Id { get; set; }
        public Guid TestCategoryId { get; set; }
        public string TestCategoryName { get; set; }
        public Guid QuestionCategoryId { get; set; }
        public string QuestionCategoryName { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}
