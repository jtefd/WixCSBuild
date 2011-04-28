using System;
using System.Xml;

namespace WixBuild
{
	public class WixDirectoryRef : WixTag
	{
		public WixDirectoryRef(string id) : base("DirectoryRef")
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
				AddAttribute("Id", value);
			}
		}

		public void AddComponent(WixComponent component)
		{
			AddTag(component);
		}

		public void AddDirectory(WixDirectory directory)
		{
			AddTag(directory);
		}
	}
}
