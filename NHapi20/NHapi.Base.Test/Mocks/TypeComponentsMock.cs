using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHapi.Base.Test.Mocks
{
	class TypeComponentsMock
	{
		public ArrayList DataTypes { get; set; }
		public ArrayList Descriptions { get; set; }
		public ArrayList Tables { get; set; }
		public string Description { get; set; }

		public TypeComponentsMock()
		{
			DataTypes = new ArrayList();
			Descriptions = new ArrayList();
			Tables = new ArrayList();
		}

		public void Add(string type, string description, int? table)
		{
			DataTypes.Add(type);
			Descriptions.Add(description);
			Tables.Add(table);
		}
	}
}
