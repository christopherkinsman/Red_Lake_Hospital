using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Red_Lake_Hospital_Redesign_Team6.Models.ViewModels
{
    public class DepartmentDetails
    {
        public DepartmentsDto DepartmentDto { get; set; }
        public IEnumerable<TestimonialDto> Testimonials { get; set; }
    }
}