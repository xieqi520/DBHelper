﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DBUtil
{
    /// <summary>
    /// 参数化查询SQL字符串
    /// </summary>
    public class SqlString
    {
        #region 变量属性

        protected IProvider _provider;

        protected StringBuilder _sql = new StringBuilder();

        protected List<DbParameter> _paramList = new List<DbParameter>();

        protected Regex _regex = new Regex(@"[@|:]([a-zA-Z_]{1}[a-zA-Z0-9_]+)", RegexOptions.IgnoreCase);

        /// <summary>
        /// 参数化查询的参数
        /// </summary>
        public DbParameter[] Params { get { return _paramList.ToArray(); } }

        /// <summary>
        /// 参数化查询的SQL
        /// </summary>
        public string SQL { get { return _sql.ToString(); } }

        /// <summary>
        /// SQL参数的参数名称(防止参数名称重名)
        /// </summary>
        protected HashSet<string> _dbParameterNames = new HashSet<string>();

        public string _orderBySQL = string.Empty;

        protected ISession _session { get; set; }
        #endregion

        #region 构造函数
        public SqlString(IProvider provider, ISession session, string sql = null, params object[] args)
        {
            _provider = provider;
            _session = session;

            if (sql != null)
            {
                Append(sql, args);
            }
        }
        #endregion

        #region Append
        /// <summary>
        /// 追加参数化SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="args">参数</param>
        public SqlString Append(string sql, params object[] args)
        {
            if (args == null) throw new Exception("参数args不能为null");

            Dictionary<string, object> dict = new Dictionary<string, object>();
            MatchCollection mc = _regex.Matches(sql);
            foreach (Match m in mc)
            {
                string val1 = m.Groups[1].Value;
                if (!dict.ContainsKey(val1))
                {
                    dict.Add(val1, null);
                    sql = ReplaceSql(sql, m.Value, val1);
                }
            }

            if (args.Length < dict.Keys.Count) throw new Exception("SqlString.AppendFormat参数不够");

            List<string> keyList = dict.Keys.ToList();
            for (int i = 0; i < keyList.Count; i++)
            {
                string key = keyList[i];
                object value = args[i];
                Type valueType = value != null ? value.GetType() : null;

                if (valueType == typeof(SqlValue))
                {
                    SqlValue sqlValue = value as SqlValue;
                    if (sqlValue.Value.GetType().Name != typeof(List<>).Name)
                    {
                        string markKey = _provider.GetParameterMark() + key;
                        sql = sql.Replace(markKey, string.Format(sqlValue.Sql, markKey));
                        _paramList.Add(_provider.GetDbParameter(key, sqlValue.Value));
                    }
                    else
                    {
                        string markKey = _provider.GetParameterMark() + key;
                        sql = sql.Replace(markKey, string.Format(sqlValue.Sql, markKey));
                        string[] keyArr = sqlValue.Sql.Replace("(", string.Empty).Replace(")", string.Empty).Replace("@", string.Empty).Split(',');
                        IList valueList = (IList)sqlValue.Value;
                        for (int k = 0; k < valueList.Count; k++)
                        {
                            object item = valueList[k];
                            _paramList.Add(_provider.GetDbParameter(keyArr[k], item));
                        }
                    }
                }
                else
                {
                    _paramList.Add(_provider.GetDbParameter(key, value));
                }
            }

            _sql.Append(string.Format(" {0} ", sql.Trim()));

            return this;
        }
        #endregion

        #region AppendIf
        /// <summary>
        /// 追加参数化SQL
        /// </summary>
        /// <param name="condition">当condition等于true时追加SQL，等于false时不追加SQL</param>
        /// <param name="sql">SQL</param>
        /// <param name="args">参数</param>
        public SqlString AppendIf(bool condition, string sql, params object[] args)
        {
            if (condition)
            {
                Append(sql, args);
            }

            return this;
        }
        #endregion

        #region AppendFormat
        /// <summary>
        /// 封装 StringBuilder AppendFormat 追加非参数化SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="args">参数</param>
        public void AppendFormat(string sql, params object[] args)
        {
            if (_regex.IsMatch(sql)) throw new Exception("追加参数化SQL请使用Append");
            _sql.AppendFormat(string.Format(" {0} ", sql.Trim()), args);
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return _sql.ToString();
        }
        #endregion

        #region ReplaceSql
        private string ReplaceSql(string sql, string oldStr, string name)
        {
            string newStr = _provider.GetParameterMark() + name;
            return sql.Replace(oldStr, newStr);
        }
        #endregion

        #region ForContains
        /// <summary>
        /// 创建 Like SQL
        /// </summary>
        public SqlValue ForContains(string value)
        {
            return _provider.ForContains(value);
        }
        #endregion

        #region ForStartsWith
        /// <summary>
        /// 创建 Like SQL
        /// </summary>
        public SqlValue ForStartsWith(string value)
        {
            return _provider.ForStartsWith(value);
        }
        #endregion

        #region ForEndsWith
        /// <summary>
        /// 创建 Like SQL
        /// </summary>
        public SqlValue ForEndsWith(string value)
        {
            return _provider.ForEndsWith(value);
        }
        #endregion

        #region ForDateTime
        /// <summary>
        /// 创建 日期时间类型转换 SQL
        /// </summary>
        /// <param name="dateTime">日期时间</param>
        public SqlValue ForDateTime(DateTime dateTime)
        {
            return _provider.ForDateTime(dateTime);
        }
        #endregion

        #region ForList
        /// <summary>
        /// 创建 in 或 not in SQL
        /// </summary>
        public SqlValue ForList(IList list)
        {
            return _provider.ForList(list);
        }
        #endregion

    }
}
