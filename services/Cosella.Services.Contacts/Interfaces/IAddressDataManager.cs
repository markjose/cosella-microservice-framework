namespace Cosella.Services.Contacts.Interfaces
{
    using Model;

    public interface IAddressDataManager
    {
        bool IsSupportedCountryCode(string countryCode);

        AddressResponse LookupAddress(string areaCode, string nameNumber, string countryCode);

        AddressSummaryResponse[] SummariseAll();
    }
}