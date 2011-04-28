using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace WixBuild
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class CommandLineMain
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				CommandLineParser cli = new CommandLineParser(args);

				if (cli.Exists("?"))
				{
					ShowHelp();
					Environment.Exit(0);
				}

				BuildConfig buildConfig = new BuildConfig();

				buildConfig.Directory = cli.GetOpt("source");
				buildConfig.PackageName = cli.GetOpt("package");
				buildConfig.Version = cli.GetOpt("version");

				if (cli.Exists("author"))
				{
					buildConfig.Manufacturer = cli.GetOpt("author");
				}
				else
				{
					buildConfig.Manufacturer = Environment.UserName;
				}

				if (cli.Exists("path"))
				{
					buildConfig.Path = true;
				}

				if (cli.Exists("shortcut"))
				{
					buildConfig.Shortcut = cli.GetOpt("shortcut");
				}

				BuildManager builder = new BuildManager(buildConfig);

				if (cli.Exists("verbose"))
				{
					BuildSession.Session.Verbose = true;
				}

				builder.build();

				if (cli.Exists("tc-build-num"))
				{
					Console.WriteLine("##teamcity[buildNumber '{0}']", buildConfig.Version);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				ShowHelp();
				Environment.Exit(1);
			}
		}

		private static void ShowHelp()
		{
			string msg = @"WiXBuild - WiX Generator and Build Manager
 
 usage:	wixbuild.exe [/?] /source sourceDir /version versionNo /package packageName
  
  /source			Specify directory of files to be used in the build
  /version			Specify version number of the package
  /package			Specify name of the package
  /author			Specify author of the package

  /path				Add the installation directory to the user's PATH

  /shortcut			Add the given file to the user's Start Menu as a shortcut

  /verbose			Show extra information

  /tc-build-num		When running under TeamCity, echo a service message to update the build number


  /?				Show help (this page)
";

			Console.Error.WriteLine(msg);
		}
	}
}
