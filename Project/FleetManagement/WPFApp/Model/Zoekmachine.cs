using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFApp.Interfaces;
using WPFApp.Model.Response;
using WPFApp.Views.MVVM;

namespace WPFApp.Model {
    public class Zoekmachine {
        private static string _diepteSeparator;

        public Zoekmachine(string diepteSeparator = " >> ") {
            _diepteSeparator = diepteSeparator.StartsWith(" ") && diepteSeparator.EndsWith(" ") ? diepteSeparator : throw new ArgumentException("DiepteSeparator dient minstens 1 spatie aan voor en achterkant te bevatten");
        }

        private KeyValuePair<List<Type>, string> _parseZoekfilter(Type gekozenType, string zoekfilter) {
            List<Type> pad = new() { };
            if (zoekfilter.Contains(_diepteSeparator)) {
                if (zoekfilter.EndsWith(_diepteSeparator) || zoekfilter.StartsWith(_diepteSeparator)) { throw new ArgumentException("Er ontbreekt een veld."); }

                string[] zoekfilterArgs = zoekfilter.Split(_diepteSeparator);

                Type huidigType = gekozenType;
                foreach (string naam in zoekfilterArgs) {
                    Type nieuwType = null;
                    foreach (var prop in huidigType.GetProperties()) {
                        if (prop.Name == naam) {
                            pad.Add(huidigType);
                            nieuwType = prop.PropertyType;
                            break;
                        }
                    }
                    if (nieuwType is null) { throw new ArgumentException("Er kon geen type bepaald worden met property naam: " + naam); }
                    huidigType = nieuwType;
                }

                return new KeyValuePair<List<Type>, string>(pad, zoekfilterArgs[zoekfilterArgs.Length - 1]);
            } else {
                pad.Add(gekozenType);
                return new KeyValuePair<List<Type>, string>(pad, zoekfilter.Trim());
            }
        }

        private object _geefWaardeVanPropertyRecursief(List<Type> types, string propertyNaam, object instantie) {
            List<Type> huidigeTypes = types.ToList();

            var instantieType = instantie.GetType();

            foreach (var property in instantieType.GetProperties()) {

                var waarde = property.GetValue(instantie, null);

                if (property.Name == propertyNaam && huidigeTypes.Count <= 1) {
                    return waarde;
                } else {
                    if (huidigeTypes.Count > 0) {
                        if (huidigeTypes[huidigeTypes.Count > 1 ? 1 : 0] == property.PropertyType) {
                            if (property.PropertyType.FullName != "System.String"
                                && !property.PropertyType.IsPrimitive) {
                                List<Type> recursieveTypes = types.ToList();
                                recursieveTypes.Remove(recursieveTypes.First());
                                var recursieveOperatie = _geefWaardeVanPropertyRecursief(recursieveTypes, propertyNaam, waarde);
                                if (recursieveOperatie is not null) { return recursieveOperatie; }
                            }
                        }
                    } else {
                        return null;
                    }
                }
            }

            return null;
        }

        public List<T> ZoekMetFilter<T>(List<Func<List<T>>> dataCollectieActies, string zoekfilter, object zoekterm) {

            List<T> dataCollectieResultaat = new();
            List<T> filterDataResultaat = new();

            foreach (Func<List<T>> dataCollectieActie in dataCollectieActies) {
                dataCollectieActie.Invoke()?.ForEach(x => dataCollectieResultaat.Add(x));
            }

            KeyValuePair<List<Type>, string> zoekfilterParseResultaat = _parseZoekfilter(typeof(T), zoekfilter);


            foreach (T b in dataCollectieResultaat) {
                var res = _geefWaardeVanPropertyRecursief(zoekfilterParseResultaat.Key, zoekfilterParseResultaat.Value, b);
                if (res is not null) {
                    if (JsonConvert.SerializeObject((object)res) == JsonConvert.SerializeObject((object)zoekterm)) {
                        filterDataResultaat.Add(b);
                    }
                }

            }

            return filterDataResultaat;
        }

        public List<string> GeefZoekfilterVelden(Type huidigType, List<string> blacklistVelden = null, int maxNiveau = 1, int huidigNiveau = 1) {
            if (huidigNiveau < 1 || maxNiveau < 1) {
                throw new ArgumentException("Huidig niveau en max niveau zijn minimum 1.");
            }

            if (blacklistVelden is null) { blacklistVelden = new(); }

            List<string> padenOmgevormd = new();
            List<List<string>> paden = new();

            foreach (PropertyInfo p in huidigType.GetProperties()) {
                if (huidigNiveau > maxNiveau) { break; }
                List<string> nieuwPad = new();
                nieuwPad.Add(p.Name);

                if (p.PropertyType.FullName != "System.String"
                    && p.PropertyType.Assembly == Assembly.GetExecutingAssembly()
                    && !p.PropertyType.IsPrimitive
                    && !p.PropertyType.FullName.StartsWith("System.")) {

                    foreach (PropertyInfo t in p.PropertyType.GetProperties()) {
                        nieuwPad.Add(t.Name); paden.Add(nieuwPad.ToList()); nieuwPad.Clear(); nieuwPad.Add(p.Name);
                        List<string> deelPad = GeefZoekfilterVelden(t.PropertyType, null, maxNiveau, huidigNiveau + 1);
                        foreach (string s in deelPad) {
                            nieuwPad.Add(t.Name); nieuwPad.Add(s); paden.Add(nieuwPad.ToList()); nieuwPad.Clear(); nieuwPad.Add(p.Name);
                        }
                    }

                } else {
                    paden.Add(nieuwPad);
                }
            }

            foreach (List<string> ls in paden) {
                bool res = false;

                foreach (string s in ls) {
                    if (res) { continue; }
                    res = blacklistVelden.Any(l => s.Contains(l));
                    if (res) { continue; }
                    res = ls.Any(l => s.Contains(l) && s != l);
                }

                if (!res) {
                    StringBuilder samengesteldPad = new();
                    ls.ForEach(s => samengesteldPad.Append(s + _diepteSeparator));
                    samengesteldPad.Length -= _diepteSeparator.Length;
                    padenOmgevormd.Add(samengesteldPad.ToString());
                }
            }
            return padenOmgevormd;
        }

    }
}