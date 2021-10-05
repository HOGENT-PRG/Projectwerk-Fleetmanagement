# Architectuur

## Doel van de pagina

Deze pagina dient louter om zaken **tijdens** het ontwikkelingsproces nader te verklaren en bevat elementen
die opgenomen **kunnen** worden in de Documentatie **nadat** zij al dan niet geimplementeerd werden.

## Presentatielaag

placeholder

## Unit testing

placeholder

## Business laag

### FleetManager & IFleetManager


Gebruikt patroon: [GRASP - controller](https://en.wikipedia.org/wiki/GRASP_(object-oriented_design))

Het controller patroon heeft de verantwoordelijkheid om om te gaan met systeem operaties en bevindt zich typisch onder het niveau van de user interface.

In dit geval is deze klasse het primaire aanspreekpunt van de WPF applicatie.

*"Problem: Who should be responsible for handling an input system event?
Solution: A use case controller should be used to deal with all system events of a use case, and may be used for more than one use case. For instance, for the use cases Create User and Delete User, one can have a single class called UserController, instead of two separate use case controllers."*

In dit geval bevindt de Controller zich op het niveau van de business laag en is de verantwoordelijkheid gegeneraliseerd, dit aangezien het gebruikt kan worden om meerdere use case's af te handelen onder 1 dak.

Een alternatieve implementatiemethode zou kunnen zijn om de Manager op te splitsen in:
- VoertuigController
- BestuurderController
- TankkaartController

.. welke de applicatie overzichtelijker zou maken indien het in de toekomst onderwerp wordt van uitbreidingen.

*"The controller should delegate the work that needs to be done to other objects; it coordinates or controls the activity. It should not do much work itself."*

De Manager roept functies aan van de Data Laag en:
- vormt om in objecten (EF?)
- retourneert de status van een CRUD operatie samen met eventuele objecten of error messages

*"Coupling is usually contrasted with cohesion. Low coupling often correlates with high cohesion, and vice versa. Low coupling is often thought to be a sign of a well-structured computer system and a good design, and when combined with high cohesion, supports the general goals of high readability and maintainability."*

Als men de coupling van deze klasse echter in acht neemt (High coupling) vormt het een relatief sterk argument om de klasse alsnog onder te verdelen zoals hierboven vermeld staat. 

## Data laag
