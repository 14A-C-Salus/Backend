using Microsoft.AspNetCore.Mvc;
using Salus;

namespace Salus.WebAPI
{
    public static class ControllerBaseExtension
    {
        public static IActionResult Run(this ControllerBase controller, Func<IActionResult> action)
        {
            try
            {
                return action();
            }  
            catch (Exception ex)
            {
                try
                {
                    LogToFile(ex);
                }
                catch { }
#if DEBUG
                return controller.StatusCode(500, new
                {
                    error = ex.InnerException.GetType().Name,
                    stackTrace = ex.StackTrace
                });
#else
                return controller.StatusCode(500, new
                {
                    error = "Belső hiba"
                });

#endif
            }
        }

        private static string? path = null;
        private static object lockLogFile = new object();

        private static void LogToFile(Exception ex)
        {
            //source: https://code-maze.com/aspnetcore-read-appsettings-values-from-a-json-file/

            if (path == null)
            {
                var configuration = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json")
                                        .Build();

                path = configuration.GetValue<string>("Logfile:Path");
            }
            var fileName = Path.Combine(path, DateTime.Today.Year.ToString());
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            fileName = Path.Combine(fileName, DateTime.Today.Month.ToString());
            if (!Directory.Exists(fileName))
                Directory.CreateDirectory(fileName);
            fileName = Path.Combine(fileName, $"WebApiLog_{DateTime.Today:yyyy-MM-dd}.log");

            lock (lockLogFile)
            {
                using (var sw = new StreamWriter(fileName, true))
                {
                    sw.WriteLine($"{DateTime.Now:HH:mm:ss}\t{ex.Message}");
                    sw.WriteLine($"\t\t{ex.StackTrace?.Replace("\n", "\n\t\t")}");  
                }
            }

        }
    }
}
