using System.ComponentModel.DataAnnotations;

namespace BeFit.Dtos;

public class SessionFormDto : IValidatableObject
{
    public DateTime Start {get; set;}
    public DateTime End {get; set;}

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (End <= Start)
        {
            yield return new ValidationResult("Koniec sesji musi być późniejszy niż jej początek", [nameof(End)]);
        }
    }
}