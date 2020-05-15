# Mediporta.ApiExample

Ta solucja zawiera kod przyk³adowego i prostego klienta korzystaj¹cego z API Mediporty.  
 
## Zawartoœæ solucji
 
Solucja sk³ada siê z biblioteki **Mediporta.CommonLogic** zawieraj¹c¹ zawieraj¹cej logikê u¿ywan¹ w przyk³adach wykorzystania API oraz 2 projektów konsolowych: 1.    **Mediporta.CreateAndEditWizytaWithCookieAuth**, i 2.    **Mediporta.CreateAndEditWizytaWithTokenAuth**.
 
Projekt **Mediporta.CreateAndEditWizytaWithCookieAuth** pokazuje scenariusz, gdzie w Mediporcie jest zak³adana wizyta przez API i jest zwracane przekierowanie do strony edycji tej wizyty, gdzie autoryzacja edycji wizyty bêdzie siê odbywa³a po ciasteczku.
 
Projekt **Mediporta.CreateAndEditWizytaWithTokenAuth** równie¿ zak³ada wizyty w Mediporcie poprzez API, ale zamiast zwracaæ przekierowanie do stworzonej wizyty, zwraca XML'a z przygotowanym linkiem do edycji wizyty, w którym jest ju¿ osadzony token autoryzacyjny.
 
Solucja by³a testowana na systemie **Windows 10**.
 
## Konfiguracja
 
### Przygotowanie instancji testowej Mediporty
 
Nale¿y siê skontaktowaæ z Mediport¹ w celu za³o¿enia testowej instancji <kontakt@mediporta.pl> oraz nale¿y wys³aæ do Mediporty plik certyfikatu *.crt **bez has³a do certyfikatu**.
 
W folderze *Certificates* znajdujê siê szczegó³owa instrukcja generowania certyfikatu.
 
### Certyfikat
 
Nale¿y podmieniæ plik certyfikatu **mediportaApiExample.p12** w bibliotece na pasuj¹cy do konfiguracji instancji Mediporty.
Nastêpnie nale¿y zmodyfikowaæ plik **PrzykladowaWizyta.xml** (zgodnie z danymi skonfigurowanymi w Mediporcie ) ustawiony na instancji Mediporty.
 
Zaktualizowaæ wartoœci kluczy w **obu** plikach **App.config**.
 
Lista kluczy:
* WizytaFileName - nazwa pliku z wizyt¹
* CertificateFileName - nazwa pliku z certyfikatem
* CertificatePassword - has³o do certyfikatu
* RequestUriForHealthCheck - URL do endpointu w API sprawdzaj¹cego, czy API dzia³a
* RequestUriForCreateAndEditWizytaWithCookieAuth - URL do endpointu w API zak³adaj¹cego wizytê i zwracaj¹cego przekierowanie do wizyty
* RequestUriForCreateAndEditWizytaWithTokenAuth - URL do endpointu w API zak³adaj¹cego wizytê i zwracaj¹cego link do wizyty z tokenem, który bêdzie u¿yty do autoryzacji
 

## Uruchomienie programu
 
Program oczekuje, ¿e plik z wizyt¹ bêdzie siê znajdowaæ w folderze *Wizyty* wzglêdem pliku wykonywalnego. Domyœlnie **bin\Debug\Wizyty**.
Plik z certyfikatem powinien znaleŸæ  siê odpowiednio w folderze *Certyfikaty* wzglêdem pliku wykonywalnego. Domyœlnie **bin\Debug\Certyfikaty**.
 
Nazwy plików s¹ ustawiane w **app.config**.
 
## Logi programu
 
Program poza wypisywaniem komunikatów na konsole zapisuje równie¿ dane powi¹zane z dzia³aniem API do plików.
 
Lista plików
* RequestBody.xml - zawartoœæ requesta, jaki jest wysy³any do Mmediporty do zak³adania wizyty. Zawiera podpisany certyfikatem XML z wizyt¹.
* ResponseContentCreate.html - zwrócony content responsa z requesta wys³anego przez projekt **Mediporta.CreateAndEditWizytaWithCookieAuth**. W przypadku powodzenia bêdzie zawiera³ html strony do edycji utworzonej wizyty, a w przeciwnym przypadku szczegó³y b³êdu
* ResponseContentCreateWithUrls.html - zwrócony content responsa z requesta wys³anego przez projekt **Mediporta.CreateAndEditWizytaWithTokenAuth**. W przypadku powodzenia bêdzie zawiera³ XML z linkiem z tokenem autoryzacyjnym do edycji wizyty, a w przeciwnym przypadku szczegó³y b³êdu
* ResponseContentGetWizytaWithToken.html - zwrócony content responsa z requesta wykonanego pod link pobrany z XML'a z ResponseContentCreateWithUrls.html
