using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
	public class ValidationHelper
	{
		public static void ModelValidation(object obj)
		{
			// Model validation
			ValidationContext validationContext = new ValidationContext(obj);
			List<ValidationResult> validationResults = new();

			// true to validate all properties, if false only required attributes are validated
			bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
			if (!isValid)
			{
				throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
			}
		}
	}
}
