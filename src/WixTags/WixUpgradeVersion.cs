using System;
using System.Xml;
using System.IO;

namespace WixBuild
{
	public class WixUpgradeVersion : WixTag
	{
		public WixUpgradeVersion(string minimum, bool include_min, string maximum, bool include_max) : base("UpgradeVersion")
		{
			Minimum = minimum;
			Maximum = maximum;
			IncludeMin = include_min;
			IncludeMax = include_max;
			AddAttribute("OnlyDetect", "no");
			AddAttribute("Property", "UPGRADEFOUND");
		}

		public string Minimum
		{
			get
			{
				return GetTag().GetAttribute("Minimum");
			}
			set
			{
				AddAttribute("Minimum", value);
			}
		}	

		public string Maximum
		{
			get
			{
				return GetTag().GetAttribute("Maximum");
			}
			set
			{
				AddAttribute("Maximum", value);
			}
		}
	
		public bool IncludeMin
		{
			get
			{
				string cval = GetTag().GetAttribute("IncludeMinimum");

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
					AddAttribute("IncludeMinimum", "yes");
				}
				else
				{
					AddAttribute("IncludeMinimum", "no");
				}
			}
		}

		public bool IncludeMax
		{
			get
			{
				string cval = GetTag().GetAttribute("IncludeMaximum");

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
					AddAttribute("IncludeMaximum", "yes");
				}
				else
				{
					AddAttribute("IncludeMaximum", "no");
				}
			}
		}
	}
}
