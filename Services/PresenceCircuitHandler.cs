using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace BeFit.Services;

public class PresenceCircuitHandler(OnlineUsers onlineUsers, AuthenticationStateProvider authenticationStateProvider) : CircuitHandler
{
    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        var user = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;
        onlineUsers.Connected(circuit.Id, user.Identity?.IsAuthenticated == true ? user.Identity.Name! : "gość");
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        onlineUsers.Disconnected(circuit.Id);
        return Task.CompletedTask;
    }
}