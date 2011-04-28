using System;
using System.Xml;

namespace WixBuild
{
	public class WixShortcut : WixTag
	{
		public static readonly string PREFIX = "shrtcut";

		public WixShortcut(string id, string name, string directory, string working_directory, bool advertise) : base("Shortcut")
		{
			Id = id;
			Name = name;
			Directory = directory;
			WorkingDirectory = working_directory;
			Advertise = advertise;
		}

		public WixShortcut(string id, string name, string target, string directory, string working_directory, bool advertise, string icon) : base("Shortcut")
		{
			Id = id;
			Name = name;
			Target = target;
			Directory = directory;
			WorkingDirectory = working_directory;
			Advertise = advertise;
			Icon = icon;
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

		public string Name
		{
			get
			{
				return GetTag().GetAttribute("Name");
			}
			set
			{
				AddAttribute("Name", value);
			}
		}

		public string Target
		{
			get
			{
				return GetTag().GetAttribute("Target");
			}
			set
			{
				AddAttribute("Target", value);
			}
		}

		public string Directory
		{
			get
			{
				return GetTag().GetAttribute("Directory");
			}
			set
			{
				AddAttribute("Directory", value);
			}
		}

		public string WorkingDirectory
		{
			get
			{
				return GetTag().GetAttribute("WorkingDirectory");
			}
			set
			{
				AddAttribute("WorkingDirectory", value);
			}
		}

		public bool Advertise
		{
			get
			{
				string cval = GetTag().GetAttribute("Advertise");

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
					AddAttribute("Advertise", "yes");
				}
				else
				{
					AddAttribute("Advertise", "no");
				}
			}
		}	

		public string Icon
		{
			get
			{
				return GetTag().GetAttribute("Icon");
			}
			set
			{
				AddAttribute("Icon", value);
			}
		}
	}
}
