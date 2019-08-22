using NHapi.Base.SourceGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace NHapi.Base.DataSource.Xml
{
	 public class MessageSource : IMessageSource, ISegmentSource, IDataTypeSource, IEventMappingSource
	 {
			private bool _initialized;
			private HL7Versions _hl7Versions;

			public string FileName { get; set; }

			public MessageSource()
			{
				 _initialized = false;
				 _hl7Versions = null;
			}

			#region IMessageSource implementation
			public SegmentDef[] GetSegments(string message, string version)
			{
				 LoadXmlFile();
				 var hl7Grammar = _hl7Versions.HL7Version.Where(v => v.Version.Equals(version)).SingleOrDefault()
																		.Grammars.Where(g => g.Message.Equals(message)).SingleOrDefault();
				 ArrayList segments = new ArrayList();
				 if (null != hl7Grammar)
				 {
						foreach (var segment in hl7Grammar.Segments)
						{
							 segments.Add(new SegmentDef(segment.Name, segment.Group, segment.Required, segment.Repeating, segment.Description));
						}
				 }
				 SegmentDef[] ret = new SegmentDef[segments.Count];
				 Array.Copy(segments.ToArray(), ret, segments.Count);
				 return ret;
			}
			public void GetMessages(string version, out ArrayList messages, out ArrayList chapters)
			{
				 LoadXmlFile();
				 messages = new ArrayList();
				 chapters = new ArrayList();
				 var hl7Version = _hl7Versions.HL7Version.Where(v => v.Version.Equals(version)).SingleOrDefault();
				 if (null != hl7Version)
				 {
						foreach (var grammar in hl7Version.Grammars)
						{
							 messages.Add(grammar.Message);
							 chapters.Add(grammar.Chapter);
						}
				 }
			}
			#endregion

			#region ISegmentSource implementation
			public ArrayList GetSegments(string version)
			{
				 LoadXmlFile();
				 var hl7Grammar = _hl7Versions.HL7Version.Where(v => v.Version.Equals(version)).SingleOrDefault();
				 ArrayList segments = new ArrayList();
				 if (null != hl7Grammar)
				 {
						foreach (var segment in hl7Grammar.Segments)
						{
							 segments.Add(segment.Name);
						}
				 }
				 return segments;
			}

			public void GetSegmentDefinition(string name, string version, out ArrayList elements, out string segDesc)
			{
				 LoadXmlFile();
				 elements = new ArrayList();
				 segDesc = string.Empty;
				 var hl7Grammar = _hl7Versions.HL7Version.Where(v => v.Version.Equals(version)).SingleOrDefault();
				 ArrayList segments = new ArrayList();
				 if (null != hl7Grammar)
				 {
						var segment = hl7Grammar.Segments.Where(s => s.Name.Equals(name)).SingleOrDefault();
						segDesc = segment.Description;
						var sortedFields = segment.Fields.Field.OrderBy(f => f.Order);
						foreach (var field in sortedFields)
						{
							 var element = new SegmentElement()
							 {
									field = field.Order,
									type = field.Type,
									opt = (!field.Required).ToString(),
									repetitions = field.Repeations,
									length = (int)field.Length,
									table = string.IsNullOrEmpty(field.Table) ? 0 : Convert.ToInt32(field.Table),
									desc = field.Description
							 };
							 elements.Add(element);
						}
				 }
			}
			#endregion

			#region IDataTypeSource implementation
			public ArrayList GetTypes(string version)
			{
				 LoadXmlFile();
				 throw new NotImplementedException();
			}
			public void GetComponentDataType(string dataType, string version, out ArrayList dataTypes, out ArrayList descriptions, out ArrayList tables, out string description)
			{
				 LoadXmlFile();
				 throw new NotImplementedException();
			}
			#endregion

			#region IEventMappingSource implementation
			public void GetMessageMapping(string version, out ArrayList messageTypes, out ArrayList events, out ArrayList messageStructures)
			{
				 LoadXmlFile();
				 throw new NotImplementedException();
			}
			#endregion

			#region Utilities
			private void LoadXmlFile()
			{
				 if (!_initialized)
				 {
						if (!string.IsNullOrEmpty(FileName))
						{
							 XmlSerializer serializer = new XmlSerializer(typeof(HL7Versions));
							 using (TextReader reader = new StreamReader(FileName))
							 {
									_hl7Versions = (HL7Versions)serializer.Deserialize(reader);
							 }
							 _initialized = true;
						}
						else
						{
							 throw new Exception("Filename was not supplied.");
						}
				 }
			}
			#endregion
	 }
}
