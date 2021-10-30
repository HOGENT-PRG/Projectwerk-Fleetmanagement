using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Interfaces;
using WPFApp.Model.Response;

// Nog niet getest! <<<<<<<<<<---------------------------------------------------------------------------------
// Probeersel, indien dit niet performant is of niet werkt, wellicht aangewezen iets anders te bedenken
// TODO : Zoekmachine testen en performantie bepalen
// datetime afhandelen / parsen ? 

namespace WPFApp.Model {
    public class Zoekmachine {
        private static ICommuniceer _communicatieKanaal;

        private static string gemeenschappelijkeIdentificator = "ResponseDTO";
        private static string diepteSeparator = " >> ";

        private static readonly HashSet<Type> _relationeleTypes = 
            new HashSet<Type> {
                typeof(AdresResponseDTO), typeof(BestuurderResponseDTO),
                typeof(TankkaartResponseDTO), typeof(VoertuigResponseDTO)
            };

        public Zoekmachine(ICommuniceer communicatieKanaal) {
            _communicatieKanaal = communicatieKanaal;
        }
        
        public List<string> GeefZoekfilterVelden(Type DTOType) {
            List<string> velden = new();
            List<string> diepeVelden = new();

            if (_relationeleTypes.Contains(DTOType)) {
                PropertyInfo[] properties = DTOType.GetProperties();

                foreach (PropertyInfo p in properties) {
                    if (p.PropertyType == typeof(DateTime?)) { continue; }

                    if (_relationeleTypes.Contains(p.PropertyType)) {
                        string veldPrefix = p.Name.Replace(gemeenschappelijkeIdentificator, "") + diepteSeparator;
                        PropertyInfo[] diepeProperties = p.PropertyType.GetProperties();

                        foreach (PropertyInfo dp in diepeProperties) {
                            if (dp.PropertyType == typeof(DateTime?)) { continue; }
                            if (!_relationeleTypes.Contains(dp.PropertyType)) {
                                diepeVelden.Add(veldPrefix + dp.Name);
                            }
                        }
                    } else {
                        velden.Add(p.Name);
                    }
                }
            } else { throw new ArgumentException("Er kunnen geen zoekfilter velden voor dit type opgevraagd worden."); }

            

            velden.AddRange(diepeVelden);
            return velden;
        }

        // Diepte 0 = DTO -> DTO
        // Diepte 1 = DTO -> DTO -> DTO
        private object _geefWaardeVanPropertyRecursief(Type targetType, string propertyNaam, object instantie, int diepte=0) {
            int diepteMax = 0;
            var instantieType = instantie.GetType();

            // We overlopen alle properties van de master
            // We overlopen alle properties van een geneste instantie (recursief zichzelf opgeroepen)
            foreach (var property in instantieType.GetProperties()) {

                // Indien de property met het correcte type gevonden is (targetType = property type) - of
                // Indien de property zich in de master bevind en discrimineren niet nodig is (targetType = instantie type)
                // Indien er sprake is van recursie en de nieuwe instantie van het targetType is (targetType = instantie type)
                if((targetType == property.PropertyType || targetType == instantie.GetType()) && targetType is not null) {
                    var waarde = property.GetValue(instantie, null);

                    // Indien het gaat om een custom type
                    // (kan evt vervangen worden door controle met _relationeleTypes)
                    if (property.PropertyType.FullName != "System.String"
                        && !property.PropertyType.IsPrimitive
                        && diepte <= diepteMax) {

                        // Het gaat om een DTO in een DTO (of een ander complex type, zoals een dict)
                        // We geven deze DTO recursief mee, om er de property value uit te halen (bij diepte=0)
                        return _geefWaardeVanPropertyRecursief(targetType, propertyNaam, waarde, diepte++);

                    } else if (property.Name == propertyNaam) {
                        return waarde;
                    }
                } // -> else:  We slaan de property over omdat het het correcte targettype niet is
                  // en omdat we niet foutief de property van de master willen aanzien als dat van het
                  // genest type (bv: Bestuurder.Id aanzien wanneer we op zoek zijn naar Bestuurder.Tankkaart.Id)


            }

            // We hebben de waarde niet kunnen bepalen
            return null;
        }

        // Werkt met reflectie, is mogelijk helemaal niet performant met grote datasets
        public List<IResponseDTO> ZoekMetFilter(Type DTOType, string zoekfilter, string zoekterm) {
            if (!_relationeleTypes.Contains(DTOType)) { throw new ArgumentException("Dit type kan niet gebruikt worden bij het zoeken."); }


            List<IResponseDTO> resultaat = new();
            IList data = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(DTOType));
            string genesteProperty = "_invalid";
            Type targetType = null;

            if (zoekfilter.Contains(diepteSeparator)) {
                string[] zoekfilterArgs = zoekfilter.Split(diepteSeparator);
                string klassenaam = zoekfilterArgs[0] + gemeenschappelijkeIdentificator;

                targetType = Type.GetType(klassenaam) ?? throw new ArgumentException("Kan geen klassenaam bepalen.");
                genesteProperty = zoekfilterArgs[1];
            } else {
                targetType = DTOType;
            }

            // We krijgen DTO's terug welke we kunnen behandelen
            if (DTOType == typeof(BestuurderResponseDTO)) { data = _communicatieKanaal.geefBestuurders(inclusiefRelaties:true); }
            else if (DTOType == typeof(TankkaartResponseDTO)) { data = _communicatieKanaal.geefTankkaarten();}
            else if (DTOType == typeof(VoertuigResponseDTO)) { data = _communicatieKanaal.geefVoertuigen(); }
            else if (DTOType == typeof(AdresResponseDTO)) { data = _communicatieKanaal.geefAdressen(); } 
            else { throw new ArgumentException("Dit type kan niet gebruikt worden bij het zoeken"); }

            // Aangezien DTO's DTO's kunnen bevatten en we hun property names ook aanbieden als zoekfilter,
            // recursief waarde proberen te vergaren met reflectie (max depth = 0)
            // (Mogelijk niet performant bij grote hoeveelheden in databank)
            foreach (IResponseDTO b in data) {
                var res = _geefWaardeVanPropertyRecursief(targetType, genesteProperty, b);
                if(res is not null) {
                    if ((string)res == zoekterm) {
                        resultaat.Add(b);
                    }
                }
                
            }

            return resultaat;

            //if (DTOType == typeof(BestuurderResponseDTO)) {
            //    List<BestuurderResponseDTO> data = _communicatieKanaal.geefBestuurders(true).ToList();
            //    foreach(BestuurderResponseDTO b in data) {
            //        if((string)_geefWaardeVanKlasseProperty(genesteProperty, b) == zoekterm) {
            //            resultaat.Add(b);
            //        }
            //    }
            //} else if (DTOType == typeof(TankkaartResponseDTO)) {
            //    List<TankkaartResponseDTO> data = _communicatieKanaal.geefTankkaarten();
            //} else if (DTOType == typeof(VoertuigResponseDTO)) {
            //    List<VoertuigResponseDTO> data = _communicatieKanaal.geefVoertuigen();
            //} else if (DTOType == typeof(AdresResponseDTO)) {
            //    List<AdresResponseDTO> data = _communicatieKanaal.geefAdressen();
            //} else { throw new ArgumentException("Dit type kan niet gebruikt worden bij het zoeken"); }
        }



    }
}
