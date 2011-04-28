using System;
using System.Xml;
using System.IO;

namespace WixBuild
{
	public class WixUpgrade : WixTag
	{
		public WixUpgrade(string id) : base("Upgrade")
		{
			Id = id;
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

		public void AddUpgradeVersion(WixUpgradeVersion upgrade_version)
		{
			AddTag(upgrade_version);
		}
	}
}
