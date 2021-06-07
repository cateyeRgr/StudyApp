using System;
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
using System.Data;
using System.Windows.Threading;
using System.Security.Cryptography;
using System.Text;


namespace FlashCards11
{
    public partial class MainWindow : Window
    {

        private int labelCount;
        private int countNextClicks;
        private int loginCount;
        private int subjectInt;
        private int userInt;

        private decimal questionsAskedCount;
        private decimal correctAnswersCount;
        private decimal questionsCountToSave;

        private string contentLbl;
        //save uid for chart
        string userString = null;
        private string saltString;
        private string hashString;
        private bool existsUser = false;
        private bool existsSubject = false;


        TextBox subjectTb;
        private List<Subject> subjects { get; set; }

        //live chart data properties
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }


        public MainWindow()
        {
            InitializeComponent();

            LoadLoginTab();
            LoadNewCardTab();
            LoadQueryTab();
            
        }

        private void LoadSearchTab()
        {         
            StudyAppEntities1 Context1 = new StudyAppEntities1();
            Context1.Subject.Load();
            ICollectionView CollectionView = CollectionViewSource.GetDefaultView(Context1.Subject.Local);
            ParentGrid.DataContext = CollectionView;
        }


        //only login tab visible as long as no valid credentials entered
        private void LoadLoginTab()
        {
            passwordReLb.Visibility = Visibility.Hidden;
            passwordRePb.Visibility = Visibility.Hidden;
            newCardTab.Visibility = Visibility.Hidden;
            queryTab.Visibility = Visibility.Hidden;
            searchTab.Visibility = Visibility.Hidden;
            chartTab.Visibility = Visibility.Hidden;
        }

        private void LoadNewCardTab()
        {
            FillSubjectCB();
            subjectCB.Items.Add("Neues Thema");
            subjectCB.SelectedIndex = 0;
            listBoxSub.SelectedIndex = 0;

        }

        private void LoadQueryTab()
        {
            nextBtn.Content = "Start";
            confirmLblQuest.Content = "";
            yesRbtn.Visibility = Visibility.Hidden;
            noRbtn.Visibility = Visibility.Hidden;
            showBtn.IsEnabled = false;
            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            stopBtn.IsEnabled = false;
        }

        private void GetUserID()
        {
            using (StudyAppEntities1 db = new StudyAppEntities1())
            {
                
                User user = db.User.FirstOrDefault(x => x.User_Name == userString);
                userInt = user.User_ID;
            }

        }

        //alternative option to binding to fill a combobox
        private void FillSubjectCB()
        {
            StudyAppEntities1 Context = new StudyAppEntities1();
            var query = from r in Context.Subject
                        group r by r.Subject_Name
                        into egroup
                        orderby egroup.Key
                        select egroup;
           
  
            foreach (var item in query)
            {
                var i5 = item.Key;
                ComboBoxItem itmS = new ComboBoxItem
                {
                    Content = i5
                };

                subjectCB.Items.Add(itmS);
                subjectCB.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContext();
          
        }

        private void LoadContext()
        {
            StudyAppEntities1 Context = new StudyAppEntities1();
            Context.Subject.Load();
            listBoxSub.Items.SortDescriptions.Clear();
            listBoxSub.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("Subject_Name", System.ComponentModel.ListSortDirection.Ascending));
            ParentGrid.DataContext = Context.Subject.Local;
        }


