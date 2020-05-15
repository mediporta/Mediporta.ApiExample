using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace Mediporta.CommonLogic
{
    public static class CertificateTools
    {
        public static X509Certificate2 LoadCertificates()
        {
            Console.WriteLine("Wczytywanie hasła certyfikatu klienckiego z pliku app.config (klucz: CertificatePassword)...");
            var certificatePassword = ConfigurationManager.AppSettings["CertificatePassword"];

            Console.WriteLine("Wczytywanie nazwy pliku certyfikatu z pliku app.config (klucz: CertificateFileName)...");
            var CertificateFileName = ConfigurationManager.AppSettings["CertificateFileName"];
            Console.WriteLine($"CertificateFileName: {CertificateFileName}");

            Console.WriteLine("Wczytywanie certyfikatu klienckiego...");
            // Certyfikat wraz z kluczem prywatnym używany po stronie aplikacji klienckiej
            var clientCertificate = new X509Certificate2($"Certyfikaty/{CertificateFileName}", certificatePassword);

            return clientCertificate;
        }
    }
}
