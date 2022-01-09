﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBUtil
{
    /// <summary>
    /// 类的属性信息扩展
    /// </summary>
    public class PropertyInfoEx
    {
        /// <summary>
        /// 类的属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 类的属性信息扩展
        /// </summary>
        /// <param name="propertyInfo">类的属性信息</param>
        public PropertyInfoEx(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }
}
