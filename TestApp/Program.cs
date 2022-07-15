using Skyware.ErpNetFS;
using Skyware.ErpNetFS.Model;
using System.Text.Json;

namespace TestApp
{
    internal class Program
    {

        static async Task Main(string[] args)
        {
            Console.WriteLine("ErpNet.FS Client Test Application");


            //var c = new Client();
            //var x = await c.GetPrinters();
            //Console.WriteLine($"First: {x.FirstOrDefault().Key} {x.FirstOrDefault().Value?.SerialNumber}");

            //var c = new Client() { DeviceId = "abc" };
            //var x = await c.GetPrinterStatus();
            //Console.WriteLine($"abc: {x.Ok}");

            var r = new Receipt()
            {
                Operator = "1",
                OperatorPassword = "1",
                Items = new Item[] {
                    new Item() { Text = ".Net library", Amount = 1, UnitPrice = 1, Quantity = 1, TaxGroup = TaxGroup.TaxGroup1}
                },
                Payments = new Payment[] {
                    new Payment() { Amount = 1, PaymentType = PaymentType.Cash }
                }
            };
            var c = new Client() { DeviceId = "dt577460" };
            var x = await c.PrintFiscalReceiptAsync(r);
            Console.WriteLine($"OK: {x.Ok}, Receipt number: {x.ReceiptNumber}");

            //var c = new Client() { DeviceId = "abc" };
            //var x = await c.PrintXReport();
            //Console.WriteLine($"OK: {x.Ok}");

            //var c = new Client() { DeviceId = "abc" };
            //var x = await c.PrintZReport();
            //Console.WriteLine($"OK: {x.Ok}");

        }

    }
}