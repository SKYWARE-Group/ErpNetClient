# ErpNet.FP client

ErpNet.FP Client is a client library to simplify usage of [ErpNet.FP](https://github.com/erpnet/ErpNet.FP) fiscal server.

Example usage:

```csharp
Skyware.ErpNetFS.Client client = new("http://localhost:8001/");
var printers = await client.GetPrintersAsync();
```
