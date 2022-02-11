using System;
using System.ComponentModel;

namespace ApiCore.Utils
{
    /// <summary>
    /// 获取枚举的描述扩展
    /// </summary>
    public static class EnumExtend {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue) {
            var value = enumValue.ToString();
            var field = enumValue.GetType().GetField(value);
            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }

    }
}
