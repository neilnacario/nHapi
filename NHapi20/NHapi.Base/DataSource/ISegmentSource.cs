﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataSource
{
	/// <summary>
	/// Segment source interface.
	/// </summary>
	public interface ISegmentSource : IDataSource
	{
		/// <summary>
		/// Returns list of segments.
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		ArrayList GetSegments(string version);

		/// <summary>
		/// Returns the segment definition.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="version"></param>
		/// <param name="elements"></param>
		/// <param name="segDesc"></param>
		void GetSegmentDefinition(string name, string version, out ArrayList elements, out string segDesc);
	}
}
