using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Flipdish.Recruiting.WebhookReceiver.Helpers
{
    static class CurrencyCodeMapper
    {
        private static readonly Dictionary<string, string> SymbolsByCode;

        public static Dictionary<string, string> IsoCountryCodesAndSymbols
        {
            get
            {
                var newDictionary = SymbolsByCode.ToDictionary(entry => entry.Key,
                                                entry => entry.Value);

                return newDictionary;
            }

        }


        public static string IsoCodeToSymbol(string isoCode)
        {
            return SymbolsByCode[isoCode];
        }

        static CurrencyCodeMapper()
        {
            try
            {
                SymbolsByCode = new Dictionary<string, string>();
                IEnumerable<RegionInfo> regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .Select(x => new RegionInfo(x.Name))
                    .ToList();

                foreach (RegionInfo region in regions)
                {
                    if (!SymbolsByCode.ContainsKey(region.ISOCurrencySymbol.ToUpper()))
                    {
                        SymbolsByCode.Add(region.ISOCurrencySymbol.ToUpper(), region.CurrencySymbol);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
