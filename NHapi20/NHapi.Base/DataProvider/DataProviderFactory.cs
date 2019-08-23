using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	public static class DataProviderFactory
	{
		private static IDataTypeProvider _dataTypeSource = null;
		private static ISegmentProvider _segmentSource = null;
		private static IMessageProvider _messageSource = null;
		private static IEventMappingSource _eventMappingSource = null;

		public static void SetDataTypeSource(IDataTypeProvider source)
		{
			_dataTypeSource = source;
		}

		public static IDataTypeProvider GetDataTypeSource(IHapiLog log)
		{
			if (null == _dataTypeSource)
			{
				_dataTypeSource = new Database.DataTypeProvider(log);
			}
			return _dataTypeSource;
		}

		public static void SetSegmentSource(ISegmentProvider source)
		{
			_segmentSource = source;
		}

		public static ISegmentProvider GetSegmentSource(IHapiLog log)
		{
			if (null == _segmentSource)
			{
				_segmentSource = new Database.SegmentProvider(log);
			}
			return _segmentSource;
		}

		public static void SetMessageSource(IMessageProvider source)
		{
			_messageSource = source;
		}

		public static IMessageProvider GetMessageSource(IHapiLog log)
		{
			if (null == _messageSource)
			{
				_messageSource = new Database.MessageProvider(log);
			}
			return _messageSource;
		}

		public static void SetEventMappingSource(IEventMappingSource source)
		{
			_eventMappingSource = source;
		}

		public static IEventMappingSource GetEventMappingSource(IHapiLog log)
		{
			if (null == _eventMappingSource)
			{
				_eventMappingSource = new Database.EventMappingProvider(log);
			}
			return _eventMappingSource;
		}
	}
}
