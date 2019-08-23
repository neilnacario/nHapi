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
				 var source = new NHapi.Base.DataProvider.Xml.GrammarProvider();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataProviderFactory.SetMessageSource(source);

				 // Act
				 MessageGenerator.makeAll(baseFolder, version);

				 // Assert
			}

			[TestMethod]
			public void makeAll_SegmentsCreated_SegmentDefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 var source = new NHapi.Base.DataProvider.Xml.GrammarProvider();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataProviderFactory.SetSegmentSource(source);

				 // Act
				 SegmentGenerator.makeAll(baseFolder, version);

				 // Assert
			}

			[TestMethod]
			public void makeAll_DataTypesCreated_DefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 var source = new NHapi.Base.DataProvider.Xml.GrammarProvider();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataProviderFactory.SetDataTypeSource(source);

				 // Act
				 DataTypeGenerator.makeAll(baseFolder, version);

				 // Assert
			}

			[TestMethod]
			public void makeAll_EventMappingCreated_DefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 var source = new NHapi.Base.DataProvider.Xml.GrammarProvider();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataProviderFactory.SetEventMappingSource(source);

				 // Act
				 EventMappingGenerator.makeAll(baseFolder, version);

				 // Assert
			}
			[TestMethod]
			public void makeAll_AllCreated_DefinitionGiven()
			{
				 // Arrange
				 var baseFolder = GetBaseFolder();
				 var version = GetVersion();
				 var source = new NHapi.Base.DataProvider.Xml.GrammarProvider();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataProviderFactory.SetMessageSource(source);
				 DataProviderFactory.SetSegmentSource(source);
				 DataProviderFactory.SetDataTypeSource(source);
				 DataProviderFactory.SetEventMappingSource(source);

				 // Act
				 SourceGenerator.makeAll(baseFolder, version);

				 // Assert
			}
	 }
}
