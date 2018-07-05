using Microsoft.AspNetCore.Mvc;

namespace CoffeeApplication.Extensions.Alerts
{
    public static class AlertExtensions
    {
        public static IActionResult WithSuccess(this IActionResult result, string title)
        {
            return Alert(result, "success", title);
        }

        public static IActionResult WithWarning(this IActionResult result, string title)
        {
            return Alert(result, "warning", title);
        }

        private static IActionResult Alert(IActionResult result, string type, string title)
        {
            return new AlertDecoratorResult(result, type, title);
        }
    }
}
