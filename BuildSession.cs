using System;
using System.IO;
using System.Reflection;

namespace WixBuild
{
	/// <summary>
	/// Summary description for BuildSession.
	/// </summary>
	public class BuildSession
	{
		private static BuildSession build_session;

		public static BuildSession Session
		{
			get
			{
				if (build_session == null)
				{	
					build_session = new BuildSession();
				}

				return build_session;
			}
		}

		private bool verbose = false;

		private BuildSession()
		{
			
		}

		public bool Verbose
		{
			get
			{
				return verbose;
			}
			set
			{
				verbose = value;
			}
		}

		public DirectoryInfo InstallDir
		{
			get
			{
				FileInfo finfo = new FileInfo(Assembly.GetEntryAssembly().Location);
				return finfo.Directory;
			}
		}
	}
}
