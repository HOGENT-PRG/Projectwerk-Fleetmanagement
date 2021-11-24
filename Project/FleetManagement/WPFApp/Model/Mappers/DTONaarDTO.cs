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
		internal static IRequestDTO ResponseNaarRequest(IResponseDTO responseDTO) {
			try {
				return BronParser.ParseCast<IRequestDTO>(responseDTO);
			} catch(Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e);}
		}

		internal static IResponseDTO RequestNaarResponse(IRequestDTO requestDTO) {
			try {
				return BronParser.ParseCast<IResponseDTO>(requestDTO);
			} catch (Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
	}
}
