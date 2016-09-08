using System;

namespace ChemCloud.Web.App_Code.UEditor
{
	public class ConfigHandler : IUEditorHandle
	{
		public ConfigHandler()
		{
		}

		public object Process()
		{
			return Config.Items;
		}
	}
}