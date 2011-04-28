using System;
using System.Xml;

namespace WixBuild
{
	public class WixUIRef : WixTag
	{
		public static WixUIRef Custom
		{
			get
			{
				return new WixUIRef("WixUI_Custom");
			}
		}

		public WixUIRef(string id) : base("UIRef")
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
	}
}
