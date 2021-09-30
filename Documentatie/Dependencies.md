Even ter info met betrekking tot het VS project (normaal zouden de dependencies reeds juist moeten staan):

WPFApp **DEPENDS ON** BusinessLaag **DEPENDS ON** DataLaag

Een interactie met de WPF Applicatie resulteert dus in een function call binnen de BusinessLaag.
Een operatie binnen de Businesslaag die data vereist uit de databank resulteert dus in een function call binnen de DataLaag.