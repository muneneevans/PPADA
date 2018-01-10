﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ppada.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Start : Page
    {
        public Start()
        {
            this.InitializeComponent();
            this.DataContext = App.vm;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //request for the first note           
        }

        private void bookMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //navigate to the first note in the serie
            Frame.Navigate(typeof(NotePage), 1);
        }

        private void TopicsButton_Click(object sender, RoutedEventArgs e)
        {
            //navigate to the first note in the serie
            Frame.Navigate(typeof(TopicsPage), 1);
        }
    }
}
