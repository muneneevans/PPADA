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
using Windows.UI.Core;
using Windows.Phone.UI.Input;
using Windows.UI;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ppada.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FolderItemPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private folder SelectedFolder = new folder();

        static string[] colorNames =
       {
            "Yellow",
            "Green",
            "Orange",
            "Pink",
            "Deep Sky Blue",
            "Dark Grey",
            "Red",
            "Deep Blue"
        };
        static uint[] uintColors =
        {
            0xFFFFD22F,
            0xFF669A50,
            0xFFFAA52F,
            0xFFE45181,
            0xFF67B4EA,
            0xFF949091,
            0xFFEC373E,
            0xFF2E405A
        };
        public FolderItemPage()
        {
            this.InitializeComponent();
            DataContext = App.vm;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
            HardwareButtons.BackPressed += BackButtonPresed;
        }

        
        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

       
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

       
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
           
            try
            {
                SelectedFolder = (folder)e.Parameter;
                SetControls(SelectedFolder);                
            }
            catch { }
            try
            {
                int colorindex = 0;
                List<ColorItem> item = new List<ColorItem>();
                for (int i = 0; i < uintColors.Length; i++)
                {
                    if (uintColors[i] == SelectedFolder.folder_color)
                    {
                        colorindex = i;
                    }
                    item.Add(new ColorItem() { ColorName = colorNames[i], ColorValue = ConvertColor(uintColors[i]), ColoUint = uintColors[i] });
                };

                this.ColorComboBox.ItemsSource = item;
                ColorComboBox.SelectedIndex = colorindex;
            }
            catch { }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SetControls(folder f)
        {
            if (f.folder_id == 1)
            {
                //disable the delete button
                DeleteFolderButton.Visibility = Visibility.Collapsed;
            }
            FolderNameTextBox.Text = f.folder_name;

        }
        private folder GetControlValues()
        {
            try
            {
                SelectedFolder.folder_name = FolderNameTextBox.Text;
                SelectedFolder.folder_color = ((ColorItem)ColorComboBox.SelectedItem).ColoUint;
                return SelectedFolder;
            }
            catch { return null; }
        }

        private void UpdateFolderButton_Click(object sender, RoutedEventArgs e)
        {
            App.vm.UpdateFolder(GetControlValues());
            Frame.GoBack();
        }

        private async void DeleteFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
            await App.vm.RemoveFolder(SelectedFolder);
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton DeleteBTN = (AppBarButton)sender;
            task SelectedTask = (task)DeleteBTN.DataContext;
            App.vm.RemoveTask(SelectedTask);
            FolderTaskListView.Focus(FocusState.Pointer);

        }

        private async void TaskItemGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid SelectedGrid = (Grid)sender;
            task SelectedTask = (task)SelectedGrid.DataContext;
            //this.Frame.Navigate(typeof(ItemPage ), SelectedTask);
            App.vm.SetCurrentFolder(SelectedTask.folder_id);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(ItemPage), SelectedTask));
        }

        private void TaskStatusCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                //get the task and update
                CheckBox c = (CheckBox)sender;
                task t = (task)c.DataContext;
                t.task_status = true;
                App.vm.UpdateTask(t);
            }
            catch { }
        }

        private void TaskStatusCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //get the task and update
                CheckBox c = (CheckBox)sender;
                task t = (task)c.DataContext;
                t.task_status = true;
                App.vm.UpdateTask(t);
            }
            catch { }
        }
        private void BackButtonPresed(object sender, BackPressedEventArgs e)
        {
            //
            App.vm.FilterPendingTasksByFolder(SelectedFolder);
            App.vm.InFolderPage = false;
            //Frame.GoBack();
        }

        private Color ConvertColor(uint uintCol)
        {
            byte A = (byte)((uintCol & 0xFF000000) >> 24);
            byte R = (byte)((uintCol & 0x00FF0000) >> 16);
            byte G = (byte)((uintCol & 0x0000FF00) >> 8);
            byte B = (byte)((uintCol & 0x000000FF) >> 0);

            return Color.FromArgb(A, R, G, B); ;
        }

    }
}
