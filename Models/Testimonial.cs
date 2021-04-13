using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Red_Lake_Hospital_Redesign_Team6.Models
{
    public class Testimonial
    {
        [Key]
        public int testimonial_Id { get; set; }
        public string testimonial_Content { get; set; }
        public string first_Name { get; set; }
        public string last_Name { get; set; }
        public string email { get; set; }
        public int phone_Number { get; set; }
        public bool Has_Pic { get; set; }
        public string Pic_Extension { get; set; }
        public DateTime posted_Date { get; set; }
        public bool Approved { get; set; }
        public ICollection<DepartmentsModel> Departments { get; set; }

    }
    public class TestimonialDto
    {
        public int testimonial_Id { get; set; }
        public string testimonial_Content { get; set; }
        public string first_Name { get; set; }
        public string last_Name { get; set; }
        public string email { get; set; }
        public int phone_Number { get; set; }
        public bool Has_Pic { get; set; }
        public string Pic_Extension { get; set; }
        public DateTime posted_Date { get; set; }
        public bool Approved { get; set; }
    }
}