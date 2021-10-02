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

text

### Persistentielaag

text


## Frontend design (WPF)

text