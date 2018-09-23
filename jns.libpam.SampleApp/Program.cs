using System;
using jns.libpam;

namespace jns.Pam.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var authenticator = new PamAuthenticator())
            {
                const string user = "foo";
                const string pass = "bar";

                Console.WriteLine($"Attempting to authenciate using user={user}, password={pass}");

                var success = authenticator.TryAuthenticate(user, pass);

                Console.WriteLine($"Success: {success}");
                Console.ReadKey();
            }
        }
    }
}
