using NHapi.Base.SourceGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	/// <summary>
	/// Data source interface for message structure.
	/// </summary>
	public interface IMessageSource : IDataSource
	{
		/// <summary>
		/// Returns a list of segments comprising the message structure.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="version"></param>
		/// <returns></returns>
		SegmentDef[] GetSegments(String message, String version);

		/// <summary>
		/// Returns list of messages and chapters
		/// </summary>
		/// <param name="version"></param>
		/// <param name="messages"></param>
		/// <param name="chapters"></param>
		void GetMessages(string version, out ArrayList messages, out ArrayList chapters);
	}
}
