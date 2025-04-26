using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ActivitySource ActivitySource = new("ECommerceDemoApp");

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(string productName, decimal amount)
        {
            Console.WriteLine($"DEBUG: Starting checkout for {productName}"); // Debug log 1

            using var activity = ActivitySource.StartActivity("Checkout.Process");
            if (activity == null)
            {
                Console.WriteLine("ERROR: OpenTelemetry activity was NOT created!"); // Debug log 2
            }
            else
            {
                Console.WriteLine($"DEBUG: TraceId={activity.TraceId}"); // Debug log 3
            }

            return Content($"Checkout Complete! Product: {productName}, Amount: {amount:C}");
        }
    }
}
