using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	 /// <summary>
	 /// A data type source base class.
	 /// </summary>
	 public abstract class DataProviderBase : IDataProvider
	 {
			protected IHapiLog _log;

			/// <summary>
			/// Logger setter
			/// </summary>
			public virtual IHapiLog Log
			{
				 set
				 {
						_log = value;
				 }
			}

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="log"></param>
			public DataProviderBase(IHapiLog log)
			{
				 this._log = log;
			}

			public DataProviderBase()
			{
				 this._log = null;
			}
	 }
}
