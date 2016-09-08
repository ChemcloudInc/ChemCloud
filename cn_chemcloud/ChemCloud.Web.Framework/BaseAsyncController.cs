using System;
using System.Web.Mvc;
using System.Web.SessionState;

namespace ChemCloud.Web.Framework
{
	[SessionState(SessionStateBehavior.ReadOnly)]
	public class BaseAsyncController : AsyncController
	{
		public BaseAsyncController()
		{
		}
	}
}