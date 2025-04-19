// See https://aka.ms/new-console-template for more information
using ErpNetModelDemo;
using Skyware.ErpNetFS.Model;
using Spectre.Console;

Dictionary<string, string> actions = new()
{
    { "FR", "Print receipt" },
    { "DF", "Print duplicate receipt of the last fiscal receipt" },
    { "RF", "Print reversal receipt (refund of last printed receipt)" },
    { "DT", "Set date and time" },
    { "RR", "Send raw request" },
    { "CD", "Deposit cash and print slip" },
    { "CW", "Withdraw cash and print slip" },
    { "CB", "Get cash balance" },
    { "XR", "Print X report" },
    { "ZR", "Print Z report (end of the day)" },
    { "Q", "Exit" }
};

AnsiConsole.MarkupLine("[chartreuse3_1]Fiscal printers demo[/]");

string lib = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select an API implementation:")
        .AddChoices(["Refit API Client", "Own API Client"]));
AnsiConsole.MarkupLine(string.Empty);
ApiWorker.useRefit = lib == "Refit API Client";

Dictionary<string, DeviceInfo> devices = await ApiWorker.GetPrintersAsync();

if (devices is null || devices.Count == 0)
{
    AnsiConsole.MarkupLine("No devices found.");
    return;
}

string printerKey = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select a printer:")
        .AddChoices(devices.Keys));
AnsiConsole.MarkupLine(string.Empty);

AnsiConsole.MarkupLine($"Device information for [chartreuse3_1]{printerKey}[/]");
ApiWorker.PrintPrinterInfo(devices[printerKey]);

DeviceStatusWithDateTime deviceStatus = await ApiWorker.GetPrinterStatusAsync(printerKey);
AnsiConsole.MarkupLine($"Device status for [chartreuse3_1]{printerKey}[/]");
AnsiConsole.MarkupLine($"Is OK: [chartreuse3_1]{deviceStatus.Ok}[/]");
AnsiConsole.MarkupLine(string.Empty);


KeyValuePair<string, string> action;
do
{
    action = AnsiConsole.Prompt(
        new SelectionPrompt<KeyValuePair<string, string>>()
            .Title($"Select an action to execute to {printerKey}:")
            .PageSize(20)
            .AddChoices(actions)
            .UseConverter(x => $"{x.Key}: {x.Value}"));

    switch (action.Key)
    {
        case "FR":
            await ApiWorker.FiscalReceipt(printerKey);
            break;
        case "DF":
            await ApiWorker.DuplicateReceipt(printerKey);
            break;
        case "RF":
            await ApiWorker.RefundFiscalReceipt(printerKey);
            break;
        case "DT":
            await ApiWorker.SetDate(printerKey);
            break;
        //case "RR":
        //    await Units.RawRequest(fiscalPrinterClient, printerKey);
        //    break;
        case "CD":
            await ApiWorker.PrintDeposit(printerKey);
            break;
        case "CW":
            await ApiWorker.PrintWithdraw(printerKey);
            break;
        case "CB":
            await ApiWorker.PrintBalance(printerKey);
            break;
        case "XR":
            await ApiWorker.PrintReport(printerKey, false);
            break;
        case "ZR":
            await ApiWorker.PrintReport(printerKey, true);
            break;
        default:
            break;
    }

    AnsiConsole.MarkupLine(string.Empty);

}
while (action.Key != "Q");


