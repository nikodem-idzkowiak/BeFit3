using System.ComponentModel.DataAnnotations;

namespace BeFit.Models;

public class ExerciseType
{
    public int Id {get; set;}

    [Required(ErrorMessage = "Podaj nazwę")]
    [MaxLength(100, ErrorMessage = "Nazwa może mieć najwyżej 100 znaków")]
    public string Name {get; set; } = "";
}