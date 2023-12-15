using CryptoApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CryptoApp.Controllers
{
    public class EncryptedDataConntroller : Controller
    {
        private readonly ILogger<EncryptedDataConntroller> _logger;

        public EncryptedDataConntroller(ILogger<EncryptedDataConntroller> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new EncryptedData { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
