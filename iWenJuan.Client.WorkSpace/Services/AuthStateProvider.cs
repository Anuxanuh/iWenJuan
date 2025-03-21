using Microsoft.AspNetCore.Components.Authorization;

namespace iWenJuan.Client.WorkSpace.Services;

public class AuthStateProvider : AuthenticationStateProvider
{
	private AuthenticationState authenticationState;

	public AuthStateProvider(AuthenticationService service)
	{
		authenticationState = new AuthenticationState(service.CurrentUser);

		service.UserChanged += (newUser) =>
		{
			authenticationState = new AuthenticationState(newUser);
			NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
		};
	}

	public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
		Task.FromResult(authenticationState);
}
