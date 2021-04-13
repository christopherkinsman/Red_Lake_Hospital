using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Red_Lake_Hospital_Redesign_Team6.Models;
using Red_Lake_Hospital_Redesign_Team6.Models.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace Red_Lake_Hospital_Redesign_Team6.Controllers
{
    public class TestimonialController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static TestimonialController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44349//api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        ///  Renders the view for creating a new Testimonial
        /// </summary>
        /// <returns>Returns a view for creating a new Testimonial</returns>
        /// <example>
        /// GET : Testimonial/Create/2
        /// </example>

        [HttpGet]
        public ActionResult Create(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                ViewBag.Id = id;
                return View();
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Creates a new Testimonial in the database
        /// </summary>
        /// <param name="TestimonialInfo">Form data which has the details for the new Testimonial to be entered into the database</param>
        /// <returns>
        ///     If successfull , A new Testimonial would be entered into the database.
        ///     If not , then an error would be thrown 
        /// </returns>
        /// <example>
        /// POST: Testimonial/Create/1
        /// </example>

        [HttpPost]
        [Route("Testimonial/Create/{DepartmentId}")]
        public ActionResult Create(int id, Testimonial TestimonialInfo)
        {
            string url = "TestimonialData/AddTestimonial/" + id;
            HttpContent content = new StringContent(jss.Serialize(TestimonialInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                int Id = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("DetailsofDepartment", "Department", new { Id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Renders the view to update a testimonial
        /// </summary>
        /// <param name="TestimonialId">Id of the testimonial to be updated</param>
        /// <param name="DepartmentId">Id of the department of which the testimonial is a part of.</param>
        /// <returns>
        ///     If successfull , returns a ViewModel which contains details about the testimonial and the department 
        ///     of which the testimonial is a part of.
        ///     If not successfull , returns an error.
        /// </returns>
        /// <example>
        ///     GET: Testimonial/UpdateTestimonial/1/2
        /// </example>

        [HttpGet]
        public ActionResult UpdateTestimonial(int TestimonialId, int DepartmentId)
        {
            TestimonialDetails Testimonial_Details = new TestimonialDetails();

            string url = "TestimonialData/FindTestimonial/" + TestimonialId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                TestimonialDto testimonial = response.Content.ReadAsAsync<TestimonialDto>().Result;
                Testimonial_Details.TestimonialDto = testimonial;

                url = "DepartmentData/FindDepartment/" + DepartmentId;
                response = client.GetAsync(url).Result;
                DepartmentsDto department = response.Content.ReadAsAsync<DepartmentsDto>().Result;
                Testimonial_Details.DepartmentDto = department;

                return View(Testimonial_Details);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Updates the information of a testimonial in the database.
        /// </summary>
        /// <param name="TestimonialDetails">View Model object which contains information of the testimonial 
        /// and the department</param>
        /// <returns>
        ///     If connection to the api is successfull, updates the contents of the existing testimonial and the control is 
        ///     redirected to the Action "DetailsofDepartment" which renders the view to display the details of the department
        ///     If not successfull, then an error message is thrown. 
        /// </returns>
        /// <example>
        ///     POST: Testimonial/UpdateTestimonial/{Form Data}
        /// </example>

        [HttpPost]
        public ActionResult UpdateTestimonial(TestimonialDetails TestimonialDetails)
        {
            string url = "TestimonialData/UpdateTestimonial/" + TestimonialDetails.DepartmentDto.DepartmentId + "/" + TestimonialDetails.TestimonialDto.testimonial_Id;
            HttpContent content = new StringContent(jss.Serialize(TestimonialDetails.TestimonialDto));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DetailsofDepartment", "Department", new { Id = TestimonialDetails.DepartmentDto.DepartmentId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Asks the user to confirm before deleting the Testimonial from the database permanently. Shows the information about the testimonial   
        /// </summary>
        /// <param name="DepartmentId">Id of the Department to be deleted.</param>
        /// <param name="TestimonialId">Id of the Testimonial to be deleted.</param>
        /// <returns>
        ///     If successfull, renders the details of the Testimonial to be deleted to the corresponding View.
        ///     If not successfull, then returns an error.
        /// </returns>
        /// <example>
        ///     GET: Testimonial/DeleteConfirm/5/2
        /// </example>

        [HttpGet]
        public ActionResult DeleteConfirm(int DepartmentId, int TestimonialId)
        {
            TestimonialDetails Testimonial_Details = new TestimonialDetails();
            string url = "TestimonialData/FindTestimonial/" + TestimonialId;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                TestimonialDto Testimonial = response.Content.ReadAsAsync<TestimonialDto>().Result;
                Testimonial_Details.TestimonialDto = Testimonial;

                url = "DepartmentData/FindDepartment/" + DepartmentId;
                response = client.GetAsync(url).Result;

                DepartmentsDto Department = response.Content.ReadAsAsync<DepartmentsDto>().Result;
                Testimonial_Details.DepartmentDto = Department;

                return View(Testimonial_Details);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        ///     Deletes the testimonial from the database
        /// </summary>
        /// <param name="TestimonialId">Id of the Testimonial to be deleted.</param>
        /// <param name="DepartmentId">Id of the Department to be deleted.</param>
        /// <returns>
        ///     If successfull, deletes the testimonial from the database  and the control is redirected to the 
        ///     Action "DetailsofDepartment" which renders the view to display the details of the department
        /// </returns>
        /// <example>
        ///     POST: Testimonial/DeleteTestimonial/5/2
        /// </example>

        [HttpPost]
        [Route("Testimonial/DeleteTestimonial/{TestimonialId}/{DepartmentId}")]
        public ActionResult DeleteTestimonial(int TestimonialId, int DepartmentId)
        {
            string url = "TestimonialData/DeleteTestimonial/" + TestimonialId;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("DetailsofDepartment", "Department", new { Id = DepartmentId });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Renders the view to display an error 
        /// </summary>
        /// <returns>
        /// </returns>

        public ActionResult Error()
        {
            return View();
        }

    }
}