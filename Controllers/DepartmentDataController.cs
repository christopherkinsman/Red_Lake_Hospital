using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Red_Lake_Hospital_Redesign_Team6.Models;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Red_Lake_Hospital_Redesign_Team6.Controllers
{
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List down all the departments in the database
        /// </summary>
        /// <returns>
        /// A list of Departments which includes information such as their name and Id
        /// </returns>
        /// <example>
        /// GET : api/DepartmentData/ListDepartments
        /// </example>

        [HttpGet]
        [ResponseType(typeof(IEnumerable<DepartmentsDto>))]
        public IHttpActionResult ListDepartments()
        {
            List<DepartmentsModel> Departments = db.Departments.ToList();

            List<DepartmentsDto> DepartmentDtos = new List<DepartmentsDto> { };


            foreach (var Department in Departments)
            {
                DepartmentsDto NewDepartment = new DepartmentsDto
                {
                    DepartmentId = Department.DepartmentId,
                    DepartmentName = Department.DepartmentName
                };
                DepartmentDtos.Add(NewDepartment);
            }

            return Ok(DepartmentDtos);
        }

        /// <summary>
        /// Finds a single department based on the department Id passed in.
        /// If the actor is not found returns a 404 error
        /// </summary>
        /// <param name="id">Id of the department to be found out</param>
        /// <returns>
        /// Information about the Department including details like id and name of the department.
        /// </returns>
        /// <example>
        /// GET: api/DepartmentData/FindDepartment/1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(DepartmentsDto))]
        public IHttpActionResult FindDepartment(int id)
        {

            DepartmentsModel Department = db.Departments.Find(id);
            if (Department == null)
            {
                return NotFound();
            }
            DepartmentsDto NewDepartment = new DepartmentsDto
            {
                DepartmentId = Department.DepartmentId,
                DepartmentName = Department.DepartmentName
            };
            return Ok(NewDepartment);
        }

        /// curl -H "Content-Type:application/json" -d @UpdatedDepartment.json "https://localhost:44349/api/DepartmentData/UpdateDepartment/1"
        /// <summary>
        /// Updates the details of an existing Department in the database with the new information provided.
        /// </summary>
        /// <param name="id">The id of the actor to be updated</param>
        /// <param name="Department">A Department Object, received as POST data</param>
        /// <returns>No Content</returns>
        /// <example>
        /// POST : api/DepartmentData/UpdateDepartment/2
        /// </example>

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateDepartment(int id, [FromBody] DepartmentsModel Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Department.DepartmentId)
            {
                return BadRequest();
            }

            db.Entry(Department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        /// curl -H "Content-Type:application/json" 
        /// -d @NewDepartment.json "https://localhost:44349/api/DepartmentData/AddDepartment"
        /// <summary>
        /// Adds a department to the Database
        /// </summary>
        /// <param name="Department">A department Object which has the information for the new department</param>
        /// <returns>
        ///  The id of the new department created
        /// </returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        /// FORM DATA: DepartmentModel JSON object
        /// </example>

        [HttpPost]
        [ResponseType(typeof(DepartmentsModel))]
        public IHttpActionResult AddDepartment([FromBody] DepartmentsModel Department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Departments.Add(Department);
            db.SaveChanges();

            return Ok(Department.DepartmentId);

        }

        /// curl -d "" "https://localhost:44349/api/DepartmentData/DeleteDepartment/2"
        /// <summary>
        /// Deletes a Department from the database
        /// </summary>
        /// <param name="id">Id of the Department to be deleted</param>
        /// <returns>
        /// returns the Department Object containing the details of the department deleted
        /// </returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/id
        /// </example>

        [HttpPost]
        [ResponseType(typeof(DepartmentsModel))]
        public IHttpActionResult DeleteDepartment(int id)
        {
            DepartmentsModel Department = db.Departments.Find(id);

            if (Department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(Department);
            db.SaveChanges();

            return Ok(Department);

        }

        /// <summary>
        ///  Finds the testimonials of a particular department
        /// </summary>
        /// <param name="id">Id of the department</param>
        /// <returns>
        ///     A list of Testimonial DTO's which contains the testimonials of the department.
        /// </returns>
        /// <example>
        ///     GET: api/DepartmentData/FindTestimonialsForDepartment/1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(IEnumerable<TestimonialDto>))]
        public IHttpActionResult FindTestimonialsForDepartment(int id)
        {
            List<Testimonial> testimonials = db.Testimonials
                .Where(t => t.Departments.Any(d => d.DepartmentId == id))
                .ToList();

            List<TestimonialDto> testimonialDtos = new List<TestimonialDto> { };

            foreach (var Testimonial in testimonials)
            {
                TestimonialDto NewTestimonial = new TestimonialDto
                {
                    testimonial_Id = Testimonial.testimonial_Id,
                    testimonial_Content = Testimonial.testimonial_Content,
                    first_Name = Testimonial.first_Name,
                    last_Name = Testimonial.last_Name,
                    email = Testimonial.email,
                    phone_Number = Testimonial.phone_Number,
                    Has_Pic = Testimonial.Has_Pic,
                    Pic_Extension = Testimonial.Pic_Extension,
                    posted_Date = Testimonial.posted_Date,
                    Approved = Testimonial.Approved
                };
                testimonialDtos.Add(NewTestimonial);
            }
            return Ok(testimonialDtos);
        }

        /// <summary>
        ///     Checks if the Department is present in the database.
        /// </summary>
        /// <param name="id">Id of the department</param>
        /// <returns>
        ///     Boolean 1, if the department is present in the database
        ///     Boolean 0, if the department is not present in the database.
        /// </returns>

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentId == id) > 0;
        }


    }
}
