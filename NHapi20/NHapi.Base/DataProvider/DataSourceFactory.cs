using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	public static class DataSourceFactory
	{
		private static IDataTypeSource _dataTypeSource = null;
		private static ISegmentSource _segmentSource = null;
		private static IMessageSource _messageSource = null;
		private static IEventMappingSource _eventMappingSource = null;

		public static void SetDataTypeSource(IDataTypeSource source)
		{
			_dataTypeSource = source;
		}

		public static IDataTypeSource GetDataTypeSource(IHapiLog log)
		{
			if (null == _dataTypeSource)
			{
				_dataTypeSource = new Database.DataTypeSource(log);
			}
			return _dataTypeSource;
		}

		public static void SetSegmentSource(ISegmentSource source)
		{
			_segmentSource = source;
		}

		public static ISegmentSource GetSegmentSource(IHapiLog log)
		{
			if (null == _segmentSource)
			{
				_segmentSource = new Database.SegmentSource(log);
			}
			return _segmentSource;
		}

		public static void SetMessageSource(IMessageSource source)
		{
			_messageSource = source;
		}

		public static IMessageSource GetMessageSource(IHapiLog log)
		{
			if (null == _messageSource)
			{
				_messageSource = new Database.MessageSource(log);
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
				_eventMappingSource = new Database.EventMappingSource(log);
			}
			return _eventMappingSource;
		}
	}
}
