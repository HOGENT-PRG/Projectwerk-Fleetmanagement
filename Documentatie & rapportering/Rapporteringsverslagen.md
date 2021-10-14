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
* Rijksregisternummer validatie : Abdellah
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

### To-do (in geen specifieke volgorde)

* Schrijven van unit tests voor business laag
* * Aan de hand van unit test resultaten de business laag verbeteren
* Methodes toevoegen aan Model klasses indien muteerbaarheid gewenst is
* Verderwerken aan user interface mockups
* ~~Data Laag klassendiagram~~
* Ontwikkelen valideer functie RRNValideerder

### Obstakels
* Branches beheren en pull requests maken (force push incident)

### Demo
* NVT



## Week 3 - 14/10-21/10

### Planning

* t

### Realisaties

* t

### To-do (in geen specifieke volgorde)

*

### Obstakels

* t

### Demo
* (Unit testen + UI design) ???
