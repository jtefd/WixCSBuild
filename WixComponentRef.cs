using System;
using System.Xml;

namespace WixBuild
{
	public class WixComponentRef : WixTag
	{
		public WixComponentRef(string id) : base("ComponentRef")
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
