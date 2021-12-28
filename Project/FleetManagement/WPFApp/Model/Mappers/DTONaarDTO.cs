using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
using WPFApp.Helpers;
using WPFApp.Interfaces;
using WPFApp.Views;

// Zet request dto om naar response dto en omgekeerd,
// een extra controle vind plaats mbt type waarnaar gecast wordt, heeft geen state en is dus static als helper class
namespace WPFApp.Model.Mappers {
	// De wpf behandelt bijvoorbeeld de response dto's voor weergave en het viewmodel vormt
	// deze om naar een request dto om te gebruiken bij aanroepen van ICommuniceer functies
	internal static class DTONaarDTO {

		// Indien t de verwachteInterface implementeert is dit true, anders false.
		// Wordt gebruikt om te verzekeren dat er naar een response of request dto wordt omgezet
		private static bool ImplementeertTypeInterface(Type t, Type verwachtteInterface) {
			return t.GetInterfaces().Any(i => i.GetType() == verwachtteInterface.GetType());
		}

		internal static T ResponseNaarRequest<T>(IResponseDTO responseDTO) {

			if(responseDTO is null) {
				return default(T); // = null
			}

			if (!ImplementeertTypeInterface(typeof(T), typeof(IRequestDTO))) {
				throw new MapperException($"Er dient een klasse opgegeven te worden die {nameof(IRequestDTO)} implementeert.");
			}

			try {
				return BronParser.ParseCast<T>(responseDTO);
			} catch(Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e);}
		}

		internal static T RequestNaarResponse<T>(IRequestDTO requestDTO) {

			if(requestDTO is null) {
				return default(T); // = null
			}

			if (!ImplementeertTypeInterface(typeof(T), typeof(IResponseDTO))) {
				throw new MapperException($"Er dient een klasse opgegeven te worden die {nameof(IResponseDTO)} implementeert.");
			}

			try {
				return BronParser.ParseCast<T>(requestDTO);
			} catch (Exception e) { throw new MapperException($"{MethodBase.GetCurrentMethod().Name} > {e.GetType().Name} :\n{e.Message}", e); }
		}
	}
}
