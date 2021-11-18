using System;
using System.Linq;
using System.Reflection;
using Fixer.IO.CcyCnvrtr.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fixer.IO.CcyCnvrtr.ConsoleApp
{
    public class Program
    {
         static void Main(string[] args)
        {

            var builder = new HostBuilder()
          .ConfigureServices((hostingContext, services) =>
          {
              services.AddHostedService<ScheduleProgramTask>();
          })
          .ConfigureLogging((hostingContext, logging) =>
          {
              logging.AddConfiguration(hostingContext.Configuration);
              logging.AddConsole();
              logging.AddDebug();
          });

            builder.UseConsoleLifetime();
            builder.Start();
            bool endApp = false;
            //using (var host = CreateHostBuilder(args).Build())
            //{
            //    await host.StartAsync();
            //    var lifetime = host.Services.GetRequiredService<IHostApplicationLifetime>();

            //    // do work here / get your work service ...

            //    lifetime.StopApplication();
            //    await host.WaitForShutdownAsync();
            //}

            // Display title as the C# console Fixer.io Calculator app.
            Console.WriteLine("Fixer.io - Rate Calculator - C#\r--------------------------------\n");

            while (!endApp)
            {
                Console.WriteLine("\nPlease select the fuction you want to use:");
                Console.WriteLine("Press 1: To use simple conversion between 2 curriencies");
                Console.WriteLine("Press 2: To use simple conversion between 2 curriencies for specific date");
                Console.WriteLine("Press 3: To exit");
                Console.WriteLine("Input value");
                var inputAsString = Console.ReadKey();

                int val;
                while (!int.TryParse(inputAsString.KeyChar.ToString(), out val))
                {
                    Console.WriteLine(": It is not a valid selection!");
                    inputAsString = Console.ReadKey();
                }
                if (val == 1)
                    SimpleCurrencyConverter();
                else if (val == 2)
                    SimpleCurrencyConverterWithDate();
                else if (val == 3)
                    endApp = true;
                else
                    Console.WriteLine(": Not a valid selection!" + Environment.NewLine + "--------------------------------\n");
            }
            Console.WriteLine("\nGood Bye!");
        }

        private static async void SimpleCurrencyConverterWithDate()
        {
            var serviceProvider = GetServiceProvider();
            var fixerManager = serviceProvider.GetService<IFixerManager>();
            try
            {
                Type t = typeof(Fixer.IO.CcyCnvrtr.Models.Symbols);
                FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo fi in fields)
                {
                    Console.Write(fi.Name + " ");
                    //Console.Write(fi.GetValue(null).ToString());
                }
                // Declare variables and set to empty.
                string ccyInput1 = "";
                string ccyInput2 = "";
                double amount = 0;
                DateTime dt = DateTime.Now;

                // Ask the user to type the first Currency.ß
                Console.WriteLine("\nType 1st (From) currency, and then press Enter: ");

                do
                {
                    char c = Console.ReadKey().KeyChar;
                    if (c == '\n')
                        break;

                    ccyInput1 += c;
                } while (ccyInput1.Length < 3);

                Console.WriteLine("\nType 2nd currency, and then press Enter: ");

                do
                {
                    char c = Console.ReadKey().KeyChar;
                    if (c == '\n')
                        break;

                    ccyInput2 += c;
                } while (ccyInput2.Length < 3);

                Console.WriteLine("\nInput amount you want to convert from ");
                var amountAsString = Console.ReadLine();


                while (!double.TryParse(amountAsString, out amount))
                {
                    Console.WriteLine("This is not a number!");
                    amountAsString = Console.ReadLine();
                }
                Console.WriteLine("\nType date (dd/mm/yy), and then press Enter:\n(Default date is todays date if you want to go with default date\nthen click enter without entring any date)");
                amountAsString = Console.ReadLine();
                
                bool inputValid = true;
                if (string.IsNullOrEmpty(amountAsString))
                    inputValid = false;

                while (inputValid)
                {
                    dt = new DateTime();
                    while (!DateTime.TryParse(amountAsString, out dt))
                    {
                        Console.WriteLine("\nThis is not a date (dd/mm/yy)!");
                        amountAsString = Console.ReadLine();
                    }
                    if (dt > DateTime.Now || dt < Convert.ToDateTime("12/12/12"))
                    {
                        Console.Write("\nInvalid Date. Please enter present or past date (dd/mm/yy)");
                    }
                    else
                    {
                        Console.WriteLine(dt);
                        inputValid = false;
                    }
                }

                Console.WriteLine(fixerManager.Convert(ccyInput1, ccyInput2, amount, dt));
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {argumentException.Message}");
            }
            catch (System.Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine($"Unexpected error:  { exception.Message }");
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("Unexpected error!");
            }

        }

        private static void SimpleCurrencyConverter()
        {
            var serviceProvider = GetServiceProvider();
            var fixerManager = serviceProvider.GetService<IFixerManager>();
            try
            {
                Console.WriteLine();
                Type t = typeof(Fixer.IO.CcyCnvrtr.Models.Symbols);
                FieldInfo[] fields = t.GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo fi in fields)
                {
                    Console.Write(fi.Name + " ");
                    //Console.Write(fi.GetValue(null).ToString());
                }
                // Declare variables and set to empty.
                string ccyInput1 = "";
                string ccyInput2 = "";
                double amount = 0;

                // Ask the user to type the first Currency.
                Console.WriteLine("\nType 1st (From) currency, and then press Enter: ");

                do
                {
                    char c = Console.ReadKey().KeyChar;
                    if (c == '\n')
                        break;

                    ccyInput1 += c;
                } while (ccyInput1.Length < 3);

                Console.WriteLine("\nType 2nd currency, and then press Enter: ");

                do
                {
                    char c = Console.ReadKey().KeyChar;
                    if (c == '\n')
                        break;

                    ccyInput2 += c;
                } while (ccyInput2.Length < 3);

                Console.WriteLine("\nInput amount you want to convert from ");
                var amountAsString = Console.ReadLine();


                while (!double.TryParse(amountAsString, out amount))
                {
                    Console.WriteLine("\nThis is not a number!");
                    amountAsString = Console.ReadLine();
                }

                Console.WriteLine(fixerManager.Convert(ccyInput1, ccyInput2, amount));
            }
            catch (ArgumentException argumentException)
            {
                Console.WriteLine();
                Console.WriteLine($"Error: {argumentException.Message}");
            }
            catch (System.Exception exception)
            {
                Console.WriteLine();
                Console.WriteLine($"Unexpected error:  { exception.Message }");
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("Unexpected error!");
            }
        }

        public static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            //
            // Register the services here
            //
            services.UseFixer(new FixerConfig
            {
                BaseUri = "http://data.fixer.io/api/",
                APIKey = "a4793baae7800001f62bd763956be086"
            });

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
          .ConfigureServices((hostContext, services) =>
          {
              var serviceProvider = GetServiceProvider();
              var fixerManager = serviceProvider.GetService<IFixerManager>();
              // remove the hosted service
               services.AddHostedService<ScheduleProgramTask>();

              // register your services here.
          });
    }
}
