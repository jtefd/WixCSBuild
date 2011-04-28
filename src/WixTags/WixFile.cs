using System;
using System.Xml;
using System.IO;

namespace WixBuild
{
	public class WixFile : WixTag
	{
		public static readonly string PREFIX = "file";

		public WixFile(string id, string name, string source) : base("File")
		{
			Id = id;
			Name = name;
			Source = source;
			AddAttribute("KeyPath", "yes");
		}

		public WixFile(string id, string source) : this(id, new FileInfo(source).Name, source)
		{
		}

		public WixFile(string source) : this(source, new FileInfo(source).Name, source)
		{
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

		public string Source
		{
			get
			{
				return GetTag().GetAttribute("Source");
			}
			set
			{
				AddAttribute("Source", value);
			}
		}

		public void AddShortcut(WixShortcut shortcut)
		{
			AddTag(shortcut);
		}
	}
}
