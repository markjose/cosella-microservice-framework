namespace Cosella.Services.Contacts.Model
{
    public class AddressRequest
    {
        public string AreaCode { get; internal set; }
        public string CountryCode { get; internal set; }
        public string NameNumber { get; internal set; }
    }
}