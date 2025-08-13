
using System.ComponentModel;

namespace VersionInfo
{
    /// <summary>
    /// 版本类型枚举
    /// </summary>
    public enum VersionType
    {
        /// <summary>
        /// 正式发布版本 - 用于生产环境部署
        /// </summary>
        [Description("正式发布版本")]
        Release,

        /// <summary>
        /// 开发版本 - 功能开发和内部测试使用
        /// </summary>
        [Description("开发版本")]
        Dev,

        /// <summary>
        /// 热修复版本 - 紧急问题修复
        /// </summary>
        [Description("热修复版本")]
        Hotfix,

        /// <summary>
        /// 测试版本 - 发布前的候选版本
        /// </summary>
        [Description("测试版本")]
        Beta
    }


}
