using System.ComponentModel.DataAnnotations;

namespace SpaceWatch.Core.Models.DefaultArea
{
	public class RegistrationModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Username")]
		public string Email { get; set; } = null!;

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;

		[Required]
		[Compare("Password")]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; } = null!;

		[Required]
		[Display(Name = "First Name")]
		[StringLength(100, MinimumLength = 2)]
		public string FirstName { get; set; } = null!;

		[Required]
		[Display(Name = "Last Name")]
		[StringLength(100, MinimumLength = 2)]
		public string LastName { get; set; } = null!;

		[Display(Name = "Address")]
		[Required]
		[StringLength(300, MinimumLength = 2)]
		public string Address1 { get; set; } = null!;

		[Display(Name = "Second Address")]
		[StringLength(300, MinimumLength = 2)]
		public string? Address2 { get; set; }

		[Required]
		[RegularExpression(@"(^1300\d{6}$)|(^1800|1900|1902\d{6}$)|(^0[2|3|7|8]{1}[0-9]{8}$)|(^13\d{4}$)|(^04\d{2,3}\d{6}$)")]
		public string PhoneNumber { get; set; } = null!;

		[Display(Name = "Post Code")]
		[Required]
		[RegularExpression("^[1-9]{1}[0-9]{3}$")]
		public string PostCode { get; set; } = null!;

		public bool AcceptUserAgreement { get; set; }

		public string? RegistrationInValid { get; set; }

		public int CategoryId { get; set; }
	}
}
