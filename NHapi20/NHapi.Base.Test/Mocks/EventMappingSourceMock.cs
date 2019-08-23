using NHapi.Base.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace NHapi.Base.Test.Mocks
{
	class EventMappingSourceMock : IEventMappingSource
	{
		public ArrayList EventMap { get; set; }

		public EventMappingSourceMock()
		{
			EventMap = new ArrayList();
		}

		public void GetMessageMapping(string version, out ArrayList messageTypes, out ArrayList events, out ArrayList messageStructures)
		{
			messageTypes = new ArrayList();
			events = new ArrayList();
			messageStructures = new ArrayList();

			foreach (EventMappingContainerMock item in EventMap)
			{
				messageTypes.Add(item.Type);
				events.Add(item.Event);
				messageStructures.Add(item.Structure);
			}
		}
	}
}
