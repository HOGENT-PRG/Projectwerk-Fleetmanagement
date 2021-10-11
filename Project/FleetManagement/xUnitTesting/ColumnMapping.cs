using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using BusinessLaag.Model;
using BusinessLaag.Model.Attributes;
using BusinessLaag;
using Xunit;
using Xunit.Abstractions;
using System;

namespace xUnitTesting
{
    public class ColumnMapping
    {
        private readonly ITestOutputHelper output;

        public ColumnMapping(ITestOutputHelper output)
        {
            this.output = output;
        }

        public string DictionaryToString(Dictionary<string, string> dictionary)
        {
            string dictionaryString = "{";
            foreach (KeyValuePair<string, string> keyValues in dictionary)
            {
                dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";
            }
            return dictionaryString.TrimEnd(',', ' ') + "}";
        }
        
        [Fact]
        public Dictionary<string, string> GetTableMapping()
        //public Dictionary<string, string> GetTableMapping(object myClass)
        {
            Bestuurder myClass = new Bestuurder(1, "Doe", "John", "Leliestraat 10, 9000 Gent", 1633730469, "99021512345", RijbewijsSoort.A, null, null);
            Dictionary<string, string> _mapping = new Dictionary<string, string>();
            string ClassName = myClass.GetType().Name;

            //_mapping.Add("OPTIMAL_TABLE_NAME", myClass.GetType().Name.ToLower());

            object[] classAttrs = myClass.GetType().GetCustomAttributes(true);
            foreach(object attr in classAttrs)
            {
                TableMapAttribute classAttr = attr as TableMapAttribute;
                try { _mapping.Add(ClassName, classAttr.TableName); } catch (Exception e) { };
            }

            PropertyInfo[] props = myClass.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    TableMapAttribute colAttr = attr as TableMapAttribute;
                    if (colAttr != null)
                    {
                        if (colAttr.ColumnName != null)
                        {
                            string propName = prop.Name;
                            string auth = colAttr.ColumnName;
                            _mapping.Add(propName, auth);
                        }                        
                    }
                }
            }

            foreach (KeyValuePair<string, string> entry in _mapping)
            {
                if (entry.Key == ClassName) { _mapping.Add("ClassName", ClassName); break; }
            }

            output.WriteLine(this.DictionaryToString(_mapping));
            return _mapping;
        }


    }
}

// ------------- Wat nog beter zou zijn is dat de klasse zelf ook een attribuut toegekend krijgt
// ------------- Zodat de tabelnaam ingesteld kan worden, momenteel:
// Tabelnaam = class name tolower
// Elke column dient appart gemapped te worden binnen de class dmv:
// [TableMap("id")]
// Implementatie met aangepast functie hierboven
// https://stackoverflow.com/a/6637710/8623540
