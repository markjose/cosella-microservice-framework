namespace Cosella.Services.Contacts.Model
{
    public class AddressSummaryResponse
    {
        public string AreaCode { get; internal set; }
        public string CountryCode { get; internal set; }
        public string NameNumber { get; internal set; }
        public AddressResponseState State { get; internal set; }
    }
}