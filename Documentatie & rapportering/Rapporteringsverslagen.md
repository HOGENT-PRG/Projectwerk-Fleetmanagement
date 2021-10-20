# Verslagen & versies


## Doel van verslagen

"Elke week wordt er een stand van zaken gegeven (planning / realisaties / obstakels / demo)"

## Week 1 - 30/09-07/10

### Planning

*   Meeting gepland op Discord (zaterdag 02/10)
*   Fysieke meeting gepland (woensdag 06/10)

### Realisaties

*   Github repository aangemaakt, rollen toegekend, filestructuur gecreerd
*   Eerste versie van het klassendiagram business laag (met relaties en enkele methodes)
*   Aanmaken van het VS project en toevoegen van lagen, configureren van de dependencies
*   Maken van 2 user interface mockups (SplashScreen, Bestuurders): [UI_mockups](https://github.com/HOGENT-PRG/Projectwerk-Fleetmanagement/tree/main/UI_mockups)
*   Eerste versie klasses business laag

### To-do (in geen specifieke volgorde)

* UI mockups maken
* Unit tests voor business laag v1
* Rijksregisternummer klasse + valideer functie
* Logica Controllers + Fleetmanager

### Obstakels

*   Hoe zoeken we eenvoudig een object op in de databank ("Bij het maken van lijstweergaves in de applicatie moet het telkens
mogelijk zijn om op alle zichtbare velden in de lijst te kunnen filteren.") - Huidige gedachtegang gaat uit naar query + Type.GetProperties (reflection) + linq
*   Wat is de beste aanpak mbt branches en synchroon samenwerken

### Demo

*   NVT

## Week 2 - 07/10-14/10

### Planning

* ~~Klassendiagram Data Laag : Robrecht~~ OK
* ~~Rijksregisternummer validatie : Abdellah~~ OK
* Unit tests model business laag (**NOK: overgedragen naar volgende week**) ~~& rechtzetting property visibilities business laag~~ (OK) : Benjamin

### Realisaties

* Aanpassen layer dependencies (zie [Motivering_Architectuur.md](https://github.com/HOGENT-PRG/Projectwerk-Fleetmanagement/blob/main/Documentatie%20%26%20rapportering/Motiveringen_Architectuur.md))
* Controllers hernoemd naar Managers
* Repositories toegevoegd in de data laag (met NotImplemented methodes)
* De Managers en Repositories voorzien van interfaces
* De Managers en Repositories voorzien van custom exceptions
* FleetManager voorzien van inline commentaar
* Aanmaak van StartSequence klasse
* Herwerken van klassendiagram zodat deze de codebase accuraat representeert: toevoegen layer dependencies, corrigeren visibilities properties, toevoegen interfaces, toevoegen data laag klasses (StartSequence nog niet inbegrepen)
* Toevoegen logica in rijksregisternummer

### To-do (op volgorde)

* Zie volgende week

### Obstakels
* Branches beheren en pull requests maken (force push incident)

### Demo
* NVT



## Week 3 - 14/10-21/10

### Planning

#### Op volgorde

* ~~Toevoegen methodes aan Model klassendiagram **Benjamin**
* ~~^ Indien voltooid, toevoegen Adres klasse en methoden implementeren in Model **Abdellah**
* ~~Schrijven van unit tests voor business laag Model **Benjamin**
* ~~* Aan de hand van unit test resultaten de business laag verbeteren **Benjamin**

#### Gedurende de week

* ~~Er dient vanaf deze week een powerpoint aangemaakt te worden van enkele slides die de vooruitgang samenvat
* Ontwerp klassendiagram Presentatielaag in apart VP project om conflicten te vermijden **Robrecht**
* ~~Veranderen van Engelstalige benamingen naar Nederlands in klassendiagram en project **Benjamin**
* ~~De StartupSequence klasse achterwege laten (truncate te riskant om te automatiseren in geval van applicatie in productie) en een DatabaseConfigureerder klasse maken 
*  ~~^ Truncate table en mock data functies toewijden aan unit testing project (evt gebruik van aparte debug db met zelfde design) **Benjamin**

#### Indien taken al klaar zijn

* Database design beginnen
* Dit design vertalen naar SQL
* ~~Eventuele aanmaak van een InitializationManager (+ InitializationRepository ) met als taken:
* ~~* Controle of databank bestaat, zonee wordt een nieuwe databank gemaakt ( https://stackoverflow.com/a/39500898/8623540 )
* ~~* Controle of databank over de juiste tabellen beschikt, indien nee worden deze aangemaakt ( Voorbeeld van query https://pastebin.com/raw/tTciV7Cb )
* Verderwerken aan user interface mockups (lage prioriteit, database design krijgt voorrang)

### Realisaties

* Toevoegen methodes aan Model klassendiagram 
* Toevoegen Adres klasse en methoden implementeren in Model 
* Schrijven van unit tests voor business laag Model 
* Aan de hand van unit test resultaten de business laag verbeteren 
* Powerpoint
* Veranderen van Engelstalige benamingen naar Nederlands in klassendiagram en project
* Aanmaken databaseconfigureerder + variant voor unit testing 

### To-do volgende week

* Database design beginnen
* Dit design vertalen naar SQL
* Logica Managers beginnen
* Verderwerken aan user interface mockups (lage prioriteit, database design krijgt voorrang)
* Ontwerp klassendiagram Presentatielaag 

### Obstakels

* 

### Demo
* PPT: Unit testen, model klassen, dbconfig
