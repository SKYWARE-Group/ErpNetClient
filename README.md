# <img src="https://github.com/SKYWARE-Group/ErpNetClient/blob/master/ErpNetClient/Assets/erp-net-fs-client.png" width="48" height="48"/> ErpNetClient

This project is a client library for interaction with [ErpNet.FP fiscal server](https://github.com/erpnet/ErpNet.FP). It encapsulates both data model and http invocations.

![NuGet Version](https://img.shields.io/nuget/v/Skyware.ErpNetFS.Model?label=Skyware.ErpNetFS.Model&color=green)
![NuGet Version](https://img.shields.io/nuget/v/Skyware.ErpNetFS.Client?label=Skyware.ErpNetFS.Client&color=green)
[![.NET](https://github.com/SKYWARE-Group/ErpNetClient/actions/workflows/dotnet.yml/badge.svg)](https://github.com/SKYWARE-Group/ErpNetClient/actions/workflows/dotnet.yml)

## Getting started

```c#
Skyware.ErpNetFS.Client client = new("http://localhost:8001/");
var printers = await client.GetPrintersAsync();
```

## CLI usage of the server

Even this command has no relation to the current project, it explains how it works

```bash
curl -i -X POST -H "Content-Type:application/json" -d "{\"uniqueSaleNumber\": \"DT279013-0001-0000052\", \"operator\": \"1\", \"operatorPassword\": \"1\", \"items\": [{\"text\": \"test\", \"quantity\": 1, \"unitPrice\": 1, \"taxGroup\": 1 }], \"payments\": [{\"amount\": 1, \"paymentType\": \"cash\" } ]}" http://localhost:8001/printers/dt797821/receipt/
```
