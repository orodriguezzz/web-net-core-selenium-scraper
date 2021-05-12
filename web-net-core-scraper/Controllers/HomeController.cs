using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using web_net_core_scraper.Models;

namespace web_net_core_scraper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection collection)
        {
            string url = collection["input-url-target"].ToString();
            string wrapper = collection["input-object-wrapper"].ToString();
            string detail = collection["input-object-detail"].ToString();

            List<string> result = new List<string>();

            try
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--remote-debugging-port=9222");
                options.AddArgument("--ignore-certificate-errors");
                options.BinaryLocation = Environment.GetEnvironmentVariable("GOOGLE_CHROME_SHIM");

                var chromeDriverPath = "";
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (env == "Development")
                    chromeDriverPath = Directory.GetCurrentDirectory();
                else
                    chromeDriverPath = "/app/.chromedriver/bin/";

                IWebDriver driver = new ChromeDriver(options);

                driver.Navigate().GoToUrl(url);

                var divs = driver.FindElements(By.CssSelector(wrapper)).ToList();
                foreach (var div in divs)
                {
                    var h1Text = div.FindElement(By.CssSelector(detail)).Text;

                    result.Add(h1Text);
                }
            }
            catch (Exception ex)
            {
                
            }

            ViewBag.result = result;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
