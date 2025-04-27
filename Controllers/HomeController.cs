using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace ECommerceDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Tracer _tracer;

        public HomeController(Tracer tracer)
        {
            _tracer = tracer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            using (var span = _tracer.StartActiveSpan("CheckoutOperation", SpanKind.Server))
            {
                span.SetAttribute("ecommerce.checkout.product", "Laptop");
                span.SetAttribute("ecommerce.checkout.price", 1500);
                span.SetAttribute("ecommerce.checkout.currency", "USD");

                System.Threading.Thread.Sleep(500);

                span.End();
            }

            return View();
        }
    }
}
