# <img src="https://github.com/SKYWARE-Group/ErpNetClient/blob/master/ErpNetClient/Assets/erp-net-fs-client.png" width="48" height="48"/> ErpNetClient

This project is a client library for interaction with [ErpNet.FP fiscal server](https://github.com/erpnet/ErpNet.FP). It encapsulates both data model and http invocations.

[![Nuget Badge](https://img.shields.io/nuget/v/Skyware.ErpNetFS.Client)](https://www.nuget.org/packages/Skyware.ErpNetFS.Client)
[![.NET](https://github.com/SKYWARE-Group/ErpNetClient/actions/workflows/dotnet.yml/badge.svg)](https://github.com/SKYWARE-Group/ErpNetClient/actions/workflows/dotnet.yml)

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
var x = await c.PrintFiscalReceiptAsync(r);
Console.WriteLine($"OK: {x.Ok}, Receipt number: {x.ReceiptNumber}");
```

## CLI usage of the server

Even this command has no relation to the current project, it explains how it works

```bash
curl -i -X POST -H "Content-Type:application/json" -d "{\"uniqueSaleNumber\": \"DT279013-0001-0000052\", \"operator\": \"1\", \"operatorPassword\": \"1\", \"items\": [{\"text\": \"test\", \"quantity\": 1, \"unitPrice\": 1, \"taxGroup\": 1 }], \"payments\": [{\"amount\": 1, \"paymentType\": \"cash\" } ]}" http://localhost:8001/printers/dt797821/receipt/
```
