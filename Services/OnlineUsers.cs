using System.Collections.Concurrent;

namespace BeFit.Services;

public class OnlineUsers
{
    private readonly ConcurrentDictionary<string, string> circuits = new();

    public event Action? Changed;

    public IReadOnlyList<string> Names => circuits.Values.Distinct().Order().ToList();

    public void Connected(string circuitId, string userName)
    {
        circuits[circuitId] = userName;
        Changed?.Invoke();
    }

    public void Disconnected(string circuitId)
    {
        if (circuits.TryRemove(circuitId, out _))
        {
            Changed?.Invoke();
        }
    }
}