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

            //var r = new Receipt()
            //{
            //    Operator = "1",
            //    OperatorPassword = "1",
            //    Items = new Item[] {
            //        new Item() { Text = "LIS iLab", Amount = 1, UnitPrice = 0.01m, Quantity = 1, TaxGroup = TaxGroup.TaxGroup1}
            //    },
            //    Payments = new Payment[] {
            //        new Payment() { Amount = 0.01m, PaymentType = PaymentType.Cash }
            //    }
            //};
            //var c = new Client() { DeviceId = "dt937256" };
            //var x = await c.PrintFiscalReceiptAsync(r);
            //Console.WriteLine($"OK: {x.Ok}, Receipt number: {x.ReceiptNumber}");

            //var rfnd = new ReversalReceipt()
            //{
            //    Operator = "1",
            //    OperatorPassword = "1",
            //    FiscalMemorySerialNumber = x.FiscalMemorySerialNumber,
            //    ReceiptDateTime = x.ReceiptDateTime,
            //    Reason = ReversalReason.Refund,
            //    ReceiptNumber = x.ReceiptNumber,
            //    Items = new Item[] {
            //        new Item() { Text = "LIS iLab", Amount = 1, UnitPrice = 0.01m, Quantity = 1, TaxGroup = TaxGroup.TaxGroup1}
            //    },
            //    Payments = new Payment[] {
            //        new Payment() { Amount = 0.01m, PaymentType = PaymentType.Cash }
            //    }
            //};
            //var y = await c.PrintRefundReceiptAsync(rfnd);
            //Console.WriteLine($"OK: {y.Ok}, Receipt number: {y.ReceiptNumber}");

            //var c = new Client() { DeviceId = "abc" };
            //var x = await c.PrintXReport();
            //Console.WriteLine($"OK: {x.Ok}");

            //var c = new Client() { DeviceId = "abc" };
            //var x = await c.PrintZReport();
            //Console.WriteLine($"OK: {x.Ok}");

            var c = new Client() { DeviceId = "dt937256" };
            var res = await c.SendRawRequestAsync(new RawRequest() { Request = "P800\t200\t" });
            res = await c.SendRawRequestAsync(new RawRequest() { Request = "P1200\t200\t" });
            res = await c.SendRawRequestAsync(new RawRequest() { Request = "P1800\t200\t" });
            Console.WriteLine($"OK: {res.Ok}");
        }

    }
}