﻿using ppada.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ppada.Models;
using Windows.System;
using Windows.ApplicationModel.DataTransfer;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ppada.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private DataTransferManager _dataTransferManager;
        //my variables
        Note currentNote = new Note();

        public NotePage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            this._dataTransferManager = DataTransferManager.GetForCurrentView();
            this._dataTransferManager.DataRequested += OnDataRequested;


            DBHelper db = new DBHelper();
            Note selectedNote = db.GetNote((int)e.Parameter);
            this.currentNote = selectedNote;
            if (setAttributes(selectedNote))
            {
                //tis all good bro
            }
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = this.currentNote.content;
            e.Request.Data.Properties.Description = this.currentNote.content;
            e.Request.Data.SetWebLink( new Uri("http://www.ppada.vorane.com")); 
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        private bool setAttributes(Note note)
        {
            try
            {
                this.titleTextBlock.Text = note.title;
                this.contentTextBlock.Text = note.content;
                return true;
            }
            catch

            {
                return false;
            }
        }

        #endregion

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //go to the next 
                DBHelper db = new DBHelper();
                Note nextNote = db.GetNote(this.currentNote.id - 1);
                currentNote = nextNote;
                if (setAttributes(nextNote))
                {
                    //alls good bro
                }
            }
            catch 
            {

                //throw;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //go to the next 
                DBHelper db = new DBHelper();
                Note nextNote = db.GetNote(this.currentNote.id + 1);
                currentNote = nextNote;
                if (setAttributes(nextNote))
                {
                    //alls good bro
                }
            }
            catch (Exception exp)
            {
            }
        }

        private async void annoteButton_Click(object sender, RoutedEventArgs e)
        {
            NewAnnotationPage n = new NewAnnotationPage();
            await n.ShowAsync();
        }

        private void bookMarkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //go to the next                                 
                App.vm.ToggleBookmark(this.currentNote);                
            }
            catch (Exception exp)
            {
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //search for the text
           
        }

        private void SearchTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == VirtualKey.Enter) {
                App.vm.SearchNote(this.SearchTextBox.Text);
                this.Frame.Navigate(typeof(SearchPage), this.SearchTextBox.Text);
            }
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }
    }
}
