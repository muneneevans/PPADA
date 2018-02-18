using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ppada.Models;
using Windows.UI.Popups;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace ppada.Views
{
    public sealed partial class NewAnnotationPage : ContentDialog
    {
        public NewAnnotationPage()
        {
            this.InitializeComponent();
            DataContext = App.vm;

        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ContentDialog_SaveButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //check for empty text
            if (this.title.Text == "" || this.content.Text == "")
            {
                //show error message
                MessageDialog msgbox = new MessageDialog("please enter content ");
                //Calling the Show method of MessageDialog class  
                //which will show the MessageBox  
                await msgbox.ShowAsync();
                return;
            }
            else
            {
                //create a new annotation
                DBHelper dbh = new DBHelper();
                Annotation newAnnotation = new Annotation();
                newAnnotation.title = this.title.Text;
                newAnnotation.content = this.content.Text;
                App.vm.createNewAnnotation(newAnnotation);
            }
        }

        private void ContentDialog_CancelButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
