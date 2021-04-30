using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Red_Lake_Hospital_Redesign_Team6.Models.ViewModels
{
    public class UpdateFeedback
    {
        public FeedbackDto feedback { get; set; }
        public IEnumerable<DepartmentsDto> departments { get; set; }
    }
}