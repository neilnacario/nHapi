using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataSource
{
	 /// <summary>
	 /// A data type source base class.
	 /// </summary>
	 public abstract class DataTypeSourceBase : IDataSource
	 {
			protected readonly IHapiLog log;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="log"></param>
			public DataTypeSourceBase(IHapiLog log)
			{
				 this.log = log;
			}
	 }
}
