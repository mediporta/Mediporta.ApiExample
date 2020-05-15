# ReadMe

Do poprawnego działania klienta API potrzebne jest posiadanie wygenerowanego certyfikatu - poniższa instrukcja przedstawia zestaw przykładowych poleceń do jego generowania.

## Przygotowanie instancji testowej Mediporty

Należy się skontaktować z Mediportą w celu założenia testowej instancji <kontakt@mediporta.pl> oraz należy wysłać do Mediporty plik certyfikatu *.crt **bez hasła do certyfikatu**.

Poniżej znajduje się instrukcja generowania przykładowego certyfikatu.

## Przykładowe polecenia użyte do generowanie certyfikatu:

1.	Tworzenie klucza:
keytool -genkeypair -alias mediportaApiExampleCert -keyalg RSA-keysize 2048 -storetype PKCS12 -keystore mediportaApiExample.p12 -validity 3650
2.	Odczyt
openssl pkcs12 -in mediportaApiExample.p12 -info
3.	Eksport certyfikatu
openssl pkcs12 -in mediportaApiExample.p12 -clcerts -nokeys -out mediportaApiExampleCert.crt
4.	Podgląd certyfikatu
openssl x509 -in mediportaApiExampleCert.crt -text -noout

Załóżmy, że hasło do przykładowego certyfikatu to **testPass** (takie jest domyślnie ustawione w configu).

## Uruchomienie programu

Jeśli certyfikat zostanie podmieniony pod tą samą nazwą to zostanie automatycznie skopiowany do folderu ze skompilowaną wersja klienta przy budowie aplikacji. Jeśli, chcą Państwo użyć innej nazwy pliku to należy go przekopiować ręcznie do folderu **bin\Debug\Certyfikaty** i ustawić jego nazwę w odpowiednim kluczu w app.config

