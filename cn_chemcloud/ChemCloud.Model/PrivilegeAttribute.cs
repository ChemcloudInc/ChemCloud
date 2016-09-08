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
        /// ��Ӧ�̵�������
        /// </summary>
        /// <param name="groupName">������������</param>
        /// <param name="name">������������</param>
        /// <param name="pid">��������ID</param>
        /// <param name="url">������ַ��ҳ�棩</param>
        /// <param name="controller">����������</param>
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