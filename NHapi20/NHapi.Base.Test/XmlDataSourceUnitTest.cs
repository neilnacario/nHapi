using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.DataProvider;
using NHapi.Base.SourceGeneration;

namespace NHapi.Base.Test
{
	 [TestClass]
	 public class XmlDataSourceUnitTest : GeneratorUnitTestBase
	 {
			[TestMethod]
			public void makeAll_MessageCreated_MessageGrammarDefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 ConfigurationSettings.XmlFilename = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 // Act
				 MessageGenerator.makeAll(baseFolder, version);

				 // Assert
				 // TODO
			}

			[TestMethod]
			public void makeAll_SegmentsCreated_SegmentDefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 ConfigurationSettings.XmlFilename = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 // Act
				 SegmentGenerator.makeAll(baseFolder, version);

				 // Assert
				 // TODO
			}

			[TestMethod]
			public void makeAll_DataTypesCreated_DefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 ConfigurationSettings.XmlFilename = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 // Act
				 DataTypeGenerator.makeAll(baseFolder, version);

				 // Assert
				 // TODO
			}

			[TestMethod]
			public void makeAll_EventMappingCreated_DefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 ConfigurationSettings.XmlFilename = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 // Act
				 EventMappingGenerator.makeAll(baseFolder, version);

				 // Assert
				 // TODO
			}
			[TestMethod]
			public void makeAll_AllCreated_DefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 ConfigurationSettings.XmlFilename = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 // Act
				 SourceGenerator.makeAll(baseFolder, version);

				 // Assert
				 // TODO
			}
	 }
}
