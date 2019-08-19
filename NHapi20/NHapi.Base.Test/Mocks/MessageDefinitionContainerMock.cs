using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHapi.Base.Test.Mocks
{
	class MessageDefinitionContainerMock
	{
		public string Message { get; set; }
		public string Chapter { get; set; }

		public ArrayList Segments { get; set; }

		public MessageDefinitionContainerMock()
		{
			Segments = new ArrayList();
		}
	}
}
