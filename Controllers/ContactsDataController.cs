using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Red_Lake_Hospital_Redesign_Team6.Models;
using System.Diagnostics;

namespace Red_Lake_Hospital_Redesign_Team6.Controllers
{
    public class ContactsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ContactsData/5
        [ResponseType(typeof(IEnumerable<ContactDto>))]
        public IHttpActionResult GetContacts()
        {
            List<Contact> Contacts = db.Contacts.ToList();
            List<ContactDto> ContactDtos = new List<ContactDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Contact in Contacts)
            {
                ContactDto NewContact = new ContactDto
                {
                    contact_id = Contact.contact_id,
                    Firstname = Contact.Firstname,
                    Lastname = Contact.Lastname,
                    Email = Contact.Email,
                    Phone = Contact.Phone,
                    Message = Contact.Message,
                    DepartmentName = Contact.Department.DepartmentName
                };
                ContactDtos.Add(NewContact);
            }

            return Ok(ContactDtos);
        }

        [HttpGet]
        [ResponseType(typeof(ContactDto))]
        public IHttpActionResult FindContact(int id)
        {
            //Find the data
            Contact contact = db.Contacts.Find(id);
            //if not found, return 404 status code.
            if (contact == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            ContactDto ContactDtos = new ContactDto
            {
                contact_id = contact.contact_id,
                Firstname = contact.Firstname,
                Lastname = contact.Lastname,
                Email = contact.Email,
                Phone = contact.Phone,
                Message = contact.Message,
                DepartmentName = contact.Department.DepartmentName
            };


            //pass along data as 200 status code OK response
            return Ok(ContactDtos);
        }

        [HttpGet]
        [ResponseType(typeof(DepartmentsDto))]
        public IHttpActionResult FindDepartmentForContact(int id)
        {
            //Finds the first team which has any players
            //that match the input playerid
            DepartmentsModel departments = db.Departments
                .Where(d => d.Contact.Any(c => c.contact_id == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (departments == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            DepartmentsDto DepartmentsDtos = new DepartmentsDto
            {
                DepartmentId = departments.DepartmentId,
                DepartmentName = departments.DepartmentName
            };


            //pass along data as 200 status code OK response
            return Ok(DepartmentsDtos);
        }

        // PUT: api/ContactsData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateContact(int id, [FromBody] Contact contact)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contact.contact_id)
            {
                return BadRequest();
            }


            db.Entry(contact).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Contact))]
        [HttpPost]
        [Route("api/contactsdata/addcontact")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddContact([FromBody] Contact contact)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contacts.Add(contact);
            db.SaveChanges();

            return Ok(contact.contact_id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteContact(int id)
        {
            Contact contacts = db.Contacts.Find(id);
            if (contacts == null)
            {
                return NotFound();
            }

            db.Contacts.Remove(contacts);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContactExists(int id)
        {
            return db.Contacts.Count(e => e.contact_id == id) > 0;
        }
    }
}