using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHapi.Base.Log;
using NHapi.Base.SourceGeneration;

namespace NHapi.Base.DataProvider.Database
{
	 class GrammarProvider : DataProviderBase, IMessageProvider, ISegmentProvider, IDataTypeProvider, IEventMappingProvider
	 {
			private IMessageProvider _messageProvider = null;
			private ISegmentProvider _segmentProvider = null;
			private IDataTypeProvider _dataTypeProvider = null;
			private IEventMappingProvider _eventMappingProvider = null;
			public GrammarProvider(IHapiLog log) : base(log)
			{
				 _messageProvider = new MessageProvider(log);
				 _segmentProvider = new SegmentProvider(log);
				 _dataTypeProvider = new DataTypeProvider(log);
				 _eventMappingProvider = new EventMappingProvider(log);
			}

			public GrammarProvider() : this(null)
			{
			}

			public override IHapiLog Log
			{
				 set
				 {
						_messageProvider.Log = value;
						_segmentProvider.Log = value;
						_dataTypeProvider.Log = value;
						_eventMappingProvider.Log = value;
						base.Log = value;
				 }
			}

			public GrammarProvider(IMessageProvider messageProvider, ISegmentProvider segmentProvider, IDataTypeProvider dataTypeProvider, IEventMappingProvider eventMappingProvider)
			{
				 _messageProvider = messageProvider;
				 _segmentProvider = segmentProvider;
				 _dataTypeProvider = dataTypeProvider;
				 _eventMappingProvider = eventMappingProvider;
			}

			#region IMessageProvider 
			public void GetMessages(string version, out ArrayList messages, out ArrayList chapters)
			{
				 _messageProvider.GetMessages(version, out messages, out chapters);
			}

			public SegmentDef[] GetSegments(string message, string version)
			{
				 return _messageProvider.GetSegments(message, version);
			}
			#endregion

			#region  ISegmentProvider
			public void GetSegmentDefinition(string name, string version, out ArrayList elements, out string segDesc)
			{
				 _segmentProvider.GetSegmentDefinition(name, version, out elements, out segDesc);
			}

			public ArrayList GetSegmentNames(string version)
			{
				 return _segmentProvider.GetSegmentNames(version);
			}
			#endregion

			#region  IDataTypeProvider

			public void GetComponentDataType(string dataType, string version, out ArrayList dataTypes, out ArrayList descriptions, out ArrayList tables, out string description)
			{
				 _dataTypeProvider.GetComponentDataType(dataType, version, out dataTypes, out descriptions, out tables, out description);
			}

			public ArrayList GetTypeNames(string version)
			{
				 return _dataTypeProvider.GetTypeNames(version);
			}
			#endregion

			#region  IEventMappingProvider
			public void GetMessageMapping(string version, out ArrayList messageTypes, out ArrayList events, out ArrayList messageStructures)
			{
				 _eventMappingProvider.GetMessageMapping(version, out messageTypes, out events, out messageStructures);
			}
			#endregion
	 }
}
