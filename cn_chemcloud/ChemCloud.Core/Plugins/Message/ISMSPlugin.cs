using ChemCloud.Core.Plugins;
using System;

namespace ChemCloud.Core.Plugins.Message
{
	public interface ISMSPlugin : IMessagePlugin, IPlugin
	{
		string GetBuyLink();

		string GetLoginLink();

		string GetSMSAmount();
	}
}