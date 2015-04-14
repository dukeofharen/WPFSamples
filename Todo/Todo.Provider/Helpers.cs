using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Provider
{
	public class Helpers
	{
		public static string JsonSerialize<T>(T t)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			using (MemoryStream ms = new MemoryStream())
			{
				ser.WriteObject(ms, t);
				string jsonString = Encoding.UTF8.GetString(ms.ToArray());
				return jsonString;
			}
		}

		public static T JsonDeserialize<T>(string jsonString)
		{
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
			{
				T obj = (T)ser.ReadObject(ms);
				return obj;
			}
		}
	}
}
