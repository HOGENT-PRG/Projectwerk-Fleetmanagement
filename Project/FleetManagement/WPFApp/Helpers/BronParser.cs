using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using WPFApp.Exceptions;
using Newtonsoft.Json;
using System.Xml;
using System.Collections;

namespace WPFApp.Helpers {
    public static class BronParser {

        // Deze functie werd ontwikkeld maar wordt nergens gebruikt
        // Laat toe om een JObject om te vormen naar een geneste dictionary
        public static object MaakNestedDictionaryVanJObject(object reqObj) {
            switch (reqObj) {
                case JObject jObject:
                    return (
                        (IEnumerable<KeyValuePair<string, JToken>>)jObject)
                        .ToDictionary(j => j.Key, j => MaakNestedDictionaryVanJObject(j.Value)
                    );
                case JArray jArray:
                    return jArray.Select(MaakNestedDictionaryVanJObject).ToList();
                case JValue jValue:
                    return jValue.Value;
                default:
                    // Indien het type geen van bovenstaande is wordt een exception geworpen
                    throw new BronParserException($"Dit type kan niet geparsed worden: {reqObj.GetType()}");
            }
		}

        // Wordt gebruikt bij de omzettingen van/naar DTO's
        // Bewijst zijn nut bij ApiCommuniceerder, waar het gebruikt wordt om van ontvangen json het schema te valideren en om nulls weg te laten middels toJsonFilterNulls en dat indien men wenst om te vormen naar een json string.
        // Wordt ook gebruikt door de BusinessCommuniceerder om omzettingen te doen van/naar DTO/Domeinmodel. Dit laat toe om, indien de gelegenheid zich voordoet, centraal wijzigingen in deze functie aan te brengen zonder hiervoor meerdere wijzigingen aan te brengen aan de communiceerder zelf.

        // T                    Het type waarnaar gecast wordt
        // data                 Het oorspronkelijke object
        // validatieSchema      Een optioneel JSchema waarmee de structuur van data gevalideerd wordt
        // toJsonFilterNulls    Indien er omgezet wordt naar string (json) bepaalt dit argument
        //                      of nulls aanwezig in data geen deel uitmaken van de
        //                      resulterende json string
        //                      Dit argument wordt genegeerd indien er niet gecast
        //                      wordt naar een json string
		public static T ParseCast<T>(object data, JSchema ValidatieSchema=null, bool toJsonFilterNulls=true) {

            var JSS = new JsonSerializer {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // https://dotnetfiddle.net/frCSa3
            };

            JObject parsed_as_json = null;
            JObject parsed_as_obj = null;

            try { 
                if (data.GetType() == typeof(string)) 
                    { parsed_as_json = JObject.Parse((string)data); 
                } 
            } catch(JsonReaderException) { }

            try { 
                parsed_as_obj = JObject.FromObject(data, JSS); 
            } catch (ArgumentException e) when (e.Message.Contains("JObject instance expected")) { }
            
            JObject parseResultaat = parsed_as_json is not null 
                                     ? parsed_as_json 
                                     : parsed_as_obj is not null 
                                         ? parsed_as_obj 
                                         : throw new BronParserException(
                                            "Kon de data niet parsen. " +
                                            "Indien je een JSON string wenst te parsen, " +
                                            "controleer of je JSON valid is."
                                           );

            if (ValidatieSchema is not null) {
                if (!parseResultaat.IsValid(ValidatieSchema)) {
                    throw new BronParserException(
                        "Het JSON object voldoet niet aan de voorwaarden " +
                        "opgelegd in het meegegeven schema."
                    );
                }
            }

            /**Optie 1 - omzetten naar json string**/
            if (typeof(T) == typeof(string)) {                                          
                string filteredResultaat;

                if (toJsonFilterNulls) {
                    // Omweg (from+to) vereist aangezien anders nulls niet genegeerd worden
                    // (komt door JTokenType.Null)
                    // https://stackoverflow.com/questions/33027409/json-net-serialize-jobject-while-ignoring-null-properties
                    filteredResultaat = JObject.FromObject(
                                            parseResultaat.ToObject(typeof(object)),
                                            JSS
                                        ).ToString();
                } else {
                    filteredResultaat = parseResultaat.ToString();
                }

                return (T)Convert.ChangeType(filteredResultaat, typeof(T));

            /**Optie 2 - specifiek omzetten naar een object met type T **/
            } else if ((typeof(T) == typeof(JObject) || typeof(T).GetType().IsClass) 
                        && typeof(T) != typeof(object)) {            // https://dotnetfiddle.net/puxB1a
                return (T)parseResultaat.ToObject(typeof(T));

            /**Optie 3 - generiek omzetten naar een "object" **/
            } else {                                                                    
                return (T)parseResultaat.ToObject(typeof(object)); 
            }

            throw new BronParserException("Kon de data niet parsen.");
        }
    }
}


