# Mediporta.ApiExample

Ta solucja zawiera kod przyk�adowego i prostego klienta korzystaj�cego z API Mediporty.  
 
## Zawarto�� solucji
 
Solucja sk�ada si� z biblioteki **Mediporta.CommonLogic** zawieraj�c� zawieraj�cej logik� u�ywan� w przyk�adach wykorzystania API oraz 2 projekt�w konsolowych: 1.    **Mediporta.CreateAndEditWizytaWithCookieAuth**, i 2.    **Mediporta.CreateAndEditWizytaWithTokenAuth**.
 
Projekt **Mediporta.CreateAndEditWizytaWithCookieAuth** pokazuje scenariusz, gdzie w Mediporcie jest zak�adana wizyta przez API i jest zwracane przekierowanie do strony edycji tej wizyty, gdzie autoryzacja edycji wizyty b�dzie si� odbywa�a po ciasteczku.
 
Projekt **Mediporta.CreateAndEditWizytaWithTokenAuth** r�wnie� zak�ada wizyty w Mediporcie poprzez API, ale zamiast zwraca� przekierowanie do stworzonej wizyty, zwraca XML'a z przygotowanym linkiem do edycji wizyty, w kt�rym jest ju� osadzony token autoryzacyjny.
 
Solucja by�a testowana na systemie **Windows 10**.
 
## Konfiguracja
 
### Przygotowanie instancji testowej Mediporty
 
Nale�y si� skontaktowa� z Mediport� w celu za�o�enia testowej instancji <kontakt@mediporta.pl> oraz nale�y wys�a� do Mediporty plik certyfikatu *.crt **bez has�a do certyfikatu**.
 
W folderze *Certificates* znajduj� si� szczeg�owa instrukcja generowania certyfikatu.
 
### Certyfikat
 
Nale�y podmieni� plik certyfikatu **mediportaApiExample.p12** w bibliotece na pasuj�cy do konfiguracji instancji Mediporty.
Nast�pnie nale�y zmodyfikowa� plik **PrzykladowaWizyta.xml** (zgodnie z danymi skonfigurowanymi w Mediporcie ) ustawiony na instancji Mediporty.
 
Zaktualizowa� warto�ci kluczy w **obu** plikach **App.config**.
 
Lista kluczy:
* WizytaFileName - nazwa pliku z wizyt�
* CertificateFileName - nazwa pliku z certyfikatem
* CertificatePassword - has�o do certyfikatu
* RequestUriForHealthCheck - URL do endpointu w API sprawdzaj�cego, czy API dzia�a
* RequestUriForCreateAndEditWizytaWithCookieAuth - URL do endpointu w API zak�adaj�cego wizyt� i zwracaj�cego przekierowanie do wizyty
* RequestUriForCreateAndEditWizytaWithTokenAuth - URL do endpointu w API zak�adaj�cego wizyt� i zwracaj�cego link do wizyty z tokenem, kt�ry b�dzie u�yty do autoryzacji
 

## Uruchomienie programu
 
Program oczekuje, �e plik z wizyt� b�dzie si� znajdowa� w folderze *Wizyty* wzgl�dem pliku wykonywalnego. Domy�lnie **bin\Debug\Wizyty**.
Plik z certyfikatem powinien znale��  si� odpowiednio w folderze *Certyfikaty* wzgl�dem pliku wykonywalnego. Domy�lnie **bin\Debug\Certyfikaty**.
 
Nazwy plik�w s� ustawiane w **app.config**.
 
## Logi programu
 
Program poza wypisywaniem komunikat�w na konsole zapisuje r�wnie� dane powi�zane z dzia�aniem API do plik�w.
 
Lista plik�w
* RequestBody.xml - zawarto�� requesta, jaki jest wysy�any do Mmediporty do zak�adania wizyty. Zawiera podpisany certyfikatem XML z wizyt�.
* ResponseContentCreate.html - zwr�cony content responsa z requesta wys�anego przez projekt **Mediporta.CreateAndEditWizytaWithCookieAuth**. W przypadku powodzenia b�dzie zawiera� html strony do edycji utworzonej wizyty, a w przeciwnym przypadku szczeg�y b��du
* ResponseContentCreateWithUrls.html - zwr�cony content responsa z requesta wys�anego przez projekt **Mediporta.CreateAndEditWizytaWithTokenAuth**. W przypadku powodzenia b�dzie zawiera� XML z linkiem z tokenem autoryzacyjnym do edycji wizyty, a w przeciwnym przypadku szczeg�y b��du
* ResponseContentGetWizytaWithToken.html - zwr�cony content responsa z requesta wykonanego pod link pobrany z XML'a z ResponseContentCreateWithUrls.html
