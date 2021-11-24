﻿using BusinessLaag.Model;
using BusinessLaag.Model.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DataLaag.Helpers {

	// TODO : functioneel moet dit nog op de proef gesteld worden, dit gebeurt tijdens het schrijven van de 
	// manager tests, kan zijn dat er nog een foutje zit in de reader keys / manier van parsen

	// Deze klasse werkt ondersteunend en heeft als enige functie het parsen van een sqldatareader state naar
	// een gewenst object. Door dit toe te passen wordt de parsing niet per opslagklasse herhaalt en is het makkelijker
	// te onderhouden en aan te passen.
	// Een vereiste is echter wel dat elke opslag klasse dezelfde standaard volgt met betrekking tot
	// naamgeving van de kolomnamen zodat de parser geen probleem ondervindt bij het zoeken naar de kolom.
	internal static class QueryParser {
		// Indien het gewenst is om exceptions te throwen indien een parse null retourneert, deze aanzetten.
		// Met nullable parameters wordt geen rekening gehouden aangezien deze toegelaten zijn voor de opbouw
		// van relaties tussen objecten.
		private static bool DebuggingExceptions = true;

		private static string MaakExceptionMessage(SqlDataReader r, string functienaam) {
			List<DbColumn> kolommen = r.GetColumnSchema().ToList();
			string basis = $"QueryParser - Kon de data niet parsen in functie {functienaam}\nKolommen ({r.FieldCount}) en hun waarden:";
			StringBuilder sb = new(basis);
			foreach(int i in Enumerable.Range(1, r.FieldCount)) {
				sb.Append($"\n{kolommen[i].ColumnName}={JsonConvert.SerializeObject(r.GetValue(i) == DBNull.Value ? "" : r.GetValue(i))}");
			}
			return sb.ToString();
		}

		// Parsers
		public static TankkaartBrandstof ParseReaderNaarTankkaartBrandstof(SqlDataReader r) {
			return (TankkaartBrandstof)(Enum.Parse(typeof(TankkaartBrandstof), (string)r["TankkaartBrandstof"], true));
		}

		public static Tankkaart ParseReaderNaarTankkaart(SqlDataReader r, List<TankkaartBrandstof> tbs=null, Bestuurder b=null) {

			if (!r.IsDBNull(r.GetOrdinal("TankkaartId"))) {
				return new Tankkaart((int?)r["TankkaartId"], (string)r["TankkaartKaartnummer"], (DateTime)r["TankkaartVervaldatum"], (string)r["TankkaartPincode"], tbs, b);
			}

			if (DebuggingExceptions) { throw new ArgumentNullException(MaakExceptionMessage(r, MethodBase.GetCurrentMethod().Name)); }
			return null;
		}

		public static Adres ParseReaderNaarAdres(SqlDataReader r) {
			if (!r.IsDBNull(r.GetOrdinal("BestuurderAdresId"))) {

				return new Adres((int)r["BestuurderAdresId"], (string)r["BestuurderAdresStraatnaam"], (string)r["BestuurderAdresHuisnummer"], (string)r["BestuurderAdresPostcode"], (string)r["BestuurderAdresPlaatsnaam"], (string)r["BestuurderAdresProvincie"], (string)r["BestuurderAdresLand"]);
			
			}

			if (DebuggingExceptions) { throw new ArgumentNullException(MaakExceptionMessage(r, MethodBase.GetCurrentMethod().Name)); }
			return null;
		}

		public static Voertuig ParseReaderNaarVoertuig(SqlDataReader r, Bestuurder b=null) {

			if (!r.IsDBNull(r.GetOrdinal("VoertuigId"))) {

				Merk merk = (Merk)(Enum.Parse(typeof(Merk), (string)r["VoertuigMerk"], true));
				VoertuigBrandstof brandstof = (VoertuigBrandstof)(Enum.Parse(typeof(VoertuigBrandstof), (string)r["VoertuigBrandstof"], true));
				Voertuigsoort soort = (Voertuigsoort)(Enum.Parse(typeof(Voertuigsoort), (string)r["VoertuigSoort"], true));
				string? kleur = r.IsDBNull(r.GetOrdinal("VoertuigKleur")) ? null : (string)r["VoertuigKleur"];
				int? aantalDeuren = r.IsDBNull(r.GetOrdinal("VoertuigAantalDeuren")) ? null : (int)r["VoertuigKleur"];

				return new Voertuig((int)r["BestuurderVoertuigId"], merk, (string)r["VoertuigModel"], (string)r["VoertuigNummerplaat"], brandstof, soort, kleur, aantalDeuren, b, (string)r["VoertuigChasisnummer"]);

			}

			if (DebuggingExceptions) { throw new ArgumentNullException(MaakExceptionMessage(r, MethodBase.GetCurrentMethod().Name)); }
			return null;
		}

		public static Bestuurder ParseReaderNaarBestuurder(SqlDataReader r, Tankkaart t=null, Voertuig v=null, Adres a=null) {

			if (!r.IsDBNull(r.GetOrdinal("BestuurderId"))) {
				RijbewijsSoort rijbewijssoort = (RijbewijsSoort)(Enum.Parse(typeof(RijbewijsSoort), (string)r["BestuurderRijbewijssoort"], true));

				return new Bestuurder((int)r["BestuurderId"], (string)r["BestuurderNaam"], (string)r["BestuurderVoornaam"], a, (DateTime)r["BestuurderGeboortedatum"], (string)r["BestuurderRijksregisternummer"], rijbewijssoort, v, t);
			}

			if (DebuggingExceptions) { throw new ArgumentNullException(MaakExceptionMessage(r, MethodBase.GetCurrentMethod().Name)); }
			return null;
		}
	}
}