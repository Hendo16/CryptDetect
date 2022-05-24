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
            JObject results = await upload_files_testAsync(fPaths);

            //No Match Found
            if(results["code"].ToString() == "404")
            {
                return RedirectToAction("Error", "no match");
            }

            //Wrong File Type
            if (results["code"].ToString() == "123")
            {
                return RedirectToAction("Error", "wrong type");
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
            JObject results = await upload_files_testAsync(new List<string> {fPath});



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

        public async Task<JObject> upload_files_testAsync(List<string> files)
        {
            var client = new HttpClient { 
            BaseAddress = new Uri("http://localhost:8001")
                };

            MultipartFormDataContent content_stream = new MultipartFormDataContent();

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                var stream = System.IO.File.OpenRead(file);
                content_stream.Add(new StreamContent(stream), "userUpload", @"\"+filename);
                content_stream.Add(new StringContent("89078908"), "sessionID");
            }

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