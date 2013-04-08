using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;

namespace WorkWork
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void stopLogging_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            try
            {
                if (startLogging.Text.Contains("Tap"))
                {
                    Dispatcher.BeginInvoke(() =>
          {
              stopLogging.Text = "Tap the start button first!";
          });
                    return;
                }
                string today = startTextBox.ValueString;

                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();
                //Open existing file

                IsolatedStorageFileStream FS = ISF.OpenFile("WorkWork.txt", FileMode.Append, FileAccess.Write);



                Dispatcher.BeginInvoke(() =>
                {
                    stopLogging.Text = today;
                    string startTest = startLogging.Text;
                    using (StreamWriter SW = new StreamWriter(FS))
                    {
                        /*
                        if (today.Contains("PM"))
                        {
                            today = today.Remove(today.Length - 3);
                            startTest = startTest.Remove(today.Length - 3);
                        }
                        */


                        DateTime dt = DateTime.Now;
                        DateTime logDT = DateTime.Parse(dt.Year +"-"+ dt.Month +"-"+ dt.Day + " " + today);//new DateTime(dt.Year, dt.Month, dt.Day, Int16.Parse(today.Split(':')[0]), Int16.Parse(today.Split(':')[1]), 0);
                        DateTime startDT = DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + startTest);//new DateTime(dt.Year, dt.Month, dt.Day, Int16.Parse(startTest.Split(':')[0]), Int16.Parse(startTest.Split(':')[1]), 0);

                          if (logDT < startDT)
                          {
                              stopLogging.Text = "Stoptime before starttime!";
                              return;
                          }
                          else
                          {
                              SW.Write(logDT.ToString() + "\n");
                          }
                        // SW.Write("test");
                    }
                    loadData();  
                });
            }
            catch (Exception ex)
            {
                //do nothing... first run of program
            }

          

        }

        private void startLogging_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
           

             try
                {


                    string today = startTextBox.ValueString; // DateTime.Now.ToShortTimeString(); // = responseFromServer == null ? "Unknown" : responseFromServer.ToString();

            if (today.Equals(startLogging.Text))
                return;

            IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();
            //Open existing file

            IsolatedStorageFileStream FS = ISF.OpenFile("WorkWork.txt", FileMode.Append, FileAccess.Write);



            Dispatcher.BeginInvoke(() =>
            {
                startLogging.Text = today;
                stopLogging.Text = "Tap to stop logging";
                using (StreamWriter SW = new StreamWriter(FS))
                {
                  /*  if (today.Contains("PM"))
                    {
                       today = today.Remove(today.Length - 3);
                    }
                    */
                    DateTime dt = DateTime.Now;
                    DateTime logDT = DateTime.Parse(dt.Year + "-" + dt.Month + "-" + dt.Day + " " + today);//new DateTime(dt.Year, dt.Month, dt.Day, Int16.Parse(today.Split(':')[0]), Int16.Parse(today.Split(':')[1]), 0);
                  
                        SW.Write(logDT.ToString() + "\n");
                    
                //  SW.Write(DateTime.Now.ToString() + "\n");
                        // SW.Write("test");
                    }
          
                });
            
             }
                  catch (Exception ex)
                {
                    //do nothing... first run of program
                }


             


           

        }
        public class log {

            public string startTime { get; set; }
            public string stopTime { get; set; }
            public string day { get; set; }
            public string hours { get; set; }
      
        }

        List<string> logs;

        private void loadData() {
            logs = new List<string>();

           
            List<log> logItems = new List<log>();
            try
            {



                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();

                IsolatedStorageFileStream FS = ISF.OpenFile("WorkWork.txt", FileMode.Open, FileAccess.Read);

                using (StreamReader SR = new StreamReader(FS))
                {


                    string line = "";
                    while (!SR.EndOfStream)
                    {

                        var x = SR.ReadLine();

                        logs.Add(x);

                    }



                }


            }
            catch (Exception ex)
            {
                //do nothing... first run of program
            }


            if (logs.Count > 0)
            {
                try
                {
                    if (logs.Count % 2 == 1)
                    {

                        startLogging.Text = DateTime.Parse(logs[logs.Count - 1]).ToShortTimeString();
                    }
                    else
                    {
                        startLogging.Text = DateTime.Parse(logs[logs.Count - 2]).ToShortTimeString();
                        stopLogging.Text = DateTime.Parse(logs[logs.Count - 1]).ToShortTimeString();
                    }

                    for (int i = 0; i < logs.Count; i++)
                    {
                        if (i + 1 < logs.Count)
                        {
                            TimeSpan ts1 = DateTime.Parse((string)logs[i + 1]).Subtract(DateTime.Parse((string)logs[i]));

                            logItems.Add(new log() { startTime = (string)logs[i], stopTime = (string)logs[i + 1], day = DateTime.Parse((string)logs[i]).ToString("dddd"), hours = Math.Round(ts1.TotalHours, 1).ToString() });
                            i++;
                        }
                        else
                            logItems.Add(new log() { startTime = (string)logs[i], stopTime = "", day = "Today", hours = "?" });
                    }

                    timeBox.ItemsSource = null;
                    timeBox.ItemsSource = logItems;

                }
                catch (Exception ex)
                { 
                
                }
            }
            /*
            Dispatcher.BeginInvoke(() =>
            {
                startTextBox.Value = DateTime.Now;
            });
             */ 
        }

        private void Panorama_Loaded(object sender, RoutedEventArgs e)
        {
            loadData();     


        }

        private void Email_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string emailBody = "";

            foreach (string s in logs)
            {
                emailBody = emailBody + " \n" + s;
            }

             EmailComposeTask emailcomposer = new EmailComposeTask();
            
             emailcomposer.Subject = "Work Log";
             emailcomposer.Body = emailBody;
             emailcomposer.Show();

        }

        private void Clear_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

            try
            {

                timeBox.ItemsSource = null; 
                IsolatedStorageFile ISF = IsolatedStorageFile.GetUserStoreForApplication();
                //Open existing file

                IsolatedStorageFileStream FS = ISF.OpenFile("WorkWork.txt", FileMode.Create, FileAccess.Write);



                Dispatcher.BeginInvoke(() =>
                {

                    using (StreamWriter SW = new StreamWriter(FS))
                    {




                        //  IPHistoryBox.Items.Clear();
                        //IPHistoryBox.ItemsSource = ipitems;
                        SW.Write("");
                        // SW.Write("test");
                    }

                });
            }
            catch (Exception ex)
            {
                //do nothing... 
            }


             

        }

        private void Panorama_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void homepage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.URL = "http://mobile.martinschultz.dk";
            task.Show();
        }

    
    }
}