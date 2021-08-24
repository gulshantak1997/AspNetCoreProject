using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.ViewModels
{
    public class QuestionsRelatedValueViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Required]
        public string Type { get; set; }

        public int QuestionId { get; set; }
        public string Values { get; set; }

        public List<SelectListItem> SelectList { get; set; }


    }

    //survay form model
    public class QuestionsRelatedsurvayViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }        
        public string Type { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public List<SelectListItem> SelectList { get; set; }

        public string TeamId { get; set; }
    }


}
