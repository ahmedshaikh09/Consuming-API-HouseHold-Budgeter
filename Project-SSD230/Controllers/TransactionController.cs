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
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/transaction/getAll/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<Transaction>>(data);

            ViewBag.Result = result;

            return View();
        }

        public ActionResult CreateTransaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTransaction(int id, TransactionCreateEditViewModel formData)
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

            var url = $"http://localhost:56327/api/transaction/create/{id}";


            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Title", formData.Title));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("TransactionDate", formData.TransactionDate.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", formData.Amount.ToString()));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<Transaction>(data);

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
        public ActionResult UpdateTransaction(int? id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/transaction/get/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var result = JsonConvert.DeserializeObject<TransactionCreateEditViewModel>(data);

            return View(result);
        }

        [HttpPost]
        public ActionResult UpdateTransaction(int id, TransactionCreateEditViewModel formData)
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

            var url = $"http://localhost:56327/api/transaction/edit/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Title", formData.Title));
            parameters.Add(new KeyValuePair<string, string>("Description", formData.Description));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", formData.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("TransactionDate", formData.TransactionDate.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", formData.Amount.ToString()));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var houseHold = JsonConvert.DeserializeObject<Transaction>(data);

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

        public ActionResult DeleteTransaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteTransaction(int id)
        { 
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/transaction/delete/{id}";

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

        public ActionResult VoidTransaction()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VoidTransaction(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/transaction/void/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
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
    }
}