using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.Test.Mocks;
using NHapi.Base.DataProvider;
using NHapi.Base.SourceGeneration;
using System.IO;

namespace NHapi.Base.Test
{
	 [TestClass]
	 public class MessageGeneratorUnitTest : GeneratorUnitTestBase
	 {
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

			[TestMethod]
			public void makeAll_MessageWithGroupORUR01Created_MessageDefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 var source = new MessageSourceMock();
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");
				 var message = new MessageDefinitionContainerMock();
				 message.Message = "ORU_R01";
				 message.Chapter = "5.7.3.1";
				 message.Segments.Add(new SegmentDef("MSH", "", true, false, "Message Header"));
				 message.Segments.Add(new SegmentDef("SFT", "", false, true, "Software Segment"));
				 message.Segments.Add(new SegmentDef("{", "PATIENT_RESULT", true, true, ""));
				 message.Segments.Add(new SegmentDef("[", "PATIENT", false, false, ""));
				 message.Segments.Add(new SegmentDef("PID", "", true, false, "Patient Identification"));
				 message.Segments.Add(new SegmentDef("PD1", "", false, false, "Patient Additional Demographic"));
				 message.Segments.Add(new SegmentDef("NTE", "", false, true, "Notes and Comments"));
				 message.Segments.Add(new SegmentDef("NK1", "", false, true, "Next of Kin / Associated Parties"));
				 message.Segments.Add(new SegmentDef("[", "VISIT", false, false, ""));
				 message.Segments.Add(new SegmentDef("PV1", "", true, false, "Patient Visit"));
				 message.Segments.Add(new SegmentDef("PV2", "", false, false, "Patient Visit - Additional Information"));
				 message.Segments.Add(new SegmentDef("]", "VISIT", false, false, ""));
				 message.Segments.Add(new SegmentDef("]", "PATIENT", true, false, ""));
				 message.Segments.Add(new SegmentDef("{", "ORDER_OBSERVATION", true, true, ""));
				 message.Segments.Add(new SegmentDef("ORC", "", false, false, "Common Order"));
				 message.Segments.Add(new SegmentDef("OBR", "", true, false, "Observation Request"));
				 message.Segments.Add(new SegmentDef("NTE", "", false, true, "Notes and Comments"));
				 message.Segments.Add(new SegmentDef("[", "TIMING_QTY", false, true, ""));
				 message.Segments.Add(new SegmentDef("{", "TIMING_QTY", false, true, ""));
				 message.Segments.Add(new SegmentDef("TQ1", "", true, false, "Timing/Quantity"));
				 message.Segments.Add(new SegmentDef("TQ2", "", false, true, "Timing/Quantity Relationship"));
				 message.Segments.Add(new SegmentDef("}", "TIMING_QTY", false, true, ""));
				 message.Segments.Add(new SegmentDef("]", "TIMING_QTY", false, true, ""));
				 message.Segments.Add(new SegmentDef("CTD", "", false, false, "Contact Data"));
				 message.Segments.Add(new SegmentDef("[", "OBSERVATION", false, true, ""));
				 message.Segments.Add(new SegmentDef("{", "OBSERVATION", false, true, ""));
				 message.Segments.Add(new SegmentDef("OBX", "", true, false, "Observation/Result"));
				 message.Segments.Add(new SegmentDef("NTE", "", false, true, "Notes and Comments"));
				 message.Segments.Add(new SegmentDef("}", "OBSERVATION", false, true, ""));
				 message.Segments.Add(new SegmentDef("]", "OBSERVATION", false, true, ""));
				 message.Segments.Add(new SegmentDef("FT1", "", false, true, "Financial Transaction"));
				 message.Segments.Add(new SegmentDef("CTI", "", false, true, "Clinical Trial Identification"));
				 message.Segments.Add(new SegmentDef("[", "SPECIMEN", false, true, ""));
				 message.Segments.Add(new SegmentDef("{", "SPECIMEN", false, true, ""));
				 message.Segments.Add(new SegmentDef("SPM", "", true, false, "Specimen"));
				 message.Segments.Add(new SegmentDef("OBX", "", false, true, "Observation/Result"));
				 message.Segments.Add(new SegmentDef("}", "SPECIMEN", false, true, ""));
				 message.Segments.Add(new SegmentDef("]", "SPECIMEN", false, true, ""));
				 message.Segments.Add(new SegmentDef("}", "ORDER_OBSERVATION", true, false, ""));
				 message.Segments.Add(new SegmentDef("}", "PATIENT_RESULT", true, true, ""));
				 message.Segments.Add(new SegmentDef("DSC", "", false, false, "Continuation Pointer"));
				 source.Messages.Add(message);
				 DataSourceFactory.SetMessageSource(source);

				 // Act
				 MessageGenerator.makeAll(baseFolder, version);

				 // Assert
				 Assert.IsTrue(File.Exists($"{messageTargetFolder}ORU_R01.cs"));
				 Assert.IsTrue(File.Exists($"{groupTargetFolder}ORU_R01_PATIENT_RESULT.cs"));
			}
	 }
}
