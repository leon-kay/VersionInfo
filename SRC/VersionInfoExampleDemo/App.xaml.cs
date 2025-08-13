using System.Configuration;
using System.Data;
using System.Windows;

namespace VersionInfoExampleDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<IVersionService>(() =>
            //{
            //    //var versionService = new VersionService();
            //    //// 初始化版本信息
            //    //versionService.UpdateVersionInfo(
            //    //    BuildInfo.VERSION,
            //    //    Enum.Parse<VersionType>(BuildInfo.VERSION_TYPE),
            //    //    BuildInfo.DESCRIPTION
            //    //);
            //    //return versionService;
            //});
        }
    }

}
