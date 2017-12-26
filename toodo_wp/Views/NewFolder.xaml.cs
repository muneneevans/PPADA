using toodo_wp.Common;
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
using toodo_wp.Models;
using Windows.UI.Core;
using Windows.UI;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace toodo_wp.Views
{
 
    public sealed partial class NewFolder : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        static string[] colorNames =
       {
            "Yellow",
            "Green",
            "Orange",
            "Pink",
            "DeepSkyBlue",
            "DarkGrey",
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
        static string[] ColorLabel = {"Red","Purple","Blue","Green","Yellow","Orange"};
        static uint[] ColorUnit = { 0xFF1744, 0xD500F9, 0x2979FF, 0x00E676, 0xFFEA00, 0xFF9100 };

        public NewFolder()
        {
            this.InitializeComponent();

            DataContext = App.vm;
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
            FolderNameTextBox.Focus(Windows.UI.Xaml.FocusState.Keyboard);
            SetControls();
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void SetControls()
        {
            try
            {
                List<ColorItem> item = new List<ColorItem>();
                for (int i = 0; i < uintColors.Length; i++)
                {
                    item.Add(new ColorItem() { ColorName = colorNames[i], ColorValue = ConvertColor(uintColors[i]), ColoUint = uintColors[i] });
                };

                this.ColorComboBox.ItemsSource = item;
            }
            catch { }
        }

        private void CreateFolderButton_Click(object sender, RoutedEventArgs e)
        {
            folder NewFolder = new folder();
            NewFolder.folder_name = FolderNameTextBox.Text;
            NewFolder.folder_color = ((ColorItem)ColorComboBox.SelectedItem).ColoUint;
            App.vm.AddFolder(NewFolder);

            Frame.GoBack();
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
