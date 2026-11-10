namespace BeFit.Services;

public class ChangeNotifier
{
    public event Action? ExerciseTypesChanged;

    public event Action<String>? UserDataChanged;

    public void NotifyExerciseTypesChanged() => ExerciseTypesChanged?.Invoke();

    public void NotifyUserDataChanged(string userId) => UserDataChanged?.Invoke(userId);
}