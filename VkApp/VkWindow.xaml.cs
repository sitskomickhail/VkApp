using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VkApp.FileManager;
using VkApp.VKWorker;

namespace VkApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GroupLinks _links;

        public MainWindow()
        {
            InitializeComponent();

            _links = new GroupLinks();
            Users user = new Users();
            var res = user.GetUsersDictionary;
            var res2 = user.GetUsersDictionary;
            cbGames.ItemsSource = _links.GetUsefullGames();
            cbGames.Text = cbGames.Items[0].ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VK_Navigate vkBot = new VK_Navigate();
            vkBot.TestWork(tbMessage.Text);
        }
    }
}
