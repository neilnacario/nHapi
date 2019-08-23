using NHapi.Base.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHapi.Base.SourceGeneration;
using System.Collections;

namespace NHapi.Base.Test.Mocks
{
	class MessageProviderMock : IMessageProvider
	{
		public ArrayList Messages { get; set; }

		public MessageProviderMock()
		{
			Messages = new ArrayList();
		}

		public void GetMessages(string version, out ArrayList messages, out ArrayList chapters)
		{
			messages = new ArrayList();
			chapters = new ArrayList();
			foreach (MessageDefinitionContainerMock message in Messages)
			{
				messages.Add(message.Message);
				chapters.Add(message.Chapter);
			}
		}

		public SegmentDef[] GetSegments(string message, string version)
		{
			ArrayList returnValue = new ArrayList();
			foreach (MessageDefinitionContainerMock messageDefinition in Messages)
			{
				if (messageDefinition.Message.Equals(message))
				{
					returnValue = messageDefinition.Segments;
				}
			}
			SegmentDef[] ret = new SegmentDef[returnValue.Count];
			Array.Copy(returnValue.ToArray(), ret, returnValue.Count);
			return ret;
		}
	}
}
