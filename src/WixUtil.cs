using System;
using System.IO;
using System.Security.Cryptography;

namespace WixBuild
{
	public class WixUtil
	{
		public static string ConvertVersion(int[] ver)
		{
			return string.Format("{0}.{1}.{2}.{3}", ver[0], ver[1], ver[2], ver[3]);
		}

		public static int[] ConvertVersion(string version)
		{
			int[] ver = new int[4];

			int count = 0;

			foreach (string i in version.Split('.'))
			{
				if (count == 4)
				{
					break;
				}

				ver[count] = Convert.ToInt16(i);
				count++;
			}

			return ver;
		}

		public static string MD5Sum(string str)
		{
			MD5 md5 = MD5.Create();

			byte[] hash_b = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(str));

			return System.BitConverter.ToString(hash_b).Replace("-", "");
		}

		public static string GenUpgradeGuid(string package_name)
		{
			MD5 md5 = MD5.Create();

			byte[] hash_b = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(package_name));

			Guid guid = new Guid(hash_b);

			return guid.ToString("D");
		}

		public static string GenGuid(string filename)
		{
			FileStream fs = File.OpenRead(filename);
			MD5 md5 = MD5.Create();

			byte[] hashsum = md5.ComputeHash(fs);

			Guid guid = new Guid(hashsum);
			
			return guid.ToString("D");
		}

		public static string GenWixID(string prefix, string text)
		{
			return string.Format("{0}{1}", prefix.ToUpper(), MD5Sum(text.ToUpper()));
		}

		public static void LogMsg(string msg)
		{
			if (BuildSession.Session.Verbose)
			{
				Console.WriteLine(msg);
			}
		}
	}
}
