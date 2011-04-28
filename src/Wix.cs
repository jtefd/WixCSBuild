using System;
using System.Xml;
using System.IO;

namespace WixBuild
{
	public class Wix
	{
		private static XmlDocument wxs;

		public static XmlDocument WXS
		{
			get
			{
				if (Wix.wxs == null)
				{
					Wix.wxs = new XmlDocument();
				}

				return Wix.wxs;
			}
		}

		private XmlElement root;

		public Wix()
		{
			root = WXS.CreateElement("Wix");
			root.SetAttribute("xmlns", "http://schemas.microsoft.com/wix/2006/wi");
			root.SetAttribute("xmlns:util", "http://schemas.microsoft.com/wix/UtilExtension");

			Wix.WXS.AppendChild(root);
		}

		public void addProduct(WixProduct product)
		{
			root.AppendChild(product.GetTag());
		}

		public string GetSource()
		{
			StringWriter sw = new StringWriter();

			XmlTextWriter xw = new XmlTextWriter(sw);
			xw.Formatting = Formatting.Indented;

			Wix.WXS.WriteTo(xw);

			return sw.ToString();
		}
	}
}
