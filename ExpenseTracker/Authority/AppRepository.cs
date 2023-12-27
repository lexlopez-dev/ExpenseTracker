using System;
namespace ExpenseTracker.Authority
{
	public static class AppRepository
	{
		private static List<Application> _applications = new List<Application>()
		{
			new Application()
			{
				ApplicationId = 1,
				ApplicationName = "TestApp",
				ClientId = "d5d26276-fbb0-48fc-a0d4-a2e89084de5d",
				Secret = "965a3652-8015-479a-8cb4-a21fb5e4da4b"
            }
		};

		public static bool Authenticate(string clientId, string secret)
		{
			return _applications.Any(x => x.ClientId == clientId && x.Secret == secret);
		}

		public static Application? GetApplicationByClientId(string clientId)
		{
			return _applications.FirstOrDefault(x => x.ClientId == clientId);
		}
	} 
}

