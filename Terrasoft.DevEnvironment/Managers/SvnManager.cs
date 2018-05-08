namespace Terrasoft.DevEnvironment.Managers {
	using SharpSvn;
	using SharpSvn.Security;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;

	public class SvnManager {

		public string SvnUserName { get; }
		public string SvnUserPassword { get; }

		public SvnManager(string userName, string userPasword) {
			SvnUserName = userName;
			SvnUserPassword = userPasword;
		}

		public void Ping(string path) {
			using (SvnClient client = new SvnClient()) {
				client.Authentication.ForceCredentials(SvnUserName, SvnUserPassword);
				if (!client.GetInfo(new Uri(path), out SvnInfoEventArgs info)) {
					throw new Exception("Cann't reach svn server.");
				}
			}
		}

		public void Checkout(string repoUrl, string destinationFolderPath) {
			using (SvnClient client = new SvnClient()) {
				client.Authentication.ForceCredentials(SvnUserName, SvnUserPassword);
				client.CheckOut(new Uri(repoUrl), destinationFolderPath);
			}
		}

		public void DeletenAuthenticationCachedItems() {
			using (SvnClient client = new SvnClient()) {
				foreach (var svnAuthenticationCacheItem in client.Authentication.GetCachedItems(SvnAuthenticationCacheType.UserNamePassword)) {
					svnAuthenticationCacheItem.Delete();
				}
			}
		}

		public void Checkout(string repoUrl, int revision, string destinationFolderPath) {
			using (SvnClient client = new SvnClient()) {
				client.Authentication.ForceCredentials(SvnUserName, SvnUserPassword);
				client.CheckOut(new Uri(repoUrl), destinationFolderPath, new SvnCheckOutArgs() { Revision = revision });
			}
		}

		public List<string> GetDirectories(string repoUrl) {
			List<string> files = new List<string>();
			using (SvnClient svnClient = new SvnClient()) {
				svnClient.Authentication.ForceCredentials(SvnUserName, SvnUserPassword);
				Collection<SvnListEventArgs> contents;
				if (svnClient.GetList(new Uri(repoUrl), out contents)) {
					foreach (SvnListEventArgs item in contents) {
						files.Add(item.Path);
					}
				}
			}
			return files;
		}

	}

}
