using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.DataProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHapi.Base.Test
{
	 [TestClass]
	 public class GeneratorUnitTestBase
	 {
			[AssemblyInitialize]
			public static void AssemblyInitialize(TestContext context)
			{
				 DeleteFolderContents(GetBaseFolder());
			}

			[AssemblyCleanup]
			public static void AssemblyCleanup()
			{
			}

			[TestInitialize]
			public void TestInitialization()
			{
				 DataProviderFactory.Instance.SetProvider(null);
			}

			protected static string GetBaseFolder()
			{
				 return "C:\\test\\";
			}

			protected static string GetVersion()
			{
				 return "2.5";
			}

			protected static string GetTargetFolder(string subFolder)
			{
				 return $"{GetBaseFolder()}NHapi.Model.V{GetVersion().Replace(".", "")}\\{subFolder}\\";
			}

			protected static void DeleteFolderContents(string folderPath)
			{
				 DirectoryInfo di = new DirectoryInfo(folderPath);

				 foreach (FileInfo file in di.GetFiles())
				 {
						file.Delete();
				 }
				 foreach (DirectoryInfo dir in di.GetDirectories())
				 {
						dir.Delete(true);
				 }
			}
	 }
}