        private async void saveBtn_Click(object sender, RoutedEventArgs e) /////////////////////////////////////////////////////ändern!!!!!!!!!!!!!//////////////////////////////////////////////////7
        {
            ////Test, if entry fields are empty
            //if (String.IsNullOrEmpty(contentTB.Text) || String.IsNullOrEmpty(queryKeywordTB.Text))
            //{
            //    confirmLbl.Content = "Bitte alle Felder ausfüllen";
            //}

                  //else
                  //{
                  //    StudyAppEntities1 ctx = new StudyAppEntities1();
                  //    Subject s = ctx.Subject.FirstOrDefault(x => x.Subject_Name == subjectTb.Text);
                  //    //Item i = ctx.Item.FirstOrDefault(x => x.Item_Name == queryKeywordTB.Text);
                  //    //if (s != null && i != null)
                  //    if (ctx.Subject.FirstOrDefault(x => x.Subject_Name == subjectTb.Text) == null && (ctx.Item.FirstOrDefault(x => x.Item_Name == queryKeywordTB.Text) == null))
                  //    {
                  //        confirmLbl.Content = "Schon in Datenbank vorhanden.";
                  //    }
                  //    //else if (s != null)
                  //    else if (ctx.Subject.FirstOrDefault(x => x.Subject_Name == subjectTb.Text) == null)
                  //    {
                  //        //suche nach id subject
                  //        //id subject mit itm content in item tabelle einfügen

                  //        using (StudyAppEntities1 db = new StudyAppEntities1())
                  //        {
                  //            Item i1 = new Item
                  //            {
                  //                Item_Name = queryKeywordTB.Text,
                  //                Item_Content = contentTB.Text,
                  //                Subject_ID = s.Subject_ID
                  //            };

                  //            db.Item.Add(i1);
                  //            db.SaveChanges();
                  //        }
                  //    }
            //else
            //{

                    if (subjectCB.IsVisible)         
                    {
                        int subjectInt;

                  

                        try
                        {


                            //new subject from textbox entries
                            //ToDo: use of @params to avoid sql-injection
                            using (StudyAppEntities1 db = new StudyAppEntities1())
                            {
                                Subject s1 = new Subject
                                {
                                    Subject_Name = subjectCB.Text
                                };

                                db.Subject.Add(s1);
                                db.SaveChanges();
                            }

                            using (StudyAppEntities1 db = new StudyAppEntities1())
                            {
                                Subject subject = db.Subject.FirstOrDefault(x => x.Subject_Name.Contains(subjectCB.Text));
                                subjectInt = subject.Subject_ID;

                            }

                            using (StudyAppEntities1 db = new StudyAppEntities1())
                            {
                                Item i3 = new Item
                                {
                                    Item_Name = queryKeywordTB.Text,
                                    Item_Content = contentTB.Text,
                                    Subject_ID = subjectInt
                                };

                                db.Item.Add(i3);
                                db.SaveChanges();
                            }
                            LoadSearchTab();

                        }
                        catch (Exception)
                        {
                            confirmLblQuest.Visibility = Visibility.Visible;
                            confirmLblQuest.Content = "Datenbank nicht erreichbar";
                        }  
                    }

                    


                    else if (subjectTb.IsVisible)
                    {

                          if (CheckExistingSubject(subjectTb.Text))
                          {
                              confirmLbl.Content = "Fach in Auswahl vorhanden";

                          }
                          else { 
                            try
                            {
                            using (StudyAppEntities1 db = new StudyAppEntities1())
                            {
                                Subject s2 = new Subject
                                {
                                    Subject_Name = subjectTb.Text
                                };

                                db.Subject.Add(s2);
                                db.SaveChanges();
                            }

                            using (StudyAppEntities1 db = new StudyAppEntities1())
                            {
                                Subject subject = db.Subject.FirstOrDefault(x => x.Subject_Name.Contains(subjectTb.Text));
                                subjectInt = subject.Subject_ID;

                            }

                            using (StudyAppEntities1 db = new StudyAppEntities1())
                            {
                                Item i1 = new Item
                                {
                                    Item_Name = queryKeywordTB.Text,
                                    Item_Content = contentTB.Text,
                                    Subject_ID = subjectInt

                                };

                                db.Item.Add(i1);
                                db.SaveChanges();
                            }
                            subjectTb.Clear();
                            LoadSearchTab();

                        //SqlConnection sqlCon = ServerConnection.GetDBConnection();
                        ////String sql = "Select * from StudyAppItem";//old

                            //sqlCon.Open();
                            //int subjectInt;
                            //SqlCommand sqlCmd = new SqlCommand();
                            //sqlCmd.CommandText = "Insert into Subject (Subject_Name) values (subjectTb.Text)";

                            ////sqlCmd.CommandText = "Insert into Subject (Subject_Name) values (@SubjectName)";
                            ////sqlCmd.Parameters.Add("@SubjectName", SqlDbType.VarChar).Value = subjectTb.Text;
                            //sqlCmd.ExecuteNonQuery();
                            //sqlCmd.Parameters.Clear();

                            //using (StudyAppEntities1 db = new StudyAppEntities1())
                            //{
                            //    Subject subject = db.Subject.FirstOrDefault(x => x.Subject_Name.Contains(subjectTb.Text));
                            //    subjectInt = subject.Subject_ID;
                            //}
                            ////sql = "Insert into StudyAppItem (item_name, item_content, item_subject) values ('" + queryKeywordTB.Text + "','" + contentTB.Text + "', '" + subjectTb.Text + "')";//old
                            //sqlCmd.CommandText = "Insert into Item (Item_Name, Item_Content, Subject_ID) values ('" + queryKeywordTB.Text + "','" + contentTB.Text + "', subjectInt)";

                            ////sqlCmd.CommandText = "Insert into StudyAppItem (Item_Name, Item_Content) values (@ItemName, @Content, @SubjectID)";
                            ////sqlCmd.Parameters.Add("@ItemName", SqlDbType.VarChar).Value = queryKeywordTB.Text;
                            ////sqlCmd.Parameters.Add("@Content", SqlDbType.VarChar).Value = contentTB.Text;
                            ////sqlCmd.Parameters.Add("@SubjectID", SqlDbType.VarChar).Value = subjectInt;
                            //sqlCmd.ExecuteNonQuery();
                            //sqlCon.Close();

                            }
                                catch (Exception)
                            {
                                confirmLblQuest.Visibility = Visibility.Visible;
                                confirmLblQuest.Content = "Datenbank nicht erreichbar";
                            }
                    }

                //}


            }

                ClearCardTab();
                confirmLbl.Content = "Daten wurden gespeichert";
                await Task.Delay(3000);
                confirmLbl.Content = "";

        }
        

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearCardTab();
        }

        private void ClearCardTab()
        {
            queryKeywordTB.Clear();
            contentTB.Clear();
            confirmLbl.Content = "";
        }

        // to select random database item:
        public int randomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public int getRowCount()////////////////////////////////////////////////////////////////geändert
        {
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlConnection sqlCon = ServerConnection.GetDBConnection();
            //String sql = "Select * from StudyAppItem";//old
            String sql = "Select * from Item";
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

        /// <summary>
        /// populate QueryTab/ Abfrage
        /// </summary>
        /// <param name="count"></param>
        public void populateQueryTab(int count)//Query #ndern!! -> prozente erst nach Stopp-Klick gestartet
        {
            //SqlConnection sqlCon = new SqlConnection(connectionString);
            SqlConnection sqlCon = ServerConnection.GetDBConnection();

            //Random Number between beginning and end of table
            int randomInt = randomNumber(0, count);
            string randomIntString = randomInt.ToString();

            String sql = "Select Subject_Name, Item_Name, Item_Content from Item i join Subject s on i.Subject_ID = s.Subject_ID And i.Item_ID  =" + randomIntString;
            SqlCommand sqlCom = new SqlCommand(sql, sqlCon);
            sqlCon.Open();
            SqlDataReader dr = sqlCom.ExecuteReader();

            try
            {
                while (dr.Read())
                {
                    querySubjectLbl.Content = dr["Subject_Name"].ToString();
                    queryKeywordLbl.Content = dr["Item_Name"].ToString();
                    //save content for later use when 'Lösung' is clicked
                    contentLbl = dr["Item_Content"].ToString();
                }
                dr.Close();
                sqlCom.Dispose();
                sqlCon.Close();
            }
            catch (Exception)
            {
                confirmLblQuest.Content = "Datenbank nicht erreichbar";
            }
            return;
        }

        
        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            labelCount = 0;
            countNextClicks++;
            nextBtn.Content = "Nächste";
            //reset values after each "next" click
            confirmLblQuest.Content = "";
            queryContentTB.Text = "";
            showBtn.IsEnabled = true;
            yesRbtn.IsChecked = false;
            noRbtn.IsChecked = false;
            yesRbtn.Visibility = Visibility.Hidden;
            noRbtn.Visibility = Visibility.Hidden;

            getRowCount();
            populateQueryTab(labelCount);
            questionsAskedCount++;

            //next button doesn't work until one radio button is checked
            if (yesRbtn.IsChecked == false && noRbtn.IsChecked == false)
            {
                nextBtn.IsEnabled = false;
            }
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
           
            correctAnswersCount++;
            radioButtonIsChecked();
            //setGetPercentage();

            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            nextBtn.IsEnabled = true;
            stopBtn.IsEnabled = true;
        }

        private void noRbtn_Click(object sender, RoutedEventArgs e)
        {
            radioButtonIsChecked();
            //setGetPercentage();

            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            nextBtn.IsEnabled = true;
            stopBtn.IsEnabled = true;

        }

        //disable second radiobutton/ only one time choosing per question, enabling "next" button
        private void radioButtonIsChecked()
        {
            setGetPercentage();

            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            nextBtn.IsEnabled = true;
            stopBtn.IsEnabled = true;
        }


        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            //save percentage correct answers to database
            SavePercentage();
            countNextClicks = 0;
            questionsCountToSave = questionsAskedCount;
            questionsAskedCount = 0.0m;
            correctAnswersCount = 0.0m;
            pointsPercLbl.Content = 0;

            nextBtn.Content = "Start";
            nextBtn.IsEnabled = true;
            confirmLblQuest.Content = "";
            yesRbtn.Visibility = Visibility.Hidden;
            noRbtn.Visibility = Visibility.Hidden;
            yesRbtn.IsEnabled = false;
            noRbtn.IsEnabled = false;
            
        }


        //calculate right answer percentage 
        public decimal setGetPercentage()
        {
            decimal correctPerc;

            if (questionsAskedCount > 0)
            {
                correctPerc = correctAnswersCount * 100.0m / questionsAskedCount;
                pointsPercLbl.Content = string.Format("{0:0.00} %", correctPerc);
                return correctPerc;
            }
            else return 0.00m;
}

        //store changes in db
        private void SavePercentage()
        {
            using (StudyAppEntities1 db = new StudyAppEntities1())
            {
                Session s = new Session
                    {
                        Session_Date = System.DateTime.Now,
                        Session_Points = setGetPercentage(),
                        User_ID = userInt,
                    };

                db.Session.Add(s);
                db.SaveChanges();
            }
        }


            private void LoadGraphic() { 
            List<double> pointsList = new List<double>();
            List<DateTime> dateList = new List<DateTime>();
            List<string> dateStringList = new List<string>();

      
            SqlConnection sqlCon = ServerConnection.GetDBConnection();
            sqlCon.Open();
            string sqlString = "Select Session_Points, Session_Date from Session where User_ID=" + userInt;
            SqlCommand cmd = new SqlCommand(sqlString, sqlCon);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                pointsList.Add(Convert.ToDouble(dr["Session_Points"]));
                dateList.Add(Convert.ToDateTime(dr["Session_Date"]));
            }



            //using (StudyAppEntities1 db = new StudyAppEntities1())
            //{
            //    var query = from s in db.Session
            //                where s.User_ID == userInt
            //                select s.Session_Points;
            //    foreach (int q in query)
            //    {
            //        pointsList.Add(Convert.ToDouble(q));
            //    }
            //}

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(pointsList),
                    //LineSmoothness = 0, //straight line instead of points on line
                    //PointGeometry = null,
                    //PointGeometrySize = 0,
                }
            };
            foreach (var x in dateList)
            {
                var xSplitString = x.ToString().Split(' ');
                var xDateString = xSplitString[0];
                dateStringList.Add(xDateString);
            }
            Labels = dateStringList.ToArray();

            DataContext = this;
            //sqlCon.Close();

        }


        //check if only combobox selection is changed (not just tabcontrol)
        private void SubjectCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                if (newCardTab.IsSelected)
                {
                    //subscribe to DropDownClosed event to display textbox 
                    subjectCB.DropDownClosed += SubjectCB_DropDownClosed;
                }
        }

        private void SubjectCB_DropDownClosed(object sender, EventArgs e)
        {
            if (subjectCB.SelectedItem.ToString() == "Neues Thema")
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

        //Login-Tab logic
        private void LoginBt_Click(object sender, RoutedEventArgs e)
        {
            //checking if login or registration required
            //-->login
            if (passwordRePb.Visibility == Visibility.Hidden)
            {
                //check if password is correct
                if (CheckPassword())
                {
                    //save correct user name for chart
                    userString = TbUsername.Text;
                    GetUserID();
                    LoadGraphic();

                    PbPassword.Password = "";
                    TbUsername.Clear();
    
                    newCardTab.Visibility = Visibility.Visible;
                    queryTab.Visibility = Visibility.Visible;
                    searchTab.Visibility = Visibility.Visible;
                    chartTab.Visibility = Visibility.Visible;
                    tControl.SelectedIndex = 1;
                    loginCount++;
                    //GetUserID();

                }
                else
                {
                    LbMessage.Content = "Login fehlgeschlagen";
                }
            }
            //--> registration
            else
            {
                //user must be unique -> checking for user
                CheckExistingUser(TbUsername.Text);
                bool equals = PasswordsCompare();
                if (existsUser == false)
                {
                    //create 8 byte salt
                    saltString = CreateSalt(8);
                    hashString = CreateNConcatenate(saltString);
                    if (equals)
                    {
                        SaveLogin();
                        SavePercentage();
                        PbPassword.Password = "";
                        passwordRePb.Password = "";
                        TbUsername.Clear();
                        tControl.SelectedIndex = 1;
                    }
                    else
                    {
                        LbMessage.Content = "Passwort stimmt nicht überein";
                    }

                }
                else
                {
                    LbMessage.Content = "User bereits vorhanden";
                }
            }
        }

        private bool CheckExistingUser(string userName)
        {
            StudyAppEntities1 ctx = new StudyAppEntities1();
            User u = ctx.User.FirstOrDefault(x => x.User_Name == userName);
            if (u != null)
            {
                return existsUser = true;
            }
            return existsUser=false;
        }

        private bool CheckExistingSubject(string subjectName)
        {
            StudyAppEntities1 ctx = new StudyAppEntities1();
            Subject s = ctx.Subject.FirstOrDefault(x => x.Subject_Name == subjectName);
            if (s != null)
            {
                return existsSubject = true;
            }
            return existsSubject =false;
        }


        private string CreateSalt(int size)
        {
            // Generate cryptographic random number using cryptographic service provider
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            // Return base64 string representation of random number
            return Convert.ToBase64String(buff);
        }

        //combine salt and password entered at registration
        private string CreateNConcatenate(string saltString)
        {
            string saltNPass = saltString + PbPassword.Password;

            // Create a new instance of the hash crypto service provider.
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            // Convert the passwod data to hash to an array of Bytes.
            byte[] bytValue = Encoding.UTF8.GetBytes(saltNPass);

            // Compute the Hash. This returns an array of Bytes.
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            // represent the hash value as a base64-encoded string to transmit it over network
            string base64 = Convert.ToBase64String(bytHash);
            return base64;
        }

        //register user/ save user data
        private void SaveLogin()
        {
            using (StudyAppEntities1 db = new StudyAppEntities1())
            {
                User l = new User
                {
                    User_Name = TbUsername.Text,
                    Hash = hashString,
                    Salt = saltString
                };

                db.User.Add(l);
                db.SaveChanges();
            }
            ClearReg();
            LbMessage.Content = "Registrierung erfolgreich.";
            newCardTab.Visibility = Visibility.Visible;
            queryTab.Visibility = Visibility.Visible;
            searchTab.Visibility = Visibility.Visible;
            chartTab.Visibility = Visibility.Visible;

        }

        private bool CheckPassword()
        {
            string username = TbUsername.Text;
            string password = PbPassword.Password;

            return CheckPassword(username, password);
        }


        private bool CheckPassword(string userName, string password)
        {
            // Salt-Wert aus Datenbank auslesen
            string salt = GetSaltFromDB(userName);

            //Nutzername nicht gefunden
            if (salt == null)
            {
                return false;
            }

            // Umwandeln des Salt in byte-Array
            byte[] saltBytes = Convert.FromBase64String(salt);

            // Bestimmen des Passwort-Hash-Wert für eingegebenes Passwort
            //Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes);
            //// Werte müssen identisch zu den Werten beim Generieren des Passwortes sein
            //rfc2898DeriveBytes.IterationCount = numberOfIterations;
            //byte[] enteredHash = rfc2898DeriveBytes.GetBytes(20);


            string enteredHash = CreateNConcatenate2(PbPassword.Password, TbUsername.Text);
            //// Umwandeln von byte-Array in String
            //string str = Convert.ToBase64String(enteredHash);
            // Erwarteten Hash-Wert aus Datenbank auslesen
            string expectedHash = GetHashFromDB(userName);

            // Vergleichen der Hash-Werte (evtl. Sicherheitsrisiko)
            bool hashesMatch = enteredHash.Equals(expectedHash);
            // Testausgabe
            //Console.WriteLine($"Salt (aus DB):       {salt}");
            //Console.WriteLine($"Hash (aus DB):       {expectedHash}");
            //Console.WriteLine($"Hash (aus Eingabe):  {str}");
            //Console.WriteLine($"Hash Werte gleich:   {hashesMatch}");
            return hashesMatch;
        }

        //combine salt from DB with password at login
        private string CreateNConcatenate2(string password, string userName)
        {
            string saltString = GetSaltFromDB(userName);
            string saltNPass = saltString + password;

            // Create a new instance of the hash crypto service provider.
            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();
            // Convert the passwod data to hash to an array of Bytes.
            byte[] bytValue = Encoding.UTF8.GetBytes(saltNPass);

            // Compute the Hash. This returns an array of Bytes.
            byte[] bytHash = hashAlg.ComputeHash(bytValue);
            // represent the hash value as a base64-encoded string to transmit it over network
            string base64 = Convert.ToBase64String(bytHash);
            return base64;
        }

        private static string GetHashFromDB(string userName)
        {
            StudyAppEntities1 ctx = new StudyAppEntities1();
            User l = ctx.User.FirstOrDefault(x => x.User_Name == userName);
            if (l != null)
            {
                return l.Hash;
            }
            return null;
        }

        private static string GetSaltFromDB(string userName)
        {
            StudyAppEntities1 ctx = new StudyAppEntities1();
            User l = ctx.User.FirstOrDefault(x => x.User_Name == userName);
            if (l != null)
            {
                return l.Salt;
            }
            return null;
        }



        private void RegisterBt_Click(object sender, RoutedEventArgs e)
        {
            if (passwordRePb.Visibility == Visibility.Hidden)
            {
                LoginTabToRegisterTab();
                //loginBt.Visibility = Visibility.Hidden;
                //registerBt.Visibility = Visibility.Hidden;
                //Main.Content = new Page1();
            }
            else
            //register button becomes back button
            {
                RegisterTabToLoginTab();
            }
        }

        private void LoginTabToRegisterTab()
        {
            passwordReLb.Visibility = Visibility.Visible;
            passwordRePb.Visibility = Visibility.Visible;
            loginBt.Content = "Speichern";
            registerBt.Content = "Zurück";
        }


        private void RegisterTabToLoginTab()
        {
            passwordReLb.Visibility = Visibility.Hidden;
            passwordRePb.Visibility = Visibility.Hidden;
            loginBt.Content = "Anmeldung";
            registerBt.Content = "Registrierung";
        }

        private void ClearReg()
        {
            TbUsername.Clear();
            PbPassword.Clear();
            passwordRePb.Clear();
        }

        private void PasswordRePb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordsCompare();
        }

        private bool PasswordsCompare()
        {
            //keep checking if 2 passwords in login/ register tab match
            if (PbPassword.Password != passwordRePb.Password)
            {
                LbMessage.Content = "Passwort stimmt nicht überein.";
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);

                LbMessage.Visibility = Visibility.Visible;
                timer.Tick += (s, en) => {
                    LbMessage.Visibility = Visibility.Collapsed;
                    timer.Stop();
                };
                timer.Start();
                return false;
            }
            return true;
        }

        private void DeleteBtn_Click_1(object sender, RoutedEventArgs e)
        {
            int itemSelectedID = (dataGrid.SelectedItem as Item).Item_ID;
            using (StudyAppEntities1 Context = new StudyAppEntities1())
            {
                Item i= (from r in Context.Item where r.Item_ID == itemSelectedID select r).SingleOrDefault();
                Context.Item.Remove(i);
                Context.SaveChanges();
                dataGrid.ItemsSource = Context.Item.ToList();
            }
        }


        //private void SearchTB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        //{

        //    StudyAppEntities1 Context = new StudyAppEntities1();

        //    System.ComponentModel.ICollectionView CollectionView = CollectionViewSource.GetDefaultView(Context.Item.Local);
        //    //ParentGrid.DataContext = CollectionView;
        //    //string filter = searchTB.Text.ToLower();
        //    //CollectionView.Filter = (x => ((Item)x).Item_Name.ToLower().Contains(filter));

        //    var b = Context.Item.FirstOrDefault(x => x.Item_Name.Contains(searchTB.Text));
        //    if (b != null)
        //    {
        //        CollectionView.MoveCurrentTo(b);
        //    }
        //}

        //private void TControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (searchTab.IsSelected)
        //    {
        //        StudyAppEntities1 Context = new StudyAppEntities1();
        //        searchGrid.ItemsSource = Context.Item.ToList();
        //    }

        //}
    }
}
