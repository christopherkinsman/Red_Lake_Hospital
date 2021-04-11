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
    public class DonationController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static DonationController()
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
        
        // GET: Donation
        public ActionResult Index()
        {
            return View();
        }

        // GET: Donation/List
        public ActionResult List()
        {
            string url = "donationdata/getdonations";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<DonationDto> SelectedDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
                return View(SelectedDonations);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Donation/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
/*
        // GET: Donation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Donation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
*/

        // GET: Donation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Donation/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
/*
        // GET: Donation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Donation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }*/
    }
}
