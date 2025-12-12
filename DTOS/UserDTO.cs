namespace Training_API.DTOS
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? Phone { get; set; }

        public bool? IsAdmin { get; set; }

        public DateTime? CreatedAt { get; set; }
        public List<AddressDTO>? AddressList { get; set; }
    }
}
