using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHapi.Base.Log;
using System.Data.OleDb;

namespace NHapi.Base.DataProvider.Database
{
	class EventMappingProvider : DataProviderBase, IEventMappingProvider
	{
		public EventMappingProvider(IHapiLog log) : base(log)
		{
		}

		public void GetMessageMapping(string version, out ArrayList messageTypes, out ArrayList events, out ArrayList messageStructures)
		{
			messageTypes = new ArrayList();
			events = new ArrayList();
			messageStructures = new ArrayList();
			//get list of data types
			OleDbConnection conn = NormativeDatabase.Instance.Connection;
			String sql =
				"SELECT * from HL7EventMessageTypes inner join HL7Versions on HL7EventMessageTypes.version_id = HL7Versions.version_id where HL7Versions.hl7_version = '" +
				version + "'";
			OleDbCommand temp_OleDbCommand = new OleDbCommand();
			temp_OleDbCommand.Connection = conn;
			temp_OleDbCommand.CommandText = sql;
			OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();
			while (rs.Read())
			{
				messageTypes.Add((string)rs["message_typ_snd"]);
				events.Add((string)rs["event_code"]);
				messageStructures.Add((string)rs["message_structure_snd"]);
			}
		}
	}
}
