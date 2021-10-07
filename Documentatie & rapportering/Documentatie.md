# Documentatie

## Doel van de documentatie

"Daarnaast moeten we er ook rekening mee houden dat deze applicatie zal moeten worden onderhouden (misschien niet door ons) en dat er dus voldoende documentatie beschikbaar is zodat een andere programmeur dit ook zou kunnen uitvoeren."

"Voorbeeld in de organisatie-rol staat er dat deze persoon ervoor zorgt dat er voldoende documentatie is. Dit wil niet zeggen dat deze persoon alle documentatie moet schrijven. Dit betekent wel dat deze persoon er moet op toezien dat elke klasse is gedocumenteerd (op een eenduidige manier), dat overzichtschemas van de klassen (rol architectuur) en testen (rol testing) op één plaats beschikbaar zijn en dat de documentatie inhoudelijk voldoet. "

## Installatie & initialisatie van het project

### Afstemmen configuratiebestand

text

### SQL Server databank

text

## Lagen, klassen en architectuur

### Architectureel design (beslissingen / motivaties)

#### Dependencies

WPFApp **DEPENDS ON** BusinessLaag **DEPENDS ON** DataLaag

Een interactie met de WPF Applicatie resulteert dus in een function call binnen de BusinessLaag.
Een operatie binnen de Businesslaag die data vereist uit de databank resulteert dus in een function call binnen de DataLaag.

Hieronder een vergelijking tussen het MVC model en de dependencies zoals ze nu ingesteld staan.


![Vergelijking](https://i.imgur.com/eZnbI9K.png)


### Diagrammen en schemas

text

### Presentatielaag (WPF)

#### UI componenten

Er wordt gebruik gemaakt van MaterialDesign libraries om [componenten](https://www.nuget.org/packages/MaterialDesignThemes/4.3.0-ci49) en [kleuren](https://www.nuget.org/packages/MaterialDesignColors/2.0.4-ci49) toe te voegen.

### Businesslaag 

#### Helper **RRNValideerder** 

Het Rijksregisternummer (RRN) dient gevalideerd te worden aan de hand van [dit document](https://nl.wikipedia.org/wiki/Rijksregisternummer).

Er waren enkele mogelijkheden om dit te bereiken, als volgt:
* het RRN valideren in de BestuurderController klasse - aangezien dit niet de verantwoordelijkheid van deze klasse is, is dit ondergeschikt
* het RRN valideren in de Bestuurder klasse aangezien het RRN toebehoort aan deze persoon (maar dat is niet echt de verantwoordelijkheid van de Bestuurder)
* **de gekozen methode, het aanmaken van een RRNValideerder als Helper/Utility** klasse welke beschikt over een methode valideer

Bij instantiering van de Bestuurder klasse zal in de constructor body gebruik gemaakt worden van deze helper, indien validatie faalt zal er een ArgumentException gethrowt worden. Indien validatie succesvol is wordt het gevalideerdeRRN geretourneerd en de attribuut Rijksregisternummer in klasse Bestuurder gepopuleerd.

De [Single-Responsibility-Principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) voor deze helper is dus het valideren van een RRN.

#### FleetManager + Voertuig-, Bestuurder- & TankkaartController

Gebruikt patroon: [GRASP - controller](https://en.wikipedia.org/wiki/GRASP_(object-oriented_design))

Bij de eerste versies van het CD was er sprake van 1 enkele klasse (FleetManager) die instond voor het afhandelen van interacties van de presentatielaag.
Aangezien, bij eventuele uitbreidingen, het geheel onoverzichtelijk zou worden is besloten om de FleetManager op te splitsen in 3 Controller klassen en de FleetManager deze te laten beheren.

- VoertuigController
- BestuurderController
- TankkaartController

Elke Controller staat in voor:
- intermediaire rol tussen presentatielaag en datalaag
- waarborgen domeinregels (bijv. geen 2 personen met zelfde rijksregisternummer toegestaan)
- transformatie van data afkomstig van de datalaag naar Model objecten
- rapporteren over de uitgevoerde functie (geslaagd/niet geslaagd) en het retourneren van eventuele data naar de persistentielaag

Aangezien een controller in sommige gevallen (bijv updaten van Voertuig na het verwijderen van een/zijn Bestuurder) een andere controller moet kunnen aanspreken,
wordt als argument de fleetmanager meegegeven.

De FleetManager:
- beheert Controllers, eenvoudig uit te breiden indien nodig
- bevat de link tussen de business laag en de data laag door instantiering van DataManager

Indien de FleetManager niet zou bestaan is:
- interactie tussen controllers niet mogelijk (behalve door ze tig maal te instantieren)

*"Problem: Who should be responsible for handling an input system event?
Solution: A use case controller should be used to deal with all system events of a use case, and may be used for more than one use case. For instance, for the use cases Create User and Delete User, one can have a single class called UserController, instead of two separate use case controllers."*

*"The controller should delegate the work that needs to be done to other objects; it coordinates or controls the activity. It should not do much work itself."*

De beheerder "geeft het werk door" aan de Controller, welke op zijn beurt instaat voor het op correcte wijze aanroepen (coordineren) van andere controllers en functies in de datalaag.
De **verantwoordelijkheid van de Controller** is om te verzekeren dat de domeinregels gerespecteerd worden en werkt hiervoor samen met de datalaag en andere controllers.

*"Coupling is usually contrasted with cohesion. Low coupling often correlates with high cohesion, and vice versa. Low coupling is often thought to be a sign of a well-structured computer system and a good design, and when combined with high cohesion, supports the general goals of high readability and maintainability."*

Bij de FleetManager kan de cohesion als hoog aanschouwt worden (zijn taak is enkel en alleen beheren) en de coupling als low (zelfs indien het model 5 maal groter zou zijn in de toekomst) -- dit heeft volgens de definitie hierboven het resultaat dat readability en maintainability verbeterd zijn. Indien de FleetManager de verantwoordelijke geweest zou zijn voor zowel het beheer als het coordineren zou dit niet het geval zijn en uitbreidingen in de weg staan.

De Controllers kunnen zich focussen op het coordineren van business events door gebruik te maken van het beheer van de FleetManager.

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
