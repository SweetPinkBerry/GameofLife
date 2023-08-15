using Color = Microsoft.Maui.Graphics.Color;
using Microsoft.Maui.Controls.Shapes;

namespace MauiApp1;

public partial class GamePage : ContentPage
{
    readonly Brush aliveBrush = new SolidColorBrush(Color.FromArgb("#FF69B4"));
    readonly Brush deadBrush = new SolidColorBrush(Color.FromArgb("#ffc6e9"));

    public GamePage(int x, int y)
	{
        //Create View and vertical content
        var scrollView = new ScrollView();
        var verticalStackLayout = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        Rectangle[,] shapeGrid = new Rectangle[x, y]; //grid for shapes
        String[,] valuesGrid = new String[x, y]; //grid for values
        var gridLayout = CreateShapeGrid(shapeGrid, valuesGrid, x, y);

        //Label for displaying the current generation
        Label label = new()
        {
            Text = "Generation: 1",
            FontSize = 24,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        var horizontalStackLayout = new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        //Button for starting game
        Button gameButton = new()
        {
            Text = "Start game",
            BackgroundColor = Color.FromArgb("#ffc6e9"),
            TextColor = Color.FromArgb("#FF69B4"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        gameButton.Clicked += (sender, args) =>
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    valuesGrid[i, j] = ".";
                    shapeGrid[i, j].Fill = deadBrush;
                }
            }
            //Set random rectangles as alive
            Random random = new();
            for (int i = 0; i < ((x * y) / 2); i++)
            {
                int xPos = random.Next(x);
                int yPos = random.Next(y);
                valuesGrid[xPos, yPos] = "#";
                shapeGrid[xPos, yPos].Fill = aliveBrush;
            }
            RunGame(new GameofLife(x, y), valuesGrid, shapeGrid, label, gameButton, x, y);
        };
        horizontalStackLayout.Children.Add(gameButton);

        //Button for starting game
        Button editButton = new Button
        {
            Text = "Edit parameters",
            BackgroundColor = Color.FromArgb("#ffc6e9"),
            TextColor = Color.FromArgb("#FF69B4"),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        editButton.Clicked += async (sender, args) =>
        {
            await Navigation.PopAsync();
        };
        horizontalStackLayout.Children.Add(editButton);

        //Add View to content of page
        verticalStackLayout.Children.Add(gridLayout);
        verticalStackLayout.Children.Add(label);
        verticalStackLayout.Children.Add(horizontalStackLayout);
        scrollView.Content = verticalStackLayout;
        Content = scrollView;
    }

    //Create the visual and value grids, and return a visual layout for the grid to be added to the View
    private VerticalStackLayout CreateShapeGrid(Rectangle[,] shapeGrid, String[,] valuesGrid, int x, int y)
    {
        var gridLayout = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        for (int i = 0; i < x; i++)
        {
            //Create horizontal layout
            var horizontalStackLayout = new HorizontalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            //Create values and rectangles for row x and add to layout
            for (int j = 0; j < y; j++)
            {
                valuesGrid[i, j] = ".";
                shapeGrid[i, j] = new Rectangle
                {
                    WidthRequest = 20,
                    HeightRequest = 20,
                    Fill = deadBrush
                };
                horizontalStackLayout.Children.Add(shapeGrid[i, j]);
            }
            gridLayout.Children.Add(horizontalStackLayout);
        }
        return gridLayout;
    }

    //Perform Game of Life until there are no live cells or a stable shapes has been found
    private async void RunGame(GameofLife game, String[,] valuesGrid, Rectangle[,] shapeGrid, Label label, Button button, int x, int y)
    {
        await Task.Delay(1000);
        Tuple<String[,], int, bool> generation = game.NextGen(valuesGrid);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                if (generation.Item1[i, j] == "#") shapeGrid[i, j].Fill = aliveBrush;
                else shapeGrid[i, j].Fill = deadBrush;
            }
        }
        label.Text = "Generation: " + generation.Item2;
        valuesGrid = generation.Item1;
        if (!generation.Item3)
        {
            label.Text = "The game ended in " + generation.Item2 + " generations";
            button.Text = "Start new game";
        }
        else RunGame(game, valuesGrid, shapeGrid, label, button, x, y);
    }
}