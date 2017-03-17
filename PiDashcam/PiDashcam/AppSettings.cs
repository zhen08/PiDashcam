using System.IO;
using Newtonsoft.Json;

namespace PiDashcam
{
	public class AppSettings<T> where T : new()
	{
		private const string DEFAULT_FILENAME = "settings.json";

		public void Save(string fileName = DEFAULT_FILENAME)
		{
			string json = JsonConvert.SerializeObject(this, Formatting.Indented);
			File.WriteAllText(fileName, json);
		}

		public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
		{
			string json = JsonConvert.SerializeObject(pSettings, Formatting.Indented);
			File.WriteAllText(fileName, json);
		}

		public static T Load(string fileName = DEFAULT_FILENAME)
		{
			T t = default(T);
			if (File.Exists(fileName))
				using (StreamReader file = File.OpenText(fileName))
				{
					JsonSerializer serializer = new JsonSerializer();
					t = (T)serializer.Deserialize(file, typeof(T));
				}
			return t;
		}
	}
}
