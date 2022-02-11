using System;
using System.Reflection;

namespace ApiCore.Utils
{

    /// <summary>
    /// 实体类映射帮助类
    /// </summary>
    public class MapperHelper {

        /// <summary>
        /// 复制数据到指定的类对象
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">原实例</param>
        /// <returns>新实例</returns>
        static public T MapperTo<T>(object obj) where T : new() {
            if (obj == null)
                return default(T);

            try {
                T instance = Activator.CreateInstance<T>();
                var ty = instance.GetType();

                var nowTy = obj.GetType();
                var properties = nowTy.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                for (int i = 0; i < properties.Length; i++) {
                    var name = properties[i].Name;
                    var item = ty.GetProperty(name, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                    if (item != null && item.CanWrite && (IsBulitinType(item.PropertyType) || item.PropertyType.IsEnum)) {
                        var value = properties[i].GetValue(obj);
                        if (value == null) continue;

                        if (item.PropertyType.IsEnum) {
                            item.SetValue(instance, Enum.Parse(item.PropertyType, value.ToString()), null);
                        } else
                            item.SetValue(instance, Convert.ChangeType(value, item.PropertyType), null);
                    }
                }
                return instance;
            } catch (Exception ex) {
                throw ex;
            }
        }

        static private bool IsBulitinType(Type type) {
            return (type == typeof(object) || Type.GetTypeCode(type) != TypeCode.Object);
        }
    }
}
