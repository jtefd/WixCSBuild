using System;
using System.IO;
using System.Collections;
using System.Security.Cryptography;

namespace WixBuild
{
	/// <summary>
	/// Summary description for FileHarvest.
	/// </summary>
	public class FileHarvest
	{
		private string directoryStr;

		public FileHarvest(string directoryStr)
		{
			this.directoryStr = directoryStr;
		}

		public WixComponentGroup ProcessDirectory(WixDirectoryRef root_dir_ref) 
		{
			WixComponentGroup comp_grp = new WixComponentGroup(directoryStr);

			Hashtable dirs = new Hashtable();

			Stack dirStack = new Stack();
			dirStack.Push(directoryStr);

			while (dirStack.Count > 0)
			{
				DirectoryInfo dir = new DirectoryInfo((string) dirStack.Pop());

				try
				{
					foreach (FileInfo f in dir.GetFiles())
					{
						if (BuildSession.Session.Verbose)
						{
							WixUtil.LogMsg(f.FullName);
						}

						WixComponent comp = new WixComponent(f.FullName);
						comp.AddGuid("Guid", WixUtil.GenGuid(f.FullName));

						comp_grp.AddComponentRef(comp.ComponentRef);

						WixFile wfile = new WixFile(f.FullName);
						comp.AddFile(wfile);

						if (dirs.Count == 0)
						{
							root_dir_ref.AddComponent(comp);
						}
						else
						{
							((WixDirectory) dirs[WixUtil.MD5Sum(dir.FullName)]).AddComponent(comp);
						}
					}

					foreach (DirectoryInfo d in dir.GetDirectories())
					{
						dirStack.Push(d.FullName);

						string parent_md5 = WixUtil.MD5Sum(dir.FullName);
						string child_md5 = WixUtil.MD5Sum(d.FullName);

						WixDirectory wix_dir = new WixDirectory(d);

						if (dirs.Contains(parent_md5))
						{
							((WixDirectory) dirs[parent_md5]).AddDirectory(wix_dir);
						}
						else
						{
							root_dir_ref.AddDirectory(wix_dir);
						}

						dirs.Add(child_md5, wix_dir);
					}
				}
				catch (Exception)
				{
					
				}
			}
			
			return comp_grp;
		}
	}
}
