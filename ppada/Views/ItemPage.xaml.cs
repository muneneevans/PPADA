using ppada.Common;
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
using ppada.TaskViewModels;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ppada.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ItemPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private task SelectedTask = new task();

        public ItemPage()
        {
            this.InitializeComponent();
            this.DataContext = App.vm;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        #region NavigationHelper registration

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

            try {
                this.SelectedTask = (task)e.Parameter;

                //set the values on the screen       
                SetControlValues(this.SelectedTask);
            }catch{ }
        }
        
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SetControlValues(task t)
        {
            try
            {
                TaskNameTextBox.Text = t.task_name;                
                TaskDatePicker.Date = t.task_deadline;
                TaskTimePicker.Time = t.task_deadline.TimeOfDay;
                TaskPriorityCombobox.SelectedIndex = t.task_priority - 1;

                try
                {
                    TaskDetailsTextBox.Text = t.task_details;
                }
                catch
                {
                    TaskDetailsTextBox.Text = "";
                }

            }
            catch { }
        }
        private task GetControlValues()
        {
            try
            {
                task t = this.SelectedTask;
                t.task_name = TaskNameTextBox.Text;
                t.task_details = TaskDetailsTextBox.Text;
                DateTime date = Convert.ToDateTime(TaskDatePicker.Date.ToString());
                DateTime time = Convert.ToDateTime(TaskTimePicker.Time.ToString());
                t.task_deadline = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);                
                t.task_priority = Convert.ToInt16(((ComboBoxItem)this.TaskPriorityCombobox.SelectedItem).Tag.ToString());
                t.folder_id = ((folder)FolderCombobox.SelectedItem).folder_id;
                
                return t;
            }
            catch { return null; }
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            task UpdatedTask = new task();            
            UpdatedTask = GetControlValues();            
            App.vm.UpdateTask(UpdatedTask);

            Frame.GoBack();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            App.vm.RemoveTask(this.SelectedTask);
            Frame.GoBack();
        }
    }
}
