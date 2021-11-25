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

        /// <summary>
        /// Indien een geneste dictionary van een JObject gewenst is kan deze functie die voorzien.
        /// Op te merken is dat een JObject reeds beschikt over gelijkaardige functionaliteiten van een dictionary
        /// Roept zichzelf recursief op om alle diepte niveaus af te gaan en een dictionary te vormen.
        /// </summary>
        /// <param name="reqObj">Als startwaarde een JObject of JArray.</param>
        /// <returns></returns>
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
                    throw new BronParserException($"Dit type kan niet geparsed worden: {reqObj.GetType()}");
            }
		}

        /// <summary>
        /// Deze functie bewijst zijn nut indien er gebruik gemaakt wordt van de ApiCommuniceerder.
        /// Het laat in eerste instantie toe om binnenkomende data, in het geval van de api is dat json,
        /// te valideren aan de hand van een validatieschema.
        /// 
        /// Na eventuele validatie wordt het in de functie aangemaakte JObject omgevormd naar het gewenste type: T.
        /// In ons geval zal het volgende zaken afhandelen:
        /// --- **** Bij het construeren van een verzoek voor de API Laag, een RequestDTO omvormen naar JSON.
        /// --- By default (dmv toJsonFilterNulls) worden de null values weggelaten uit de resulterende JSON string. 
        /// --- Dit laat toe om bij de RequestDTO 1 of meerdere properties in te stellen en hiermee een verkorte
        /// --- versie van de klasse in JSON string formaat terug te krijgen.
        /// --- vb enkel de property Straat van AdresRequestDTO is ingesteld, resultaat: { "Straat" : "waarde" }
        /// --- Interessant voor queries waarbij slechts 1 of enkele waarden nodig zijn.
        /// xxx
        /// --- **** JSON omvormen naar een JObject(collecties) of het gewenste DTO type
        /// --- Bij collecties geven we het JObject terug, waaruit de waardes gehaald kunnen worden op een
        /// --- gelijkaardige manier als een dictionary.
        /// --- Bv een List<Adres> in JSON string formaat wordt een JObject waarover geitereerd kan worden om de
        /// --- individuele waarden door middel van nog een call naar deze functie te casten naar het gewenste DTO Type.
        /// xxx
        /// --- **** Aangezien de DTO's in de presentatielaag gebaseerd zijn op de Model klasses in de business laag
        /// --- **** kunnen we business laag objecten direct casten naar de geschikte DTO.
        /// </summary>
        /// <typeparam name = "T" > Het gewenste return type van de functie, voor collecties gebruik maken van JObject indien de collectie genest is of als value van een key ingesteld staat.</typeparam>
        /// <param name = "data" > De bron die omgezet dient te worden in type T</param>
        /// <param name = "ValidatieSchema" > Default = null, Een optioneel validatieschema</param>
        /// <param name = "toJsonFilterNulls" /> Default = true, Voor gebruik bij het omzetten naar JSON string, zorgt ervoor dat de key en value van null waarden niet zullen voorkomen in de output indien de waarde null is.</param>
		/// <returns></returns>
		public static T ParseCast<T>(object data, JSchema ValidatieSchema=null, bool toJsonFilterNulls=true) {

            var JSS = new JsonSerializer {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // https://dotnetfiddle.net/frCSa3
            };

            JObject parsed_as_json = null;
            JObject parsed_as_obj = null;

            try { if (data.GetType() == typeof(string)) { parsed_as_json = JObject.Parse((string)data); } } 
            catch(JsonReaderException) { }

            try { parsed_as_obj = JObject.FromObject(data, JSS); } 
            catch (ArgumentException e) when (e.Message.Contains("JObject instance expected")) { }
            
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

            /**Optie 1**/
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

            /**Optie 2**/
            } else if ((typeof(T) == typeof(JObject) || typeof(T).GetType().IsClass) 
                        && typeof(T) != typeof(object)) {            // https://dotnetfiddle.net/puxB1a
                return (T)parseResultaat.ToObject(typeof(T));

            /**Optie 3**/
            } else {                                                                    
                return (T)parseResultaat.ToObject(typeof(object)); 
            }

            throw new BronParserException("Kon de data niet parsen.");
        }
    }
}


