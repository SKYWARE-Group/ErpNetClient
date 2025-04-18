using Skyware.ErpNetFS.Model;
using Spectre.Console;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ErpNetModelDemo;

public class Units
{

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

    public static async Task FiscalReceipt(IFiscalPrinterClient client, string deviceKey)
    {
        Receipt receipt = new()
        {
            Operator = "1",
            OperatorPassword = "0000",
            Items = [
                new Item
                {
                    Text = "Some service",
                    TaxGroup = TaxGroup.TaxGroup1,
                    Quantity = 1m,
                    UnitPrice = 0.01m,
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
        DeviceStatusWithReceiptInfo res =  await client.PrintFiscalReceiptAsync(deviceKey, receipt);
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

    public static void PrintMessages(IEnumerable<StatusMessage>? messages)
    {
        if (messages is null) return;
        foreach (var message in messages)
        {
            if (string.IsNullOrEmpty(message.Code))
            {
                AnsiConsole.Markup($"NONE: ");
            }
            else if (message.Code.StartsWith("E"))
            {
                AnsiConsole.Markup($"[red1]{message.Code}[/]: ");
            }
            else
            {
                AnsiConsole.Markup($"[grey69]{message.Code}[/]: ");
            }
            AnsiConsole.MarkupLine($"{message.Text}");

        }
    }

}
