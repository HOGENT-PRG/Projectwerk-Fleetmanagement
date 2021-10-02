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

text

### Businesslaag 

#### Klasse **Rijksregisternummer** 

Het Rijksregisternummer (RRN) dient gevalideerd te worden aan de hand van [dit document](https://nl.wikipedia.org/wiki/Rijksregisternummer).

Er waren enkele mogelijkheden om dit te bereiken, als volgt:
* het RRN valideren in de FleetManager klasse - aangezien dit niet de verantwoordelijkheid van deze klasse is dit ondergeschikt
* het RRN valideren in de Bestuurder klasse aangezien het RRN toebehoort aan deze persoon, aangezien de valideerfunctie vrij uitgebreid zal worden is dit niet de meest optimale beslissing (maar er kan echter wel beargumenteerd worden dat dit een correcte aanpak zou zijn)
* **de gekozen methode, het aanmaken van een Rijksregisternummer** klasse welke beschikt over een private methode valideer die aangeroepen wordt wanneer het object aangemaakt wordt, indien validatie faalt zal er een exception gethrowt worden (die op gepaste wijze opgevangen wordt in de FleetManager en gereturnt wordt naar de Presentatielaag)

De [Single-Responsibility-Principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) voor deze klasse is dus het valideren van de input tijdens aanmaak van het object.

De klasse beschikt daarnaast over een override functie ToString, welke het RRN retourneert in string formaat.


#### Klasse **FleetManager**

De algemene architectuur doet denken aan de opdracht Adresbeheer van het eerste jaar, in dat project was er in de Business Layer sprake van een Adresbeheerder welke als intermediair fungeerde tussen de Data en WPF laag.

Op een gelijkaardige manier kan dus de FleetManager dezelfde functie uitoefenen. 

Voorbeeld van structuur in AdresBeheer:

![Structuur](https://cdn.discordapp.com/attachments/893498024972673044/893502925773627412/unknown.png)

**DOCUMENTATIE NOG UIT TE BREIDEN VOOR DEZE KLASSE NADAT FUNCTIES ONTWIKKELD ZIJN**

### Persistentielaag

text


## Frontend design (WPF)

text
