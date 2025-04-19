using Refit;
using Skyware.ErpNetFS.Model;
using Spectre.Console;

namespace ErpNetModelDemo;

// Ignore Spelling: xreport zreport darkorange

public class ApiWorker
{
    private static Receipt? lastKnownReceipt = null;
    private static DeviceStatusWithReceiptInfo? lastKnownReceiptStatus = null;

    public static bool useRefit = true;

    private static readonly IFiscalPrinterClient refitClient = RestService.For<IFiscalPrinterClient>("http://localhost:8001/");
    private static readonly Skyware.ErpNetFS.Client ownClient = new("http://localhost:8001/");

    public static async Task<Dictionary<string, DeviceInfo>> GetPrintersAsync()
    {
        return useRefit
            ? await refitClient.GetPrintersAsync()
            : await ownClient.GetPrintersAsync();
    }

    public static async Task<DeviceStatusWithDateTime> GetPrinterStatusAsync(string deviceKey)
    {
        return useRefit
            ? await refitClient.GetPrinterStatusAsync(deviceKey)
            : await ownClient.GetPrinterStatusAsync(deviceKey);
    }

    public static async Task<DeviceInfo> GetPrinterInfoAsync(string deviceKey)
    {
        return useRefit
            ? await refitClient.GetPrinterAsync(deviceKey)
            : await ownClient.GetPrinterInfoAsync(deviceKey);
    }

    public static void PrintPrinterInfo(DeviceInfo device)
    {
        AnsiConsole.MarkupLine($"Device URI: [chartreuse3_1]{device.Uri}[/]");
        AnsiConsole.MarkupLine($"Device Serial Number: [chartreuse3_1]{device.SerialNumber}[/]");
        AnsiConsole.MarkupLine($"Device Manufacturer: [chartreuse3_1]{device.Manufacturer}[/]");
        AnsiConsole.MarkupLine($"Device Model: [chartreuse3_1]{device.Model}[/]");
        AnsiConsole.MarkupLine($"Device Firmware Version: [chartreuse3_1]{device.FirmwareVersion}[/]");
        AnsiConsole.MarkupLine($"Device Tax Identification Number: [chartreuse3_1]{device.TaxIdentificationNumber}[/]");
        AnsiConsole.MarkupLine(string.Empty);
    }

    public static async Task FiscalReceipt(string deviceKey)
    {
        Receipt receipt = new()
        {
            Operator = "1",
            OperatorPassword = "1", // "0000"
            Items = [
                new Item
                {
                    Text = new string('=', 30),
                    Type = ItemType.Comment
                },
                new Item
                {
                    Text = "ErpNet.FS Demo",
                    Type = ItemType.Comment
                },
                new Item
                {
                    Text = new string('=', 30),
                    Type = ItemType.Comment
                },
                new Item
                {
                    Text = "Sample service",
                    TaxGroup = TaxGroup.TaxGroup1,
                    Quantity = 1m,
                    UnitPrice = 0.01m,
                    Amount = 0.01m,
                },
            ],
            Payments = [
                new Payment
                {
                    Amount = 0.01m,
                    PaymentType = PaymentType.Cash,
                },
            ],
        };

        DeviceStatusWithReceiptInfo res = useRefit
            ? await refitClient.PrintFiscalReceiptAsync(deviceKey, receipt)
            : await ownClient.PrintFiscalReceiptAsync(receipt, deviceKey);

        if (res.Ok)
        {
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
            AnsiConsole.MarkupLine($"Receipt number: [chartreuse3_1]{res.ReceiptNumber}[/]");
            lastKnownReceipt = receipt;
            lastKnownReceiptStatus = res;
        }
        else
        {
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");
        }
        PrintMessages(res.Messages);
        AnsiConsole.MarkupLine(string.Empty);
    }

