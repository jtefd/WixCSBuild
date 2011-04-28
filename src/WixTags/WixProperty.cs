using System;
using System.Xml;

namespace WixBuild
{
	public class WixProperty : WixTag
	{
		public WixProperty(string id, string val) : base("Property")
		{
			Id = id;
			Value = val;
		}

		public string Id
		{
			get
			{
				return GetTag().GetAttribute("Id");
			}
			set
			{
				AddAttribute("Id", value.ToUpper());
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

	}
}
