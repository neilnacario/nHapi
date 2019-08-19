using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.Test.Mocks;
using NHapi.Base.DataSource;
using NHapi.Base.SourceGeneration;
using System.IO;

namespace NHapi.Base.Test
{
	[TestClass]
	public class MessageGeneratorUnitTest : GeneratorUnitTestBase
	{
		private static string GetBaseFolder()
		{
			return "C:\\test\\";
		}

		private static string GetVersion()
		{
			return "2.5";
		}

		private static string GetTargetFolder(string subFolder)
		{
			return $"{GetBaseFolder()}NHapi.Model.V{GetVersion().Replace(".", "")}\\{subFolder}\\";
		}

		[ClassInitialize]
		public static void TestFixtureSetup(TestContext context)
		{
			DeleteFolderContents(GetBaseFolder());
		}

		//[ClassCleanup]
		//public static void TestFixtureTearDown()
		//{
		//}


		[TestMethod]
		public void makeAll_MessageACKCreated_MessageDefinitionGiven()
		{
			// Arrange
			var baseFolder = GetBaseFolder();
			var version = GetVersion();
			var source = new MessageSourceMock();
			var messageTargetFolder = GetTargetFolder("Message");
			var message = new MessageDefinitionContainerMock();
			message.Message = "ACK";
			message.Chapter = "2.14.1";
			message.Segments.Add(new SegmentDef("MSH", "", true, false, "Message Header"));
			message.Segments.Add(new SegmentDef("SFT", "", false, true, "Software Segment"));
			message.Segments.Add(new SegmentDef("MSA", "", true, false, "Message Acknowledgment"));
			message.Segments.Add(new SegmentDef("ERR", "", false, true, "Error"));
			source.Messages.Add(message);
			DataSourceFactory.SetMessageSource(source);

			// Act
			MessageGenerator.makeAll(baseFolder, version);

			// Assert
			Assert.IsTrue(File.Exists($"{messageTargetFolder}ACK.cs"));
		}

		[TestMethod]
		public void makeAll_MessageWithGroupRSPK11Created_MessageDefinitionGiven()
		{
			// Arrange
			var baseFolder = GetBaseFolder();
			var version = GetVersion();
			var source = new MessageSourceMock();
			var messageTargetFolder = GetTargetFolder("Message");
			var groupTargetFolder = GetTargetFolder("Group");
			var message = new MessageDefinitionContainerMock();
			message.Message = "RSP_K11";
			message.Chapter = "5.3.1.2";
			// Refer to page 5-16 of V2.5/CH05.pdf on how the below structure was 
			message.Segments.Add(new SegmentDef("MSH", "", true, false, "Message Header"));
			message.Segments.Add(new SegmentDef("SFT", "", false, true, "Software Segment"));
			message.Segments.Add(new SegmentDef("MSA", "", true, false, "Message Acknowledgment"));
			message.Segments.Add(new SegmentDef("ERR", "", false, false, "Error"));
			message.Segments.Add(new SegmentDef("QAK", "", true, false, "Query Acknowledgment"));
			message.Segments.Add(new SegmentDef("QPD", "", true, false, "Query Parameter Definition"));
			message.Segments.Add(new SegmentDef("[", "ROW_DEFINITION", true, false, ""));
			message.Segments.Add(new SegmentDef("RDF", "", true, false, "Table Row Definition"));
			message.Segments.Add(new SegmentDef("RDT", "", false, true, "Table Row Data"));
			message.Segments.Add(new SegmentDef("]", "ROW_DEFINITION", true, false, ""));
			message.Segments.Add(new SegmentDef("DSC", "", false, false, "Continuation Pointer"));
			source.Messages.Add(message);
			DataSourceFactory.SetMessageSource(source);

			// Act
			MessageGenerator.makeAll(baseFolder, version);

			// Assert
			Assert.IsTrue(File.Exists($"{messageTargetFolder}RSP_K11.cs"));
			Assert.IsTrue(File.Exists($"{groupTargetFolder}RSP_K11_ROW_DEFINITION.cs"));
		}
	}
}
