using System.Security.Claims;

namespace iWenJuan.Client.WorkSpace.Services;

public class AuthenticationService
{
	public event Action<ClaimsPrincipal>? UserChanged;
	private ClaimsPrincipal? currentUser;

	public ClaimsPrincipal CurrentUser
	{
		get { return currentUser ?? new(); }
		set
		{
			currentUser = value;

			if (UserChanged is not null)
				UserChanged(currentUser);
		}
	}
}
