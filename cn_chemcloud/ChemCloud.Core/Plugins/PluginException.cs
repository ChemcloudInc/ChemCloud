using ChemCloud.Core;
using System;

namespace ChemCloud.Core.Plugins
{
	public class PluginException : HimallException
	{
		public PluginException()
		{
			Log.Info(Message, this);
		}

		public PluginException(string message) : base(message)
		{
			Log.Info(message, this);
		}

		public PluginException(string message, Exception inner) : base(message, inner)
		{
			Log.Info(message, inner);
		}
	}
}