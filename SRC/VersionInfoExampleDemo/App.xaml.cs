using System.Configuration;
using System.Data;
using System.Windows;
using VersionInfo;

namespace VersionInfoExampleDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string VERSION = "9.1.2";
        private const VersionType VERSION_TYPE = VersionType.Dev;
        private const string DESCRIPTION = "新增设备自动重连功能";
        private const string BUILD_TIME = "20250813-1545";

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IVersionService>(() =>
            {
                var versionService = new VersionService();
                versionService.UpdateVersionInfo(VERSION, VERSION_TYPE, DESCRIPTION, BUILD_TIME);
                return versionService;
            });
        }
    }

}
