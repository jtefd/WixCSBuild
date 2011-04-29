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
		private DirectoryInfo buildDir;
		private string name;

		private BuildConfig buildConfig;

		public BuildManager(BuildConfig buildConfig)
		{
			this.buildConfig = buildConfig;
		}

		public void build()
		{
			wix = new Wix();

			DirectoryInfo dirinfo = new DirectoryInfo(buildConfig.Directory);

			name = dirinfo.Name;

			buildDir = new DirectoryInfo(Environment.GetEnvironmentVariable("TMP")).CreateSubdirectory(WixUtil.MD5Sum(dirinfo.FullName));
			
			if (buildDir.Exists)
			{
				buildDir.Delete(true);
			}
			
			buildDir.Create();

			Console.WriteLine("Using temporary directory: {0}", buildDir.FullName);
			
			WixProduct product = new WixProduct(
				buildConfig.PackageFullTitle,
				buildConfig.Manufacturer, 
				WixUtil.ConvertVersion(buildConfig.Version), 
				WixUtil.GenUpgradeGuid(buildConfig.PackageName)
			);
			
			product.AddPackage(new WixPackage(true));
			product.AddMedia(new WixMedia(1, "product.cab", true));

			WixDirectory app_root = WixDirectory.CreateApplicationRoot(buildConfig.PackageName);
			WixDirectory pfiles = WixDirectory.ProgramFiles;
			WixDirectory smenu = WixDirectory.StartMenu;
			WixDirectory target = WixDirectory.Target;

			pfiles.AddDirectory(app_root);
			target.AddDirectory(pfiles);
			target.AddDirectory(smenu);

			WixDirectory start_menu_dir = new WixDirectory(buildConfig.PackageFullTitle);

			smenu.AddDirectory(start_menu_dir);

			product.AddDirectory(target);
	
			FileHarvest harvest = new FileHarvest(dirinfo.FullName);

			WixDirectoryRef app_root_ref = app_root.DirectoryRef;

			WixComponentGroup comp_group = harvest.ProcessDirectory(app_root_ref);

			if (buildConfig.Path)
			{
				WixComponent env_comp = new WixComponent("env_path");
				env_comp.AddGuid("Guid");
				env_comp.AddEnvironment(new WixEnvironment("path", "set", false, false, "last", "[APPLICATIONROOTDIRECTORY]"));
				env_comp.KeyPath = true;

				comp_group.AddComponentRef(env_comp.ComponentRef);
				app_root_ref.AddComponent(env_comp);
			}

			if (buildConfig.Shortcut != null)
			{
				FileInfo shortcut_file = new FileInfo(buildConfig.Shortcut);

				if (shortcut_file.Exists)
				{
					//string id = "";

					foreach (XmlElement element in (app_root_ref.GetTag().SelectNodes("//File")))
					{
						if (element.GetAttribute("Source").ToUpper().Equals(shortcut_file.FullName.ToUpper()))
						{
							//id = element.GetAttribute("Id");

							WixShortcut shortcut = new WixShortcut(new FileInfo(buildConfig.Shortcut).Name, new FileInfo(buildConfig.Shortcut).Name, start_menu_dir.Id, "INSTALLDIR", false);

							element.AppendChild(shortcut.GetTag());
						}
					}
				}
			}

			product.AddDirectoryRef(app_root_ref);
			product.AddComponentGroup(comp_group);

			WixFeature feature = new WixFeature("Main", 1);
			feature.AddComponentGroupRef(comp_group.ComponentGroupRef);

			product.AddFeature(feature);
			
			product.AddUIRef(WixUIRef.Custom);
			product.AddProperty(new WixProperty("wixui_installdir", app_root.Id));

			WixUpgrade upgrade = new WixUpgrade(product.GetTag().GetAttribute("UpgradeCode"));
			upgrade.AddUpgradeVersion(new WixUpgradeVersion("0.0.0.1", true, buildConfig.Version, false));

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
			procinf.WorkingDirectory = buildDir.FullName;

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
			string filename = Path.Combine(buildDir.FullName, "main.wxs");
			SaveWxs(filename);

			File.Copy(Path.Combine(BuildSession.Session.InstallDir.FullName, "WixUI_Custom.wxs"), Path.Combine(buildDir.FullName, "WixUI_Custom.wxs"));

			ProcessStartInfo procinf = SetupProcess("candle.exe");
			procinf.Arguments = "*.wxs";
			
			Process p = Process.Start(procinf);

			PrintProcessOutput(p);

			p.WaitForExit();
		}

		private void Light()
		{
			string outname = String.Format("{0}.msi", Path.Combine(Environment.CurrentDirectory, buildConfig.PackageFullTitle));
			
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
					bool innerFlag = false;

					if (p.StandardOutput.Peek() != -1)
					{
						Console.WriteLine(p.StandardOutput.ReadLine());
						innerFlag = true;
					}

					if (p.StandardError.Peek() != -1)
					{
						Console.Error.WriteLine(p.StandardError.ReadLine());
						innerFlag = true;
					}

					flag = innerFlag;
				}
				Console.Write(p.StandardOutput.ReadToEnd());
				Console.Error.Write(p.StandardError.ReadToEnd());
			}
		}
	}
}
