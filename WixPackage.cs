using System;
using System.Xml;

namespace WixBuild
{
	/// <summary>
	/// Summary description for WixPackage.
	/// </summary>
	public class WixPackage : WixTag
	{
		public WixPackage(bool compressed) : base("Package")
		{
			Compressed = compressed;
		}

		public bool Compressed
		{
			get
			{
				string cval = GetTag().GetAttribute("Compressed");

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
					AddAttribute("Compressed", "yes");
				}
				else
				{
					AddAttribute("Compressed", "no");
				}
			}
		}	
	}
}
