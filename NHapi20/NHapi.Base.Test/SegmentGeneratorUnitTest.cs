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
		[TestMethod]
		public void makeAll_SegmentMSHCreated_SegmentDefinitionGiven()
		{
			// Arrange
			var baseFolder = GetBaseFolder();
			var version = GetVersion();
			var source = new SegmentSourceMock();
			var targetFolder = GetTargetFolder("Segment");
			var segment = new SegmentDefinitionContainerMock();
			segment.Description = "Message Header";
			source.Segments["MSH"] = segment;
			int fieldCount = 1;
			AddField(segment.Elements, fieldCount++, "ST", "R", 1, 1, 0, "Field Separator");
			AddField(segment.Elements, fieldCount++, "ST", "R", 1, 4, 0, "Encoding Characters");
			AddField(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Sending Application");
			AddField(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Sending Facility");
			AddField(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Receiving Application");
			AddField(segment.Elements, fieldCount++, "HD", "O", 1, 227, 0, "Receiving Facility");
			AddField(segment.Elements, fieldCount++, "TS", "R", 1, 26, 0, "Date/Time Of Message");
			AddField(segment.Elements, fieldCount++, "ST", "O", 1, 40, 0, "Security");
			AddField(segment.Elements, fieldCount++, "MSG", "R", 1, 15, 0, "Message Type");
			AddField(segment.Elements, fieldCount++, "ST", "R", 1, 20, 0, "Message Control ID");
			AddField(segment.Elements, fieldCount++, "PT", "R", 1, 3, 0, "Processing ID");
			AddField(segment.Elements, fieldCount++, "VID", "R", 1, 60, 0, "Version ID");
			AddField(segment.Elements, fieldCount++, "NM", "O", 1, 15, 0, "Sequence Number");
			AddField(segment.Elements, fieldCount++, "ST", "O", 1, 180, 0, "Continuation Pointer");
			AddField(segment.Elements, fieldCount++, "ID", "O", 1, 2, 155, "Accept Acknowledgment Type");
			AddField(segment.Elements, fieldCount++, "ID", "O", 1, 2, 155, "Application Acknowledgment Type");
			AddField(segment.Elements, fieldCount++, "ID", "O", 1, 3, 399, "Country Code");
			AddField(segment.Elements, fieldCount++, "ID", "O", 0, 16, 211, "Character Set");
			AddField(segment.Elements, fieldCount++, "CE", "O", 1, 250, 0, "Principal Language Of Message");
			AddField(segment.Elements, fieldCount++, "ID", "O", 1, 20, 356, "Alternate Character Set Handling Scheme");
			AddField(segment.Elements, fieldCount++, "EI", "O", 0, 427, 0, "Message Profile Identifier");

			DataSourceFactory.SetSegmentSource(source);

			// Act
			SegmentGenerator.makeAll(baseFolder, version);

			// Assert
			Assert.IsTrue(File.Exists($"{targetFolder}MSH.cs"));
		}

		private void AddField(ArrayList elements, int field, string type, string optional, int repetitions, int length, int table, string description)
		{
			var element = new SegmentElement() { field = field, type = type, opt = optional, repetitions = repetitions, length = length, table = table, desc = description };
			elements.Add(element);
		}
	}
}
