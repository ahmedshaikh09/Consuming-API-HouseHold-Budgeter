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
    public class AccountManagementController : Controller
    {
        // GET: AccountManagement
        public ActionResult Index(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/bankaccount/getAll/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<BankAccount>>(data);

            ViewBag.Result = result;

            return View();
        }

        public ActionResult CreateBankAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBankAccount(int id, BankAccountCreateEditViewModel formData)
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

            var url = $"http://localhost:56327/api/bankaccount/create/{id}";


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

                var result = JsonConvert.DeserializeObject<BankAccount>(data);

                return RedirectToAction("Index", "HouseHold");
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

            return RedirectToAction("Index", "HouseHold");
        }

        [HttpGet]
        public ActionResult UpdateBankAccount(int? id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/bankaccount/get/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var result = JsonConvert.DeserializeObject<BankAccountCreateEditViewModel>(data);

            return View(result);
        }

        [HttpPost]
        public ActionResult UpdateBankAccount(int id, BankAccountCreateEditViewModel formData)
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

            var url = $"http://localhost:56327/api/bankaccount/edit/{id}";

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
                var houseHold = JsonConvert.DeserializeObject<BankAccount>(data);

                return RedirectToAction("Index", "HouseHold");
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
            return RedirectToAction("Index", "HouseHold");
        }

        public ActionResult DeleteBankAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteBankAccount(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/bankaccount/delete/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //Success. Do appropriate action.
                return RedirectToAction("Index", "HouseHold");
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
            return RedirectToAction("Index", "HouseHold");
        }

        public ActionResult UpdateBalance(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/bankaccount/calculate/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var data = response.Content.ReadAsStringAsync().Result;

                ViewBag.Balance = data;

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
    }
}