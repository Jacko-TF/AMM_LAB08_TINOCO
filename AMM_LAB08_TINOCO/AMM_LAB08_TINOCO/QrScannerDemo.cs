using AMM_LAB08_TINOCO.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AMM_LAB08_TINOCO
{
    public partial class QrScannerDemo : ContentPage
    {
       public QrScannerDemo()
        {
            StackLayout stack = new StackLayout();

            var button = new Button
            {
                Text = "Click",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            var entry = new Entry
            { 
                Text = "Text Do Scan" 
            };

            button.Clicked += async (sender, e) => 
            {
                try
                {
                    var scanner = DependencyService.Get<IQrScanningService>();

                    var result = await scanner.ScanAsync();
                    if(result != null) 
                    { 
                        entry.Text = result;
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            };
            stack.Children.Add(button);
            stack.Children.Add(entry);
            Content = stack;
        }
    }
}
