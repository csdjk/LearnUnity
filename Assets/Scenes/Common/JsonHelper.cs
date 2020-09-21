using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Common {


    public class JsonHelper {

        public static string ObjectToJson (object obj) {
            // 获取所有属性(名称,值)
            Type type = obj.GetType ();

            PropertyInfo[] allProperty = type.GetProperties ();
            // 
            StringBuilder builder = new StringBuilder ();
            builder.Append ("{");
            foreach (var item in allProperty) {
                builder.Append ($"\"{item.Name}\":\"{item.GetValue(obj)}\"");
            }
            builder.Remove (builder.Length - 1, 1);
            builder.Append ("}");
            return builder.ToString ();
        }

        public static T JsonToObject<T> (string jsonStr) where T : new () {
            T instance = new T ();

            Type type = instance.GetType ();
            // "{"id":"123",name:"zzz"}" -> id:123,name:zzz
            jsonStr = jsonStr.Replace ("\"", string.Empty).Replace ("{", string.Empty).Replace ("}", string.Empty);
            string[] keyValue = jsonStr.Split (':', ',');
            for (int i = 0; i < keyValue.Length; i += 2) {
                // keyValue[i] 属性名
                // keyValue[i+1] 属性值
                // 获取属性
                PropertyInfo property = type.GetProperty(keyValue[i]);
                // 类型转换
                object value = Convert.ChangeType(instance,property.PropertyType);
                property.SetValue(instance,value);
            }

            return instance;
        }
    }
}