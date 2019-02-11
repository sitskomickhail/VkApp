using Microsoft.Win32;
using System;
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
        private BackgroundWorker _worker;

        public MainWindow()
        {
            InitializeComponent();
            _vk = new VkNavigate();
            _links = new GroupLinks();

            cbGames.ItemsSource = _links.GetUsefullGames();
            cbGames.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            _worker = new BackgroundWorker();
            string[] values = { cbGames.Text, tbMessage.Text };
            DoWorkEventArgs args = new DoWorkEventArgs(cbGames.Text);
            if (rbtnAdd.IsChecked == true)
            {
                if (String.IsNullOrWhiteSpace(tbMessage.Text)) { MessageBox.Show("Введите ваше сообщение", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                _worker.DoWork += Worker_AddFriends;
                _worker.RunWorkerCompleted += Worker_AddingFriendsComplited;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(tbMessage.Text)) { MessageBox.Show("Введите ваше сообщение", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                _worker.DoWork += Worker_StartSend;
                _worker.RunWorkerCompleted += Worker_SendingComplited;

            }
            btnStart.IsEnabled = false;
            _worker.RunWorkerAsync(values);
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
            if (btnStart.IsEnabled == false)
            {
                var result = MessageBox.Show("Вы действительно хотите выйти? Работа ботов будет прекращена",
                                "Attention",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question,
                                MessageBoxResult.No);

                if (result == MessageBoxResult.No)
                    e.Cancel = true;
                else
                {
                    _worker.WorkerSupportsCancellation = true;
                    _worker.CancelAsync();
                    _worker.Dispose();
                    _vk.CloseDrivers();
                }
            }
        }

        #region BACK_WORKER
        private void Worker_AddFriends(object sender, DoWorkEventArgs e)
        {
            string[] values = e.Argument as string[];
            _vk.StartAdding(values[0], values[1]);
        }

        private void Worker_AddingFriendsComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
            if (_worker.WorkerSupportsCancellation)
                MessageBox.Show("Друзья успешно добавлены!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void Worker_StartSend(object sender, DoWorkEventArgs e)
        {
            string[] values = e.Argument as string[];
            _vk.CheckAndMail(values[0], values[1]);
        }

        private void Worker_SendingComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
            MessageBox.Show("Сообщения успешно отправлены!", "Complited", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        private void rbtnAdd_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
                if (cbGames.Text == "Metro 2033")
                    tbMessage.Text = "Привет! Добавляю для игры Метро 2033";
                else if (cbGames.Text == "Vampire Legend")
                    tbMessage.Text = "Привет! Добавляю для игры Легенда о вампире";
                else
                    tbMessage.Text = $"Привет! Добавляю для игры {cbGames.Text}";
            else
                tbMessage.Text = "Привет! Добавляю для игры Метро 2033";
        }

        private void rbtnSend_Checked(object sender, RoutedEventArgs e)
        {
            tbMessage.Text = "Введите ваше сообщение...";
        }

        private void cbGames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
                if (rbtnAdd.IsChecked == true && !String.IsNullOrEmpty(cbGames.Text))
                {
                    if (cbGames.Text == "Metro 2033")
                        tbMessage.Text = "Привет! Добавляю для игры Легенда о вампире";
                    else if (cbGames.Text == "Vampire Legend")
                        tbMessage.Text = "Привет! Добавляю для игры Метро 2033";
                    else
                        tbMessage.Text = $"Привет! Добавляю для игры {cbGames.Text}";
                }
        }
    }
}