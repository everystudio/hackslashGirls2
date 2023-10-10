using System;
using UnityEngine;

namespace ES3Types
{
    [UnityEngine.Scripting.Preserve]
    [ES3PropertiesAttribute("testPrefab")]
    public class ES3UserType_ESTest : ES3ComponentType
    {
        public static ES3Type Instance = null;

        public ES3UserType_ESTest() : base(typeof(ESTest)) { Instance = this; priority = 1; }


        protected override void WriteComponent(object obj, ES3Writer writer)
        {
            var instance = (ESTest)obj;

            writer.WritePrivateFieldByRef("testPrefab", instance);
        }

        protected override void ReadComponent<T>(ES3Reader reader, object obj)
        {
            var instance = (ESTest)obj;
            Debug.Log("read");
            foreach (string propertyName in reader.Properties)
            {
                switch (propertyName)
                {

                    case "testPrefab":
                        instance = (ESTest)reader.SetPrivateField("testPrefab", reader.Read<UnityEngine.GameObject>(), instance);
                        Debug.Log("created");
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
        }
    }


    public class ES3UserType_ESTestArray : ES3ArrayType
    {
        public static ES3Type Instance;

        public ES3UserType_ESTestArray() : base(typeof(ESTest[]), ES3UserType_ESTest.Instance)
        {
            Instance = this;
        }
    }
}