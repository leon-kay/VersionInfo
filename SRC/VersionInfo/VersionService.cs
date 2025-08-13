using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace VersionInfo
{
    /// <summary>
    /// 版本服务实现
    /// </summary>
    public class VersionService : IVersionService
    {
        #region 私有字段
        private string _version = "1.0.0";
        private VersionType _type = VersionType.Release;
        private string _description = "";
        private DateTime _lastUpdateTime = DateTime.Now;
        private string _buildTime = "";
        #endregion

        #region 构造函数
        public VersionService() { }

        public VersionService(string version, VersionType type, string description = "", string buildTime = "")
        {
            UpdateVersionInfo(version, type, description, buildTime);
        }
        #endregion

        #region 更新版本信息
        public void UpdateVersionInfo(string version, VersionType type, string description = "", string buildTime = "")
        {
            _version = version ?? "1.0.0";
            _type = type;
            _description = description ?? "";
            _buildTime = buildTime ?? DateTime.Now.ToString("yyyyMMdd-HH:mm"); 


        }

        public void Reset()
        {
            _version = "1.0.0";
            _type = VersionType.Release;
            _description = "";
            _lastUpdateTime = DateTime.Now;
        }

        #endregion

        #region 获取程序集版本信息
        /// <summary>
        ///  获取当前程序集的文件版本信息
        /// </summary>
        public AssemblyVersionInfo GetCurrentAssemblyInfo()
        {
            try
            {
                var assembly = Assembly.GetCallingAssembly();
                var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
                var assemblyVersion = assembly.GetName().Version;

                var info = new AssemblyVersionInfo
                {
                    FileVersion = fileVersionInfo.FileVersion ?? "未知",
                    AssemblyVersion = assemblyVersion?.ToString() ?? "未知",
                    ProductVersion = fileVersionInfo.ProductVersion ?? "未知",
                    Title = fileVersionInfo.ProductName ?? "未知",
                    Description = fileVersionInfo.Comments ?? "未知"
                };

                // 检查版本是否同步
                info.IsVersionSynced = fileVersionInfo.FileVersion?.StartsWith(_version) == true;
                info.SyncStatus = info.IsVersionSynced ? "✅ 已同步" : "⚠️ 未同步";

                return info;
            }
            catch (Exception ex)
            {
                return new AssemblyVersionInfo
                {
                    SyncStatus = $"❌ 获取失败: {ex.Message}"
                };
            }
        }
        #endregion

        #region 基本属性
        public string Number => _version;
        public VersionType Type => _type;
        public string Description => _description;
        public bool IsRelease => _type == VersionType.Release;
        public bool IsPreRelease => _type != VersionType.Release;
        //  获取编译时间的属性
        public string BuildTime => _buildTime;
        #endregion

        #region 版本字符串生成
        public string Current => GenerateFullVersion();
        public string Short => GenerateShortVersion();
        public string WithDescription => string.IsNullOrEmpty(_description) ? Current : $"{Current} // {_description}";

        private string GenerateFullVersion()
        {
            var typeString = _type == VersionType.Release ? "" : $"{_type}";
            return $"V{_version} [{typeString}] Build {_buildTime}";
        }

        private string GenerateShortVersion()
        {
            var typeString = _type == VersionType.Release ? "" : $"-{_type}";
            return $"v{_version}{typeString}";
        }
        #endregion

        #region 版本解析
        public int Major
        {
            get
            {
                var parts = _version.Split('.');
                return parts.Length > 0 && int.TryParse(parts[0], out int major) ? major : 0;
            }
        }

        public int Minor
        {
            get
            {
                var parts = _version.Split('.');
                return parts.Length > 1 && int.TryParse(parts[1], out int minor) ? minor : 0;
            }
        }

        public int Patch
        {
            get
            {
                var parts = _version.Split('.');
                return parts.Length > 2 && int.TryParse(parts[2], out int patch) ? patch : 0;
            }
        }

        public Version SystemVersion => new Version(Major, Minor, Patch);
        #endregion

        #region 实用方法
        public string GetDetailedInfo()
        {
            var assemblyInfo = GetCurrentAssemblyInfo();

            return $"""
                🔍 程序版本详细信息:
                  版本号: {_version} ({Major}.{Minor}.{Patch})
                  版本类型: {_type}
                  完整版本: {Current}
                  功能描述: {_description}
                  是否正式版: {(IsRelease ? "是" : "否")}
                  是否预发布: {(IsPreRelease ? "是" : "否")}
                  更新时间: {_lastUpdateTime:yyyy-MM-dd HH:mm:ss}
                  
                📁 程序集版本信息:
                  文件版本: {assemblyInfo.FileVersion}
                  程序集版本: {assemblyInfo.AssemblyVersion}
                  产品版本: {assemblyInfo.ProductVersion}
                  同步状态: {assemblyInfo.SyncStatus}
                """;
        }

        public int CompareTo(string otherVersion)
        {
            if (Version.TryParse(_version, out Version current) &&
                Version.TryParse(otherVersion, out Version other))
            {
                return current.CompareTo(other);
            }
            return string.Compare(_version, otherVersion, StringComparison.OrdinalIgnoreCase);
        }

        public bool IsValidVersionFormat(string version)
        {
            return Version.TryParse(version, out _);
        }
        #endregion
    }
}
