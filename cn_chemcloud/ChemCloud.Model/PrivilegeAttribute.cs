using System;
using System.Runtime.CompilerServices;

namespace ChemCloud.Model
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple=true, Inherited=true)]
	public class PrivilegeAttribute : Attribute
	{
		public string Action
		{
			get;
			set;
		}

		public string Controller
		{
			get;
			set;
		}

		public string GroupName
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public int Pid
		{
			get;
			set;
		}

		public string Url
		{
			get;
			set;
		}

        /// <summary>
        /// 供应商导航配置
        /// </summary>
        /// <param name="groupName">顶级导航名称</param>
        /// <param name="name">二级导航名称</param>
        /// <param name="pid">父级导航ID</param>
        /// <param name="url">导航地址（页面）</param>
        /// <param name="controller">控制器名称</param>
        /// <param name="action"></param>
		public PrivilegeAttribute(string groupName, string name, int pid, string url, string controller, string action = "")
		{
            Name = name;
            GroupName = groupName;
            Pid = pid;
            Url = url;
            Controller = controller;
            Action = action;
		}

		public PrivilegeAttribute(string controller, string action = "")
		{
            Controller = controller;
            Action = action;
		}
	}
}