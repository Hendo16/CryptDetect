using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using CryptDetect.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Cors;

public static  class common // static 不是必须

{

    private static string name = "cc";
    public static string Name
    {
        get { return name;}
        set { name = value;}
    }

}

namespace CryptDetect.Controllers
{
    public class DetectorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FileScan(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var fPaths = new List<string>();
            var sess_ID = Guid.NewGuid().ToString();
            foreach (var file in files)
            {
                string ext = Path.GetExtension(file.FileName);
                if(file.Length > 0)
                {
                    string fPath = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ext;
                    fPaths.Add(fPath);

                    using(var stream = new FileStream(fPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            JObject results = await upload_files_testAsync(fPaths, sess_ID);

            //No Match Found
            if(results["code"].ToString() == "404")
            {
                return RedirectToAction("Error", "Couldn't find a match.");
            }

            //Wrong File Type
            if (results["code"].ToString() == "123")
            {
                return RedirectToAction("Error", "Wrong File Type Provied.");
            }

            string ID = Guid.NewGuid().ToString();
            
            TempData[ID] = results["data"].ToString();



            return RedirectToAction("Results", new { ID });
        }

        [HttpPost]
        public async Task<IActionResult> TextScan(string text)
        {
            string ID = Guid.NewGuid().ToString();
            string fPath = System.IO.Path.GetTempPath() + ID + ".txt";
            using (var stream = new StreamWriter(fPath))
            {
                await stream.WriteAsync(text);
            }
            JObject results = await upload_files_testAsync(new List<string> {fPath}, ID);

            //No Match Found
            if (results["code"].ToString() == "404")
            {
                return RedirectToAction("Error");
            }

            TempData[ID] = results["data"].ToString();

            return RedirectToAction("Results", new { ID });
        }

        [HttpPost]
        public async Task<IActionResult> GithubScan(string URL)
        {
            //Pull files from the repo
            string ID = Guid.NewGuid().ToString();

            JObject results = await submitGithub(URL, ID);

            //No Match Found
            if (results["code"].ToString() == "404")
            {
                return RedirectToAction("Error");
            }

            TempData[ID] = results["data"].ToString();

            return RedirectToAction("Results", new { ID });
        }

        [HttpGet]
        public IActionResult Results(string ID)
        {
            if (TempData.ContainsKey(ID))
            {
                string data = TempData[ID].ToString();
                List<DataModel> datalist = new List<DataModel>();
                dynamic stuff = JsonConvert.DeserializeObject(data);
                foreach (var item in stuff)
                {
                    DataModel d = new DataModel();
                    d.bType = item.bType;
                    d.chance = item.chance;
                    datalist.Add(d);
                }
                return View(datalist);
            }
            return View();
        }

        private async Task<JObject> upload_files_testAsync(List<string> files, string sess_ID)
        {
            var client = new HttpClient { 
            BaseAddress = new Uri("http://localhost:8001")
                };

            MultipartFormDataContent content_stream = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var stream = System.IO.File.OpenRead(file);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.Add("Content-Type", "application/octet-stream");
                content_stream.Add(fileContent, "userUpload", @"\"+filename);
                content_stream.Add(new StringContent(sess_ID), "sessionID");
            }

            var response = await client.PostAsync(new Uri("http://localhost:8001/file_upload"), content_stream);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(response.Content);
            return JObject.Parse(result);
        }

        private async Task<JObject> submitGithub(string URL, string ID)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8001")
            };

            MultipartFormDataContent content_stream = new MultipartFormDataContent();

            content_stream.Add(new StringContent(URL), "userUpload");
            content_stream.Add(new StringContent(ID), "sessionID");

            var response = await client.PostAsync(new Uri("http://localhost:8001/file_upload"), content_stream);
            var result = await response.Content.ReadAsStringAsync();
            Debug.WriteLine(response.Content);
            return JObject.Parse(result);

        }
        public IActionResult Error(string error)
        {
            return View(error);
        }
    }
}