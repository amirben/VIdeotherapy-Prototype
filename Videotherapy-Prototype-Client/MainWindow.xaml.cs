using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using VideotherapyPrototype.Switcher;

namespace VideotherapyPrototype
{
    
    
    /// <summary>
    /// Interaction logic for the MainWindow
    /// </summary>
    public partial class MainWindow : Window //, INotifyPropertyChanged
    {

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            try
            {
                this.Loaded += MainWindow_Loaded;
                this.Closing += MainWindow_Closing;

                Switcher.Switcher.mainWindow = this;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            
        }

        

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            VideosGallery vg = new VideosGallery(this);
            Grid.SetColumn(vg, 0);
            Grid.SetRow(vg, 0);
            contentGrid.Children.Add(vg);
        }

        public void Navigate(UserControl nextPage)
        {
            this.Content = nextPage;
        }
       
    }
}
