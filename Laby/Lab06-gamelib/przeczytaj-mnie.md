Biblioteke implementuje logike do gry zgodnej z ponizszym opisem:

"""
Gracz wciela się w postać biznesmena galaktycznego. Podróżując przez galaktykę odwiedzamy różne układy planetarne i inne tajemnicze zakątki wszechświata. 

W trakcie gry gracz posługuje się walutą o nazwie kredyt galaktyczny. Gracz zajmuje niezamieszkane/nieprzejęte planety poprzez wybudowanie na nich portu kosmicznego. 

Na planetach można także wybudować (o ile gracz jest właścicielem planety) posterunek (który w kolejnych turach można rozbudowywać do: habitatów mieszkalnych, kolonii, hotelu galaktycznego oraz, jeśli gracz jest właścicielem wszystkich planet w systemie, sieci hoteli planetarnych), kopalnie (można je rozbudowywać w kolejnych turach do kopalni 2-go i 3-go stopnia) oraz farmy żywności (można je rozbudowywać do 5 kolejnych poziomów). 

W kosmosie znajduje się obszar osobliwość (szansa). Wlatując w ten obszar możemy wylosować jedną spośród następujących kart: atak piratów, karta obrony przed piratami, bilet galaktyczny, imperator zarządził pobranie jednorazowego podatku od nieruchomości (pobranie pewnego procentu od wartości nieruchomości), wygrana w galaktycznej loterii, awaria silnika statku (strata kolejki + konieczność zapłaty za holowanie).

 W galaktyce można napotkać piratów - pole atak piratów powoduje utratę 2 kolejek, jednak gracz ma możliwość zapłaty okupu lub, wykorzystania karty obrony przed piratami o ile jest w posiadaniu tej karty (wówczas gracz nie traci żadnej kolejki). 
 
 Galaktyka posiada również rozbudowaną sieć kolei galaktycznych. Jeśli gracz posiada bilet galaktyczny może (nie musi) wybrać dowolny "przystanek kolei" i do niego się przenieść. 
 
 Co kilka tur (Państwo decydują co ile), w zależności od stanu posiadania gracza doliczana jest do stanu konta pewna liczba kredytów (Państwo decydują również o przelicznikach i współczynnikach, które służą do obliczenia ostatecznej kwoty).
 
  Gracz w ciągu jednej tury każdą z atrakcji/właściwości planety może rozwinąć tylko raz (tzn. jeśli w jednej turze buduje port na planecie to nie może w tej samej turze wybudować posterunku kopalni, itp. jeśli jest właścicielem planety i posiada na niej posterunek to w ciągu jednej tury może go rozbudować tylko raz, itp.). 
  
  Jeśli gracz jest właścicielem wszystkich planet w systemie to może w nim wybudować stocznie galaktyczną (stocznia generuje dodatkowy zysk, ale nie może być rozbudowywana) oraz kopalnie w pasach asteroid (maksymalny poziom kopalni to: 5). Jeśli gracz posiada przynajmniej jedną stocznię galaktyczną to wówczas może w osobliwości wylosować dodatkową "kartę": Awaria w stoczni galaktycznej (generuje to konieczność zapłaty za usunięcie awarii lub, jeśli gracz nie posiada odpowiednich finansów, utratę stoczni w odpowiednim systemie). 
  
  Gracz może zrezygnować z ruchu (ale tylko co najwyżej 2 razy pod rząd).
"""

Biblioteka jest przygotowana z mysla o samodzielnym uzyciu poprzez konsole (dolaczylem tez program ktory ja uruchomi) jak i jako silnik gotowy do pozniejszego polaczenia z GUI.

Aby uruchomic program, nalezy wyapkowac calosc z pliku .zip 
a nastepnie wywolac komende 

```
dotnet run --project .\GamelibRunner\GamelibRunner.csproj
```

testowane na .net 10