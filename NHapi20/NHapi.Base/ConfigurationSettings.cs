using System;
using System.Configuration;

namespace NHapi.Base
{
	public class ConfigurationSettings
	{
		public static bool UseFactory
		{
			get
			{
				bool useFactory = false;
				string useFactoryFromConfig = ConfigurationManager.AppSettings["UseFactory"];
				if (useFactoryFromConfig != null && useFactoryFromConfig.Length > 0)
				{
					useFactory = Convert.ToBoolean(useFactoryFromConfig);
				}
				return useFactory;
			}
		}

		private static string _connectionString = string.Empty;

		public static string ConnectionString
		{
			get
			{
				string connFromConfig = ConfigurationManager.AppSettings["ConnectionString"];
				if (string.IsNullOrEmpty(_connectionString) && !string.IsNullOrEmpty(connFromConfig))
				{
					_connectionString = connFromConfig;
				}
				return _connectionString;
			}
			set { _connectionString = value; }
		}
		private static string _xmlFilename = string.Empty;

		public static string XmlFilename
		{
			get
			{
				string xmlFilename = ConfigurationManager.AppSettings["XmlFilename"];
				if (string.IsNullOrEmpty(_connectionString) && !string.IsNullOrEmpty(xmlFilename))
				{
							 _xmlFilename = xmlFilename;
				}
				return _xmlFilename;
			}
			set { _xmlFilename = value; }
		}
	}
}