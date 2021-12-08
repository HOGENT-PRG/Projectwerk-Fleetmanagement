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
	internal static class DTONaarDTO {

		// Indien t de verwachteInterface implementeert is dit true, anders false.
		private static bool ImplementeertTypeInterface(Type t, Type verwachteInterface) {
			return t.GetInterfaces().Any(i => i.GetType() == verwachteInterface.GetType());
		}

		internal static T ResponseNaarRequest<T>(IResponseDTO responseDTO) {

			if (!ImplementeertTypeInterface(typeof(T), typeof(IRequestDTO))) {
				throw new MapperException($"Er dient een klasse opgegeven te worden die {nameof(IRequestDTO)} implementeert.");
			}

			try {
				return BronParser.ParseCast<T>(responseDTO);
			} catch(Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e);}
		}

		internal static T RequestNaarResponse<T>(IRequestDTO requestDTO) {

			if (!ImplementeertTypeInterface(typeof(T), typeof(IResponseDTO))) {
				throw new MapperException($"Er dient een klasse opgegeven te worden die {nameof(IResponseDTO)} implementeert.");
			}

			try {
				return BronParser.ParseCast<T>(requestDTO);
			} catch (Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
	}
}
