# Architectureel design (beslissingen / motivaties)

## Globale motiveringen

### Layer dependencies

De dependencies staan ingesteld als volgt:
![Dependencies Visualized](https://i.imgur.com/hR0gUvS.png)

In de WPF applicatie worden de Managers geinstantieerd, in de constructor krijgen deze een instantie van de relevante repository mee. (bv BestuurderController ontvangt BestuurderRepository)

Hierdoor kunnen de functionaliteiten van de Data Laag gebruikt worden in de Business Laag zonder hiervoor een dependency nodig te hebben.
De Data Laag kent, dankzij zijn dependency van de Business Laag het model en kan dus aan de hand van data uit de databank, deze direct omvormen naar Model objecten.

Een interactie met de WPF Applicatie resulteert in een function call binnen de BusinessLaag.
De Business Laag past de domeinregels toe en interageert met de meegegeven repository om het doel van de aangeroepen functie te voltooien.

![Solution Layers](https://media.discordapp.net/attachments/893108471011090502/895043004274974780/1920px-Overview_of_a_three-tier_application_vectorVersion.png)

## Design klassen

>TODO: class diagrams invoegen

### Business laag

#### Helper **RRNValideerder**

#### **FleetManager + Voertuig-, Bestuurder- & TankkaartManager**

Gebruikt patroon: [GRASP - controller](https://en.wikipedia.org/wiki/GRASP_(object-oriented_design))

Bij de eerste versies van het CD was er sprake van 1 enkele klasse (FleetManager) die instond voor het afhandelen van interacties van de presentatielaag.
Aangezien, bij eventuele uitbreidingen, het geheel onoverzichtelijk zou worden is besloten om de FleetManager op te splitsen in 3 Manager klassen en de FleetManager deze te laten beheren.

- VoertuigManager
- BestuurderManager
- TankkaartManager

Elke Manager staat in voor:
- intermediaire rol tussen presentatielaag en datalaag
- waarborgen domeinregels (bijv. geen 2 personen met zelfde rijksregisternummer toegestaan)
~~- transformatie van data afkomstig van de datalaag naar Model objecten~~ → (dit is nu verantwoordelijkheid van DAL)
- rapporteren over de uitgevoerde functie (geslaagd/niet geslaagd) en het retourneren van eventuele data naar de persistentielaag.

Aangezien een Manager in sommige gevallen (bijv updaten van Voertuig na het verwijderen van een/zijn Bestuurder) een andere Manager moet kunnen aanspreken,
wordt als argument de fleetmanager meegegeven.

De FleetManager:
- beheert Managers, eenvoudig uit te breiden indien nodig

Indien de FleetManager niet zou bestaan is:
- interactie tussen Managers niet mogelijk (behalve door ze tig maal te instantieren)

*"Problem: Who should be responsible for handling an input system event?
Solution: A use case controller should be used to deal with all system events of a use case, and may be used for more than one use case. For instance, for the use cases Create User and Delete User, one can have a single class called UserController, instead of two separate use case controllers."*

*"The controller should delegate the work that needs to be done to other objects; it coordinates or controls the activity. It should not do much work itself."*

De beheerder "geeft het werk door" aan de Manager, welke op zijn beurt instaat voor het op correcte wijze aanroepen (coordineren) van andere Managers en functies in de datalaag.
De **verantwoordelijkheid van de Manager** is om te verzekeren dat de domeinregels gerespecteerd worden en werkt hiervoor samen met de datalaag en andere Managers.

*"Coupling is usually contrasted with cohesion. Low coupling often correlates with high cohesion, and vice versa. Low coupling is often thought to be a sign of a well-structured computer system and a good design, and when combined with high cohesion, supports the general goals of high readability and maintainability."*

Bij de FleetManager kan de cohesion als hoog aanschouwt worden (zijn taak is enkel en alleen beheren) en de coupling als low (zelfs indien het model 5 maal groter zou zijn in de toekomst) -- dit heeft volgens de definitie hierboven het resultaat dat readability en maintainability verbeterd zijn. Indien de FleetManager de verantwoordelijke geweest zou zijn voor zowel het beheer als het coordineren zou dit niet het geval zijn en uitbreidingen in de weg staan.

De Managers kunnen zich focussen op het coordineren van business events door gebruik te maken van het beheer van de FleetManager.

### Data laag

#### Voorbeeldklasse

text

### Presentatie laag

#### Voorbeeldklasse

text
