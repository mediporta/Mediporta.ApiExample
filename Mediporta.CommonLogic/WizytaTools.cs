using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Mediporta.CommonLogic
{
    public static class WizytaTools
    {
        public static XmlDocument LoadAndSignWizyta(X509Certificate2 clientCertificate)
        {
            InsertConsoleLogSectionBreak();

            Console.WriteLine("Wczytywanie pliku z przykładową wizytą z XML'a z app.config (klucz: WizytaFileName)...");
            var wizytaFileName = ConfigurationManager.AppSettings["WizytaFileName"];
            Console.WriteLine($"WizytaFileName: {wizytaFileName}");

            // Wczytanie przykładowego XML'a z wizytą do założenia w Mediporcie. W tym przykładzie nie używamy wszystkich dostępnych pól.
            var wizytaString = File.ReadAllText($"Wizyty/{wizytaFileName}");
            var xlmDocument = new XmlDocument();
            xlmDocument.LoadXml(wizytaString);

            // Podpisanie XML'a reprezentującego wizytę certyfikatem klienckim używać klucza prywatnego
            XmlDocument signedXml = xlmDocument.Sign(clientCertificate);

            File.WriteAllText(Consts.RequestBodyFileName, signedXml.OuterXml);
            Console.WriteLine($"RequestBody zostało zapisane do pliku {Consts.RequestBodyFileName}");

            return signedXml;
        }

        public static bool CheckServiceHealth()
        {
            InsertConsoleLogSectionBreak();

            Console.WriteLine("Wczytywanie adresu endpointu z app.config (klucz: RequestUriForHealthCheck)...");
            var requestUri = ConfigurationManager.AppSettings["RequestUriForHealthCheck"];
            Console.WriteLine($"Endpoint: {requestUri}");

            Console.WriteLine("Sprawdzanie stanu API...");
            var result = HttpClientWrapper.Get(requestUri);
            var responseContent = result.Content.ReadAsStringAsync().Result;

            if (!result.IsSuccessStatusCode)
            {
                Console.WriteLine($"Nie poprawny kod odpowiedzi API: {result.StatusCode}");
            }

            Console.WriteLine($"ResponseContent: {responseContent}");

            return result.IsSuccessStatusCode;
        }

        public static void CreateWizytaWithoutToken(XmlDocument signedXml)
        {
            InsertConsoleLogSectionBreak();

            Console.WriteLine("Wczytywanie adresu endpointu z app.config (klucz: RequestUriForCreateAndEditWizytaWithCookieAuth)...");
            var requestUri = ConfigurationManager.AppSettings["RequestUriForCreateAndEditWizytaWithCookieAuth"];
            Console.WriteLine($"Endpoint: {requestUri}");

            Console.WriteLine("Tworzenie wizyty w mediporcie...");
            // Wysłanie żądania utworzenia wizyty przy użyciu podpisanego XML'a
            CreateWizytaInMediporta(signedXml, requestUri);
        }

        public static void CreateWizytaWithToken(XmlDocument signedXml)
        {
            InsertConsoleLogSectionBreak();

            Console.WriteLine("Testowanie API z walidacją odczytu wizyty poprzez token");
            Console.WriteLine("Wczytywanie adresu endpointu z app.config (klucz: RequestUriForCreateAndEditWizytaWithTokenAuth)...");
            var requestUriForToken = ConfigurationManager.AppSettings["RequestUriForCreateAndEditWizytaWithTokenAuth"];
            Console.WriteLine($"Endpoint: {requestUriForToken}");

            Console.WriteLine("Tworzenie wizyty w mediporcie z walidacja wizyty poprzez token...");
            // Wysłanie żądania utworzenia wizyty przy użyciu podpisanego XML'a
            var fileName = CreateWizytaInMediporta(signedXml, requestUriForToken);

            InsertConsoleLogSectionBreak();

            var responseWithToken = File.ReadAllText(fileName);

            var xlmDocument = new XmlDocument();
            try
            {
                xlmDocument.LoadXml(responseWithToken);
                var expirationDateTimeAtr = xlmDocument.FirstChild.FirstChild.FirstChild.Attributes["ExpirationDateTime"];
                Console.WriteLine($"Data wygaśnięcia: {expirationDateTimeAtr.Value}");
                var urlAtr = xlmDocument.FirstChild.FirstChild.FirstChild.Attributes["Url"];
                Console.WriteLine($"Autoryzowany url: {urlAtr.Value}");
                var kindAtr = xlmDocument.FirstChild.FirstChild.FirstChild.Attributes["Kind"];
                Console.WriteLine($"Rodzaj: {kindAtr.Value}");

                Console.WriteLine("Wyświetlenie wizyty w mediporcie z tokenem jako autoryzacją...");
                var result = HttpClientWrapper.Get(urlAtr.Value);
                LogHttpResponse("GetWizytaWithToken", result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nie udało się wczytać XML'a z tokenem: {ex.Message}");
            }
        }

        private static string CreateWizytaInMediporta(XmlDocument signedXml, string requestUri)
        {
            var httpContent = new StringContent(signedXml.OuterXml, Encoding.UTF8, "application/xml");
            var result = HttpClientWrapper.Post(requestUri, httpContent);
            var fileName = LogHttpResponse(requestUri, result);

            return fileName;
        }

        private static string LogHttpResponse(string fileNameSecondSegment, HttpResponseMessage result)
        {
            Console.WriteLine("StatusCode: " + result.StatusCode);
            Console.WriteLine("AbsoluteUri: " + result.RequestMessage.RequestUri.AbsoluteUri);

            var fileName = $"{Consts.ResponseFileName}{fileNameSecondSegment.Split('/').Last()}.html";
            File.WriteAllText(fileName, result.Content.ReadAsStringAsync().Result);
            Console.WriteLine($"Content odpowiedzi został zapisany do pliku {fileName}.");

            return fileName;
        }

        private static void InsertConsoleLogSectionBreak()
        {
            Console.WriteLine(string.Empty);
            Console.WriteLine("####################################");
            Console.WriteLine(string.Empty);
        }
    }
}
