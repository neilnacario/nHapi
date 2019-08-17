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

			public static void SetDataTypeSource(IDataTypeSource dataSource)
			{
				 _dataTypeSource = dataSource;
			}

			public static IDataTypeSource GetDataTypeSource(IHapiLog log)
			{
				 if (null == _dataTypeSource)
				 {
						_dataTypeSource = new DBDataTypeSource(log);
				 }
				 return _dataTypeSource;
			}
	 }
}
