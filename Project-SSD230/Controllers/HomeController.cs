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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterDataViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = "http://localhost:56327//api/account/register";

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();

            parameters
                .Add(new KeyValuePair<string, string>("email", formData.Email));
            parameters
                .Add(new KeyValuePair<string, string>("password", formData.Password));
            parameters
                .Add(new KeyValuePair<string, string>("confirmpassword", formData.ConfirmPassword));

            var encodedParameters = new
                FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Login");
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

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDataViewModel formData)
         {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = "http://localhost:56327/token";

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", formData.UserName));
            parameters.Add(new KeyValuePair<string, string>("password", formData.Password));
            parameters.Add(new KeyValuePair<string, string>("grant_type", formData.GrantType));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<LoginData>(data);

                var cookie = new HttpCookie("SavedCookie",
                    result.AccessToken);

                Response.Cookies.Add(cookie);

                return View("Index");
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

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var cookie = Request.Cookies["SavedCookie"];

            if (cookie == null)
            {
                return RedirectToAction("Login");
            }

            var token = cookie.Value;

            var url = "http://localhost:56327/api/Account/ChangePassword";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",
            $"Bearer {token}");

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("OldPassword", formData.OldPassword));
            parameters.Add(new KeyValuePair<string, string>("NewPassword", formData.NewPassword));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));

            var encodedParameters = new
               FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
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

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = "http://localhost:56327/api/Account/ForgotPassword";

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));

            var encodedParameters = new
               FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("ResetPassword");
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

            return RedirectToAction("ChangePassword");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var url = "http://localhost:56327/api/Account/ResetPassword";

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", formData.Email));
            parameters.Add(new KeyValuePair<string, string>("Password", formData.Password));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", formData.ConfirmPassword));
            parameters.Add(new KeyValuePair<string, string>("Code", formData.Code));

            var encodedParameters = new
               FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
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

            return RedirectToAction("ResetPassword");
        }
    }
}