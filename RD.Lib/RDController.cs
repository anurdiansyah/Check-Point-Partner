using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using RD.Lib.Common;
using RD.Lib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RD.Lib
{
    public class RDController : Controller
    {
        public HttpClient httpClient;

        public RDController()
        {
            string _hashKey = string.Empty;
            string keyPath = AppDomain.CurrentDomain.BaseDirectory;
            if (System.IO.File.Exists(Path.Combine(keyPath, "rdlic.lic")))
            {
                _hashKey = RDFile.ReadStringFromTxtFile(keyPath);
            }
        }

        public static string ReferenceNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        public static Guid NewGuid()
        {
            Guid g;
            g = Guid.NewGuid();

            return g;
        }

        public string GetSysParamValue(string p_sCode)
        {
            string sResults = string.Empty;

            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(1);
            if (HttpContext.Request.Cookies["KueSysParam"] == null)
            {
                RDCustomResponse respSysParam = JsonConvert.DeserializeObject<RDCustomResponse>(GetAsync("SysParam/SysParamList", httpClient, new Dictionary<string, string>()));
                Response.Cookies.Append("KueSysParam", respSysParam.JsonData, option);
                List<SysParam> sysParams = JsonConvert.DeserializeObject<List<SysParam>>(respSysParam.JsonData);

                sResults = sysParams.Where(o => o.Code == p_sCode).FirstOrDefault().Value;
            }
            else
            {
                List<SysParam> sysParams = JsonConvert.DeserializeObject<List<SysParam>>(HttpContext.Request.Cookies["KueSysParam"]);
                sResults = sysParams.Where(o => o.Code == p_sCode).FirstOrDefault().Value;
            }

            return sResults;
        }

        public static string BuildUri(string p_sBasePath, Dictionary<string, string> p_dictParams)
        {
            try
            {
                string p_sQuery = string.Empty;
                var query = HttpUtility.ParseQueryString(p_sQuery);
                if (p_dictParams != null && p_dictParams.Count > 0)
                {
                    foreach (KeyValuePair<string, string> oParams in p_dictParams)
                    {
                        query[oParams.Key] = oParams.Value;
                    }
                    return p_sBasePath + "?" + query.ToString();
                }
                else
                {
                    return p_sBasePath;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string GetAsync(string p_sUri, HttpClient httpClient, Dictionary<string, string> p_dictParams)
        {
            string sUri = BuildUri(p_sUri, p_dictParams);

            HttpResponseMessage response = httpClient.GetAsync(sUri).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string PutAsync(string p_sUri, HttpClient httpClient, object obj, string[] p_sArray)
        {
            string sUri = BuildUri(p_sUri, null);

            var requestContent = new MultipartFormDataContent();

            string jsonObject = new JavaScriptSerializer().Serialize(obj);
            var jsonContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            string jsonStringArray = new JavaScriptSerializer().Serialize(p_sArray);
            var jsonArray = new StringContent(jsonStringArray, Encoding.UTF8, "application/json");

            requestContent.Add(jsonContent, RDConstanta.HttpContentType.JsonObject);
            requestContent.Add(jsonArray, RDConstanta.HttpContentType.JsonStringArray);

            HttpResponseMessage response = httpClient.PutAsync(sUri, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string PutAsync(string p_sUri, HttpClient httpClient, object obj, List<IFormFile> iFormFiles)
        {
            string sUri = BuildUri(p_sUri, null);

            var requestContent = new MultipartFormDataContent();

            foreach (IFormFile iFormFile in iFormFiles)
            {
                if (iFormFile != null && iFormFile.Length > 0)
                {
                    using (var br = new BinaryReader(iFormFile.OpenReadStream()))
                    {
                        byte[] data = br.ReadBytes((int)iFormFile.OpenReadStream().Length);
                        ByteArrayContent bytes = new ByteArrayContent(data);
                        bytes.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        requestContent.Add(bytes, iFormFile.Name, iFormFile.FileName);
                    }
                }
            }

            string jsonObject = new JavaScriptSerializer().Serialize(obj);
            var jsonContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            requestContent.Add(jsonContent, "JsonObject");

            HttpResponseMessage response = httpClient.PutAsync(sUri, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string PostAsync(string p_sUri, HttpClient httpClient, object obj)
        {
            string sUri = BuildUri(p_sUri, null);

            var requestContent = new MultipartFormDataContent();

            string jsonObject = new JavaScriptSerializer().Serialize(obj);
            var jsonContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            requestContent.Add(jsonContent, RDConstanta.HttpContentType.JsonObject);

            HttpResponseMessage response = httpClient.PostAsync(sUri, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string PostAsync(string p_sUri, HttpClient httpClient, object obj, string[] p_sArray)
        {
            string sUri = BuildUri(p_sUri, null);

            var requestContent = new MultipartFormDataContent();

            string jsonObject = new JavaScriptSerializer().Serialize(obj);
            var jsonContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            string jsonStringArray = new JavaScriptSerializer().Serialize(p_sArray);
            var jsonArray = new StringContent(jsonStringArray, Encoding.UTF8, "application/json");

            requestContent.Add(jsonContent, RDConstanta.HttpContentType.JsonObject);
            requestContent.Add(jsonArray, RDConstanta.HttpContentType.JsonStringArray);

            HttpResponseMessage response = httpClient.PostAsync(sUri, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string PostAsync(string p_sUri, HttpClient httpClient, object obj, List<IFormFile> iFormFiles)
        {
            string sUri = BuildUri(p_sUri, null);

            var requestContent = new MultipartFormDataContent();

            foreach (IFormFile iFormFile in iFormFiles)
            {
                if (iFormFile != null && iFormFile.Length > 0)
                {
                    using (var br = new BinaryReader(iFormFile.OpenReadStream()))
                    {
                        byte[] data = br.ReadBytes((int)iFormFile.OpenReadStream().Length);
                        ByteArrayContent bytes = new ByteArrayContent(data);
                        bytes.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        requestContent.Add(bytes, iFormFile.Name, iFormFile.FileName);
                    }
                }
            }

            string jsonObject = new JavaScriptSerializer().Serialize(obj);
            var jsonContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            requestContent.Add(jsonContent, "JsonObject");

            HttpResponseMessage response = httpClient.PostAsync(sUri, requestContent).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string DeleteAsync(string p_sUri, HttpClient httpClient, Dictionary<string, string> p_dictParams)
        {
            string sUri = BuildUri(p_sUri, p_dictParams);

            HttpResponseMessage response = httpClient.DeleteAsync(sUri).Result;
            if (response.IsSuccessStatusCode)
            {
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }

            return string.Empty;
        }

        public static string JsonFromResponse(HttpResponseMessage p_Response)
        {
            var readTask = p_Response.Content.ReadAsStringAsync();
            readTask.Wait();
            return readTask.Result;
        }

        public Guid CurrentUserId(HttpContext httpContext)
        {
            Guid id = Guid.Parse(httpContext.User.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            return id;
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
            {
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            }
            else
            {
                option.Expires = DateTime.Now.AddYears(1);
            }

            Response.Cookies.Append(key, value, option);
        }

        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }

        public void ClearCookie()
        {
            // clear all cookies
            foreach (var cookieKey in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookieKey);
            }
        }

    }
}