    public static async Task RefundFiscalReceipt(string deviceKey)
    {
        if (lastKnownReceipt is null || lastKnownReceiptStatus is null)
        {
            AnsiConsole.MarkupLine($"Error: [red]No last known receipt or its status[/]");
            AnsiConsole.MarkupLine(string.Empty);
            return;
        }
        ReversalReceipt receipt = new()
        {
            Operator = "1",
            OperatorPassword = "1", // "0000"
            Reason = ReversalReason.Refund,
            FiscalMemorySerialNumber = lastKnownReceiptStatus.FiscalMemorySerialNumber,
            ReceiptNumber = lastKnownReceiptStatus.ReceiptNumber,
            ReceiptDateTime = lastKnownReceiptStatus.ReceiptDateTime,
            Items = [
                new Item
                {
                    Text = "Sample service",
                    TaxGroup = TaxGroup.TaxGroup1,
                    Quantity = 1m,
                    UnitPrice = 0.01m,
                    Amount = 0.01m,
                },
            ],
            Payments = [
                new Payment
                {
                    Amount = 0.01m,
                    PaymentType = PaymentType.Cash,
                },
            ],
        };

        DeviceStatusWithReceiptInfo res = useRefit
            ? await refitClient.PrintRefundReceiptAsync(deviceKey, receipt)
            : await ownClient.PrintRefundReceiptAsync(receipt, deviceKey);

        if (res.Ok)
        {
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
            AnsiConsole.MarkupLine($"Receipt number: [chartreuse3_1]{res.ReceiptNumber}[/]");
        }
        else
        {
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");
        }
        PrintMessages(res.Messages);
        AnsiConsole.MarkupLine(string.Empty);
    }

    public static async Task PrintReport(string deviceKey, bool closeDay)
    {
        string reportType = closeDay ? "zreport" : "xreport";

        DeviceStatusWithDateTime res = useRefit
            ? await refitClient.PrintReportAsync(deviceKey, reportType)
            : await ownClient.PrintReportAsync(deviceKey, reportType);

        if (res.Ok)
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
        else
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");

        PrintMessages(res.Messages);
    }

    public static async Task SetDate(string deviceKey)
    {
        DeviceStatus res = useRefit
            ? await refitClient.SentPrinterTimeAsync(deviceKey, new CurrentDateTime())
            : await ownClient.SyncPrinterTimeAsync(new CurrentDateTime(), deviceKey);

        if (res.Ok)
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
        else
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");

        PrintMessages(res.Messages);
    }

    public static async Task DuplicateReceipt(string deviceKey)
    {
        DeviceStatus res = useRefit
            ? await refitClient.PrintDuplicateAsync(deviceKey)
            : await ownClient.PrintDuplicateAsync(deviceKey);

        if (res.Ok)
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
        else
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");

        PrintMessages(res.Messages);
    }

    public static async Task PrintBalance(string deviceKey)
    {
        DeviceStatusWithCashAmount res = useRefit
            ? await refitClient.GetBalanceAsync(deviceKey)
            : await ownClient.GetBalanceAsync(deviceKey);

        if (res.Ok)
        {
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
            AnsiConsole.MarkupLine($"Balance: [chartreuse3_1]{res.Amount}[/]");
        }
        else
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");

        PrintMessages(res.Messages);
    }

    public static async Task PrintDeposit(string deviceKey)
    {
        var deposit = new DepositWithdraw { Amount = 5.0m };

        DeviceStatus res = useRefit
            ? await refitClient.DepositMoneyAsync(deviceKey, deposit)
            : await ownClient.DepositMoneyAsync(deposit, deviceKey);

        if (res.Ok)
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
        else
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");

        PrintMessages(res.Messages);
    }

    public static async Task PrintWithdraw(string deviceKey)
    {
        var withdraw = new DepositWithdraw { Amount = 5.0m };

        DeviceStatus res = useRefit
            ? await refitClient.WithdrawMoneyAsync(deviceKey, withdraw)
            : await ownClient.WithdrawMoneyAsync(withdraw, deviceKey);

        if (res.Ok)
            AnsiConsole.MarkupLine($"Result: [green3]OK[/]");
        else
            AnsiConsole.MarkupLine($"Error: [red]Error[/]");

        PrintMessages(res.Messages);
    }

    public static async Task<DeviceStatusWithRawResponse> SendRawRequest(string deviceKey, string request)
    {
        var rawRequest = new RawRequest { Request = request };

        return useRefit
            ? await refitClient.SendRawRequestAsync(deviceKey, rawRequest)
            : await ownClient.SendRawRequestAsync(rawRequest, deviceKey);
    }

    public static void PrintMessages(IEnumerable<StatusMessage>? messages)
    {
        if (messages is null) return;
        foreach (var message in messages)
        {
            if (string.IsNullOrEmpty(message.Code))
            {
                AnsiConsole.Markup($"[grey69]NONE[/]: ");
            }
            else if (message.Code.StartsWith('E'))
            {
                AnsiConsole.Markup($"[red1]{message.Code}[/]: ");
            }
            else if (message.Code.StartsWith('W'))
            {
                AnsiConsole.Markup($"[darkorange]{message.Code}[/]: ");
            }
            else
            {
                AnsiConsole.Markup($"[grey69]{message.Code}[/]: ");
            }
            AnsiConsole.MarkupLine($"{message.Text}");
        }
    }
}
