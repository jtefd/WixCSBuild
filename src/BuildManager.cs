using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace WixBuild
{
	public class BuildManager
	{		
		private Wix wix;
		private DirectoryInfo build_dir;
		private string name;
		private string package_name;
		private string manufacturer;
		private string directory;
		private string version;
		private string[] features;
		private bool path;
		private string shortcut;

		public BuildManager(string package_name, string manufacturer, string directory, string version)
		{
			PackageName = package_name;
			Manufacturer = manufacturer;
			Directory = directory;
			Version = version;
		}

		public string PackageName
		{
			get
			{
				return package_name;
			}
			set
			{
				package_name = value;
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
				return manufacturer;
			}
			set
			{
				manufacturer = value;
			}
		}

		public string Directory
		{
			get
			{
				return directory;
			}
			set
			{	
				directory = value;
			}	
		}

		public string Version
		{
			get
			{
				return version;
			}
			set
			{
				version = value;
			}
		}

		public bool Path
		{
			get
			{
				return path;
			}
			set
			{
				path = value;
			}
		}

		public string Shortcut
		{
			get
			{
				return shortcut;
			}
			set
			{
				shortcut = value;
			}
		}

		public void build()
		{
			wix = new Wix();

			DirectoryInfo dirinfo = new DirectoryInfo(directory);

			name = dirinfo.Name;

			build_dir = new DirectoryInfo(Environment.GetEnvironmentVariable("TMP")).CreateSubdirectory(WixUtil.MD5Sum(dirinfo.FullName));
			
			if (build_dir.Exists)
			{
				build_dir.Delete(true);
			}
			
			build_dir.Create();

			Console.WriteLine("Using temporary directory: {0}", build_dir.FullName);
			
			WixProduct product = new WixProduct(PackageFullTitle, manufacturer, WixUtil.ConvertVersion(version), WixUtil.GenUpgradeGuid(PackageName));
			product.AddPackage(new WixPackage(true));
			product.AddMedia(new WixMedia(1, "product.cab", true));

			WixDirectory app_root = WixDirectory.CreateApplicationRoot(PackageName);
			WixDirectory pfiles = WixDirectory.ProgramFiles;
			WixDirectory smenu = WixDirectory.StartMenu;
			WixDirectory target = WixDirectory.Target;

			pfiles.AddDirectory(app_root);
			target.AddDirectory(pfiles);
			target.AddDirectory(smenu);

			WixDirectory start_menu_dir = new WixDirectory(PackageFullTitle);

			smenu.AddDirectory(start_menu_dir);

			product.AddDirectory(target);
	
			FileHarvest harvest = new FileHarvest(dirinfo.FullName);

			WixDirectoryRef app_root_ref = app_root.DirectoryRef;

			WixComponentGroup comp_group = harvest.ProcessDirectory(app_root_ref);

			if (Path)
			{
				WixComponent env_comp = new WixComponent("env_path");
				env_comp.AddGuid("Guid");
				env_comp.AddEnvironment(new WixEnvironment("path", "set", false, false, "last", "[APPLICATIONROOTDIRECTORY]"));
				env_comp.KeyPath = true;

				comp_group.AddComponentRef(env_comp.ComponentRef);
				app_root_ref.AddComponent(env_comp);
			}

			if (Shortcut != null)
			{
				FileInfo shortcut_file = new FileInfo(Shortcut);

				if (shortcut_file.Exists)
				{
					string id = "";

					foreach (XmlElement element in (app_root_ref.GetTag().SelectNodes("//File")))
					{
						if (element.GetAttribute("Source").ToUpper().Equals(shortcut_file.FullName.ToUpper()))
						{
							id = element.GetAttribute("Id");

							WixShortcut shortcut = new WixShortcut(new FileInfo(Shortcut).Name, new FileInfo(Shortcut).Name, start_menu_dir.Id, "INSTALLDIR", false);

							element.AppendChild(shortcut.GetTag());
						}
					}
				}
			}

			product.AddDirectoryRef(app_root_ref);
			product.AddComponentGroup(comp_group);

			if (features == null)
			{
				WixFeature feature = new WixFeature("Main", 1);
				feature.AddComponentGroupRef(comp_group.ComponentGroupRef);

				product.AddFeature(feature);
			}
			else
			{

			}
			
			product.AddUIRef(WixUIRef.Custom);
			product.AddProperty(new WixProperty("wixui_installdir", app_root.Id));

			WixUpgrade upgrade = new WixUpgrade(product.GetTag().GetAttribute("UpgradeCode"));
			upgrade.AddUpgradeVersion(new WixUpgradeVersion("0.0.0.1", true, Version, false));

			product.AddUpgrade(upgrade);

			WixInstallExecuteSequence i_exec_seq = new WixInstallExecuteSequence();
			i_exec_seq.AddRemoveExistingProducts();

			product.AddTag(i_exec_seq);

			wix.addProduct(product);

			Candle();
			Light();
		}

		public string GetSource()
		{
			return wix.GetSource();
		}

		private void SaveWxs(string filename)
		{
			TextWriter writer = new StreamWriter(filename);
            writer.Write(GetSource());
			writer.Close();
		}

		private ProcessStartInfo SetupProcess(string command)
		{
			command = string.Format("{0}\\wix\\{1}", new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName, command);

			ProcessStartInfo procinf = new ProcessStartInfo(command);
			procinf.CreateNoWindow = true;
			procinf.UseShellExecute = false;
			procinf.WorkingDirectory = build_dir.FullName;

			if (BuildSession.Session.Verbose)
			{
				procinf.RedirectStandardOutput = true;
				procinf.RedirectStandardError = true;
			}
			else 
			{
				procinf.RedirectStandardOutput = false;
				procinf.RedirectStandardError = false;
			}

			procinf.RedirectStandardInput = false;

			return procinf;
		}

		private void Candle()
		{
			string filename = string.Format("{0}\\main.wxs", build_dir.FullName);
			SaveWxs(filename);

			File.Copy(string.Format("{0}\\WixUI_Custom.wxs", BuildSession.Session.InstallDir.FullName), string.Format("{0}\\WixUI_Custom.wxs", build_dir.FullName));

			ProcessStartInfo procinf = SetupProcess("candle.exe");
			procinf.Arguments = "*.wxs";
			
			Process p = Process.Start(procinf);

			PrintProcessOutput(p);

			p.WaitForExit();
		}

		private void Light()
		{
			string outname = string.Format("{0}\\{1}.msi", Environment.CurrentDirectory, PackageFullTitle);
			
			ProcessStartInfo procinf = SetupProcess("light.exe");
			procinf.Arguments = string.Format("-spdb -sice:ICE08 -o \"{0}\" -ext WixUIExtension *.wixobj", outname);
			
			Process p = Process.Start(procinf);

			PrintProcessOutput(p);

			p.WaitForExit();
		}

		private void PrintProcessOutput(Process p)
		{
			if (BuildSession.Session.Verbose)
			{
				bool flag = true;

				while (flag)
				{
					bool inner_flag = false;

					if (p.StandardOutput.Peek() != -1)
					{
						Console.WriteLine(p.StandardOutput.ReadLine());
						inner_flag = true;
					}

					if (p.StandardError.Peek() != -1)
					{
						Console.Error.WriteLine(p.StandardError.ReadLine());
						inner_flag = true;
					}

					flag = inner_flag;
				}
				Console.Write(p.StandardOutput.ReadToEnd());
				Console.Error.Write(p.StandardError.ReadToEnd());
			}
		}
	}
}
