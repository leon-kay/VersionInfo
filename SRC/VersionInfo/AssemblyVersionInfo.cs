using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VersionInfo
{
    /// <summary>
    /// 程序集版本信息
    /// </summary>
    public class AssemblyVersionInfo
    {
        public string FileVersion { get; set; } = "";
        public string AssemblyVersion { get; set; } = "";
        public string ProductVersion { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsVersionSynced { get; set; }
        public string SyncStatus { get; set; } = "";
    }
}
