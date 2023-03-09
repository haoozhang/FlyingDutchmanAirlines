using System.ComponentModel.DataAnnotations;

namespace FlyingDutchmanAirlines.Models;

public class BookingData : IValidatableObject
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set => _firstName = ValidateName(value, nameof(FirstName));
    }
    
    private string _lastName;

    public string LastName
    {
        get => _lastName;
        set => _lastName = ValidateName(value, nameof(LastName));
    }

    private string ValidateName(string value, string name)
    {
        return string.IsNullOrEmpty(value)
            ? throw new ArgumentException($"Invalid {name}")
            : value;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var result = new List<ValidationResult>();

        if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
        {
            result.Add(new ValidationResult("FirstName and LastName are both null or empty."));
        } 
        else if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
        {
            result.Add(new ValidationResult("FirstName or LastName is null or empty."));
        }

        return result;
    }
}