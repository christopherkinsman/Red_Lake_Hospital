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
    public class EcardsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EcardsData/5
        [HttpGet]
        [Route("api/ecardsdata/getecards")]
        [ResponseType(typeof(IEnumerable<EcardsDto>))]
        public IHttpActionResult GetEcards()
        {
            List<Ecards> Ecards = db.Ecards.ToList();
            List<EcardsDto> EcardsDtos = new List<EcardsDto> { };

            foreach (var Ecard in Ecards)
            {
                EcardsDto NewEcard = new EcardsDto
                {
                    ecard_id = Ecard.ecard_id,
                    photo_path = Ecard.photo_path,
                    message = Ecard.message,
                    DepartmentName = Ecard.Department.DepartmentName
                };
                EcardsDtos.Add(NewEcard);
            }

            return Ok(EcardsDtos);
        }

        [HttpGet]
        [ResponseType(typeof(EcardsDto))]
        public IHttpActionResult FindContact(int id)
        {
            //Find the data
            Ecards ecard = db.Ecards.Find(id);
            //if not found, return 404 status code.
            if (ecard == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            EcardsDto EcardsDtos = new EcardsDto
            {
                ecard_id= ecard.ecard_id,
                photo_path = ecard.photo_path,
                message = ecard.message,
                DepartmentName = ecard.Department.DepartmentName
            };


            //pass along data as 200 status code OK response
            return Ok(EcardsDtos);
        }

        [HttpGet]
        [ResponseType(typeof(DepartmentsDto))]
        public IHttpActionResult FindDepartmentForEcard(int id)
        {
            DepartmentsModel departments = db.Departments
                .Where(d => d.Ecards.Any(e => e.ecard_id == id))
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

        // POST: api/EcardsData
        [HttpPost]
        [Route("api/ecardsdata/updateecards")]
        public IHttpActionResult UpdateEcard(int id, [FromBody] Ecards ecards)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ecards.ecard_id)
            {
                return BadRequest();
            }


            db.Entry(ecards).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EcardsExists(id))
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

        [ResponseType(typeof(Ecards))]
        [HttpPost]
        [Route("api/ecardsdata/addecard")]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddEcard([FromBody] Ecards ecard)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ecards.Add(ecard);
            db.SaveChanges();

            return Ok(ecard.ecard_id);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteEcard(int id)
        {
            Ecards ecards = db.Ecards.Find(id);
            if (ecards == null)
            {
                return NotFound();
            }

            db.Ecards.Remove(ecards);
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

        private bool EcardsExists(int id)
        {
            return db.Ecards.Count(e => e.ecard_id == id) > 0;
        }
    }
}