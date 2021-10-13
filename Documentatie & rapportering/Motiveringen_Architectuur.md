# Architectureel design (beslissingen / motivaties)

## Globale motiveringen

### Layer dependencies

De dependencies staan ingesteld als volgt:
![Dependencies Visualized](https://i.imgur.com/hR0gUvS.png)

In de WPF applicatie wordt de FleetManager geinstantieerd welke Managers bevat, deze Managers krijgen in de constructor deze een instantie van de relevante repository mee. (bv BestuurderController ontvangt BestuurderRepository)

Hierdoor kunnen de functionaliteiten van de Data Laag gebruikt worden in de Business Laag zonder hiervoor een dependency nodig te hebben.

**De Data Laag kent**, kent dankzij zijn dependency **van de Business Laag het model** en kan dus aan de hand van **data uit de databank, deze direct omvormen naar Model objecten**, dat was bij de vorige dependency-constructie niet het geval (daar was de Business Laag dependent op de Data Laag om zijn functies te kunnen aanroepen **dit tov** het huidige model waarbij interfaces ingezet worden die door de Managers gebruikt worden om te interageren met de databank)

Een interactie met de WPF Applicatie resulteert in een function call binnen de BusinessLaag.
De Business Laag past de domeinregels toe en interageert met de meegegeven repository om het doel van de aangeroepen functie te voltooien.

![Solution Layers](https://media.discordapp.net/attachments/893108471011090502/895043004274974780/1920px-Overview_of_a_three-tier_application_vectorVersion.png)

## Design klassen

>TODO: class diagrams invoegen

### Business laag

#### Helper **RRNValideerder**

#### **FleetManager + Voertuig-, Bestuurder- & TankkaartManager**

Gebruikt patroon: [GRASP - controller](https://en.wikipedia.org/wiki/GRASP_(object-oriented_design))

Bij de eerste versies van het CD was er sprake van 1 enkele klasse (FleetManager) die instond voor het afhandelen van alle interacties van de presentatielaag. Dit is reeds veranderd en de FleetManager heeft een duidelijk takenpakket.

Bij het opstarten van de applicatie wordt de Presentatie Laag **eenmalig** ingezet om Data Laag objecten, namelijk Repositories, aan te maken, deze worden tijdens instantiering van de FleetManager "bezorgt" aan de Business Laag. 

Deze voorgenoemde instantie van de FleetManager zal het centrale punt zijn waarmee de Presentatielaag interageert, het laat toegang tot alle functionaliteiten van iedere Manager toe. In de constructor er van worden alle soorten Managers geinstantieerd en dit met voorgenoemd Repository object.

Om de Business Laag (specifiek, de Managers) kennis te geven over welke methodes er beschikbaar zijn, worden interfaces voorzien.

**De soorten Managers welke de Presentatielaag mee in aanraking komt zijn als volgt:**

- de VoertuigManager, maakt gebruik van volgende objecten: IVoertuigRepository en de ouderlijke klasse FleetManager (ingesteld tijdens constructie)
- de BestuurderManager, maakt gebruik van volgende objecten: IBestuurderRepository en de ouderlijke klasse FleetManager (ingesteld tijdens constructie)
- de TankkaartManager, maakt gebruik van volgende objecten: ITankkaartRepository en de ouderlijke klasse FleetManager (ingesteld tijdens constructie)

**Elke Manager staat in voor:**
- intermediaire rol tussen presentatielaag en datalaag
- waarborgen domeinregels (bijv. geen 2 personen met zelfde rijksregisternummer toegestaan)

**Elke Repository staat in voor:**
- het interageren met de databank
- het deconstrueren van Model objecten naar een query
- het construeren van Model objecten aan de hand van data afkomstig van een uitgevoerd query

Het is dus de verantwoordelijkheid van de Managers om een mening te vormen (condities) over de objecten die geretourneerd worden en beslissingen te nemen.
De Repository is simpelweg de SQL expert, welke objecten afbouwt, queries uitvoert en met de resultaten weer objecten opbouwt.

**Aangezien de Manager beslissingen neemt kan het voorvallen dat hiervoor een andere Manager benodigt is**, deze kan aangeroepen worden dmv de FleetManager die bij constructie meegegeven was.

**De FleetManager:
- **Instantieert** (voorziet Managers van van Repositories en zichzelf) **en beheert Managers** (staat open als aanspreekpunt door een andere laag)
- Is eenvoudig uit te breiden indien nodig
- Voert de **StartSequence** uit

**Indien de FleetManager niet zou bestaan is:**
- Interactie tussen Managers is niet op eenvoudige wijze mogelijk (het alternatief is dan ook om ze tig maal opnieuw te instantieren in elke klasse)

*"Problem: Who should be responsible for handling an input system event?
Solution: A use case controller should be used to deal with all system events of a use case, and may be used for more than one use case. For instance, for the use cases Create User and Delete User, one can have a single class called UserController, instead of two separate use case controllers."*

*"The controller should delegate the work that needs to be done to other objects; it coordinates or controls the activity. It should not do much work itself."*

De beheerder "geeft het werk door" aan de Manager, welke op zijn beurt instaat voor het op correcte wijze aanroepen (coordineren) van andere Managers en functies in de datalaag.
De **verantwoordelijkheid van de Manager** is om te verzekeren dat de domeinregels gerespecteerd worden en werkt hiervoor samen met de datalaag en andere Managers.

*"Coupling is usually contrasted with cohesion. Low coupling often correlates with high cohesion, and vice versa. Low coupling is often thought to be a sign of a well-structured computer system and a good design, and when combined with high cohesion, supports the general goals of high readability and maintainability."*

Bij de FleetManager kan de cohesion als hoog aanschouwt worden (zijn taak is enkel en alleen beheren) en de coupling als low (zelfs indien het model 5 maal groter zou zijn in de toekomst, is het nog een begrijpbare klasse) -- dit heeft volgens de definitie hierboven het resultaat dat readability en maintainability verbeterd zijn. Indien de FleetManager de verantwoordelijke geweest zou zijn voor zowel het beheer als het werk van de Managers, het coordineren, zou dit niet het geval zijn en uitbreidingen in de weg staan.

### Data laag

#### Voorbeeldklasse

text

### Presentatie laag

#### Voorbeeldklasse

text
