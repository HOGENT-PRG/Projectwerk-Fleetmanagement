﻿using BusinessLaag.Model;
using System;
using WPFApp.Exceptions;
using WPFApp.Helpers;
using WPFApp.Model.Request;

namespace WPFApp.Model.Mappers.Business {
	internal static class RequestDTONaarDomein {
        public static Adres ConverteerNaarAdres(AdresRequestDTO adres) {
            if (adres is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(AdresRequestDTO)} maar ontving null.");
            }

            try {
                return BronParser.ParseCast<Adres>(adres);
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

        public static Bestuurder ConverteerNaarBestuurder(BestuurderRequestDTO b, bool inclusiefRelaties) {
            if (b is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(BestuurderRequestDTO)} maar ontving null.");
            }

            try {
                Adres adres = null;
                Voertuig voertuig = null;
                Tankkaart tankkaart = null;

				if (inclusiefRelaties) {
                    if(b.Adres != null) {
                        adres = ConverteerNaarAdres(b.Adres);
					}
                    if(b.Voertuig != null) {
                        b.Voertuig.Bestuurder = null;
                        voertuig = ConverteerNaarVoertuig(b.Voertuig, false);
					}
                    if(b.Tankkaart != null) {
                        b.Tankkaart.Bestuurder = null;
                        tankkaart = ConverteerNaarTankkaart(b.Tankkaart, false);
					}
				}

                b.Adres = null;
                b.Voertuig = null;
                b.Tankkaart = null;

                Bestuurder bestuurder = BronParser.ParseCast<Bestuurder>(b);

                if(voertuig != null) {
                    voertuig.ZetBestuurder(bestuurder);
				}

                if(tankkaart != null) {
                    tankkaart.ZetBestuurder(bestuurder);
				}

                bestuurder.ZetAdres(adres);
                bestuurder.ZetVoertuig(voertuig);
                bestuurder.ZetTankkaart(tankkaart);

                return bestuurder;
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

        public static Tankkaart ConverteerNaarTankkaart(TankkaartRequestDTO t, bool inclusiefRelaties) {
            if (t is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(TankkaartRequestDTO)} maar ontving null.");
            }

            try {
                Bestuurder bestuurder = null;

				if (inclusiefRelaties) {
                    if(t.Bestuurder != null) {
                        t.Bestuurder.Tankkaart = null; // anders stack overflow door circulatie tussen parse functies
                        bestuurder = ConverteerNaarBestuurder(t.Bestuurder, true);
					}
				}

                t.Bestuurder = null;

                Tankkaart tankkaart = BronParser.ParseCast<Tankkaart>(t);

                // wordt hier terug ingesteld en circulatie is vermeden
                if(bestuurder.Tankkaart is null) {
                    bestuurder.ZetTankkaart(tankkaart);
				}

                tankkaart.ZetBestuurder(bestuurder);

                return tankkaart;
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

        public static Voertuig ConverteerNaarVoertuig(VoertuigRequestDTO v, bool inclusiefRelaties) {
            if (v is null) {
                throw new MapperException($"Mapper verwachtte een instantie van {nameof(VoertuigRequestDTO)} maar ontving null.");
            }

            try {
                Bestuurder bestuurder = null;

				if (inclusiefRelaties) {
                    if(v.Bestuurder != null) {
                        v.Bestuurder.Voertuig = null; // anders stack overflow door circulatie tussen parse functies
                        ConverteerNaarBestuurder(v.Bestuurder, true);
					}
				}

                v.Bestuurder = null;

                Voertuig voertuig = BronParser.ParseCast<Voertuig>(v);

                // wordt hier terug ingesteld en circulatie is vermeden
                if(bestuurder.Voertuig is null) {
                    bestuurder.ZetVoertuig(voertuig);
				}

                voertuig.ZetBestuurder(bestuurder);

                return voertuig;
            } catch (Exception e) { throw new MapperException(e.Message, e); }
        }

    }
}