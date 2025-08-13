using System;

namespace VersionInfo
{
    /// <summary>
    /// 版本服务接口
    /// </summary>
    public interface IVersionService
    {
        #region 更新版本信息
        /// <summary>
        /// 更新版本信息
        /// </summary>
        /// <param name="version">版本号，如 "7.1.2"</param>
        void UpdateVersionInfo(string version, VersionType type, string description = "", string buildTime = "");

        /// <summary>
        /// 重置为默认版本
        /// </summary>
        void Reset();

        #endregion

        #region 基本属性
        /// <summary>
        /// 版本号
        /// </summary>
        string Number { get; }

        /// <summary>
        /// 版本类型
        /// </summary>
        VersionType Type { get; }

        /// <summary>
        /// 功能描述
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 是否为正式版本
        /// </summary>
        bool IsRelease { get; }

        /// <summary>
        /// 是否为预发布版本
        /// </summary>
        bool IsPreRelease { get; }
        #endregion

        #region 版本字符串
        /// <summary>
        /// 完整版本字符串
        /// </summary>
        string Current { get; }

        /// <summary>
        /// 简短版本字符串（不含构建时间）
        /// </summary>
        string Short { get; }

        /// <summary>
        /// 带描述的完整版本
        /// </summary>
        string WithDescription { get; }
        #endregion

        #region 版本解析
        /// <summary>
        /// 主版本号
        /// </summary>
        int Major { get; }

        /// <summary>
        /// 次版本号
        /// </summary>
        int Minor { get; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        int Patch { get; }

        /// <summary>
        /// System.Version 对象
        /// </summary>
        Version SystemVersion { get; }
        #endregion

        #region 实用方法
        /// <summary>
        /// 获取详细版本信息
        /// </summary>
        string GetDetailedInfo();

        /// <summary>
        /// 版本比较
        /// </summary>
        /// <param name="otherVersion">其他版本号</param>
        /// <returns>比较结果：-1小于，0等于，1大于</returns>
        int CompareTo(string otherVersion);

        /// <summary>
        /// 验证版本号格式是否正确
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>是否为有效的版本号格式</returns>
        bool IsValidVersionFormat(string version);

        /// <summary>
        ///  获取当前程序集的文件版本信息
        /// </summary>
        AssemblyVersionInfo GetCurrentAssemblyInfo();
        #endregion
    }
}
