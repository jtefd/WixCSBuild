using System;
using System.Collections;

namespace WixBuild
{
	/// <summary>
	/// Summary description for CommandLineParser.
	/// </summary>
	class CommandLineParser
	{
		private Hashtable user_args;

		public CommandLineParser(string[] args)
		{
			user_args = new Hashtable();

			string sw = "";
			Stack opts = null;

			for (int i = 0; i<args.Length; i++)
			{
				if (args[i].StartsWith("/"))
				{
					if (opts != null)
					{
						user_args.Add(sw, opts);	
					}

					sw = args[i].Substring(1);
					opts = new Stack();
				}
				else
				{
					opts.Push(args[i]);
				}
			}

			if (opts != null)
			{
				user_args.Add(sw, opts);	
			}
		}

		public string GetOpt(string i)
		{
			try
			{
				return (string) ((Stack) user_args[i]).Pop();
			}
			catch (System.NullReferenceException)
			{
				throw new Exception(i);
			}
		}

		public bool Exists(string i)
		{
			if (user_args.ContainsKey(i))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
