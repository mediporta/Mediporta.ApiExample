using Mediporta.CommonLogic;
using System;

namespace Mediporta.CreateAndEditWizytaWithCookieAuth
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientCertificate = CertificateTools.LoadCertificates();
            var signedXml = WizytaTools.LoadAndSignWizyta(clientCertificate);

            if (WizytaTools.CheckServiceHealth())
            {
                WizytaTools.CreateWizytaWithoutToken(signedXml);
            }

            Console.WriteLine(string.Empty);
            Console.WriteLine("Naciśnij dowolny klawisz aby zamknąć program...");
            Console.ReadKey();
        }
    }
}
