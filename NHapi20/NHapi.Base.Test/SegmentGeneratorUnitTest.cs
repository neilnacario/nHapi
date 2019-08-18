using System;
using System.Collections;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.DataSource;
using NHapi.Base.SourceGeneration;
using NHapi.Base.Test.Mocks;

namespace NHapi.Base.Test
{
	 [TestClass]
	 public class SegmentGeneratorUnitTest : GeneratorUnitTestBase
	 {
			private static string GetBaseFolder()
			{
				 return "C:\\test\\";
			}

			private static string GetVersion()
			{
				 return "2.5";
			}

			private static string GetTargetFolder()
			{
				 return $"{GetBaseFolder()}NHapi.Model.V{GetVersion().Replace(".", "")}\\Segment\\";
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
			public void makeAll_SegmentMSHCreated_SegmentDefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 var source = new SegmentSourceMock();
				 var targetFolder = GetTargetFolder();
				 var segment = new SegmentDefinitionContainerMock();
				 segment.Description = "Message Header";
				 source.Segments["MSH"] = segment;
				 int fieldCount = 1;
				 AddSegment(segment.Elements, fieldCount++, "ST", "R", 1, 1, 0, "Field Separator");
				 AddSegment(segment.Elements, fieldCount++, "ST", "R", 1, 4, 0, "Encoding Characters");
				 AddSegment(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Sending Application");
				 AddSegment(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Sending Facility");
				 AddSegment(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Receiving Application");
				 AddSegment(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Receiving Facility");
				 AddSegment(segment.Elements, fieldCount++, "TS", "R", 1, 26, 0, "Date/Time Of Message");
				 AddSegment(segment.Elements, fieldCount++, "ST", "O", 1, 40, 0, "Security");
				 AddSegment(segment.Elements, fieldCount++, "MSG", "R", 1, 15, 0, "Message Type");
				 AddSegment(segment.Elements, fieldCount++, "ST", "R", 1, 20, 0, "Message Control ID");
				 AddSegment(segment.Elements, fieldCount++, "PT", "R", 1, 3, 0, "Processing ID");
				 AddSegment(segment.Elements, fieldCount++, "VID", "R", 1, 60, 0, "Version ID");
				 AddSegment(segment.Elements, fieldCount++, "NM", "O", 1, 15, 0, "Sequence Number");
				 AddSegment(segment.Elements, fieldCount++, "ST", "O", 1, 180, 0, "Continuation Pointer");
				 AddSegment(segment.Elements, fieldCount++, "ID", "O", 1, 2, 155, "Accept Acknowledgment Type");
				 AddSegment(segment.Elements, fieldCount++, "ID", "O", 1, 2, 155, "Application Acknowledgment Type");
				 AddSegment(segment.Elements, fieldCount++, "ID", "O", 1, 3, 399, "Country Code");
				 AddSegment(segment.Elements, fieldCount++, "ID", "O", 0, 16, 211, "Character Set");
				 AddSegment(segment.Elements, fieldCount++, "CE", "O", 1, 250, 0, "Principal Language Of Message");
				 AddSegment(segment.Elements, fieldCount++, "ID", "O", 1, 20, 356, "Alternate Character Set Handling Scheme");
				 AddSegment(segment.Elements, fieldCount++, "EI", "O", 0, 427, 0, "Message Profile Identifier");

				 DataSourceFactory.SetSegmentSource(source);

				 // Act
				 SegmentGenerator.makeAll(baseFolder, version);

				 // Assert
				 Assert.IsTrue(File.Exists($"{targetFolder}MSH.cs"));
			}

			private void AddSegment(ArrayList elements, int field, string type, string optional, int repititions, int length, int table, string description)
			{
				 var element = new SegmentElement() { field = field, type = type, opt = optional, repetitions = repititions, length = length, table = table, desc = description };
				 elements.Add(element);
			}
	 }
}
