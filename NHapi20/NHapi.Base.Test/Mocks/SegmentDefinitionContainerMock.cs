using NHapi.Base.SourceGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHapi.Base.Test.Mocks
{
	 class SegmentDefinitionContainerMock
	 {
			public SegmentDefinitionContainerMock()
			{
				 Elements = new ArrayList();
			}

			public string Description { get; set; }
			public ArrayList Elements { get; set; }
	 }
}
