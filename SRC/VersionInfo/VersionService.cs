using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace VersionInfo
{
    /// <summary>
    /// 版本服务实现 - 支持自动设置程序集版本
    /// </summary>
    public class VersionService : IVersionService
    {
        #region 私有字段
        private string _version = "1.0.0";
        private VersionType _type = VersionType.Release;
        private string _description = "";
        private DateTime _lastUpdateTime = DateTime.Now;
        #endregion

        #region 构造函数
        public VersionService() { }

        public VersionService(string version, VersionType type, string description = "")
        {
            UpdateVersionInfo(version, type, description);
        }
        #endregion

        #region 更新版本信息
        public void UpdateVersionInfo(string version, VersionType type, string description = "")
        {
            _version = version ?? "1.0.0";
            _type = type;
            _description = description ?? "";
            _lastUpdateTime = DateTime.Now;

            // 🚀 自动应用到当前程序集
            ApplyToCurrentAssembly();
        }

        public void Reset()
        {
            _version = "1.0.0";
            _type = VersionType.Release;
            _description = "";
            _lastUpdateTime = DateTime.Now;
            ApplyToCurrentAssembly();
        }

        /// <summary>
        /// 🚀 自动设置当前程序集的版本信息（运行时生成版本文件）
        /// </summary>
        public void ApplyToCurrentAssembly()
        {
            try
            {
                // 获取调用程序集（主程序）
                var callingAssembly = Assembly.GetCallingAssembly();
                var assemblyLocation = callingAssembly.Location;
                var assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

                if (string.IsNullOrEmpty(assemblyDirectory))
                    return;

                // 🎯 方案1：生成 GlobalAssemblyInfo.cs 文件
                GenerateGlobalAssemblyInfo(assemblyDirectory);

                // 🎯 方案2：生成 Directory.Build.props 文件
                GenerateDirectoryBuildProps(assemblyDirectory);

                // 🎯 方案3：更新现有的 .csproj 文件
                UpdateProjectFile(assemblyDirectory);

            }
            catch (Exception ex)
            {
                // 静默处理错误，不影响主程序运行
                Debug.WriteLine($"ApplyToCurrentAssembly Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 生成 GlobalAssemblyInfo.cs 文件
        /// </summary>
        private void GenerateGlobalAssemblyInfo(string assemblyDirectory)
        {
            var globalAssemblyInfoPath = Path.Combine(assemblyDirectory, "GlobalAssemblyInfo.cs");

            var content = $@"// 🚀 此文件由 VersionInfo.dll 自动生成，请勿手动修改
// 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}
// 版本信息: {Current}

using System.Reflection;

[assembly: AssemblyVersion(""{_version}.0"")]
[assembly: AssemblyFileVersion(""{_version}.0"")]
[assembly: AssemblyInformationalVersion(""{_version}"")]
[assembly: AssemblyDescription(""{_description}"")]
[assembly: AssemblyCopyright(""Copyright © {DateTime.Now.Year}"")]

// 版本类型: {_type}
// 是否预发布: {(IsPreRelease ? "是" : "否")}
// 更新时间: {_lastUpdateTime:yyyy-MM-dd HH:mm:ss}
";

            File.WriteAllText(globalAssemblyInfoPath, content, Encoding.UTF8);
        }

        /// <summary>
        /// 生成 Directory.Build.props 文件
        /// </summary>
        private void GenerateDirectoryBuildProps(string assemblyDirectory)
        {
            var buildPropsPath = Path.Combine(assemblyDirectory, "Directory.Build.props");

            var content = $@"<!--  此文件由 VersionInfo.dll 自动生成，请勿手动修改 -->
<!-- 生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss} -->
<!-- 版本信息: {Current} -->
<Project>
  <PropertyGroup>
    <AppVersion>{_version}</AppVersion>
    <AppVersionType>{_type}</AppVersionType>
    <AppDescription>{_description}</AppDescription>
    <AssemblyVersion>{_version}.0</AssemblyVersion>
    <FileVersion>{_version}.0</FileVersion>
    <Version>{_version}</Version>
    <AssemblyDescription>{_description}</AssemblyDescription>
    <Copyright>Copyright © {DateTime.Now.Year}</Copyright>
  </PropertyGroup>
</Project>";

            File.WriteAllText(buildPropsPath, content, Encoding.UTF8);
        }

        /// <summary>
        /// 更新现有的 .csproj 文件
        /// </summary>
        private void UpdateProjectFile(string assemblyDirectory)
        {
            try
            {
                // 查找 .csproj 文件
                var csprojFiles = Directory.GetFiles(assemblyDirectory, "*.csproj");
                if (csprojFiles.Length == 0) return;

                var csprojPath = csprojFiles[0];
                var content = File.ReadAllText(csprojPath);

                // 简单的版本号替换（可以进一步完善）
                if (content.Contains("<AssemblyVersion>"))
                {
                    content = System.Text.RegularExpressions.Regex.Replace(
                        content,
                        @"<AssemblyVersion>.*?</AssemblyVersion>",
                        $"<AssemblyVersion>{_version}.0</AssemblyVersion>");
                }

                if (content.Contains("<FileVersion>"))
                {
                    content = System.Text.RegularExpressions.Regex.Replace(
                        content,
                        @"<FileVersion>.*?</FileVersion>",
                        $"<FileVersion>{_version}.0</FileVersion>");
                }

                if (content.Contains("<Version>") && !content.Contains("<VersionPrefix>"))
                {
                    content = System.Text.RegularExpressions.Regex.Replace(
                        content,
                        @"<Version>.*?</Version>",
                        $"<Version>{_version}</Version>");
                }

                File.WriteAllText(csprojPath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdateProjectFile Error: {ex.Message}");
            }
        }
        #endregion

        #region 获取程序集版本信息
        /// <summary>
        /// 🚀 获取当前程序集的文件版本信息
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
        #endregion

        #region 版本字符串生成
        public string Current => GenerateFullVersion();
        public string Short => GenerateShortVersion();
        public string WithDescription => string.IsNullOrEmpty(_description) ? Current : $"{Current} // {_description}";

        private string GenerateFullVersion()
        {
            var typeString = _type == VersionType.Release ? "" : $"-{_type}";
            var buildTime = _lastUpdateTime.ToString("yyyy-MM-dd-HHmm");
            return $"v{_version}{typeString} Build {buildTime}";
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
