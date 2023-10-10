using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("velocity")]
	public class ES3UserType_Rigidbody2D : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_Rigidbody2D() : base(typeof(UnityEngine.Rigidbody2D)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (UnityEngine.Rigidbody2D)obj;
			
			writer.WriteProperty("velocity", instance.velocity, ES3Type_Vector2.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (UnityEngine.Rigidbody2D)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "velocity":
						instance.velocity = reader.Read<UnityEngine.Vector2>(ES3Type_Vector2.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_Rigidbody2DArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_Rigidbody2DArray() : base(typeof(UnityEngine.Rigidbody2D[]), ES3UserType_Rigidbody2D.Instance)
		{
			Instance = this;
		}
	}
}