## C# bindings for libpam

A .NET Standard library for authenticating against PAM

Example:
```csharp

using (var authenticator = new PamAuthenticator())
{
    var success = authenticator.TryAuthenticate(username: "hans", password: "wurst");
}
```


Works on Linux and macOS
