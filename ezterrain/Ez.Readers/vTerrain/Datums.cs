using System;

namespace Ez.Readers.vTerrain
{
	public enum Datums : short
	{
		NO_DATUM = -2,
		UNKNOWN_DATUM = -1,
		ADINDAN = 0,
		ARC1950 = 1,
		ARC1960 = 2,
		AUSTRALIAN_GEODETIC_1966 = 3,
		AUSTRALIAN_GEODETIC_1984 = 4,
		CAMP_AREA_ASTRO = 5,
		CAPE = 6,
		EUROPEAN_DATUM_1950 = 7,
		EUROPEAN_DATUM_1979 = 8,
		GEODETIC_DATUM_1949 = 9,
		HONG_KONG_1963 = 10,
		HU_TZU_SHAN = 11,
		INDIAN = 12,
		NAD27 = 13,
		NAD83 = 14,
		OLD_HAWAIIAN_MEAN = 15,
	 	OMAN = 16,
		ORDNANCE_SURVEY_1936 = 17,
		PUERTO_RICO = 18,
		PULKOVO_1942 = 19,
		PROVISIONAL_S_AMERICAN_1956 = 20,
		TOKYO = 21,
		WGS_72 = 22,
		WGS_84 = 23
	}
}