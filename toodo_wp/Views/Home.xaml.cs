
using System;
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
using toodo_wp.Models;
using toodo_wp.TaskViewModels;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using toodo_wp.SettingsManager;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace toodo_wp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        
        public Home()
        {
            this.InitializeComponent();
            try
            {
                DataContext = App.vm;                
            }
            catch { }
            
        }

        
        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Windows.UI.ViewManagement.StatusBar.GetForCurrentView().BackgroundColor = (Windows.UI.Colors.Cyan);
            try
            {
                RootPivot.SelectedIndex = (int)ApplicationsSettingsManager.ReadResetSettingsValue(Constants.RootPivotSelectedIndex);
            }
            catch { }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ApplicationsSettingsManager.SaveSettingsValue(Constants.RootPivotSelectedIndex, RootPivot.SelectedIndex);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //removeTask();
            //DataContext = new ViewModel();
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewTask));
            
        }
                        

        private void PendingTasksListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //StackPanel sp = (StackPanel)sender;
            task t = ((task)e.ClickedItem);
            this.Frame.Navigate(typeof(ItemPage), t);
        }

        private void CreateRoutineButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewRoutine));
        }        

        private async void TaskItemGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Grid SelectedGrid = (Grid)sender;
            task SelectedTask = (task) SelectedGrid.DataContext;
            //this.Frame.Navigate(typeof(ItemPage ), SelectedTask);
            App.vm.SetCurrentFolder(SelectedTask.folder_id);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(ItemPage), SelectedTask));

        }


        private async void NewFolderButton_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(NewFolder)));
        }

        private void FolderGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //filter by the folder
            folder f = (folder)e.ClickedItem;
            App.vm.FilterPendingTasksByFolder(f);
            App.vm.SetCurrentFolder(f);
            RootPivot.Title = f.folder_name;
            RootPivot.SelectedIndex = 0; 
        }

        private async void RoutineItemGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                Grid SelectedGrid = (Grid)sender;
                routine SelectedRoutine = (routine)SelectedGrid.DataContext;
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(RoutineItemPage), SelectedRoutine));
            }
            catch { }
        }

        private async void FolderSettingButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton ClickedButton = (AppBarButton)sender;
            folder SelectedFolder =(folder) ClickedButton.DataContext;
            App.vm.FilterTasksByFolder(SelectedFolder);
            App.vm.InFolderPage = true;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(FolderItemPage), SelectedFolder));
        }

        private void TodayButton_Click(object sender, RoutedEventArgs e)
        {
            App.vm.FilterTasksByToday();            
        }

        private void AllTasksButton_Click(object sender, RoutedEventArgs e)
        {
            //App.vm.FilterTasksByFolder(App.vm.CurrentFolder);
            RootPivot.SelectedIndex = 0;
            App.vm.GetAllPendingTasks();
            App.vm.UnSetCurrentFolder();
            RootPivot.Title = "All Tasks";
        }

        private void WeekButton_Click(object sender, RoutedEventArgs e)
        {
            App.vm.FilterTasksByWeek();
        }

        private void CreateFolderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewFolder));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // got to the settings page
            this.Frame.Navigate(typeof(Settings));
        }

        private async void SettingsButton_Click_1(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem Clickeditem = (MenuFlyoutItem)sender;
            folder SelectedFolder = (folder)Clickeditem.DataContext;
            App.vm.FilterTasksByFolder(SelectedFolder);
            App.vm.InFolderPage = true;
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(FolderItemPage), SelectedFolder));
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutbase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutbase.ShowAt(senderElement);
        }
    }
}
