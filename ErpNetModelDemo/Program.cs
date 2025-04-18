// See https://aka.ms/new-console-template for more information
using ErpNetModelDemo;
using Refit;
using Skyware.ErpNetFS.Model;
using Spectre.Console;
using System;

Dictionary<string, string> actions = new()
{
    { "FR", "Print receipt" },
    { "RF", "Print reversal receipt" },
    { "RR", "Send raw request" },
    { "XR", "Print X report" },
    { "ZR", "Print Z report" },
    { "Q", "Exit" }
};

AnsiConsole.MarkupLine("[chartreuse3_1]Fiscal printers demo[/]");

IFiscalPrinterClient fiscalPrinterClient = RestService.For<IFiscalPrinterClient>("http://localhost:8001");
Dictionary<string, DeviceInfo> devices = await fiscalPrinterClient.GetPrintersAsync();

if (devices is null || devices.Count == 0)
{
    AnsiConsole.MarkupLine("No devices found.");
    return;
}

string printerKey = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select printer")
        .AddChoices(devices.Keys));
AnsiConsole.MarkupLine(string.Empty);

AnsiConsole.MarkupLine($"Device information for [chartreuse3_1]{printerKey}[/]");
Units.PrintPrinterInfo(devices[printerKey]);

DeviceStatusWithDateTime deviceStatus = await fiscalPrinterClient.GetPrinterStatusAsync(printerKey);
AnsiConsole.MarkupLine($"Device status for [chartreuse3_1]{printerKey}[/]");
AnsiConsole.MarkupLine($"Is OK: [chartreuse3_1]{deviceStatus.Ok}[/]");
AnsiConsole.MarkupLine(string.Empty);

KeyValuePair<string, string> action;
do
{

    action = AnsiConsole.Prompt(
        new SelectionPrompt<KeyValuePair<string,string>>()
            .Title("Select action")
            .AddChoices(actions)
            .UseConverter(x => $"{x.Key}: {x.Value}"));

    switch (action.Key)
    {
        case "FR":
            await Units.FiscalReceipt(fiscalPrinterClient, printerKey);
            break;
        //case "RF":
        //    await Units.RefundReceipt(fiscalPrinterClient, printerKey);
        //    break;
        //case "RR":
        //    await Units.RawRequest(fiscalPrinterClient, printerKey);
        //    break;
        //case "XR":
        //    await Units.PrintReport(fiscalPrinterClient, printerKey, action.Key);
        //    break;
        //case "ZR":
        //    await Units.PrintReport(fiscalPrinterClient, printerKey, action.Key);
        //    break;
        default:
            break;
    }

    AnsiConsole.MarkupLine(string.Empty);
}
while (action.Key != "Q");


