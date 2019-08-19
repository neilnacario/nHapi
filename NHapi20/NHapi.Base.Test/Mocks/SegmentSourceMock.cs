using NHapi.Base.DataSource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHapi.Base.Test.Mocks
{
	class SegmentSourceMock : ISegmentSource
	{
		public Dictionary<string, SegmentDefinitionContainerMock> Segments { get; set; }

		public SegmentSourceMock()
		{
			Segments = new Dictionary<string, SegmentDefinitionContainerMock>();
		}

		public void GetSegmentDefinition(string name, string version, out ArrayList elements, out string segDesc)
		{
			elements = new ArrayList();
			segDesc = string.Empty;
			SegmentDefinitionContainerMock segmentDefinition = null;
			if (Segments.TryGetValue(name, out segmentDefinition))
			{
				elements.AddRange(segmentDefinition.Elements);
				segDesc = segmentDefinition.Description;
			}
		}

		public ArrayList GetSegments(string version)
		{
			var typeList = new ArrayList();
			foreach (var type in Segments)
			{
				typeList.Add(type.Key);
			}
			return typeList;
		}
	}
}
