using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DocAsCode.PublishDoc
{
    /// <summary>
    /// Interaction logic for GithubConfigurationForm.xaml
    /// </summary>
    public partial class GithubConfigurationForm : Window
    {
        public string remoteGitPath { get; set; }
        public string localGitPath { get; set; }
        public string publishUrl { get; set; }
        public string userName { get; set; }
        public string passWord { get; set; }
        public string acessUrl { get; set; }
        public bool clearLocalGit { get; set; }
        public bool openSite { get; set; }

        public GithubConfigurationForm()
        {
            remoteGitPath = "https://github.com/openauthor/openauthor.github.io.git";
            localGitPath = "PublishFiles";
            publishUrl = "/";
            userName = "openauthor";
            passWord = "open123";
            acessUrl = "http://openauthor.github.io";
            clearLocalGit = true;
            openSite = true;
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //To remove the icon
        //[DllImport("user32.dll")]
        //static extern uint GetWindowLong(IntPtr hWnd, int nIndex);
        //[DllImport("user32.dll")]
        //static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);
        //private const int GWL_STYLE = -16;
        //private const uint WS_SYSMENU = 0x80000;
        //protected override void OnSourceInitialized(EventArgs e)
        //{
        //    IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        //    SetWindowLong(hwnd, GWL_STYLE,
        //        GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));
        //    base.OnSourceInitialized(e);
        //}
    }
}
