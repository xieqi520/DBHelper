﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBUtil
{
    public partial class DBSession : ISession
    {
        #region Insert 添加
        /// <summary>
        /// 添加
        /// </summary>
        public void Insert(object obj)
        {
            StringBuilder strSql = new StringBuilder();
            int savedCount = 0;
            DbParameter[] parameters = null;

            PrepareInsertSql(obj, _autoIncrement, ref strSql, ref parameters, ref savedCount);

            ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion

        #region InsertAsync 添加
        /// <summary>
        /// 添加
        /// </summary>
        public Task InsertAsync(object obj)
        {
            StringBuilder strSql = new StringBuilder();
            int savedCount = 0;
            DbParameter[] parameters = null;

            PrepareInsertSql(obj, _autoIncrement, ref strSql, ref parameters, ref savedCount);

            return ExecuteSqlAsync(strSql.ToString(), parameters);
        }
        #endregion

        #region Insert<T> 批量添加
        /// <summary>
        /// 批量添加
        /// </summary>
        public void Insert<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i += 500)
            {
                StringBuilder strSql = new StringBuilder();
                int savedCount = 0;
                DbParameter[] parameters = null;

                PrepareInsertSql<T>(list.Skip(i).Take(500).ToList(), _autoIncrement, ref strSql, ref parameters, ref savedCount);

                ExecuteSql(strSql.ToString(), parameters);
            }
        }
        #endregion

        #region InsertAsync<T> 批量添加
        /// <summary>
        /// 批量添加
        /// </summary>
        public async Task InsertAsync<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i += 500)
            {
                StringBuilder strSql = new StringBuilder();
                int savedCount = 0;
                DbParameter[] parameters = null;

                PrepareInsertSql<T>(list.Skip(i).Take(500).ToList(), _autoIncrement, ref strSql, ref parameters, ref savedCount);

                await ExecuteSqlAsync(strSql.ToString(), parameters);
            }
        }
        #endregion

        #region PrepareInsertSql 准备Insert的SQL
        /// <summary>
        /// 准备Insert的SQL
        /// </summary>
        private void PrepareInsertSql(object obj, bool autoIncrement, ref StringBuilder strSql, ref DbParameter[] parameters, ref int savedCount)
        {
            Type type = obj.GetType();
            strSql.Append(string.Format("insert into {0}(", GetTableName(type)));
            PropertyInfoEx[] propertyInfoList = GetEntityProperties(type);
            List<string> propertyNameList = new List<string>();
            foreach (PropertyInfoEx propertyInfoEx in propertyInfoList)
            {
                PropertyInfo propertyInfo = propertyInfoEx.PropertyInfo;

                if (IsAutoIncrementPk(type, propertyInfoEx, autoIncrement)) continue;

                if (propertyInfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                {
                    propertyNameList.Add(propertyInfoEx.FieldName);
                    savedCount++;
                }
            }

            strSql.Append(string.Format("{0})", string.Join(",", propertyNameList.ToArray())));
            strSql.Append(string.Format(" values ({0})", string.Join(",", propertyNameList.ConvertAll<string>(a => _parameterMark + a).ToArray())));
            parameters = new DbParameter[savedCount];
            int k = 0;
            for (int i = 0; i < propertyInfoList.Length && savedCount > 0; i++)
            {
                PropertyInfoEx propertyInfoEx = propertyInfoList[i];
                PropertyInfo propertyInfo = propertyInfoEx.PropertyInfo;

                if (IsAutoIncrementPk(type, propertyInfoEx, autoIncrement)) continue;

                if (propertyInfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                {
                    object val = propertyInfo.GetValue(obj, null);
                    DbParameter param = _provider.GetDbParameter(_parameterMark + propertyInfoEx.FieldName, val == null ? DBNull.Value : val);
                    parameters[k++] = param;
                }
            }
        }
        #endregion

        #region PrepareInsertSql 准备批量Insert的SQL
        /// <summary>
        /// 准备批量Insert的SQL
        /// </summary>
        private void PrepareInsertSql<T>(List<T> list, bool autoIncrement, ref StringBuilder strSql, ref DbParameter[] parameters, ref int savedCount)
        {
            Type type = typeof(T);
            strSql.Append(string.Format("insert into {0}(", GetTableName(type)));
            PropertyInfoEx[] propertyInfoList = GetEntityProperties(type);
            List<string> propertyNameList = new List<string>();
            foreach (PropertyInfoEx propertyInfoEx in propertyInfoList)
            {
                PropertyInfo propertyInfo = propertyInfoEx.PropertyInfo;

                if (IsAutoIncrementPk(type, propertyInfoEx, autoIncrement)) continue;

                if (propertyInfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                {
                    propertyNameList.Add(propertyInfoEx.FieldName);
                    savedCount++;
                }
            }

            strSql.Append(string.Format("{0}) values ", string.Join(",", propertyNameList.ToArray())));
            for (int i = 0; i < list.Count; i++)
            {
                strSql.Append(string.Format(" ({0})", string.Join(",", propertyNameList.ConvertAll<string>(a => _parameterMark + a + i).ToArray())));
                if (i != list.Count - 1)
                {
                    strSql.Append(", ");
                }
            }

            parameters = new DbParameter[savedCount * list.Count];
            int k = 0;
            for (int n = 0; n < list.Count; n++)
            {
                T obj = list[n];
                for (int i = 0; i < propertyInfoList.Length && savedCount > 0; i++)
                {
                    PropertyInfoEx propertyInfoEx = propertyInfoList[i];
                    PropertyInfo propertyInfo = propertyInfoEx.PropertyInfo;

                    if (IsAutoIncrementPk(type, propertyInfoEx, autoIncrement)) continue;

                    if (propertyInfo.GetCustomAttributes(typeof(DBFieldAttribute), false).Length > 0)
                    {
                        object val = propertyInfo.GetValue(obj, null);
                        DbParameter param = _provider.GetDbParameter(_parameterMark + propertyInfoEx.FieldName + n, val == null ? DBNull.Value : val);
                        parameters[k++] = param;
                    }
                }
            }
        }
        #endregion

    }
}
