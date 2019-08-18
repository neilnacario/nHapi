using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataSource
{
	 public static class DataSourceFactory
	 {
			private static IDataTypeSource _dataTypeSource = null;
			private static ISegmentSource _segmentSource = null;

			public static void SetDataTypeSource(IDataTypeSource source)
			{
				 _dataTypeSource = source;
			}

			public static IDataTypeSource GetDataTypeSource(IHapiLog log)
			{
				 if (null == _dataTypeSource)
				 {
						_dataTypeSource = new DBDataTypeSource(log);
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
						_segmentSource = new DBSegmentSource(log);
				 }
				 return _segmentSource;
			}
	 }
}
