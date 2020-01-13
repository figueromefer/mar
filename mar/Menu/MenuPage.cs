using System;

using Xamarin.Forms;

namespace mar
{
    public class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            Title = "MENÚ";
            BackgroundColor = Color.FromHex("#3D454C");
            Menu = new MenuListView() { Margin = new Thickness(0, 20, 0, 60)};

            var layout = new StackLayout
            {
                BackgroundColor = Color.FromHex("#3D454C"),
                Spacing = 0,
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(20, 40, 20, 50),
            };
            var imagen = new Image() { Source = ImageSource.FromFile("title.png"), WidthRequest=250, Aspect = Aspect.AspectFit };
            var stackimg = new StackLayout { Padding = new Thickness(0,35,0,50),BackgroundColor = Color.FromHex("#3D454C"), HorizontalOptions = LayoutOptions.FillAndExpand, Children = { imagen } };
            layout.Children.Add(stackimg);
            Label legal = new Label() { Margin = new Thickness(10, 50, 0, 10), TextColor = Color.White, FontSize = 16, Text = "Legal" };
            Label acerca = new Label() { Margin = new Thickness(10, 0, 0, 30), TextColor = Color.White, FontSize = 16, Text = "Acerca de" };
            var redes = new Image()
            {
                HorizontalOptions = LayoutOptions.Start,
                Source = ImageSource.FromFile("redes.png"),
                WidthRequest = 140,
                Aspect = Aspect.AspectFit
            };
            var stack2 = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Orientation = StackOrientation.Vertical,
                Children = {
                    Menu,
                    legal,
                    acerca,
                    redes
                }
            };
            
            
            layout.Children.Add(stack2);

            Content = layout;
        }



    }
}


