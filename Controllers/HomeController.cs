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
            using (var activity = ActivitySource.StartActivity("Checkout Process"))
            {
                activity?.SetTag("product.name", productName);
                activity?.SetTag("order.amount", amount);
                activity?.SetTag("user.id", "user789"); // Simulated user id
            }

            return Content($"Checkout Complete! Product: {productName}, Amount: {amount:C}");
        }
    }
}
