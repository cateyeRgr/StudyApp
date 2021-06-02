﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data.Entity;
using System.Windows.Data;
using System.Linq;
using System.Windows.Controls;
using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace FlashCards11
{
    public partial class MainWindow : Window
    {
        //string connectionString = Connection.GetDBConnection().ToString();
        SqlConnection sqlCon = ServerConnection.GetDBConnection();
        int labelCount;
        int countNextClicks;
        int questionsAskedCount;
        double correctAnswersCount;
        string contentLbl;
        TextBox subjectTb;
        //private bool handle = true;

        ICollectionView CollectionView;
        StudyAppEntities Context = new StudyAppEntities();

        //live chart data properties
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
            Context.StudyAppItem.Load();
            //InitiateSearchCB();
            ICollectionView CollectionView = CollectionViewSource.GetDefaultView(Context.StudyAppItem.Local);
            ParentGrid.DataContext = CollectionView;

            nextBtn.Content = "Start";
            confirmLblQuest.Content = "";
            subjectCB.SelectedIndex = 0;
            //cbSearch.SelectedIndex = 0;

            yesRbtn.Visibility = Visibility.Hidden;
            noRbtn.Visibility = Visibility.Hidden;
            showBtn.IsEnabled = false;
            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
        }

        private void InitiateSearchCB()
        {
            List<string> columnNames = new List<string>();
            //columnNames.Add("Fach");
            //columnNames.Add("Name");
            //columnNames.Add("Inhalt");
            //cbSearch.Items.Add("Fach");
            //cbSearch.Items.Add("Stichwort");
            //cbSearch.Items.Add("Inhalt");
        }


        private void DisplaySubject()
        {
            Context.StudyAppItem.Load();
            CollectionView = CollectionViewSource.GetDefaultView(Context.StudyAppItem.Local);
            ParentGrid.DataContext = CollectionView;

            //populate Combo-Box
            using (var db = new StudyAppEntities())
            {

                var queryS = from s in db.StudyAppItem
                                 //every item occurs only once
                             group s by s.item_subject into newGroup
                             orderby newGroup.Key ascending
                             select newGroup;

                foreach (var item in queryS)
                {
                    var icS = item.Key;
                    ComboBoxItem itmS = new ComboBoxItem
                    {
                        Content = icS
                    };

                    subjectCB.Items.Add(itmS);
                    subjectCB.VerticalAlignment = VerticalAlignment.Center;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGraphic();
            DisplaySubject();
            subjectCB.Items.Add("Neues Fach");
            
        }

        private async void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //SqlConnection sqlCon = new SqlConnection(connectionString);            
            //    //Test, if entry fields are not empty
            //if (queryContentTB.Text.Length > 0 && contentTB.Text.Length > 0)
            //{
            if (String.IsNullOrEmpty(contentTB.Text) || String.IsNullOrEmpty(queryKeywordTB.Text))
            {
                confirmLbl.Content = "Bitte alle Felder ausfüllen.";
            }

            else { 
                string sql = null;

                if (subjectCB.IsVisible)
                {
                     sql = "Insert into StudyAppItem (item_name, item_content, item_subject) values ('" + queryKeywordTB.Text + "','" + contentTB.Text + "', '" + subjectCB.Text + "')";
                }
                else if (subjectTb.IsVisible)
                {
                    sql = "Insert into StudyAppItem (item_name, item_content, item_subject) values ('" + queryKeywordTB.Text + "','" + contentTB.Text + "', '" + subjectTb.Text + "')";
                }

                SqlCommand sqlCom = new SqlCommand(sql, sqlCon);
                SqlDataAdapter adapter = new SqlDataAdapter();
                String stichwortText = queryKeywordTB.Text;
                String inhaltText = contentTB.Text;

                adapter.InsertCommand = new SqlCommand(sql, sqlCon);
                sqlCon.Open();

                try
                {
                    adapter.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    confirmLblQuest.Visibility = Visibility.Visible;
                    confirmLblQuest.Content = "Datenbank nicht erreichbar";
                }

                sqlCom.Dispose();
                sqlCon.Close();
                ClearTB();
                subjectTb.Clear();
                confirmLbl.Content = "Daten wurden gespeichert";
                await Task.Delay(3000);
                confirmLbl.Content = "";
      
            }
            //else
            //{
            //    confirmLbl.Content = "Alle Felder ausfüllen.";
            //}
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            labelCount = 0;
            countNextClicks++;

            //reset values after each "next" click
            nextBtn.Content = "Nächste";
            confirmLblQuest.Content = "";
            queryContentTB.Text = "";
            showBtn.IsEnabled = true;
            yesRbtn.IsChecked = false;
            noRbtn.IsChecked = false;
            yesRbtn.Visibility = Visibility.Hidden;
            noRbtn.Visibility = Visibility.Hidden;

            getRowCount();
            setRandomLabels(labelCount);
            questionsAskedCount++;

            //next button doesn't work until one radio button is checked
            if (yesRbtn.IsChecked == false && noRbtn.IsChecked == false)
            {
                nextBtn.IsEnabled = false;
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearTB();
        }

        void ClearTB()
        {
            //fachTB.Text = "";
            //subjectTB.Clear();
            //stichwortTB.Text = "";
            queryKeywordTB.Clear();
            //inhaltTB.Text = "";
            contentTB.Clear();
            confirmLbl.Content = "";
        }

        // to select random database item:
        public int randomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public int getRowCount()
        {
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlConnection sqlCon = ServerConnection.GetDBConnection();
            String sql = "Select * from StudyAppItem";
            sqlCon.Open();
            SqlCommand sqlCom = new SqlCommand(sql, sqlCon);
            SqlDataReader reader = sqlCom.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    labelCount++;
                }
                reader.Close();
                sqlCom.Dispose();
                sqlCon.Close();
            }
            catch (Exception)
            {
                queryContentTB.Text = "Datenbank nicht erreichbar";
            }
            return 0;
        }

        public void setRandomLabels(int count)
        {
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlConnection sqlCon = ServerConnection.GetDBConnection();

            //Random Number between beginning and end of table
            int randomInt = randomNumber(0, count);
            string randomIntString = randomInt.ToString();

            String sql = "Select item_subject, item_name, item_content from StudyAppItem where item_id =" + randomIntString;
            SqlCommand sqlCom = new SqlCommand(sql, sqlCon);
            sqlCon.Open();
            SqlDataReader reader = sqlCom.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    querySubjectLbl.Content = reader["item_subject"].ToString();
                    queryKeywordLbl.Content = reader["item_name"].ToString();
                    //save content for later use when 'Lösung' is clicked
                    contentLbl = reader["item_content"].ToString();
                }
                reader.Close();
                sqlCom.Dispose();
                sqlCon.Close();
            }
            catch (Exception)
            {
                confirmLblQuest.Content = "Datenbank nicht erreichbar";
            }
            return;
        }

        private void solutionBtn_Click(object sender, RoutedEventArgs e)
        {
            //Show radio buttons and question after first click on "next"
            confirmLblQuest.Content = "Richtig gelöst?";
            yesRbtn.Visibility = Visibility.Visible;
            noRbtn.Visibility = Visibility.Visible;
            yesRbtn.IsEnabled = true;
            noRbtn.IsEnabled = true;
            //pass value from Datareader
            queryContentTB.Text = contentLbl;

        }

        private void yesRbtn_Click(object sender, RoutedEventArgs e)
        {
            //disable second radiobutton/ only one time choosing per question, enabling "next" button
            correctAnswersCount++;
            setPercentage();
            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            nextBtn.IsEnabled = true;
        }

        private void noRbtn_Click(object sender, RoutedEventArgs e)
        {
            setPercentage();
            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            nextBtn.IsEnabled = true;

        }

        //calculate percentage of right answers
        public void setPercentage()
        {
            double correctPerc = (correctAnswersCount * 100) / questionsAskedCount;
            pointsPercLbl.Content = string.Format("{0:0.00} %", correctPerc);
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            countNextClicks = 0;
            questionsAskedCount = 0;
            correctAnswersCount = 0;
            pointsPercLbl.Content = 0;

            nextBtn.Content = "Start";
            nextBtn.IsEnabled = true;
            confirmLblQuest.Content = "";
            yesRbtn.Visibility = Visibility.Hidden;
            noRbtn.Visibility = Visibility.Hidden;
            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
        }

        //Load knowledge table content
        private void LoadGrid()
        {
            var data = from r in Context.StudyAppItem select r;
            dgSearch.ItemsSource = data.ToList();
        }

        //private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string filterStr = tbFilter.Text;
        //    CollectionView.Filter = (x => ((StudyAppItem)x).item_content.Contains(filterStr));
        //}



        //private void CbSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (handle) Handle();
        //    handle = true;
        //}

        //private void CbSearch_DropDownClosed(object sender, EventArgs e)
        //{
        //    ComboBox cmb = sender as ComboBox;
        //    handle = !cmb.IsDropDownOpen;
        //    Handle();
        //}

        //private void Handle()
        //{
        //    switch (cbSearch.SelectedItem.ToString())
        //    {
        //        case "Fach":
        //            string filterStr = cbSearch.Text;
        //            Binding binding1 = new Binding("item_subject");

        //            break;
        //        case "Stichwort":
        //            //Handle for the second combobox
        //            break;
        //        case "Inhalt":
        //            //Handle for the third combobox
        //            break;
        //    }
        //}


        
       private void LoadGraphic() { 
            List<double> pointsList = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            List<string> dateStringList = new List<string>();

            sqlCon.Open();
            SqlCommand cmd = new SqlCommand("Select SessionPPoints, SessionPDate from SessionP", sqlCon);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                pointsList.Add(Convert.ToDouble(dr["SessionPPoints"]));
                dateList.Add(Convert.ToDateTime(dr["SessionPDate"]));
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(pointsList)
                }
            };
            foreach(var x in dateList)
            {
                var xSplitString = x.ToString().Split(' ');
                var xDateString = xSplitString[0];
                dateStringList.Add(xDateString);
            }
            Labels = dateStringList.ToArray();
            DataContext = this;
            sqlCon.Close();
                
        }



        private void SubjectCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (newCardTab.IsSelected)
                {
                    //subscribe to DropDownClosed event to display textbox 
                    subjectCB.DropDownClosed += SubjectCB_DropDownClosed;
                }
            }
        }

        private void SubjectCB_DropDownClosed(object sender, EventArgs e)
        {
            if (subjectCB.SelectedItem.ToString() == "Neues Fach")
            {
                subjectCB.Visibility = Visibility.Hidden;
                subjectTb = new TextBox();
                //TextBox layout
                DropShadowEffect myDropShadowEffect = new DropShadowEffect();
                myDropShadowEffect.ShadowDepth = 2;
                myDropShadowEffect.Direction = 330;
                myDropShadowEffect.Opacity = 0.5;
                myDropShadowEffect.BlurRadius = 5;
                subjectTb.Margin = new Thickness(8);
                subjectTb.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4488FF"));
                subjectTb.VerticalAlignment = VerticalAlignment.Center;
                subjectTb.Effect = myDropShadowEffect;

                //add new TextBox to Grid
                newCardGrid.Children.Add(subjectTb);
                Grid.SetRow(subjectTb, 0);
                Grid.SetColumn(subjectTb, 1);
                Grid.SetColumnSpan(subjectTb, 2);

            }
        }
    }
}
