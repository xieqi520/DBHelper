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
        public SysUser Get(string id)
        {
            using (var session = DBHelper.GetSession())
            {
                return session.FindById<SysUser>(id);
            }
        }
        #endregion

        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        public void Insert(SysUser info)
        {
            using (var session = DBHelper.GetSession())
            {
                info.CreateTime = DateTime.Now;
                session.Insert(info);
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
                session.Insert<SysUser>(list);
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
                await session.InsertAsync<SysUser>(list);
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
                session.Update(list);
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
        public void Delete(string id)
        {
            using (var session = DBHelper.GetSession())
            {
                session.DeleteById<SysUser>(id);
            }
        }
        #endregion

    }
}
