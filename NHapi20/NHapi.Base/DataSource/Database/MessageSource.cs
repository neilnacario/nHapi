using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using NHapi.Base.Log;
using NHapi.Base.SourceGeneration;

namespace NHapi.Base.DataSource.Database
{
	class MessageSource : DataSourceBase, IMessageSource
	{
		public MessageSource(IHapiLog log) : base(log)
		{
		}

		public void GetMessages(string version, out ArrayList messages, out ArrayList chapters)
		{
			//get list of messages ...
			using (OleDbConnection conn = NormativeDatabase.Instance.Connection)
			{
				String sql = getMessageListQuery(version);
				OleDbCommand stmt = SupportClass.TransactionManager.manager.CreateStatement(conn);
				OleDbCommand temp_OleDbCommand;
				temp_OleDbCommand = stmt;
				temp_OleDbCommand.CommandText = sql;
				OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();
				messages = new ArrayList();
				chapters = new ArrayList();
				while (rs.Read())
				{
					messages.Add(Convert.ToString(rs[1 - 1]));
					chapters.Add(Convert.ToString(rs[2 - 1]));
				}
				rs.Close();
				NormativeDatabase.Instance.returnConnection(conn);

				if (messages.Count == 0)
				{
					log.Warn("No version " + version + " messages found in database " + conn.Database);
				}
			}
		}

		/// <summary> Returns an SQL query with which to get a list of messages from the normative
		/// database.  
		/// </summary>
		private static String getMessageListQuery(String version)
		{
			// UNION because the messages are defined in different tables for different versions.
			return "SELECT distinct  [message_type]+'_'+[event_code] AS msg_struct, '[AAA]'" +
				  " FROM HL7Versions RIGHT JOIN HL7EventMessageTypeSegments ON HL7EventMessageTypeSegments.version_id = HL7Versions.version_id " +
				  "WHERE HL7Versions.hl7_version ='" + version + "' and Not (message_type='ACK') " + "UNION " +
				  "select distinct HL7MsgStructIDs.message_structure, [section] from HL7Versions RIGHT JOIN (HL7MsgStructIDSegments " +
				  " inner join HL7MsgStructIDs on HL7MsgStructIDSegments.message_structure = HL7MsgStructIDs.message_structure " +
				  " and HL7MsgStructIDSegments.version_id = HL7MsgStructIDs.version_id) " +
				  " ON HL7MsgStructIDSegments.version_id = HL7Versions.version_id " + " where HL7Versions.hl7_version = '" +
				  version + "' and HL7MsgStructIDs.message_structure not like 'ACK_%'"; //note: allows "ACK" itself
		}

		/// <summary> Queries the normative database for a list of segments comprising
		/// the message structure.  The returned list may also contain strings
		/// that denote repetition and optionality.  Choice indicators (i.e. begin choice,
		/// next choice, end choice) for alternative segments are ignored, so that the class
		/// structure allows all choices.  The matter of enforcing that only a single choice is
		/// populated can't be handled by the class structure, and should be handled elsewhere.
		/// </summary>
		public SegmentDef[] GetSegments(string message, string version)
		{
			/*String sql = "select HL7Segments.seg_code, repetitional, optional, description " +
			"from (HL7MsgStructIDSegments inner join HL7Segments on HL7MsgStructIDSegments.seg_code = HL7Segments.seg_code " +
			"and HL7MsgStructIDSegments.version_id = HL7Segments.version_id) " +
			"where HL7Segments.version_id = 6 and message_structure = '" + message + "' order by seq_no";*/
			String sql = getSegmentListQuery(message, version);
			//System.out.println(sql.toString()); 	
			SegmentDef[] segments = new SegmentDef[200]; //presumably there won't be more than 200
			OleDbConnection conn = NormativeDatabase.Instance.Connection;
			OleDbCommand stmt = SupportClass.TransactionManager.manager.CreateStatement(conn);
			OleDbCommand temp_OleDbCommand;
			temp_OleDbCommand = stmt;
			temp_OleDbCommand.CommandText = sql;
			OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();
			int c = -1;
			while (rs.Read())
			{
				String name = SegmentGenerator.altSegName(Convert.ToString(rs[1 - 1]));
				bool repeating = rs.GetBoolean(2 - 1);
				bool optional = rs.GetBoolean(3 - 1);
				String desc = Convert.ToString(rs[4 - 1]);
				String groupName = Convert.ToString(rs[6 - 1]);

				//ignore the "choice" directives ... the message class structure has to include all choices ...
				//  if this is enforced (i.e. exception thrown if >1 choice populated) this will have to be done separately.
				if (!(name.Equals("<") || name.Equals("|") || name.Equals(">")))
				{
					c++;
					segments[c] = new SegmentDef(name, groupName, !optional, repeating, desc);
				}
			}
			rs.Close();
			SegmentDef[] ret = new SegmentDef[c + 1];
			Array.Copy(segments, 0, ret, 0, c + 1);
			return ret;
		}

		/// <summary> Returns an SQL query with which to get a list of the segments that
		/// are part of the given message from the normative database.  The query
		/// varies with different versions.  The fields returned are as follows:
		/// segment_code, repetitional, optional, description
		/// </summary>
		private static String getSegmentListQuery(String message, String version)
		{
			String sql = null;

			sql = "SELECT HL7Segments.seg_code, repetitional, optional, HL7Segments.description, seq_no, groupname " +
				  "FROM HL7Versions RIGHT JOIN (HL7Segments INNER JOIN HL7EventMessageTypeSegments ON (HL7Segments.version_id = HL7EventMessageTypeSegments.version_id) " +
				  "AND (HL7Segments.seg_code = HL7EventMessageTypeSegments.seg_code)) " +
				  "ON HL7Segments.version_id = HL7Versions.version_id " + "WHERE (((HL7Versions.hl7_version)= '" + version +
				  "') " + "AND (([message_type]+'_'+[event_code])='" + message + "')) UNION " +
				  "select HL7Segments.seg_code, repetitional, optional, HL7Segments.description, seq_no, groupname  " +
				  "from HL7Versions RIGHT JOIN (HL7MsgStructIDSegments inner join HL7Segments on HL7MsgStructIDSegments.seg_code = HL7Segments.seg_code " +
				  "and HL7MsgStructIDSegments.version_id = HL7Segments.version_id) " +
				  "ON HL7Segments.version_id = HL7Versions.version_id " + "where HL7Versions.hl7_version = '" + version +
				  "' and message_structure = '" + message + "' order by seq_no";
			return sql;
		}

	}
}
