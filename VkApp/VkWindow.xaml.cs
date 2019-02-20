using LogInfo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VkApp.FileManager;
using VkApp.Models;
using static LogInfo.LogIO;

namespace VkApp
{
    public partial class MainWindow : Window
    {
        private VkNavigate _vk;
        private BackgroundWorker _worker;
        private Logging _logger = new Logging(WriteLog);

        public MainWindow()
        {
            InitializeComponent();
            _vk = new VkNavigate();

            cbGames.ItemsSource = new List<string>() { "Metro 2033", "Vampire Legend" };
            cbGames.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            _logger += ShowLog;
            _worker = new BackgroundWorker();
            string[] values = { cbGames.Text, tbMessage.Text };
            DoWorkEventArgs args = new DoWorkEventArgs(cbGames.Text);
            if (rbtnAdd.IsChecked == true)
            {
                if (String.IsNullOrWhiteSpace(tbMessage.Text)) { MessageBox.Show("Введите ваше сообщение", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                tbMessage.Background = new SolidColorBrush(Colors.LightBlue);
                tbMessage.IsReadOnly = true;
                tbMessage.FontWeight = FontWeights.Bold;
                tbMessage.Text = String.Empty;
                _worker.DoWork += Worker_AddFriends;
                _worker.RunWorkerCompleted += Worker_AddingFriendsComplited;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(tbMessage.Text)) { MessageBox.Show("Введите ваше сообщение", "Error", MessageBoxButton.OK, MessageBoxImage.Error); return; }
                tbMessage.Background = new SolidColorBrush(Colors.LightBlue);
                tbMessage.IsReadOnly = true;
                tbMessage.FontWeight = FontWeights.Bold;
                tbMessage.Text = String.Empty;
                _worker.DoWork += Worker_StartSend;
                _worker.RunWorkerCompleted += Worker_SendingComplited;
            }
            btnStart.IsEnabled = false;
            _worker.RunWorkerAsync(values);
        }

        public void ShowLog(string tmp, Log log)
        {
            this.Dispatcher.Invoke(() => tbMessage.Text = tbMessage.Text + log + '\n');
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
            MessageBox.Show("Друзья успешно добавлены!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Worker_AddingFriendsComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
            tbMessage.Background = new SolidColorBrush(Colors.White);
            tbMessage.IsReadOnly = false;
            tbMessage.FontWeight = FontWeights.Normal;
        }


        private void Worker_StartSend(object sender, DoWorkEventArgs e)
        {
            string[] values = e.Argument as string[];
            if (_vk.CheckAndMail(values[0], values[1]))
                MessageBox.Show("Сообщения успешно отправлены!", "Complited", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Worker_SendingComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
            tbMessage.Background = new SolidColorBrush(Colors.White);
            tbMessage.IsReadOnly = false;
            tbMessage.FontWeight = FontWeights.Normal;
        }
        #endregion

        private void rbtnAdd_Checked(object sender, RoutedEventArgs e)
        {
            if (this.IsInitialized)
            {
                if (btnStart.IsEnabled)
                {
                    if (cbGames.Text == "Metro 2033")
                        tbMessage.Text = "Привет! Добавляю для игры Метро 2033";
                    else if (cbGames.Text == "Vampire Legend")
                        tbMessage.Text = "Привет! Добавляю для игры Легенда о вампире";
                    else
                        tbMessage.Text = $"Привет! Добавляю для игры {cbGames.Text}";
                }
            }
            else
                tbMessage.Text = "Привет! Добавляю для игры Метро 2033";
        }

        private void rbtnSend_Checked(object sender, RoutedEventArgs e)
        {
            if (btnStart.IsEnabled)
                tbMessage.Text = "Введите ваше сообщение...";
        }

        private void cbGames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
                if (btnStart.IsEnabled)
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