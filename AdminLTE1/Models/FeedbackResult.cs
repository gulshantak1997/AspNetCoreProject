using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{

    public class FeedbackResultViewmodel
    {
        public int Id { get; set; }
        public string TeamId { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public string OptionId { get; set; }
        public string Type { get; set; }
    }

    public class FeedbackResult
    {
        public int Id { get; set; }
        public string TeamId{ get; set; }
        public int QuestionId{ get; set; }
        public string Answer{ get; set; }
        public string OptionId { get; set; }
    }
}

