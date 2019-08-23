using NHapi.Base.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHapi.Base.DataProvider
{
	 /// <summary>
	 /// Data provider factory
	 /// </summary>
	 public class DataProviderFactory
	 {
			private static IDataProvider _dataProvider = null;
			private static DataProviderFactory _instance = null;

			/// <summary>
			/// Get singleton instance.
			/// </summary>
			public static DataProviderFactory Instance
			{
				 get
				 {
						if (null == _instance)
						{
							 _instance = new DataProviderFactory();
						}
						return _instance;
				 }
			}

			/// <summary>
			/// Provider setter
			/// </summary>
			/// <param name="dataProvider"></param>
			public void SetProvider(IDataProvider dataProvider)
			{
				 _dataProvider = dataProvider;
			}

			/// <summary>
			/// Provider getter
			/// </summary>
			/// <typeparam name="TProvider"></typeparam>
			/// <param name="log"></param>
			/// <returns></returns>
			public TProvider GetProvider<TProvider>(IHapiLog log)
			{
				 Initialize();
				 _dataProvider.Log = log;
				 return (TProvider)_dataProvider;
			}

			private void Initialize()
			{
				 if (null == _dataProvider)
				 {
						if (!string.IsNullOrEmpty(ConfigurationSettings.XmlFilename))
						{
							 _dataProvider = new Xml.GrammarProvider() { FileName = ConfigurationSettings.XmlFilename };
						}
						else
						{
							 _dataProvider = new Database.GrammarProvider();
						}

				 }
			}
	 }
}
