using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
        private DataTransferManager _dataTransferManager;
        public Start()
        {
            this.InitializeComponent();
            this.DataContext = App.vm;

            this._dataTransferManager = DataTransferManager.GetForCurrentView();
            this._dataTransferManager.DataRequested += OnDataRequested;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void annotationsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AnnotationsPage));
        }

        private void BookmarksMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BookmarksPage));
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = "PPADA App";
            e.Request.Data.Properties.Description = "guidelines in public procurement";
            e.Request.Data.SetWebLink(new Uri("http://www.ppada.vorane.com"));
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }
    }
}
