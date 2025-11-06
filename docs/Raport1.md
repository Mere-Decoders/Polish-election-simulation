## Cel Projektu
Stworzenie Serwisu Zwiększającego Świadomość i Znajomość innych metod przeliczania głosów w wyborach na mandaty. Ilustrowanie wpływu wyboru metody przeliczania głosów na wyniki wyborów.  

## Funkcjonalności
1. M (must have):
    - Obliczanie wyników wyborów dla zestawów danych
    - Wyświetlanie wyników obliczeń w formie Mapy oraz Wykresów
2. S (should have):
    - Tworzenie własnych rozkładów głosów w powiatach
    - Dodawanie i usuwanie partii w rozkładach głosów
    - Generowanie statycznych linków do policzonych zestawów danych
    - Wyświetlanie obok siebie wyników dwóch różnych obliczeń
    - Tworzenie profilu użytkownika
    - Zapisywanie własnych danych na serwerze przypisanych do profilu użytkownika
    - Używanie własnych danych zapisanych na serwerze
    - Eksport danych do pliku
    - Import danych z pliku
3. C (could have):
    - Tworzenie własnych:
        1. Podziałów powiatów na okręgi wyborcze
        2. Metod podziału mandatów
    - Umieszczanie zmonetyzowanych reklam na stronie
    - Eksportowanie wizualizacji do pliku JPEG lub PNG
    - Dzielenie się wynikami symulacji na platformach społecznościowych
4. W (won't have)
    - Inny kraj

## Architektura i Technologie
1. C# ASP NET CORE - web API

   ``Dlaczego?`` Wybraliśmy ASP.NET Core Web API, bo jest szybki, stabilny i 
dobrze wspiera modularną architekturę. Ułatwia tworzenie REST API,
ma wbudowane narzędzia potrzebne do pracy z backendem. 
Wymusza i ścisłe typowanie i niektóre konwencje projektowe.
Dzięki temu kod jest czytelny, łatwy w utrzymaniu i gotowy do rozbudowy w przyszłości.


2. Vue.js

   ``Dlaczego?`` Vue.js jest lekki, szybki i prosty w nauce, a jednocześnie wystarczająco elastyczny, by tworzyć złożone interfejsy użytkownika.
   Posiada intuicyjną składnię i dobry ekosystem narzędzi (np. Vue Router, Pinia), co ułatwia tworzenie dynamicznych widoków, wizualizacji danych oraz komponentów interaktywnych.
   Umożliwia płynne odświeżanie wyników obliczeń i łatwe łączenie z backendem poprzez REST API.


3. PostgreSQL

   ``Dlaczego?`` PostgreSQL jest stabilną i wydajną bazą danych typu open-source, dobrze wspieraną przez .NET.
   Oferuje silne wsparcie dla złożonych zapytań, transakcji i typów danych, co jest istotne przy przechowywaniu danych o wynikach wyborów i rozkładach głosów.
   


4. Planujemy deployment na chmurę jednego z większych providerów np. Azure ze względu na istnienie darmowych tierów oraz łatwe skalowanie wraz z wzrostem ilości użytkowników (wystarczy zmienić tier i nic nie trzeba migrować)

## Role w Projekcie
1. Diego Ostoja-Kowalski
   - Frontend Developer
   - Q&A oraz Testing 
2. Marcin Madej
   - Backend Developer
3. Piotr Janiszewski
    - Frontend Developer
    - Księżniczka
4. Stanisław Kucharski
    - Backend Developer
    - DevOps Engineer

### Linki
Repozytorium GitHub https://github.com/Mere-Decoders/Polish-election-simulation

Project Board Github https://github.com/orgs/Mere-Decoders/projects/1