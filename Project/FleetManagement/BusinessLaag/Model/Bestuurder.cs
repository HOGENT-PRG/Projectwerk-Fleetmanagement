using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessLaag.Exceptions;
using BusinessLaag.Helpers;
using BusinessLaag.Model.Attributes;
using System.Threading.Tasks;

namespace BusinessLaag
{
#nullable enable
    [TableMap(tableName:"bestuurder")] public class Bestuurder
    {
        [TableMap(colName: "id")]  public int? Id { get; private set; }
        [TableMap(colName: "naam")]  public string Naam { get; private set; }
        [TableMap(colName: "voornaam")] public string Voornaam { get; private set; }
        [TableMap(colName: "adres")] public string? Adres { get; private set; }
        [TableMap(colName: "geboortedatum")] public long GeboorteDatum { get; private set; }

        [TableMap(colName: "rijksregisternummer")] public string RijksRegisterNummer { get; private set; }

        [TableMap(colName: "rijbewijssoort")] public RijbewijsSoort RijbewijsSoort { get; private set; }

        [TableMap(colName: "voertuig_id")] public Voertuig? Voertuig { get; private set; }

        [TableMap(colName: "tankkaart_id")] public Tankkaart? Tankkaart { get; private set; }

        public Bestuurder(int? id, string naam, string voornaam, string? adres, long geboortedatum, 
            string rijksregisternummer, RijbewijsSoort rijbewijssoort, Voertuig? voertuig, Tankkaart? tankkaart)
        {
            RRNValideerder RRNValideerder = new();

            Id = id;
            Naam = naam.Length > 1 ? naam : throw new BestuurderException("Naam moet bestaan uit minstens 2 karakters");
            Voornaam = voornaam.Length > 1 ? voornaam : throw new BestuurderException("Voornaam moet bestaan uit minstens 2 karakters");
            Adres = adres;
            GeboorteDatum = geboortedatum > -2208988800 ? geboortedatum : throw new BestuurderException("Geboortejaar moet na 1900 zijn");
            RijksRegisterNummer = RRNValideerder.valideer(rijksregisternummer);
            RijbewijsSoort = rijbewijssoort;
            Voertuig = voertuig;
            Tankkaart = tankkaart;
        }
#nullable disable
    }
}
