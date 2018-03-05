using System.Windows;

namespace MRMS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Bootstrapper bootstrapper = new Bootstrapper();
            Bootstrapper.Instance.Run();
        }
    }
}
