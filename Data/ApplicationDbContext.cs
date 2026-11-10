using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BeFit.Models;

namespace BeFit.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<ExerciseType> ExerciseTypes {get; set;} = default!;
    public DbSet<Session> Sessions {get; set;} = default!;
    public DbSet<Exercise> Exercises {get; set;} = default!;
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Exercise>()
            .HasOne(e => e.ExerciseType)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Id = "5d1f3a52-0b0e-4b8f-9a63-0f5d0c4a7e21",
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            ConcurrencyStamp = "a3c1d6e8-7b2f-4c9d-8e1a-2f6b5c4d3e10"
        });

        builder.Entity<ExerciseType>().HasData(
            new ExerciseType { Id = 1, Name = "Przysiad" },
            new ExerciseType { Id = 2, Name = "Martwy ciąg" },
            new ExerciseType { Id = 3, Name = "Wyciskanie sztangi leżąc"},
            new ExerciseType { Id = 4, Name = "Wiosłowanie sztangą" },
            new ExerciseType { Id = 5, Name = "Podciąganie na drążku" },
            new ExerciseType { Id = 6, Name = "Wyciskanie żołnierskie" });
    }
}
