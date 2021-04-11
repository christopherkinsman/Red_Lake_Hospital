using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Red_Lake_Hospital_Redesign_Team6.Models
{
    public class Contact
    {
        [Key]
        public int contact_id { get; set; }

        public string contact_fname { get; set; }

        public string contact_lname { get; set; }

        public string contact_email { get; set; }

        public string contact_phone { get; set; }
        
        public string contact_message { get; set; }


        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        public virtual DepartmentsModel Department { get; set; }

        //Establish a 1-to-many relationship with departments table
        public ICollection<DepartmentsModel> Departments { get; set; }
    }

    public class ContactDto
    {
        public int contact_id { get; set; }

        public string contact_fname { get; set; }

        public string contact_lname { get; set; }

        public string contact_email { get; set; }

        public string contact_phone { get; set; }

        public string contact_message { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}