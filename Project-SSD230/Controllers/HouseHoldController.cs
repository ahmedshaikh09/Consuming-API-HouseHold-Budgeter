using Newtonsoft.Json;
using Project_SSD230.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Project_SSD230.Controllers
{
    public class HouseHoldController : Controller
    {
        // GET: HouseHold

        public ActionResult Index()
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = "http://localhost:56327/api/houseHold/get-all";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<HouseHold>>(data);

            ViewBag.Result = result;

            return View();
        }

        public ActionResult CreateHouseHold()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateHouseHold(HouseHoldCreateEditViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = "http://localhost:56327/api/houseHold/create";


            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<HouseHold>(data);

                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                ViewBag.Result = result;
                ViewBag.Errors = result.ModelState.Values.ToList();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult UpdateHouseHold(int? id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/houseHold/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var result = JsonConvert.DeserializeObject<List<HouseHoldCreateEditViewModel>>(data);

            return View();
        }

        [HttpPost]
        public ActionResult UpdateHouseHold(int id, HouseHoldCreateEditViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/houseHold/edit/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("Name", formData.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var houseHold = JsonConvert.DeserializeObject<HouseHold>(data);

                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                ViewBag.Result = result;
                ViewBag.Errors = result.ModelState.Values.ToList();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult GetMembers(int? id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/houseHold/{id}/get-all-members";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<List<Members>>(data);

                ViewBag.Result = result;

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                ViewBag.Result = result;
                ViewBag.Errors = result.ModelState.Values.ToList();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }

            return View();

        }

        public ActionResult InviteMembers()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InviteMembers(int id, InviteMembersViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/houseHold/{id}/invite";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("UserEmail", formData.UserEmail));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);
 

                ViewBag.Result = result;
                ViewBag.Errors =  result.ModelState.Values.ToList();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }

            return View();
        }

        public ActionResult JoinHouseHold()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinHouseHold(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/houseHold/{id}/join";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                ViewBag.Result = result;
                ViewBag.Errors = result.ModelState.Values.ToList();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }

            return View();
        }

        public ActionResult LeaveHouseHold()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LeaveHouseHold(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/houseHold/{id}/leave";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Success. Do appropriate action.
                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<APIErrorData>(data);

                ViewBag.Result = result;
                ViewBag.Errors = result.ModelState.Values.ToList();

                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }
            return View();
        }
    }
}