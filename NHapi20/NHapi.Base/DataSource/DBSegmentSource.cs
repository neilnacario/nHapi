using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using NHapi.Base.Log;
using NHapi.Base.SourceGeneration;

namespace NHapi.Base.DataSource
{
	class DBSegmentSource : DataSourceBase, ISegmentSource
	{
		public DBSegmentSource(IHapiLog log) : base(log)
		{
		}

		public ArrayList GetSegments(string version)
		{
			//get list of data types
			OleDbConnection conn = NormativeDatabase.Instance.Connection;
			String sql =
				"SELECT seg_code, [section] from HL7Segments, HL7Versions where HL7Segments.version_id = HL7Versions.version_id AND hl7_version = '" +
				version + "'";
			OleDbCommand temp_OleDbCommand = new OleDbCommand();
			temp_OleDbCommand.Connection = conn;
			temp_OleDbCommand.CommandText = sql;
			OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();

			ArrayList segments = new ArrayList();
			while (rs.Read())
			{
				String segName = Convert.ToString(rs[1 - 1]);
				if (Char.IsLetter(segName[0]))
					//segments.Add(altSegName(segName));
					segments.Add(segName);
			}
			temp_OleDbCommand.Dispose();
			NormativeDatabase.Instance.returnConnection(conn);

			if (segments.Count == 0)
			{
				log.Warn("No version " + version + " segments found in database " + conn.Database);
			}

			return segments;
		}

		public void GetSegmentDefinition(string name, string version, out ArrayList elements, out string segDesc)
		{
			SegmentElement se;
			elements = new ArrayList();
			segDesc = null;
			OleDbConnection conn = NormativeDatabase.Instance.Connection;
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT HL7SegmentDataElements.seg_code, HL7SegmentDataElements.seq_no, ");
			sql.Append("HL7SegmentDataElements.repetitional, HL7SegmentDataElements.repetitions, ");
			sql.Append("HL7DataElements.description, HL7DataElements.length_old, HL7DataElements.table_id, ");
			sql.Append("HL7SegmentDataElements.req_opt, HL7Segments.description, HL7DataElements.data_structure ");
			sql.Append(
				"FROM HL7Versions RIGHT JOIN (HL7Segments INNER JOIN (HL7DataElements INNER JOIN HL7SegmentDataElements ");
			sql.Append("ON (HL7DataElements.version_id = HL7SegmentDataElements.version_id) ");
			sql.Append("AND (HL7DataElements.data_item = HL7SegmentDataElements.data_item)) ");
			sql.Append("ON (HL7Segments.version_id = HL7SegmentDataElements.version_id) ");
			sql.Append("AND (HL7Segments.seg_code = HL7SegmentDataElements.seg_code)) ");
			sql.Append("ON (HL7Versions.version_id = HL7Segments.version_id) ");
			sql.Append("WHERE HL7SegmentDataElements.seg_code = '");
			sql.Append(name);
			sql.Append("' and HL7Versions.hl7_version = '");
			sql.Append(version);
			sql.Append("' ORDER BY HL7SegmentDataElements.seg_code, HL7SegmentDataElements.seq_no;");
			OleDbCommand stmt = SupportClass.TransactionManager.manager.CreateStatement(conn);
			OleDbCommand temp_OleDbCommand;
			temp_OleDbCommand = stmt;
			temp_OleDbCommand.CommandText = sql.ToString();
			OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();

			while (rs.Read())
			{
				if (segDesc == null)
					segDesc = Convert.ToString(rs[9 - 1]);
				se = new SegmentElement();
				se.field = Convert.ToInt32(rs.GetValue(2 - 1));
				se.rep = Convert.ToString(rs[3 - 1]);
				if (rs.IsDBNull(4 - 1))
					se.repetitions = 0;
				else
					se.repetitions = Convert.ToInt32(rs.GetValue(4 - 1));

				if (se.repetitions == 0)
				{
					if (se.rep == null || !se.rep.ToUpper().Equals("Y".ToUpper()))
					{
						se.repetitions = 1;
					}
				}
				se.desc = Convert.ToString(rs[5 - 1]);
				if (!rs.IsDBNull(6 - 1))
				{
					se.length = DetermineLength(rs);
				}

				se.table = Convert.ToInt32(rs.GetValue(7 - 1));
				se.opt = Convert.ToString(rs[8 - 1]);
				se.type = Convert.ToString(rs[10 - 1]);
				//shorten CE_x to CE
				if (se.type.StartsWith("CE"))
					se.type = "CE";

				elements.Add(se);
				/*System.out.println("Segment: " + name + " Field: " + se.field + " Rep: " + se.rep +
						" Repetitions: " + se.repetitions + " Desc: " + se.desc + " Length: " + se.length +
						" Table: " + se.table + " Segment Desc: " + segDesc);*/
			}
			rs.Close();
			stmt.Dispose();
			NormativeDatabase.Instance.returnConnection(conn);
		}

		private int DetermineLength(OleDbDataReader rs)
		{
			string length = rs.GetValue(6 - 1) as string;
			if (!string.IsNullOrEmpty(length) && length.Contains(".."))
			{
				length = length.Split(new[] { ".." }, StringSplitOptions.None).Last();
			}
			if (length == "." || string.IsNullOrEmpty(length))
			{
				length = "0";
			}
			return Convert.ToInt32(length);
		}
	}
}
