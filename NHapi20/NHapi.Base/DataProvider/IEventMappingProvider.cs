using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	/// <summary>
	/// Interface for event mapping source.
	/// </summary>
	public interface IEventMappingSource : IDataProvider
	{
		/// <summary>
		/// Returns mapping of type and event to message structure.
		/// </summary>
		/// <param name="version"></param>
		/// <param name="messageTypes"></param>
		/// <param name="events"></param>
		/// <param name="messageStructures"></param>
		void GetMessageMapping(string version, out ArrayList messageTypes, out ArrayList events, out ArrayList messageStructures);
	}
}
