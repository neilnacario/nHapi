using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.DataSource;
using NHapi.Base.SourceGeneration;
using NHapi.Base.Test.Mocks;
using System.IO;

namespace NHapi.Base.Test
{
	[TestClass]
	public class EventMappingGeneratorUnitTest : GeneratorUnitTestBase
	{
		[TestMethod]
		public void makeAll_MapCreated_MappingGivens()
		{
			// Arrange
			var baseFolder = GetBaseFolder();
			var version = GetVersion();
			var source = new EventMappingSourceMock();
			var messageTargetFolder = GetTargetFolder("EventMapping");
			source.EventMap.Add(new EventMappingContainerMock() { Type = "ADT", Event = "A01", Structure = "ADT_A01" });
			source.EventMap.Add(new EventMappingContainerMock() { Type = "ADT", Event = "A02", Structure = "ADT_A02" });
			source.EventMap.Add(new EventMappingContainerMock() { Type = "ADT", Event = "A03", Structure = "ADT_A03" });
			DataSourceFactory.SetEventMappingSource(source);

			// Act
			EventMappingGenerator.makeAll(baseFolder, version);

			// Assert
			Assert.IsTrue(File.Exists($"{messageTargetFolder}EventMap.properties"));
		}
	}
}
