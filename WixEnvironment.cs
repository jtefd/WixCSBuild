using System;
using System.Xml;

namespace WixBuild
{
	public class WixEnvironment : WixTag
	{
		public static readonly string PREFIX = "env";

		public WixEnvironment(string name, string action, bool permanent, bool system, string part, string val) : base("Environment")
		{
			Name = name;
			Action = action;
			Permanent = permanent;
			System = system;
			Part = part;
			Value = val;
			
			AddAttribute("Id", WixUtil.GenWixID(PREFIX, Name));
		}

		public WixEnvironment(string name, string action, bool permanent, bool system, string val) : base("Environment")
		{
			Name = name;
			Action = action;
			Permanent = permanent;
			System = system;
			Value = val;

			AddAttribute("Id", WixUtil.GenWixID(PREFIX, Name));
		}

		public string Id
		{
			get
			{
				return GetTag().GetAttribute("Id");
			}
		}

		public string Name
		{
			get
			{
				return GetTag().GetAttribute("Name");
			}
			set
			{
				AddAttribute("Name", value.ToUpper());
			}
		}
		
		public string Action
		{
			 get
			 {
				 return GetTag().GetAttribute("Action");
			 }
			 set
			 {
				 AddAttribute("Action", value);
			 }
		}

		public string Part
		{
			get
			{
				return GetTag().GetAttribute("Part");
			}
			set
			{
				AddAttribute("Part", value);
			}
		}

		public string Value
		{
			get
			{
				return GetTag().GetAttribute("Value");
			}
			set
			{
				AddAttribute("Value", value);
			}
		}

		public bool Permanent
		{
			get
			{
				string cval = GetTag().GetAttribute("Permanent");

				if (cval.Equals("yes"))
				{	
					return true;
				}
				else
				{
					return false;
				}
			}
			set
			{
				if (value == true)
				{
					AddAttribute("Permanent", "yes");
				}
				else
				{
					AddAttribute("Permanent", "no");
				}
			}
		}	

		public bool System
		{
			get
			{
				string cval = GetTag().GetAttribute("System");

				if (cval.Equals("yes"))
				{	
					return true;
				}
				else
				{
					return false;
				}
			}
			set
			{
				if (value == true)
				{
					AddAttribute("System", "yes");
				}
				else
				{
					AddAttribute("System", "no");
				}
			}
		}	
	}
}
