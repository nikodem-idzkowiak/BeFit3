using System.ComponentModel.DataAnnotations;
using BeFit.Data;

namespace BeFit.Models;

public class Exercise
{
    public int Id {get; set;}
    public int Weight {get; set;}
    public int NumOfReps {get; set;}
    public int NumOfSeries {get; set;}

    [MaxLength(200)]
    public string? Comment {get; set;}
    
    public int ExerciseTypeId {get; set;}
    public ExerciseType? ExerciseType {get; set;}

    public int SessionId {get; set;}
    public Session? Session {get; set;}

    public string CreatedById {get; set;} = "";
    public ApplicationUser? CreatedBy {get; set;}
}