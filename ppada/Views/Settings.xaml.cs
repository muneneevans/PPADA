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
using Windows.UI.Popups;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ppada.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Settings()
        {
            this.InitializeComponent();
            this.DataContext = App.vm;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void DeleteAllTasksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // confirm the deletion
                var TaskMessageDialog = new MessageDialog("All tasks will be permanently lost");

                TaskMessageDialog.Commands.Add(new UICommand("Continue", new UICommandInvokedHandler(DeleteAllTasks)));
                TaskMessageDialog.Commands.Add(new UICommand("Cancel"));

                TaskMessageDialog.DefaultCommandIndex = 1;
                TaskMessageDialog.CancelCommandIndex = 1;

                await TaskMessageDialog.ShowAsync();
            }
            catch { }
        }

        private void DeleteAllTasks(IUICommand command)
        {
            try
            {
                App.vm.DeleteAllTasks();
            }
            catch { }
        }

        private async void DeleteAllRoutinesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // confirm the deletion
                var RoutinesMessageDialog = new MessageDialog("All routines will be permanently lost");

                RoutinesMessageDialog.Commands.Add(new UICommand("Continue", new UICommandInvokedHandler(DeleteAllRoutines)));
                RoutinesMessageDialog.Commands.Add(new UICommand("Cancel"));

                RoutinesMessageDialog.DefaultCommandIndex = 1;
                RoutinesMessageDialog.CancelCommandIndex = 1;

                await RoutinesMessageDialog.ShowAsync();

            }
            catch { }

        }

        private void DeleteAllRoutines(IUICommand command)
        {
            try { App.vm.DeleteAllRoutines(); } catch { }
        }

        private async void DeleteAllFoldersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // confirm the deletion
                var FoldersMessageDialog = new MessageDialog("All folders will be permanently lost");

                FoldersMessageDialog.Commands.Add(new UICommand("Continue", new UICommandInvokedHandler(DeleteAllFolders)));
                FoldersMessageDialog.Commands.Add(new UICommand("Cancel"));

                FoldersMessageDialog.DefaultCommandIndex = 1;
                FoldersMessageDialog.CancelCommandIndex = 1;

                await FoldersMessageDialog.ShowAsync();

            }
            catch { }
        }

        private void DeleteAllFolders(IUICommand command)
        {
            try
            {
                App.vm.DeleteAllFolders();
            }
            catch
            {
            }
        }
    }
}
