namespace Cosella.Services.Contacts.Model
{
    public enum AddressResponseState
    {
        Invalid,
        NotFound,
        Partial,
        Complete
    }

    public class AddressResponse
    {
        public string AreaCode { get; internal set; }
        public string CountryCode { get; internal set; }
        public string InvalidMessage { get; internal set; }
        public string NameNumber { get; internal set; }
        public AddressResponseState State { get; internal set; }
    }
}