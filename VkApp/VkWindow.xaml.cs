using Microsoft.Win32;
using System.Windows;
using VkApp.FileManager;
using VkApp.VKWorker;

namespace VkApp
{
    public partial class MainWindow : Window
    {
        private GroupLinks _links;
        private ThreadWorker _worker;

        public MainWindow()
        {
            InitializeComponent();

            _links = new GroupLinks();
            _worker = new ThreadWorker();
            cbGames.ItemsSource = _links.GetUsefullGames();
            if (cbGames.ItemsSource != null)
                cbGames.Text = cbGames.Items[0].ToString();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnAdd.IsChecked == true)
                _worker.StartDriver_Work(cbGames.Text);
            else if (pinPath.Text == "Файл не выбран")
                _worker.StartDriver_Send(tbMessage.Text, cbGames.Text);
            else
                _worker.StartDriver_Send(tbMessage.Text, cbGames.Text, pinPath.Text);
        }

        private void btnAddFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "text files|*.txt";
            if (file.ShowDialog().Value)
            {
                _links.CreateLinksFile(file.FileName);
                cbGames.ItemsSource = _links.GetUsefullGames();
            }
        }
    }
}