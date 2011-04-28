using System;
using System.Xml;
using System.IO;

namespace WixBuild
{
	public class WixDirectory : WixTag
	{
		public static WixDirectory ProgramFiles
		{
			get
			{
				return new WixDirectory("ProgramFilesFolder", "PFiles");
			}
		}

		public static WixDirectory Target
		{
			get 
			{
				return new WixDirectory("TARGETDIR", "SourceDir");
			}
		}

		public static WixDirectory StartMenu
		{
			get
			{
				return new WixDirectory("ProgramMenuFolder", "PMenu");
			}
		}

		public static WixDirectory CreateApplicationRoot(string name)
		{
			return new WixDirectory("APPLICATIONROOTDIRECTORY", name);
		}

		public static readonly string PREFIX = "dir";

		public WixDirectory(string id, string name) : base("Directory")
		{
			Id = id;
			Name = name;
		}

		public WixDirectory(string name) : this(WixUtil.GenWixID(PREFIX, name), name)
		{
		}

		public WixDirectory(DirectoryInfo dir) : this(WixUtil.GenWixID(PREFIX, dir.FullName), dir.Name)
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
				AddAttribute("Id", value);
			}
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

		public WixDirectoryRef DirectoryRef
		{
			get
			{	
				return new WixDirectoryRef(Id);
			}
		}

		public void AddDirectory(WixDirectory directory)
		{
			AddTag(directory);
		}

		public void AddFile(WixFile file)
		{
			AddTag(file);
		}

		public void AddComponent(WixComponent component)
		{
			AddTag(component);
		}
	}
}
