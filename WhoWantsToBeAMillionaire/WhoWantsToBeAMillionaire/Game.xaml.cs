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
using System.Windows.Shapes;
using System.IO;
using System.Threading;

namespace WhoWantsToBeAMillionaire
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        static Random rnd = new Random();
        static int taskno;
        static int selectedIndex;
        static Task currentTask;
        static List<Task> tasks = new List<Task>();
        static List<int> prizes = new List<int>();
        static List<Button> helps = new List<Button>();
        static bool gameOver = false;
        string name;
        static string prizeWon = "0 Ft";

        static List<Button> buttons = new List<Button>();

        public Game(string name)
        {
            InitializeComponent();
            lbPrize.Items.Add("1 000 000 Ft");
            lbPrize.Items.Add("750 000 Ft");
            lbPrize.Items.Add("500 000 Ft");
            lbPrize.Items.Add("200 000 Ft");
            lbPrize.Items.Add("100 000 Ft");
            lbPrize.Items.Add("50 000 Ft");
            lbPrize.Items.Add("20 000 Ft");
            lbPrize.Items.Add("10 000 Ft");
            lbPrize.Items.Add("5 000 Ft");
            lbPrize.Items.Add("0 Ft");

            selectedIndex = 9;
            name = name;

            buttons.Add(btnAns0);
            buttons.Add(btnAns1);
            buttons.Add(btnAns2);
            buttons.Add(btnAns3);

            helps.Add(btnHalf);
            helps.Add(btnVote);
            helps.Add(btnCall);

            lbPrize.SelectedIndex = selectedIndex;
            FillPrizes();

            Read();
            GameHandler();
        }

        public void FillPrizes()
        {
            prizes.Add(1000000);
            prizes.Add(750000);
            prizes.Add(500000);
            prizes.Add(200000);
            prizes.Add(100000);
            prizes.Add(50000);
            prizes.Add(20000);
            prizes.Add(10000);
            prizes.Add(5000);
        }

        public void GameHandler()
        {
            if (!gameOver && selectedIndex > 0)
            {
                InitializeUI();
                prizeWon = lbPrize.SelectedItem.ToString();
                currentTask = ChooseTask();
                SetGamePanel(currentTask);
                tasks.Remove(currentTask);
                if (!btnVote.IsEnabled)
                {
                    btnVote.IsEnabled = true;
                }
                if (!btnCall.IsEnabled)
                {
                    btnCall.IsEnabled = true;
                }
                if (!btnHalf.IsEnabled)
                {
                    btnHalf.IsEnabled = true;
                }
            }

            if (selectedIndex < 0)
            {
                GameWon();
            }
        }

        public void GameWon()
        {
            MessageBox.Show(String.Format("Gratulálok {0}! Mától te is milliomos vagy!", name));
            Close();
        }

        public void InitializeUI()
        {
            btnAns0.Background = Brushes.Transparent;
            btnAns1.Background = Brushes.Transparent;
            btnAns2.Background = Brushes.Transparent;
            btnAns3.Background = Brushes.Transparent;
            btnAns0.Foreground = Brushes.GhostWhite;
            btnAns1.Foreground = Brushes.GhostWhite;
            btnAns2.Foreground = Brushes.GhostWhite;
            btnAns3.Foreground = Brushes.GhostWhite;

            btnAns0.Visibility = Visibility.Visible;
            btnAns1.Visibility = Visibility.Visible;
            btnAns2.Visibility = Visibility.Visible;
            btnAns3.Visibility = Visibility.Visible;

            lblAns0.Content = "";
            lblAns1.Content = "";
            lblAns2.Content = "";
            lblAns3.Content = "";
        }
        public Task ChooseTask()
        {
            taskno = rnd.Next(0, tasks.Count);
            return tasks[taskno];
        }
        public void SetGamePanel(Task currentTask)
        {
            lblQuestion.Text = currentTask.Question;
            btnAns0.Content = currentTask.Ans0;
            btnAns1.Content = currentTask.Ans1;
            btnAns2.Content = currentTask.Ans2;
            btnAns3.Content = currentTask.Ans3;
        }

        static bool Check(Button btn)
        {
            if (btn.Content.ToString() == currentTask.Correct)
            {
                return true;
            }
            return false;
        }

        static void Read()
        {
            StreamReader sr = new StreamReader("bank.txt");

            while (!(sr.EndOfStream))
            {
                string[] line = sr.ReadLine().Split(';');

                Task task = new Task(line[0], line[1], line[2], line[3], line[4], line[5]);
                tasks.Add(task);
            }

            sr.Close();
        }

        public void ShowCorrect()
        {
            if (btnAns0.Content.ToString() == currentTask.Correct)
            {
                btnAns0.Background = Brushes.Orange;
            }
            else if (btnAns1.Content.ToString() == currentTask.Correct)
            {
                btnAns1.Background = Brushes.Orange;
            }
            else if (btnAns2.Content.ToString() == currentTask.Correct)
            {
                btnAns2.Background = Brushes.Orange;
            }
            else
            {
                btnAns3.Background = Brushes.Orange;
            }
        }

        private void btnAns0_Click(object sender, RoutedEventArgs e)
        {
            WaitForAns(btnAns0);

            if (Check(btnAns0))
            {
                Thread.Sleep(1000);
                btnAns0.Background = Brushes.Green;
                selectedIndex--;
                lbPrize.SelectedIndex = selectedIndex;
                GameHandler();
            }
            else
            {
                btnAns0.Background = Brushes.Red;
                ShowCorrect();
                GameOver();
            }
        }

        public void GameOver()
        {
            gameOver = true;
            MessageBox.Show(string.Format("A játéknak vége! {0}-ot nyertél!", prizeWon));
            Close();
        }

        public void WaitForAns(Button btn)
        {
            btn.Background = Brushes.Yellow;
            btn.Foreground = Brushes.Black;

            Thread.Sleep(rnd.Next(1000, 2001));
        }

        private void btnHalf_Click(object sender, RoutedEventArgs e)
        {
            List<Button> stayVisible = new List<Button>();
            stayVisible.Add(buttons.First(b => b.Content.ToString() == currentTask.Correct));

            while (stayVisible.Count != 2)
            {
                int i = rnd.Next(0, buttons.Count);
                while (buttons[i].Content.ToString() == currentTask.Correct)
                {
                    i = rnd.Next(0, buttons.Count);
                }
                stayVisible.Add(buttons[i]);
            }

            foreach (Button button in buttons)
            {
                bool done = false;
                foreach (Button visible in stayVisible)
                {
                    if (button.Content.ToString() == visible.Content.ToString())
                    {
                        button.Visibility = Visibility.Visible;
                        done = true;
                        break;
                    }
                }
                if (!done)
                {
                    button.Visibility = Visibility.Hidden;
                }
            }
            btnHalf.Visibility = Visibility.Hidden;
            btnVote.IsEnabled = false;
            btnCall.IsEnabled = false;
            helps.Remove(btnHalf);
        }

        private void btnAns1_Click(object sender, RoutedEventArgs e)
        {
            WaitForAns(btnAns1);

            if (Check(btnAns1))
            {
                Thread.Sleep(1000);
                btnAns1.Background = Brushes.Green;
                selectedIndex--;
                lbPrize.SelectedIndex = selectedIndex;
                GameHandler();
            }
            else
            {
                btnAns1.Background = Brushes.Red;
                ShowCorrect();
                GameOver();
            }
        }

        private void btnAns2_Click(object sender, RoutedEventArgs e)
        {
            WaitForAns(btnAns2);

            if (Check(btnAns2))
            {
                Thread.Sleep(1000);
                btnAns2.Background = Brushes.Green;
                selectedIndex--;
                lbPrize.SelectedIndex = selectedIndex;
                GameHandler();
            }
            else
            {
                btnAns2.Background = Brushes.Red;
                ShowCorrect();
                GameOver();
            }
        }

        private void btnAns3_Click(object sender, RoutedEventArgs e)
        {
            WaitForAns(btnAns3);

            if (Check(btnAns3))
            {
                Thread.Sleep(1000);
                btnAns3.Background = Brushes.Green;
                selectedIndex--;
                lbPrize.SelectedIndex = selectedIndex;
                GameHandler();
            }
            else
            {
                btnAns3.Background = Brushes.Red;
                ShowCorrect();
                GameOver();
            }
        }

        private void btnVote_Click(object sender, RoutedEventArgs e)
        {
            int chanceofCorrect = rnd.Next(0, 10);
            int[] chances = new int[4];



            int maxChance = 100;
            int correctChance = rnd.Next(25, 41);

            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i].Content.ToString() == currentTask.Correct)
                {
                    correctChance = rnd.Next(25, 41);

                    chances[int.Parse(buttons[i].Name.Substring(buttons[i].Name.Length - 1))] += correctChance;
                    maxChance -= correctChance;
                }
                else
                {
                    int incorrectChance = rnd.Next(0, maxChance);
                    while (incorrectChance > correctChance)
                    {
                        incorrectChance = rnd.Next(0, maxChance);
                    }

                    if (i == 3)
                    {
                        chances[int.Parse(buttons[i].Name.Substring(buttons[i].Name.Length - 1))] += maxChance;
                    }
                    else
                    {
                        chances[int.Parse(buttons[i].Name.Substring(buttons[i].Name.Length - 1))] += incorrectChance;
                        maxChance -= incorrectChance;
                    }

                }
            }

            lblAns0.Content = chances[0]+ "%";
            lblAns1.Content = chances[1] + "%";
            lblAns2.Content = chances[2] + "%";
            lblAns3.Content = chances[3] + "%";

            btnVote.Visibility = Visibility.Hidden;
            btnHalf.IsEnabled = false;
            btnCall.IsEnabled = false;

            helps.Remove(btnVote);
        }

        private void btnCall_Click(object sender, RoutedEventArgs e)
        {
            int i = rnd.Next(0, 3);
            buttons[i].Background = Brushes.Yellow;
            buttons[i].Foreground = Brushes.Black;

            btnCall.Visibility = Visibility.Hidden;
            btnHalf.IsEnabled = false;
            btnVote.IsEnabled = false;

            helps.Remove(btnCall);
        }
    }
}
