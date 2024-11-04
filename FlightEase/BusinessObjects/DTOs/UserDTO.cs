namespace BusinessObjects.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Gender { get; set; }

        public string? Nationality { get; set; }

        public string? Address { get; set; }

        public string? Fullname { get; set; }

        public DateTime? Dob { get; set; }

        public string? Role { get; set; }

        public string? Rank { get; set; }
        public int? MembershipId { get; set; }


        public string? Status { get; set; }
    }
}
