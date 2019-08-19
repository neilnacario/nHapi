using NHapi.Base.Log;
using NHapi.Base.SourceGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataSource
{
	/// <summary>
	/// Data type source from database.
	/// The methods in this class were lifted from <see cref="DataTypeGenerator"/>.
	/// </summary>
	class DBDataTypeSource : DataSourceBase, IDataTypeSource
	{
		public DBDataTypeSource(IHapiLog log) : base(log)
		{

		}

		/// <summary>
		/// Returns list of HL7 data types.
		/// </summary>
		/// <param name="version">HL7 version.</param>
		/// <returns>HL7 data types.</returns>
		public ArrayList GetTypes(string version)
		{
			//get list of data types
			ArrayList types = new ArrayList();
			OleDbConnection conn = NormativeDatabase.Instance.Connection;
			OleDbCommand stmt = SupportClass.TransactionManager.manager.CreateStatement(conn);
			//get normal data types ... 
			OleDbCommand temp_OleDbCommand;
			temp_OleDbCommand = stmt;
			temp_OleDbCommand.CommandText =
				"select data_type_code from HL7DataTypes, HL7Versions where HL7Versions.version_id = HL7DataTypes.version_id and HL7Versions.hl7_version = '" +
				version + "'";
			OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();
			while (rs.Read())
			{
				types.Add(Convert.ToString(rs[1 - 1]));
			}
			rs.Close();
			//get CF, CK, CM, CN, CQ sub-types ... 

			OleDbCommand temp_OleDbCommand2;
			temp_OleDbCommand2 = stmt;
			temp_OleDbCommand2.CommandText = "select data_structure from HL7DataStructures, HL7Versions where (" +
																			  "data_type_code  = 'CF' or " + "data_type_code  = 'CK' or " +
																			  "data_type_code  = 'CM' or " + "data_type_code  = 'CN' or " +
																			  "data_type_code  = 'CQ') and " +
																			  "HL7Versions.version_id = HL7DataStructures.version_id and  HL7Versions.hl7_version = '" +
																			  version + "'";
			rs = temp_OleDbCommand2.ExecuteReader();
			while (rs.Read())
			{
				types.Add(Convert.ToString(rs[1 - 1]));
			}

			stmt.Dispose();
			NormativeDatabase.Instance.returnConnection(conn);

			Console.Out.WriteLine("Generating " + types.Count + " datatypes for version " + version);
			if (types.Count == 0)
			{
				log.Warn("No version " + version + " data types found in database " + conn.Database);
			}

			return types;
		}

		/// <summary>
		/// Returns components of a data type.
		/// </summary>
		/// <param name="dataType"></param>
		/// <param name="version"></param>
		/// <param name="dataTypes"></param>
		/// <param name="descriptions"></param>
		/// <param name="tables"></param>
		/// <param name="description"></param>
		public void GetComponentDataType(string dataType, string version, out ArrayList dataTypes, out ArrayList descriptions, out ArrayList tables, out string description)
		{
			//get any components for this data type
			OleDbConnection conn = NormativeDatabase.Instance.Connection;
			OleDbCommand stmt = SupportClass.TransactionManager.manager.CreateStatement(conn);
			StringBuilder sql = new StringBuilder();
			//this query is adapted from the XML SIG informative document
			sql.Append(
				"SELECT HL7DataStructures.data_structure, HL7DataStructureComponents.seq_no, HL7DataStructures.description, HL7DataStructureComponents.table_id,  ");
			sql.Append(
				"HL7Components.description, HL7Components.table_id, HL7Components.data_type_code, HL7Components.data_structure ");
			sql.Append(
				"FROM HL7Versions LEFT JOIN (HL7DataStructures LEFT JOIN (HL7DataStructureComponents LEFT JOIN HL7Components ");
			sql.Append("ON HL7DataStructureComponents.comp_no = HL7Components.comp_no AND ");
			sql.Append("HL7DataStructureComponents.version_id = HL7Components.version_id) ");
			sql.Append("ON HL7DataStructures.version_id = HL7DataStructureComponents.version_id ");
			sql.Append("AND HL7DataStructures.data_structure = HL7DataStructureComponents.data_structure) ");
			sql.Append("ON HL7DataStructures.version_id = HL7Versions.version_id ");
			sql.Append("WHERE HL7DataStructures.data_structure = '");
			sql.Append(dataType);
			sql.Append("' AND HL7Versions.hl7_version = '");
			sql.Append(version);
			sql.Append("' ORDER BY HL7DataStructureComponents.seq_no");
			OleDbCommand temp_OleDbCommand;
			temp_OleDbCommand = stmt;
			temp_OleDbCommand.CommandText = sql.ToString();
			OleDbDataReader rs = temp_OleDbCommand.ExecuteReader();

			dataTypes = new ArrayList(20);
			descriptions = new ArrayList(20);
			tables = new ArrayList(20);
			description = null;
			while (rs.Read())
			{
				if (description == null)
					description = Convert.ToString(rs[3 - 1]);

				String de = Convert.ToString(rs[5 - 1]);
				String dt = Convert.ToString(rs[8 - 1]);
				int ta = -1;
				if (!rs.IsDBNull(4 - 1))
					ta = rs.GetInt32(4 - 1);
				//trim all CE_x to CE
				if (dt != null)
					if (dt.StartsWith("CE"))
						dt = "CE";
				//System.out.println("Component: " + de + "  Data Type: " + dt);  //for debugging
				dataTypes.Add(dt);
				descriptions.Add(de);
				tables.Add(ta);
			}
			if (dataType.ToUpper().Equals("TS"))
			{
				dataTypes[0] = "TSComponentOne";
			}

			rs.Close();
			stmt.Dispose();
			NormativeDatabase.Instance.returnConnection(conn);
		}

	}
}
