using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace WixBuild
{
	/// <summary>
	/// Summary description for BuildConfig.
	/// </summary>
	public class BuildConfig
	{
		private string _packageName, _manufacturer, _directory, _version, _shortcut;
		private bool _path;

		public BuildConfig()
		{
		}

		public string PackageName
		{
			get
			{
				return _packageName;
			}
			set
			{
				_packageName = value;
			}
		}

		public string PackageFullTitle
		{
			get
			{
				return string.Format("{0} {1}", PackageName, Version);
			}
		}

		public string Manufacturer
		{
			get
			{
				return _manufacturer;
			}
			set
			{
				_manufacturer = value;
			}
		}

		public string Directory
		{
			get
			{
				return _directory;
			}
			set
			{	
				_directory = value;
			}	
		}

		public string Version
		{
			get
			{
				return _version;
			}
			set
			{
				if (File.Exists(value))
				{
					_version = FileVersionInfo.GetVersionInfo(value).FileVersion;
				}
				else
				{
					_version = value;
				}
			}
		}

		public bool Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}

		public string Shortcut
		{
			get
			{
				return _shortcut;
			}
			set
			{
				_shortcut = value;
			}
		}
	}
}
