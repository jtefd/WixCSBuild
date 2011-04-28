using System;
using System.Xml;

namespace WixBuild
{
	public class WixComponentGroupRef : WixTag
	{
		public WixComponentGroupRef(string id) : base("ComponentGroupRef")
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
