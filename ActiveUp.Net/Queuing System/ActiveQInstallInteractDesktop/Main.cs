// Copyright 2001-2010 - Active Up SPRLU (http://www.agilecomponents.com)
//
// This file is part of MailSystem.NET.
// MailSystem.NET is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// MailSystem.NET is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

using System;
using System.Configuration.Install; 
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace ActiveQInstallInteractDesktop
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class MainClass
	{
		[DllImport("InteractDesktop.dll")]
		public static extern int InteractDesktop(string service);

		private static readonly string _serviceName = "ActiveQ";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Add code to start application here
			//
			InteractDesktop(_serviceName);

		}
	}

/*	/// <summary>
	/// Summary description for LaunchService.
	/// </summary>
	[RunInstaller(true)]
	public class InteractiveInteractDesktop : System.Configuration.Install.Installer
	{
		[DllImport("InteractDesktop.dll")]
		public static extern int InteractDesktop(string service);

		private string _serviceName = "ActiveQ";

		public InteractiveInteractDesktop()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override void Install(System.Collections.IDictionary stateServer)
		{
			try
			{

				base.Install(stateServer);

				InteractDesktop(_serviceName);			

			}

			catch(Exception ex)
			{
				EventLog ev = new EventLog();
				ev.Source = "ActiveQ";
	
				ev.WriteEntry(string.Format("ActiveQInstallInteractDesktop:\n{0}.",ex.ToString()),EventLogEntryType.Error);

				throw ex;				
			}
		}
	}*/
}
