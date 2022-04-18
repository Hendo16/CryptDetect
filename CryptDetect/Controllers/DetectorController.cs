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
            foreach (var file in files)
            {
                if(file.Length > 0)
                {
                    var fPath = Path.GetTempFileName();
                    fPaths.Add(fPath);

                    using(var stream = new FileStream(fPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            JObject results = await upload_files_testAsync(fPaths);

            //No Match Found
            if(results["code"].ToString() == "404")
            {
                return RedirectToAction("Error");
            }

            string ID = Guid.NewGuid().ToString();
            
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

        public async Task<JObject> upload_files_testAsync(List<string> files)
        {
            //NEED TO UPDATE FOR MULTIPLE FILES, ONLY WORKS FOR A SINGLE FILE RIGHT NOW
            //THIS IS ONLY FOR A SINGLE FILE, NEED TO FIGURE OUT HOW TO APPEND MULTIPLE FILES TO THE BODY
            string filename = files[0];
            var client = new HttpClient { 
                BaseAddress = new Uri("http://127.0.0.1:8001")
                };
            await using var stream = System.IO.File.OpenRead(filename);
            using var content = new MultipartFormDataContent
            {
                {new StreamContent(stream), "files", "\test.py" }
            };

            var response = await client.PostAsync(new Uri("http://127.0.0.1:8001/file_upload"),content);
            var result = await response.Content.ReadAsStringAsync();
            return JObject.Parse(result);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}