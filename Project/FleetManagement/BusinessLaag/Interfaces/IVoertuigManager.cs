﻿using BusinessLaag.Model;
using System.Collections.Generic;

namespace BusinessLaag.Interfaces
{
    public interface IVoertuigManager
    {
		Voertuig GeefVoertuigDetail(int id);
		List<Voertuig> GeefVoertuigen();
		List<Voertuig> ZoekVoertuigen(List<string> kolomnamen, List<object> zoektermen, bool likeWildcard = false);
		Voertuig GeefVoertuigZonderRelaties(int id);
		void UpdateVoertuig(Voertuig NieuwVoertuig);
		void VerwijderVoertuig(int id);
		int VoegVoertuigToe(Voertuig voertuig);
		Voertuig ZoekVoertuigMetChassisnummer(string chassisnummer);
		Voertuig ZoekVoertuigMetNummerplaat(string nummerplaat);
	}
}