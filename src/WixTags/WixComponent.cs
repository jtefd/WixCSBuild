using System;
using System.Xml;

namespace WixBuild
{
	public class WixComponent : WixTag
	{
		public static readonly string PREFIX = "cmp";

		public WixComponent(string id) : base("Component")
		{
			Id = id;
		}

		public string Id
		{
			get
			{
				return GetTag().GetAttribute("Id");
			}
			set
			{
				AddAttribute("Id", WixUtil.GenWixID(PREFIX, value));
			}
		}

		public bool KeyPath
		{
			get
			{
				string cval = GetTag().GetAttribute("KeyPath");

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
					AddAttribute("KeyPath", "yes");
				}
				else
				{
					AddAttribute("KeyPath", "no");
				}
			}
		}	

		public void AddDirectory(WixDirectory directory)
		{
			AddTag(directory);
		}

		public void AddFile(WixFile file)
		{
			AddTag(file);
		}

		public void AddComponent(WixComponent component)
		{
			AddTag(component);
		}

		public void AddShortcut(WixShortcut shortcut)
		{
			AddTag(shortcut);
		}

		public void AddEnvironment(WixEnvironment environment)
		{
			AddTag(environment);
		}

		public WixComponentRef ComponentRef 
		{
			get
			{
				return new WixComponentRef(Id);
			}
		}
	}
}
