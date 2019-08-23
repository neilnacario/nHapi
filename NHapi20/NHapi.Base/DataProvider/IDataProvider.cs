using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	 /// <summary>
	 /// A data source.
	 /// </summary>
	 public interface IDataProvider
	 {
			/// <summary>
			/// Set Logger instance
			/// </summary>
			IHapiLog Log { set; }
	 }
}
