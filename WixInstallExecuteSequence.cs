using System;
using System.Xml;
using System.IO;

namespace WixBuild
{
	public class WixInstallExecuteSequence : WixTag
	{
		public WixInstallExecuteSequence() : base("InstallExecuteSequence")
		{
		}

		public void AddRemoveExistingProducts()
		{
			WixTag tag = new WixTag("RemoveExistingProducts");
			tag.AddAttribute("After", "InstallFinalize");
			
			AddTag(tag);
		}
	}
}
