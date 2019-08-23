using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	/// <summary>
	/// A data type source.
	/// </summary>
	public interface IDataTypeSource : IDataSource
	{
		/// <summary>
		/// Returns list of HL7 data types.
		/// </summary>
		/// <param name="version">HL7 version.</param>
		/// <returns>HL7 data types.</returns>
		ArrayList GetTypes(string version);

		/// <summary>
		/// Returns components of a data type.
		/// </summary>
		/// <param name="dataType"></param>
		/// <param name="version"></param>
		/// <param name="dataTypes"></param>
		/// <param name="descriptions"></param>
		/// <param name="tables"></param>
		/// <param name="description"></param>
		void GetComponentDataType(string dataType, string version, out ArrayList dataTypes, out ArrayList descriptions, out ArrayList tables, out string description);
	}
}
