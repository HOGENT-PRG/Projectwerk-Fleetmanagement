using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
using WPFApp.Helpers;
using WPFApp.Interfaces;

namespace WPFApp.Model.Mappers {
	// De wpf behandelt bijvoorbeeld de response dto's voor weergave en het viewmodel vormt
	// deze om naar een request dto om te gebruiken bij aanroepen van ICommuniceer functies
	internal class DTONaarDTO {
		internal static T ResponseNaarRequest<T>(IResponseDTO responseDTO) {
			try {
				return BronParser.ParseCast<T>(responseDTO);
			} catch(Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e);}
		}

		internal static T RequestNaarResponse<T>(IRequestDTO requestDTO) {
			try {
				return BronParser.ParseCast<T>(requestDTO);
			} catch (Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
	}
}
