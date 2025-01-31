﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBUtil
{
    /// <summary>
    /// 数据库实现接口
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// 创建 DbConnection
        /// </summary>
        DbConnection CreateConnection(string connectionString);

        /// <summary>
        /// 生成 DbCommand
        /// </summary>
        DbCommand GetCommand();

        /// <summary>
        /// 生成 DbCommand
        /// </summary>
        DbCommand GetCommand(string sql, DbConnection conn);

        /// <summary>
        /// 生成 DbParameter
        /// </summary>
        DbParameter GetDbParameter(string name, object vallue);

        /// <summary>
        /// 生成 DbDataAdapter
        /// </summary>
        DbDataAdapter GetDataAdapter(DbCommand cmd);

        /// <summary>
        /// 带参数的SQL插入和修改语句中，参数前面的符号
        /// </summary>
        string GetParameterMark();

        /// <summary>
        /// 创建获取最大编号SQL
        /// </summary>
        string CreateGetMaxIdSql(string key, Type type);

        /// <summary>
        /// 创建分页SQL
        /// </summary>
        string CreatePageSql(string sql, string orderby, int pageSize, int currentPage, int totalRows);

        /// <summary>
        /// 创建 Like SQL
        /// </summary>
        SqlValue ForContains(string value);

        /// <summary>
        /// 创建 Like SQL
        /// </summary>
        SqlValue ForStartsWith(string value);

        /// <summary>
        /// 创建 Like SQL
        /// </summary>
        SqlValue ForEndsWith(string value);

        /// <summary>
        /// 创建 日期时间类型转换 SQL
        /// </summary>
        SqlValue ForDateTime(DateTime dateTime);

        /// <summary>
        /// 创建 in 或 not in SQL
        /// </summary>
        SqlValue ForList(IList list);

    }
}
