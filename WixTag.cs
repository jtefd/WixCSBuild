using System;
using System.Xml;
using System.Security.Cryptography;

namespace WixBuild
{
	/// <summary>
	/// Summary description for WixTag.
	/// </summary>
	public class WixTag
	{
		private XmlElement element;

		public WixTag(string tag)
		{
			element = Wix.WXS.CreateElement(tag);
		}

		public void AddGuid(string tag)
		{
			string guid = System.Guid.NewGuid().ToString();

			AddAttribute(tag, guid);
		}

		public void AddGuid(string tag, string guid)
		{
			AddAttribute(tag, guid);
		}

		public void AddTag(WixTag tag)
		{
			GetTag().AppendChild(tag.GetTag());
		}

		public void AddAttribute(string key, string val)
		{
			GetTag().SetAttribute(key, val);
		}

		public XmlElement GetTag()
		{
			return element;
		}

		public string GetSource()
		{
			return GetTag().InnerXml;
		}
	}
}
