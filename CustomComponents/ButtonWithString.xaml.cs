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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace cs_store_app_TextGame
{
    public sealed partial class ButtonWithString : UserControl
    {
        public ButtonWithString()
        {
            this.InitializeComponent();
        }

        public string Payload { get; set; }

        public new string Content
        {
            get
            {
                return btn.Content.ToString(); 
            }
            set
            {
                btn.Content = value;
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
