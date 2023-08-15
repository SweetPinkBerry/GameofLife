using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Controls.Shapes;
using CommunityToolkit.Maui.Markup;

namespace MauiApp1;
public partial class MainPage : ContentPage
{
    readonly Brush aliveBrush = new SolidColorBrush(Color.FromArgb("#FF69B4"));
    readonly Brush deadBrush = new SolidColorBrush(Color.FromArgb("#ffc6e9"));

    public MainPage()
	{
        var scrollView = new ScrollView();
		var verticalStackLayout = new VerticalStackLayout
		{
			HorizontalOptions = LayoutOptions.Center,
			VerticalOptions = LayoutOptions.Center
		};

        //Slider for X-axis value
        Slider slideX = new()
        {
            Minimum = 10,
            Maximum = 100,
            WidthRequest = 100
        };
        Label labelX = new()
        {
            Text = "X Value: " + Convert.ToInt32(slideX.Value).ToString(),
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        slideX.ValueChanged += (sender, args) =>
        {
            labelX.Text = "X Value: " + Convert.ToInt32(args.NewValue).ToString();
        };

        //Slider for Y-axis value
        Slider slideY = new()
        {
            Minimum = 10,
            Maximum = 100,
            WidthRequest = 100
        };
        Label labelY = new()
        {
            Text = "Y Value: " + Convert.ToInt32(slideY.Value).ToString(),
            FontSize = 12,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        slideY.ValueChanged += (sender, args) =>
        {
            labelY.Text = "Y Value: " + Convert.ToInt32(args.NewValue).ToString();
        };

        //Button for creating the game using the values entered into the sliders
        Button createGameButton = new()
        {
            Text = "Generate game",
            BackgroundColor = Color.FromArgb("#ffc6e9"),
            TextColor = Color.FromArgb("#FF69B4"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        createGameButton.Clicked += async (sender, args) =>
        {
            await Navigation.PushAsync(new GamePage(Convert.ToInt32(slideY.Value), Convert.ToInt32(slideX.Value)));
        };

        //Add outmost vertical layout to view and set the content of the window
        verticalStackLayout.Children.Add(MainPage.CreateSlider(slideX, labelX));
        verticalStackLayout.Children.Add(MainPage.CreateSlider(slideY, labelY));
        verticalStackLayout.Children.Add(createGameButton);
        scrollView.Content = verticalStackLayout;
		Content = scrollView;
    }

    private static HorizontalStackLayout CreateSlider(Slider slider, Label label) 
    {
        var horizontalStackLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        horizontalStackLayout.Children.Add(slider);
        horizontalStackLayout.Children.Add(label);
        return horizontalStackLayout;
    }
}

