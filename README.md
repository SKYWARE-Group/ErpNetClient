# <img src="https://github.com/SKYWARE-Group/ErpNetClient/blob/master/ErpNetClient/Assets/erp-net-fs-client.png" width="48" height="48"/> ErpNetClient

This project is a client library for interaction with [ErpNet.FP fiscal server](https://github.com/erpnet/ErpNet.FP). It encapsulates bith data model and http invokations.

## Getting started

```c#
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
var c = new Client() { DeviceId = "abc" };
var x = await c.PrintFiscalReceipt(r);
Console.WriteLine($"OK: {x.Ok}, Receipt number: {x.ReceiptNumber}");
```
