using System;
using System.Xml;

namespace WixBuild
{
	public class WixMedia : WixTag
	{
		public WixMedia(int id, string cabinet, bool embedcab) : base("Media")
		{
			Id = id;
			Cabinet = cabinet;
			EmbedCab = embedcab;
		}

		public int Id
		{
			get
			{
				return Convert.ToInt32(GetTag().GetAttribute("Id"));
			}
			set
			{
				AddAttribute("Id", Convert.ToString(value));
			}
		}	

		public string Cabinet
		{
			get
			{
				return GetTag().GetAttribute("Cabinet");
			}
			set
			{
				AddAttribute("Cabinet", value);
			}
		}

		public bool EmbedCab
		{
			get
			{
				string cval = GetTag().GetAttribute("EmbedCab");

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
					AddAttribute("EmbedCab", "yes");
				}
				else
				{
					AddAttribute("EmbedCab", "no");
				}
			}
		}	
	}
}
