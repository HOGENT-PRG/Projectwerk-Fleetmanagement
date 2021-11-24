# Documentatie

## Doel van de documentatie

"Daarnaast moeten we er ook rekening mee houden dat deze applicatie zal moeten worden onderhouden (misschien niet door ons) en dat er dus voldoende documentatie beschikbaar is zodat een andere programmeur dit ook zou kunnen uitvoeren."

"Voorbeeld in de organisatie-rol staat er dat deze persoon ervoor zorgt dat er voldoende documentatie is. Dit wil niet zeggen dat deze persoon alle documentatie moet schrijven. Dit betekent wel dat deze persoon er moet op toezien dat elke klasse is gedocumenteerd (op een eenduidige manier), dat overzichtschemas van de klassen (rol architectuur) en testen (rol testing) op één plaats beschikbaar zijn en dat de documentatie inhoudelijk voldoet. "

## Installatie & initialisatie van het project

### Afstemmen configuratiebestand

text

### SQL Server databank

text

## Lagen

### Diagrammen en schemas

text

### Presentatielaag (WPF)

#### UI componenten

Er wordt gebruik gemaakt van MaterialDesign libraries om [componenten](https://www.nuget.org/packages/MaterialDesignThemes/4.3.0-ci49) en [kleuren](https://www.nuget.org/packages/MaterialDesignColors/2.0.4-ci49) toe te voegen.

### Businesslaag 

#### Helper **RRNValideerder** 

Het Rijksregisternummer (RRN) dient gevalideerd te worden aan de hand van [dit document](https://nl.wikipedia.org/wiki/Rijksregisternummer).

Er is een publieke methode valideer beschikbaar welke volgens de specificaties uitgelijnd op bovenstaande website het rijksregisternummer valideert.
Bij instantiering van de Bestuurder klasse zal in de constructor body gebruik gemaakt worden van deze helper, indien validatie faalt zal er een ArgumentException gethrowt worden. Indien validatie succesvol is wordt het gevalideerdeRRN geretourneerd en de attribuut Rijksregisternummer in klasse Bestuurder gepopuleerd.

De [Single-Responsibility-Principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) voor deze helper is het valideren van een RRN.

#### FleetManager

text

#### VoertuigController

text

#### BestuurderController

text

#### TankkaartController

text

#### Voertuig

text

#### Bestuurder 

text

#### Tankkaart

text

### Persistentielaag

text
