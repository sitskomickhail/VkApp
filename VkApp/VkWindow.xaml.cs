using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using VkApp.FileManager;
using VkApp.Models;

namespace VkApp
{
    public partial class MainWindow : Window
    {
        private GroupLinks _links;
        private VkNavigate _vk;

        public MainWindow()
        {
            InitializeComponent();
            _vk = new VkNavigate();
            _links = new GroupLinks();

            cbGames.ItemsSource = _links.GetUsefullGames();
            if (cbGames.ItemsSource != null)
                cbGames.Text = cbGames.Items[0].ToString();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker _worker = new BackgroundWorker();
            btnStart.IsEnabled = false;
            if (rbtnAdd.IsChecked == true)
            {
                DoWorkEventArgs args = new DoWorkEventArgs(cbGames.Text);
                _worker.DoWork += Worker_AddFriends;
                _worker.RunWorkerCompleted += Worker_AddingFriendsComplited;
                _worker.RunWorkerAsync(cbGames.Text);
            }
            else
            {
                string[] values = { cbGames.Text, tbMessage.Text };
                DoWorkEventArgs args = new DoWorkEventArgs(values);
                _worker.DoWork += Worker_StartSend;
                _worker.RunWorkerCompleted += Worker_SendingComplited;

                _worker.RunWorkerAsync(values);
            }
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _vk.CloseDrivers();
        }

        #region BACK_WORKER
        private void Worker_AddFriends(object sender, DoWorkEventArgs e)
        {
            string game = e.Argument.ToString();
            _vk.StartAdding(game);
        }

        private void Worker_AddingFriendsComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
            MessageBox.Show("Друзья успешно добавлены!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Worker_StartSend(object sender, DoWorkEventArgs e)
        {
            string[] values = e.Argument as string[];
            _vk.CheckAndMail(values[1], values[0]);
        }

        private void Worker_SendingComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
            MessageBox.Show("Сообщения успешно отправлены!", "Complited", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion
    }
}