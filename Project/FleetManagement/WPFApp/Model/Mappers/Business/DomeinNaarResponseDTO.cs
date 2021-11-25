using BusinessLaag.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFApp.Exceptions;
using WPFApp.Helpers;
using WPFApp.Model.Response;

namespace WPFApp.Model.Mappers.Business {
	internal static class DomeinNaarResponseDTO {
        public static AdresResponseDTO ConverteerAdres(Adres a) {
            if (a is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(Adres)} maar ontving null.");
            }

            try {
                return BronParser.ParseCast<AdresResponseDTO>(a);
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

        public static BestuurderResponseDTO ConverteerBestuurder(Bestuurder b, bool inclusiefRelaties) {
            if (b is null) { 
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(Bestuurder)} maar ontving null."); 
            }

            try {
                AdresResponseDTO geconvAdres = null;
                VoertuigResponseDTO geconvVoertuig = null;
                TankkaartResponseDTO geconvTankkaart = null;

                if (inclusiefRelaties) {
                    if (b.Adres != null) {
                        geconvAdres = ConverteerAdres(b.Adres);
                    }
                    if (b.Voertuig != null) {
                        b.Voertuig.ZetBestuurder(null);
                        geconvVoertuig = ConverteerVoertuig(b.Voertuig, false);
                    }
                    if (b.Tankkaart != null) {
                        b.Tankkaart.ZetBestuurder(null);
                        geconvTankkaart = ConverteerTankkaart(b.Tankkaart, false);
                    }
                }

                b.ZetAdres(null);
                b.ZetVoertuig(null);
                b.ZetTankkaart(null);

                BestuurderResponseDTO geconvBestuurder = BronParser.ParseCast<BestuurderResponseDTO>(b);

                if(geconvVoertuig != null) {
                    geconvVoertuig.Bestuurder = geconvBestuurder;
				}

                if(geconvTankkaart != null) {
                    geconvTankkaart.Bestuurder = geconvBestuurder;
				}

                geconvBestuurder.Adres = geconvAdres;
                geconvBestuurder.Voertuig = geconvVoertuig;
                geconvBestuurder.Tankkaart = geconvTankkaart;

                return geconvBestuurder;
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

        public static TankkaartResponseDTO ConverteerTankkaart(Tankkaart t, bool inclusiefRelaties) {
            if (t is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(Tankkaart)} maar ontving null.");
            }

            try {
                BestuurderResponseDTO geconvBestuurder = null;
                if (inclusiefRelaties) {
                    if (t.Bestuurder != null) {
                        t.Bestuurder.ZetTankkaart(null); // anders stack overflow door circulatie tussen parse functies
                        geconvBestuurder = ConverteerBestuurder(t.Bestuurder, true);
                    }
                }

                t.ZetBestuurder(null);

                TankkaartResponseDTO geconvTankkaart = BronParser.ParseCast<TankkaartResponseDTO>(t);

                // wordt hier terug ingesteld en circulatie is vermeden
                if (geconvBestuurder is not null) {
                    if (geconvBestuurder.Tankkaart is null) {
                        geconvBestuurder.Tankkaart = geconvTankkaart;
                    }
                }

                geconvTankkaart.Bestuurder = geconvBestuurder;

                return geconvTankkaart;
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

        public static VoertuigResponseDTO ConverteerVoertuig(Voertuig v, bool inclusiefRelaties) {
            if (v is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(Voertuig)} maar ontving null.");
            }

            try {
                BestuurderResponseDTO geconvBestuurder = null;
                if (inclusiefRelaties) {
                    if (v.Bestuurder != null) {
                        v.Bestuurder.ZetVoertuig(null); // anders stack overflow door circulatie tussen parse functies
                        geconvBestuurder = ConverteerBestuurder(v.Bestuurder, true);
                    }
                }

                v.ZetBestuurder(null);

                VoertuigResponseDTO geconvVoertuig = BronParser.ParseCast<VoertuigResponseDTO>(v);

                // wordt hier terug ingesteld en circulatie is vermeden
                if (geconvBestuurder != null) {
                    geconvBestuurder.Voertuig = geconvVoertuig;
				}

                geconvVoertuig.Bestuurder = geconvBestuurder;

                return geconvVoertuig;
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }
    }
}
