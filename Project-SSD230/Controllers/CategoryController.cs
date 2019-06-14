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
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/category/getAll/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;

            var data = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<List<Category>>(data);

            ViewBag.Result = result;

            return View();
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(int id ,  CategoryCreateEditViewModel formData)
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

            var url = $"http://localhost:56327/api/category/create/{id}";


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

                var result = JsonConvert.DeserializeObject<Category>(data);

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

            return RedirectToAction("Index" , "HouseHold");
        }

        [HttpGet]
        public ActionResult UpdateCategory(int? id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home"); 
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/category/get/{id}";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var result = JsonConvert.DeserializeObject<CategoryCreateEditViewModel>(data);

            return View(result);
        }

        [HttpPost]
        public ActionResult UpdateCategory(int id, CategoryCreateEditViewModel formData)
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

            var url = $"http://localhost:56327/api/category/edit/{id}";

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
                var houseHold = JsonConvert.DeserializeObject<Category>(data);

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

        public ActionResult DeleteCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var token = cookie.Value;

            var url = $"http://localhost:56327/api/category/delete/{id}";

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

                return RedirectToAction(nameof(Index));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }
            return RedirectToAction("Index", "HouseHold");
        }
    }
}