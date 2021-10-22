using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using WPFApp.Exceptions;
using Newtonsoft.Json;

namespace WPFApp
{
    internal static class ExterneBronParser {
        private static object _maakNestedObjectVanJObject(object reqObj) {
            switch (reqObj) {
                case JObject jObject:
                    return ((IEnumerable<KeyValuePair<string, JToken>>)jObject).ToDictionary(j => j.Key, j => _maakNestedObjectVanJObject(j.Value));
                case JArray jArray:
                    return jArray.Select(_maakNestedObjectVanJObject).ToList();
                case JValue jValue:
                    return jValue.Value;
                default:
                    throw new ExterneBronParserException($"Dit type kan niet geparsed worden: {reqObj.GetType()}");
            }
        }

        public static Dictionary<string, object> Parse(object data,JSchema ValidatieSchema=null,
            bool InkomendeBronJsonStringVerwacht=true, bool retourneerJObject=true)
        {
            JObject parsed_as_json = null;
            JObject parsed_as_obj = null;

    /**/    if (InkomendeBronJsonStringVerwacht) 
                 try { parsed_as_json = JObject.Parse((string)data); } catch { }
            else try { parsed_as_obj = JObject.FromObject(data); } catch { }
            

    /**/    if (parsed_as_obj == null && parsed_as_json == null) {
                throw new ExterneBronParserException("Kon de data niet parsen. Staat InkomendeBronJsonStringVerwacht juist ingesteld?");
            }

    /**/    if (InkomendeBronJsonStringVerwacht && parsed_as_json is not null) {
                if (ValidatieSchema is not null)
                    if (!parsed_as_json.IsValid(ValidatieSchema))
                        throw new ExterneBronParserException("Het JSON object voldoet niet aan de voorwaarden opgelegd in het meegegeven schema.");

                if (retourneerJObject) return new Dictionary<string, object>() { { "JObject", parsed_as_json } };
                else return (Dictionary<string, object>)_maakNestedObjectVanJObject(parsed_as_json);

            }

    /**/    if(!InkomendeBronJsonStringVerwacht && parsed_as_obj is not null){
                if(retourneerJObject)
                    return new Dictionary<string, object>() { { "JObject", parsed_as_obj } };
                else return (Dictionary<string, object>)_maakNestedObjectVanJObject(parsed_as_obj);
            }

            throw new ExterneBronParserException("Kon de data niet parsen.");
        }
    }
}

    // https://en.wikipedia.org/wiki/Mediator_pattern
    internal class InteractionMediator
    {
        // In tegenstelling tot de FleetManager die
        // - initieert en beschikbaar stelt (managers & DatabankConfigureerder)
        // fungeert de mediator als funnel met "passthrough functies" en additionele data parsing.
        //
        // Indien de applicatie in een later stadium losgekoppeld zou worden
        // van de managers, zal dit de plaats zijn om api call request initierende functies
        // te callen / parsing configuraties te wijzigen
        // welke hetzelfde gewenste resultaat opleveren, in plaats van de manager functies.
        // 
        // Hierboven een voorlopige experimentele, ongeteste static class die
        // parsed naar een dictionary.
        // 
        // Eventueel wordt de json scheme gevalideert en indien gewenst omgezet in een
        // een recursief samengestelde Dictionary<string, object>  
        // Het is ook mogelijk om in de plaats daarvan rechtstreeks een dict met <"JObject", JObject>
        // te returnen

        public InteractionMediator()
        {

        }
    }

