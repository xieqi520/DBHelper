﻿using System;
using DBUtil;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DAL
{
    /// <summary>
    /// 用户
    /// </summary>
    public class SysUserDal
    {
        #region 根据ID查询单个记录
        /// <summary>
        /// 根据ID查询单个记录
        /// </summary>
        public SysUser Get(long id)
        {
            using (var session = DBHelper.GetSession())
            {
                return session.FindById<SysUser>(id);
            }
        }
        #endregion

        #region 查询总数
        /// <summary>
        /// 根据ID查询单个记录
        /// </summary>
        public int GetTotalCount()
        {
            using (var session = DBHelper.GetSession())
            {
                return session.GetSingle<int>("select count(*) from sys_user");
            }
        }
        #endregion

        #region 查询集合
        /// <summary>
        /// 查询集合
        /// </summary>
        public List<SysUser> GetList(string sql)
        {
            using (var session = DBHelper.GetSession())
            {
                return session.FindListBySql<SysUser>(sql);
            }
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        public long Insert(SysUser info)
        {
            using (var session = DBHelper.GetSession())
            {
                info.CreateTime = DateTime.Now;
                session.Insert(info);
                long id = session.GetSingle<long>("select @@IDENTITY");
                return id;
            }
        }
        #endregion

        #region 添加(异步)
        /// <summary>
        /// 添加
        /// </summary>
        public async Task InsertAsync(SysUser info)
        {
            using (var session = await DBHelper.GetSessionAsync())
            {
                info.CreateTime = DateTime.Now;
                await session.InsertAsync(info);
            }
        }
        #endregion

        #region 批量添加
        /// <summary>
        /// 批量添加
        /// </summary>
        public void Insert(List<SysUser> list)
        {
            list.ForEach(item => item.CreateTime = DateTime.Now);

            using (var session = DBHelper.GetSession())
            {
                try
                {
                    session.BeginTransaction();
                    session.Insert(list);
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.RollbackTransaction();
                    throw ex;
                }
            }
        }
        #endregion

        #region 批量添加(异步)
        /// <summary>
        /// 批量添加
        /// </summary>
        public async Task InsertAsync(List<SysUser> list)
        {
            list.ForEach(item => item.CreateTime = DateTime.Now);

            using (var session = await DBHelper.GetSessionAsync())
            {
                await session.InsertAsync(list);
            }
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        public void Update(SysUser info)
        {
            using (var session = DBHelper.GetSession())
            {
                info.UpdateTime = DateTime.Now;
                session.Update(info);
            }
        }
        #endregion

        #region 修改(异步)
        /// <summary>
        /// 修改
        /// </summary>
        public async Task UpdateAsync(SysUser info)
        {
            using (var session = await DBHelper.GetSessionAsync())
            {
                info.UpdateTime = DateTime.Now;
                var task = session.UpdateAsync(info);
                await task;
            }
        }
        #endregion

        #region 批量修改
        /// <summary>
        /// 批量修改
        /// </summary>
        public void Update(List<SysUser> list)
        {
            list.ForEach(item => item.UpdateTime = DateTime.Now);

            using (var session = DBHelper.GetSession())
            {
                try
                {
                    session.BeginTransaction();
                    session.Update(list);
                    session.CommitTransaction();
                }
                catch (Exception ex)
                {
                    session.RollbackTransaction();
                    throw ex;
                }
            }
        }
        #endregion

        #region 批量修改(异步)
        /// <summary>
        /// 批量修改
        /// </summary>
        public async Task UpdateAsync(List<SysUser> list)
        {
            list.ForEach(item => item.UpdateTime = DateTime.Now);

            using (var session = await DBHelper.GetSessionAsync())
            {
                var task = session.UpdateAsync(list);
                await task;
            }
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete(long id)
        {
            using (var session = DBHelper.GetSession())
            {
                session.DeleteById<SysUser>(id);
            }
        }
        #endregion

    }
}
