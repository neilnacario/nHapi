using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.DataSource;
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
				 var source = new NHapi.Base.DataSource.Xml.MessageSource();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataSourceFactory.SetMessageSource(source);

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
				 var source = new NHapi.Base.DataSource.Xml.MessageSource();
				 source.FileName = "Mocks\\reverse-nHapi.xml";
				 var messageTargetFolder = GetTargetFolder("Message");
				 var groupTargetFolder = GetTargetFolder("Group");

				 DataSourceFactory.SetSegmentSource(source);

				 // Act
				 SegmentGenerator.makeAll(baseFolder, version);

				 // Assert
			}
	 }
}
