namespace Training_API.DTOS
{
    public class AddressDTO
    {
        public int AddressId { get; set; }

        public int? UserId { get; set; }

        public string? FullName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

    }
}
