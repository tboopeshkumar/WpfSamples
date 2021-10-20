using System.Windows.Controls;
using WebBrowser.Viewmodel;

namespace WebBrowser.View
{
    /// <summary>
    /// Interaction logic for BrowserView.xaml
    /// </summary>
    public partial class BrowserView : UserControl
    {
        public BrowserView()
        {
            InitializeComponent();
            this.DataContext = new BrowserViewModel();
        }
    }
}
