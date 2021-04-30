using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Red_Lake_Hospital_Redesign_Team6.Models.ViewModels
{
    public class UpdateFeedbackViewModel
    {
        public FeedbackDto feedback { get; set; }
        public IEnumerable<DepartmentsDto> departments { get; set; }

        public int DepartmentId { get; set; }


    }
}