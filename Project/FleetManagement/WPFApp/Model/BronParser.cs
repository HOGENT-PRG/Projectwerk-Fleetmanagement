using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using WPFApp.Exceptions;
using WPFApp.Model.Response;

namespace WPFApp.Model {
    internal static class BronParser {
        public static object MaakNestedDictionaryVanJObject(object reqObj) {
            switch (reqObj) {
                case JObject jObject:
                    return ((IEnumerable<KeyValuePair<string, JToken>>)jObject).ToDictionary(j => j.Key, j => MaakNestedDictionaryVanJObject(j.Value));
                case JArray jArray:
                    return jArray.Select(MaakNestedDictionaryVanJObject).ToList();
                case JValue jValue:
                    return jValue.Value;
                default:
                    throw new BronParserException($"Dit type kan niet geparsed worden: {reqObj.GetType()}");
            }
        }

        public static T Parse<T>(object data,
            bool IsJsonString = true, Type castNaarDTOType = null,
            JSchema ValidatieSchema=null){

            List<Type> validTypes = new() { typeof(string), typeof(object), typeof(JObject), castNaarDTOType };
    /**/    if (!validTypes.Contains(typeof(T)))
                throw new BronParserException("Omzetten slechts mogelijk naar json-string, object of DTO type.");

            JObject parsed_as_json = null;
            JObject parsed_as_obj = null;

    /**/    if (IsJsonString) 
                 try { parsed_as_json = JObject.Parse((string)data); } catch { }
            else try { parsed_as_obj = JObject.FromObject(data); } catch { }
            

    /**/    if (parsed_as_obj == null && parsed_as_json == null) {
                throw new BronParserException("Kon de data niet parsen. Staat IsJsonString juist ingesteld?");
            }

    /**/    if (parsed_as_json is not null) {
                if (ValidatieSchema is not null)
                    if (!parsed_as_json.IsValid(ValidatieSchema))
                        throw new BronParserException("Het JSON object voldoet niet aan de voorwaarden opgelegd in het meegegeven schema.");

                if(castNaarDTOType is not null) return (T)parsed_as_json.ToObject(castNaarDTOType);
                if (typeof(T) == typeof(string)) return (T)Convert.ChangeType(parsed_as_json.ToString(), typeof(T));
                else return (T)parsed_as_json.ToObject(typeof(object));
            }

    /**/    if(parsed_as_obj is not null){
                if (castNaarDTOType is not null) return (T)parsed_as_obj.ToObject(castNaarDTOType);
                if (typeof(T) == typeof(string)) return (T)Convert.ChangeType(parsed_as_obj.ToString(), typeof(T));
                else return (T)parsed_as_obj.ToObject(typeof(object));
            }

            throw new BronParserException("Kon de data niet parsen.");
        }
    }
}


