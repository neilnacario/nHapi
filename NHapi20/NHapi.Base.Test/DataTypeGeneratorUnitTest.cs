using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHapi.Base.DataSource;
using NHapi.Base.SourceGeneration;
using NHapi.Base.Test.Mocks;
using NHapi.Base;
using System.IO;

namespace NHapi.Base.Test
{

	[TestClass]
	public class DataTypeGeneratorUnitTest : GeneratorUnitTestBase
	{
		[TestMethod]
		public void makeAll_PrimitiveTypesCreated_TypeDefinitionsGiven()
		{
			// Arrange
			var baseFolder = GetBaseFolder();
			var version = GetVersion();
			var source = new DataTypeSourceMock();
			var targetFolder = GetTargetFolder("Datatype");
			// Primitive types that are generated: FT, ST, TX, NM, SI, TN, GTS
			// Primitieve types that must be coded manually: IS, ID, DT, DTM, and TM
			AddPrimitiveComponent(source, "FT", "Formatted Text Data");
			AddPrimitiveComponent(source, "ST", "Structured Text");
			AddPrimitiveComponent(source, "TX", "Text Data");
			AddPrimitiveComponent(source, "NM", "Numeric");
			AddPrimitiveComponent(source, "SI", "Sequence ID");
			//AddPrimitiveComponent(source, "TN", "Telephone Number"); // HL7 version 2.3.1
			AddPrimitiveComponent(source, "GTS", "General Timing Specification");
			AddPrimitiveComponent(source, "IS", "??");
			AddPrimitiveComponent(source, "ID", "??");
			DataSourceFactory.SetDataTypeSource(source);

			// Act
			DataTypeGenerator.makeAll(baseFolder, version);

			// Assert
			Assert.IsTrue(File.Exists($"{targetFolder}FT.cs"));
			Assert.IsTrue(File.Exists($"{targetFolder}ST.cs"));
			Assert.IsTrue(File.Exists($"{targetFolder}TX.cs"));
			Assert.IsTrue(File.Exists($"{targetFolder}NM.cs"));
			Assert.IsTrue(File.Exists($"{targetFolder}SI.cs"));
			Assert.IsFalse(File.Exists($"{targetFolder}IS.cs"));
			Assert.IsFalse(File.Exists($"{targetFolder}ID.cs"));
		}

		[TestMethod]
		public void makeAll_CompositeTypeHDCreated_TypeDefinitionGiven()
		{
			// Arrange
			var baseFolder = GetBaseFolder();
			var version = GetVersion();
			var source = new DataTypeSourceMock();
			var targetFolder = GetTargetFolder("Datatype");

			var components = new TypeComponentsMock();

			components.Add("IS", "Namespace ID", 300);
			components.Add("ST", "Universal ID", 0);
			components.Add("ID", "Universal ID Type", 301);

			components.Description = "Hierarchic Designator";
			source.Types["HD"] = components;

			DataSourceFactory.SetDataTypeSource(source);

			// Act
			DataTypeGenerator.makeAll(baseFolder, version);

			// Assert
			Assert.IsTrue(File.Exists($"{targetFolder}HD.cs"));
		}

		#region Utilities
		private void AddPrimitiveComponent(DataTypeSourceMock source, string type, string description)
		{
			var components = new TypeComponentsMock();
			components.DataTypes.Add(type);
			components.Description = description;
			source.Types[type] = components;
		}


		#endregion
	}
}
