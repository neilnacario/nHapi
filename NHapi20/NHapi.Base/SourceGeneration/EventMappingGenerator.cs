using NHapi.Base.DataProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace NHapi.Base.SourceGeneration
{
	public class EventMappingGenerator
	{
		public static void makeAll(String baseDirectory, String version)
		{
			//make base directory
			if (!(baseDirectory.EndsWith("\\") || baseDirectory.EndsWith("/")))
			{
				baseDirectory = baseDirectory + "/";
			}
			FileInfo targetDir =
				SourceGenerator.makeDirectory(baseDirectory + PackageManager.GetVersionPackagePath(version) + "EventMapping");

			var log = DataProviderFactory.Instance.GetProvider<IEventMappingProvider>(null);
			ArrayList messageTypes;
			ArrayList events;
			ArrayList messageStructures;
			log.GetMessageMapping(version, out messageTypes, out events, out messageStructures);
			using (StreamWriter sw = new StreamWriter(targetDir.FullName + @"\EventMap.properties", false))
			{
				sw.WriteLine("#event -> structure map for " + version);
				for (int i = 0; i < messageTypes.Count; i++)
				{
					string messageType = string.Format("{0}_{1}", messageTypes[i], events[i]);
					string structure = (string)messageStructures[i];

					sw.WriteLine(string.Format("{0} {1}", messageType, structure));
				}
			}
		}
	}
}