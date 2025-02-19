﻿using NHapi.Base.DataProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHapi.Base.Test.Mocks
{
	 class DataTypeProviderMock : DataProviderBase, IDataTypeProvider
	 {
			public Dictionary<string, TypeComponentsMock> Types { get; set; }

			public DataTypeProviderMock()
			{
				 Types = new Dictionary<string, TypeComponentsMock>();
			}

			public void GetComponentDataType(string dataType, string version, out ArrayList dataTypes, out ArrayList descriptions, out ArrayList tables, out string description)
			{
				 dataTypes = new ArrayList();
				 descriptions = new ArrayList();
				 tables = new ArrayList();
				 description = string.Empty;
				 TypeComponentsMock typeComponents = null;
				 if (Types.TryGetValue(dataType, out typeComponents))
				 {
						dataTypes.AddRange(typeComponents.DataTypes);
						descriptions.AddRange(typeComponents.Descriptions);
						tables.AddRange(typeComponents.Tables);
						description = typeComponents.Description;
				 }
			}

			public ArrayList GetTypeNames(string version)
			{
				 var typeList = new ArrayList();
				 foreach (var type in Types)
				 {
						typeList.Add(type.Key);
				 }
				 return typeList;
			}
	 }
}
