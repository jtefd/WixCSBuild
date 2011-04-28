using System;
using System.Xml;

namespace WixBuild
{
	public class WixFeature : WixTag
	{
		public static readonly string PREFIX = "feat";

		public WixFeature(string id, string title, int level) : base("Feature")
		{
			Id = id;
			Title = title;
			Level = level;
		}

		public WixFeature(string title, int level) : this(title.ToLower(), title, level)
		{

		}

		public string Id
		{
			get
			{
				return GetTag().GetAttribute("Id");
			}
			set
			{
				AddAttribute("Id", WixUtil.GenWixID(PREFIX, value));
			}
		}	

		public string Title
		{
			get
			{
				return GetTag().GetAttribute("Title");
			}
			set
			{
				AddAttribute("Title", value);
			}
		}

		public int Level
		{
			get
			{
				return Convert.ToInt32(GetTag().GetAttribute("Level"));
			}
			set
			{
				AddAttribute("Level", Convert.ToString(value));
			}
		}	

		public void AddComponentGroupRef(WixComponentGroupRef componentgroupref)
		{
			AddTag(componentgroupref);
		}
	}
}
