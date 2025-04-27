using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace ECommerceDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly Tracer _tracer;

        public HomeController(TracerProvider tracerProvider)
        {
            _tracer = tracerProvider.GetTracer("ECommerceDemoAppTracer");
        }

        public IActionResult Index()
        {
            // This is a normal page
            return View();
        }

        public IActionResult Checkout()
        {
            // Example: adding custom span
            using (var span = _tracer.StartActiveSpan("CheckoutOperation", SpanKind.Server))
            {
                span.SetAttribute("ecommerce.checkout.product", "Laptop");
                span.SetAttribute("ecommerce.checkout.price", 1500);
                span.SetAttribute("ecommerce.checkout.currency", "USD");

                // Simulate work
                System.Threading.Thread.Sleep(500);

                span.End();
            }

            return View();
        }
    }
}
