using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
	public class CreateUserDTO
	{
		public string? Email { get; set; }

		public string? Password { get; set; }

		public string? Gender { get; set; }

		public string? Address { get; set; }

		public string? Fullname { get; set; }

		public DateTime? Dob { get; set; }
		public string? Role { get; set; }

	}
}
