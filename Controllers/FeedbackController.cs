using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Red_Lake_Hospital_Redesign_Team6.Models;
using Red_Lake_Hospital_Redesign_Team6.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Net;

namespace Red_Lake_Hospital_Redesign_Team6.Controllers
{
    public class FeedbackController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static FeedbackController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler)
            {
                //change this to match your own local port number
                BaseAddress = new Uri("https://localhost:44349/api/")
            };
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Routes to a dynamically generated "Feedback List" Page. Gathers information from the database.
        /// </summary>
        /// <returns>A dynamic "Feedback List" webpage which provides a list of all blogs in the database.</returns>
        /// <example>GET : /Feedback/List</example>
        // GET: Blog/List
        [Authorize(Roles = "admin")]
        public ActionResult List()
        {
            string url = "feedbackapi/getfeedback";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<FeedbackDto> SelectedFeedback = response.Content.ReadAsAsync<IEnumerable<FeedbackDto>>().Result;
                return View(SelectedFeedback);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Feedback Show" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Feedback</param>
        /// <returns>A dynamic "Feedback Show" webpage which provides detailes for a selected blog.</returns>
        /// <example>GET : /Feedback/Show/5</example>
        [Authorize(Roles = "admin")]
        public ActionResult Show(int id)
        {
            ShowFeedbackViewModel ViewModel = new ShowFeedbackViewModel();
            string url = "feedbackapi/findfeedback/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                FeedbackDto SelectedFeedback = response.Content.ReadAsAsync<FeedbackDto>().Result;
                ViewModel.Feedback = SelectedFeedback;
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Feedback Create" Page. Adds information from the database.
        /// </summary>
        /// <returns>A dynamic "Feedback Create" webpage which can add a blog entry to the database.</returns>
        /// <example>GET : /Feedback/Create</example>
        public ActionResult Create()
        {
            UpdateFeedbackViewModel ViewModel = new UpdateFeedbackViewModel();
            string url = "departmentsdata/getdepartments";

            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentsDto> PotentialDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentsDto>>().Result;
            ViewModel.departments = PotentialDepartments;

            return View(ViewModel);
        }

        public ActionResult CreateConfirm()
        {
            return View();
        }

        /// <summary>
        /// Routes to a dynamically generated "Feedback Create" Page. Adds information from the database.
        /// </summary>
        /// <returns>A dynamic "Feedback Create" webpage which can add a blog entry to the database.</returns>
        /// <example>GET : /Feedback/Create</example>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(UpdateFeedbackViewModel FeedbackViewInfo)
        {

            FeedbackModel FeedbackInfo = new FeedbackModel
            {
                feedback_id = FeedbackViewInfo.feedback.feedback_id,
                date = DateTime.Now,
                fname = FeedbackViewInfo.feedback.fname,
                lname = FeedbackViewInfo.feedback.lname,
                email = FeedbackViewInfo.feedback.email,
                title = FeedbackViewInfo.feedback.title,
                text = FeedbackViewInfo.feedback.text,
                DepartmentId = FeedbackViewInfo.DepartmentId
            };

            string url = "feedbackapi/addfeedback";
            HttpContent content = new StringContent(jss.Serialize(FeedbackInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("CreateConfirm");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }



        /// <summary>
        /// Routes to a dynamically generated "Feedback Delete" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Feedback</param>
        /// <returns>A dynamic "Feedback Show" webpage which provides information on a blog that can be deleted.</returns>
        /// <example>GET : /Feedback/Delete/5</example>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "feedbackapi/findfeedback/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                FeedbackModel SelectedModule = response.Content.ReadAsAsync<FeedbackModel>().Result;
                return View(SelectedModule);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// Routes to a dynamically generated "Feedback Delete" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Feedback</param>
        /// <returns>A dynamic "Feedback Show" webpage which provides information on a blog that can be deleted.</returns>
        /// <example>GET : /Feedback/Delete/5</example>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            string url = "feedbackapi/deletefeedback/" + id;
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}