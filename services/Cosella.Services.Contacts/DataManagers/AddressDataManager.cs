namespace Cosella.Services.Contacts.DataManagers
{
    using Interfaces;
    using Model;
    using System.Collections.Generic;
    using System;

    internal delegate AddressResponse LookupFunction(string areaCode, string nameNumber);

    public class AddressDataManager : IAddressDataManager
    {
        private readonly Dictionary<string, LookupFunction> _lookupFunctions;

        public AddressDataManager()
        {
            _lookupFunctions = new Dictionary<string, LookupFunction>
            {
                { "gb", LookupUkAddress }
            };
        }

        public bool IsSupportedCountryCode(string countryCode)
        {
            return _lookupFunctions.ContainsKey(countryCode.ToLowerInvariant());
        }

        public AddressResponse LookupAddress(string areaCode, string nameNumber, string countryCode)
        {
            if (!IsSupportedCountryCode(countryCode))
            {
                return new AddressResponse()
                {
                    State = AddressResponseState.Invalid,
                    InvalidMessage = $"The supplied country code is not supported: {countryCode}"
                };
            }

            return _lookupFunctions[countryCode](areaCode, nameNumber);
        }

        public AddressSummaryResponse[] SummariseAll()
        {
            return new AddressSummaryResponse[0];
        }

        private AddressResponse LookupUkAddress(string areaCode, string nameNumber)
        {
            return new AddressResponse()
            {
                State = AddressResponseState.NotFound,
                CountryCode = "gb",
                AreaCode = areaCode,
                NameNumber = nameNumber
            };
        }
    }
}