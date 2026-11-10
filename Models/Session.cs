using BeFit.Data;

namespace BeFit.Models;

public class Session
{
    public int Id {get; set;}
    public DateTime Start {get; set;}
    public DateTime End {get; set;}

    public List<Exercise> Exercises {get; set;} = [];

    public string CreatedById {get; set; } = "";
    public ApplicationUser? CreatedBy {get; set;}
}