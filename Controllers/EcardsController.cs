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

namespace Red_Lake_Hospital_Redesign_Team6.Controllers
{
    public class EcardsController : Controller
    {

        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static EcardsController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44349/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // GET: Ecards
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            string url = "ecardsdata/getecards";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<EcardsDto> SelectedEcards = response.Content.ReadAsAsync<IEnumerable<EcardsDto>>().Result;
                return View(SelectedEcards);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Details(int id)
        {

            ShowEcard ViewModel = new ShowEcard();

            //Pass along to the view information about who is logged in
            ViewModel.isadmin = User.IsInRole("Admin");



            string url = "ecardsdata/findecard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into contact data transfer object
                EcardsDto SelectedEcard = response.Content.ReadAsAsync<EcardsDto>().Result;
                ViewModel.ecard = SelectedEcard;


                url = "ecardsdata/finddepartmentforecard/" + id;
                response = client.GetAsync(url).Result;
                DepartmentsDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentsDto>().Result;
                ViewModel.departments = SelectedDepartment;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // POST: Ecards/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Ecards EcardInfo)
        {
            //pass along authentication credential in http request
            GetApplicationCookie();

            //Debug.WriteLine(EcardInfo.EcardMessage);
            //Debug.WriteLine(jss.Serialize(ContactInfo));
            string url = "ecardsdata/addecard";
            HttpContent content = new StringContent(jss.Serialize(EcardInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                int ecardid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = ecardid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Ecards/Edit/5
        [HttpGet]
        public ActionResult Edit(int id, FormCollection collection)
        {
            string url = "ecardsdata/details";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {

                EcardsDto SelectedEcard = response.Content.ReadAsAsync<EcardsDto>().Result;

                return View(SelectedEcard);

            }
            else
            {
                return View("Error");
            }
        }

        // GET: Player/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ecardsdata/findecard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into contact data transfer object
                EcardsDto SelectedEcard = response.Content.ReadAsAsync<EcardsDto>().Result;
                return View(SelectedEcard);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // POST: Ecard/Delete/5
        [HttpGet]
        //[ValidateAntiForgeryToken()]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "ecardsdata/deleteecard/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
