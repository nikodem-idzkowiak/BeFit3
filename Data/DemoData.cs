using Microsoft.EntityFrameworkCore.Migrations;

namespace BeFit.Data;

// Sample accounts with six weeks of training sessions - something to work on, especially in the statistics.
// The dates are counted back from the moment the migration runs (SQLite's datetime('now')), so the data is always recent.
// Every account has the password Haslo123! - admin@befit.pl is an administrator, anna@befit.pl and jan@befit.pl train.
public static class DemoData
{
    private const string PasswordHash = "AQAAAAIAAYagAAAAEBIwWpSo5g658vqQMfmCgZ45Qc/1KzK0UERjJqxyLhS/+UWxxsj80ZEt6PpAVw9ALA==";
    private const string AdministratorRoleId = "5d1f3a52-0b0e-4b8f-9a63-0f5d0c4a7e21";
    private const string AdminId = "8e1f2a3b-0001-4c5d-9e6f-7a8b9c0d1e01";
    private const string AnnaId = "8e1f2a3b-0002-4c5d-9e6f-7a8b9c0d1e02";
    private const string JanId = "8e1f2a3b-0003-4c5d-9e6f-7a8b9c0d1e03";

    // Exercise types from the HasData seed: 1 squat, 2 deadlift, 3 bench press, 4 barbell row, 5 pull-up, 6 overhead press.
    private static readonly int[][] Plans = [[1, 3, 4], [2, 6, 5]];
    private static readonly int[] BaseWeight = [0, 60, 80, 50, 40, 0, 30];

    public static void Up(MigrationBuilder migrationBuilder)
    {
        AddUser(migrationBuilder, AdminId, "admin@befit.pl");
        AddUser(migrationBuilder, AnnaId, "anna@befit.pl");
        AddUser(migrationBuilder, JanId, "jan@befit.pl");
        migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('{AdminId}', '{AdministratorRoleId}');");

        // Anna trains every other day, Jan every third day and lifts 20% more.
        AddSessions(migrationBuilder, AnnaId, firstSessionId: 1000, everyDays: 2, strength: 1.0);
        AddSessions(migrationBuilder, JanId, firstSessionId: 2000, everyDays: 3, strength: 1.2);
    }

    public static void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("DELETE FROM Exercises WHERE SessionId BETWEEN 1000 AND 2999;");
        migrationBuilder.Sql("DELETE FROM Sessions WHERE Id BETWEEN 1000 AND 2999;");
        migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{AdminId}';");
        migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id IN ('{AdminId}', '{AnnaId}', '{JanId}');");
    }

    private static void AddUser(MigrationBuilder migrationBuilder, string id, string email)
    {
        var normalized = email.ToUpperInvariant();
        migrationBuilder.Sql($"""
            INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash,
                SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
            VALUES ('{id}', '{email}', '{normalized}', '{email}', '{normalized}', 1, '{PasswordHash}',
                '{id.Replace("-", "").ToUpperInvariant()}', '{id}', 0, 0, 1, 0);
            """);
    }

    private static void AddSessions(MigrationBuilder migrationBuilder, string userId, int firstSessionId, int everyDays, double strength)
    {
        var sessionId = firstSessionId;
        for (var daysAgo = 41; daysAgo >= 1; daysAgo -= everyDays, sessionId++)
        {
            var start = 17 * 60 + daysAgo % 3 * 30;        // 17:00, 17:30 or 18:00
            var end = start + 60 + daysAgo % 4 * 10;       // 60-90 minutes
            migrationBuilder.Sql($"""
                INSERT INTO Sessions (Id, Start, End, CreatedById) VALUES ({sessionId},
                    datetime('now', 'localtime', 'start of day', '-{daysAgo} days', '+{start} minutes'),
                    datetime('now', 'localtime', 'start of day', '-{daysAgo} days', '+{end} minutes'),
                    '{userId}');
                """);

            foreach (var typeId in Plans[sessionId % 2])
            {
                // A little heavier every week: the oldest session is 41 days ago.
                var weight = (int)Math.Round((BaseWeight[typeId] + (41 - daysAgo) / 3.0) * strength);
                var heavy = typeId <= 2;
                var comment = daysAgo % 7 == 0 ? "'Dobra forma'" : "NULL";
                migrationBuilder.Sql($"""
                    INSERT INTO Exercises (Weight, NumOfReps, NumOfSeries, Comment, ExerciseTypeId, SessionId, CreatedById)
                    VALUES ({(typeId == 5 ? 0 : weight)}, {(heavy ? 5 : 8)}, {(heavy ? 5 : 3 + daysAgo % 2)}, {comment}, {typeId}, {sessionId}, '{userId}');
                    """);
            }
        }
    }
}
