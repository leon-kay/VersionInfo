using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VersionInfoExampleDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IVersionService _versionService;
        public MainWindow(IVersionService versionService)
        {
            InitializeComponent();
            _versionService = versionService;
            InitializeUI();
        }
        private void InitializeUI()
        {
            Title = $"版本号示例软件 - {_versionService.Short}";

            VersionText.Text = $"当前版本：{_versionService.Current}";

            //if (_versionService.IsPreRelease)
            //{
            //    MessageBox.Show(
            //        $"当前运行 {_versionService.Type} 版本\n版本号：{_versionService.Current}\n功能描述：{_versionService.Description}",
            //        "版本提示",
            //        MessageBoxButton.OK,
            //        MessageBoxImage.Information);
            //}
        }
    }
}