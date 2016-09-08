using System;

namespace ChemCloud.AOPProxy
{
	public enum InterceptionType
	{
		OnEntry,
		OnExit,
		OnSuccess,
		OnException,
		OnLogException
	}
}