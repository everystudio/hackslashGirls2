using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("score")]
	public class ES3UserType_TestingCircle : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_TestingCircle() : base(typeof(TestingCircle)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (TestingCircle)obj;
			
			writer.WriteProperty("score", instance.score, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (TestingCircle)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "score":
						instance.score = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_TestingCircleArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_TestingCircleArray() : base(typeof(TestingCircle[]), ES3UserType_TestingCircle.Instance)
		{
			Instance = this;
		}
	}
}