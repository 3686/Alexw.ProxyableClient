# Alexw.ProxyableClient
A simple .NET console app that fetched data a) directly and b) via a proxy for comparison.

## Configuration
Change the app settings values for the proxy and for the source http path

```
<!-- must be an absolute http proxy url -->
<add key="Proxy.Uri" value="http://104.131.14.247:8080" />

<!-- must be an absolute http url to get the data from -->
<add key="Source.Uri" value="http://example.com" />
```
Looking for an example proxy? ProxyNova have a list you can use at your own risk:
* [ProxyNova - Proxy Server List](https://www.proxynova.com/proxy-server-list/)
