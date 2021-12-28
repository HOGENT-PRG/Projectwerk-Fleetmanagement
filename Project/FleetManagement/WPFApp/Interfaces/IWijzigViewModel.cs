using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFApp.Interfaces {
	internal interface IWijzigViewModel {
		/*
			Deze interface maakt definieren van een algemeen commando in het ApplicatieOverzichtViewModel, voor het initialiseren van Wijzig windows, mogelijk.
			Het dient als garantie dat een WijzigenViewModel de volgende functionaliteit implementeert.

			In ApplicatieOverzichtViewModel wordt een commando opgeroepen welke via xaml binding een responseDTO bevat, deze wordt doorgegeven als parameter aan een WijzigenViewModel door middel van onderstaande functie.
			Bij implementatie van de functie wordt er in de body gecast naar het specifieke ResponseDTO type en het ViewModel voorbereid.
		*/
		public void BereidModelVoor(IResponseDTO responseDTO, bool isReset=false);
	}
}
