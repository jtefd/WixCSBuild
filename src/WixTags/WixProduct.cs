using System;
using System.Xml;

namespace WixBuild
{
	public class WixProduct : WixTag
	{
		public WixProduct(string name, string manufacturer, string language, int[] version, string upgrade_code) : base("Product")
		{
			AddAttribute("Id", "*");
			AddAttribute("UpgradeCode", upgrade_code);
            Name = name;
			Manufacturer = manufacturer;
			Version = version;
			Language = language;
		}

		public WixProduct(string name, string manufacturer, int[] version, string upgrade_code) : this(name, manufacturer, "1033", version, upgrade_code)
		{
		}

		public string Name 
		{
			get 
			{
				return GetTag().GetAttribute("Name");
			}
			set 
			{
				AddAttribute("Name", value);
			}
		}

		public string Manufacturer 
		{
			get 
			{
				return GetTag().GetAttribute("Manufacturer");
			}
			set 
			{
				AddAttribute("Manufacturer", value);
			}
		}

		public string Language
		{
			get 
			{
				return GetTag().GetAttribute("Language");
			}
			set 
			{
				AddAttribute("Language", value);
			}
		}

		public string UpgradeCode 
		{
			get 
			{
				return GetTag().GetAttribute("UpgradeCode");
			}
			set 
			{
				AddAttribute("UpgradeCode", value);
			}
		}

		public int[] Version 
		{
			get 
			{
				return WixUtil.ConvertVersion(GetTag().GetAttribute("Version"));
			}
			set 
			{
				AddAttribute("Version", WixUtil.ConvertVersion(value));
			}
		}

		public void AddPackage(WixPackage package)
		{
			AddTag(package);
		}

		public void AddMedia(WixMedia media)
		{
			AddTag(media);
		}

		public void AddDirectory(WixDirectory directory)
		{
			AddTag(directory);
		}

		public void AddDirectoryRef(WixDirectoryRef directory_ref)
		{
			AddTag(directory_ref);
		}

		public void AddComponentGroup(WixComponentGroup component_group)
		{
			AddTag(component_group);
		}

		public void AddProperty(WixProperty property)
		{
			AddTag(property);
		}

		public void AddFeature(WixFeature feature)
		{
			AddTag(feature);
		}

		public void AddUIRef(WixUIRef uiref)
		{
			AddTag(uiref);
		}

		public void AddUpgrade(WixUpgrade upgrade)
		{
			AddTag(upgrade);
		}
	}
}
