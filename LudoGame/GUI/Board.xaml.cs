using LudoGame.Figures;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace LudoGame.GUI
{
    /// <summary>
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class Board : Page
    {
        private static LoadGame LoadGame {get;set;} = new LoadGame();
        private static int DICE_NUMBER { get; set; } = 0;
        private List<System.Windows.Shapes.Rectangle> movingPath { get; set; } = new List<System.Windows.Shapes.Rectangle>();
        private List<System.Windows.Shapes.Rectangle> homeRed { get; set; } = new List<System.Windows.Shapes.Rectangle>();
        private List<System.Windows.Shapes.Rectangle> homeYellow { get; set; } = new List<System.Windows.Shapes.Rectangle>();
        private List<System.Windows.Shapes.Rectangle> homeGreen { get; set; } = new List<System.Windows.Shapes.Rectangle>();
        private List<System.Windows.Shapes.Rectangle> homeBlue { get; set; } = new List<System.Windows.Shapes.Rectangle>();

        private bool canRollDice { get; set; } = true;
        public Board()
        {
            InitializeComponent();
            loadPath();
            loadFinish();
            LoadGame.load();
            int i = 0;
            foreach (Figure1 figure in LoadGame.figures)
            {
                setToHome(figure, i);
                i++;
            }
            HighlightNextPlayerLabel();
        }
        private void setToHome(Figure1 figure, int tag)
        {
            figure.MoveToHome();
            drawEllipse(figure.homePosition[0], figure.homePosition[1], figure.figureColor, tag);
        }
        private void drawEllipse(int x, int y, FigureColor color, int tag)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color.ToString()));
            Grid.SetRow(ellipse, y);
            Grid.SetColumn(ellipse, x);
            ellipse.Tag = tag;
            ellipse.MouseLeftButtonDown += moveFigure;

            // Add to Grid
            MyGrid.Children.Add(ellipse);
        }

        private void rollDice(object sender, RoutedEventArgs e)
        {
            if (!canRollDice) return;
            try
            {
                Random random = new Random();
                DICE_NUMBER = random.Next(1, 7);
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "GUI", "Resources", $"dice{DICE_NUMBER}.png");
                Dice.ImageSource = new BitmapImage(new Uri(path));
            }
            catch (Exception ex)
            {

            }
            try
            {
                canRollDice =
                    !isPlayable(); //prebacuje na drugog igraca ili ceka igranje
            }catch (Exception ex) { }
        }
        private void drawRectangle(int x, int y, string color, int tag)
        {
            System.Windows.Shapes.Rectangle rectangle = new  System.Windows.Shapes.Rectangle();
            rectangle.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
            rectangle.Tag = tag;
            Grid.SetRow(rectangle, y);
            Grid.SetColumn(rectangle, x);

            // Add to Grid
            MyGrid.Children.Add(rectangle);
            movingPath.Add(rectangle);
        }

        private void drawRectangleFinish(int x, int y, string color, int tag)
        {
            System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
            rectangle.Fill = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
            rectangle.Tag = tag;
            Grid.SetRow(rectangle, y);
            Grid.SetColumn(rectangle, x);

            // Add to Grid
            MyGrid.Children.Add(rectangle);
            switch (color)
            {
                case "Red":
                    homeRed.Add(rectangle);
                    break;
                case "Yellow":
                    homeYellow.Add(rectangle);
                    break;
                case "Blue":
                    homeBlue.Add(rectangle);
                    break;
                case "Green":
                    homeGreen.Add(rectangle);
                    break;
                default: return;
            }
        }
        private bool isAnythingPlayable() //odnosi se na sve figure
        {
            foreach (Figure1 fig in LoadGame.figures)
            {
                if (fig.isInGame && fig.stepsLeftToFinish>6) //fix this
                    return true;
                if (!fig.isInGame && DICE_NUMBER == 6)
                    return true;
            }
            return false;
        }
        //provjerava da li igrac poslije bacanja moze da napravi potez
        private bool isPlayable()
        {
            foreach (Figure1 fig in LoadGame.currentPlayer().figs)
            {
                if (fig.isInGame &&
                    fig.isMoveable(DICE_NUMBER)
                    && !isFigureInPlace(fig.calculateMove(DICE_NUMBER))
                    )
                    return true;
                if (!fig.isInGame && DICE_NUMBER == 6
                    &&
                    !isFigureInPlace(fig))
                    return true;
            }
            HighlightNextPlayerLabel();
            return false;
        }
        private void loadPath()
        {
            string color = "White";
            for (int i = 0; i < 6; i++)
            {
                if (i == 1) color = "Red";
                else color = "White";
                drawRectangle(i, 6, color, i);
            }
            int place = 0;
            for (int i = 6; i < 12; i++)
            {
                drawRectangle(6, 5 - place, "White", i);
                place++;
            }
            drawRectangle(7, 0, "White", 12);
            place = 0;

            for (int i = 13; i < 19; i++)
            {
                if (place == 1) color = "Green";
                else color = "White";
                drawRectangle(8, place, color, i);
                place++;
            }
            place = 0;
            for (int i = 19; i < 25; i++)
            {
                drawRectangle(9 + place, 6, color, i);
                place++;
            }
            drawRectangle(14, 7, "White", 25);
            place = 0;
            for (int i = 26; i < 32; i++)
            {
                if (place == 1) color = "Yellow";
                else color = "White";
                drawRectangle(14 - place, 14 - 6, color, i);
                place++;
            }
            place = 0;

            for (int i = 32; i < 38; i++)
            {
                drawRectangle(14 - 6, 9 + place, color, i);
                place++;
            }
            place = 0;
            drawRectangle(7, 14, "White", 38);
            for (int i = 39; i < 45; i++)
            {
                if (place == 1) color = "Blue";
                else color = "White";
                drawRectangle(6, 14 - place, color, i);
                place++;
            }
            place = 0;
            for (int i = 45; i < 51; i++)
            {
                drawRectangle(5 - place, 8, color, i);
                place++;
            }
            drawRectangle(0, 7, "White", 51);
        }

        private void loadFinish()
        {
            string color = "Red";
            for (int i = -1; i >= -4; i--)
            {
                color = "Red";
                drawRectangleFinish((-1 * i), 7, color, i);
                color = "Yellow";
                drawRectangleFinish(14 + i, 7, color, i);
                color = "Green";
                drawRectangleFinish(7, (-1 * i), color, i);
                color = "Blue";
                drawRectangleFinish(7, 14 + i, color, i);
            }
            color = "White";
        }
        //logika za pomjeranje figure na koju smo kliknuli i pravila
        private void moveFigure(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Ellipse el)
                    if (el.Tag is int tag)
                    {
                        if (canRollDice) return;
                        //provjera boje trenutnog igraca
                        Figure1 fig = LoadGame.figures[tag];
                        if (!fig.figureColor.Equals(LoadGame.currentPlayer()?.color)) //da li igrac koristi svoju figuru
                            return;
                        //provjera
                        if (!fig.isInGame && DICE_NUMBER == 6)
                        {
                            if (eatFigure(fig)) return; //ako su dve figure iste boje na istom mjestu

                            fig.MoveToStart();
                            fig.isInGame = true;
                        }
                        else if (!fig.isInGame) return;
                        else if (fig.isInGame)
                        {
                            /*if (!fig.Move(DICE_NUMBER)) {
                                if (isAnythingPlayable()) canRollDice = true; //logika da se provjeri moze li trenutni igrac ista uraditi, ako ne dobije 6 ne moze
                                return; }*/
                            if (!fig.isMoveable(DICE_NUMBER)) return;
                            if (eatFigure(fig.calculateMove(DICE_NUMBER))) return;
                            fig.Move(DICE_NUMBER);
                            //provjera da li se moze pomjeriti na slijedecu poziciju i da li neka druga zauzima polje?
                        }
                        System.Windows.Shapes.Rectangle rect = movingPath[fig.currentPos];
                        if (fig.isAtFinish)
                        {
                            try { rect = currentPlayerHome()[fig.currentPos]; }
                            catch (Exception ex)
                            { //ako je slucajno index out of bounds
                            }
                        }
                                drawEllipse(Grid.GetColumn(rect), Grid.GetRow(rect), fig.figureColor, tag);
                                deleteEllipse(el);
                                if (gameOver())
                                {
                                    MessageBox.Show(LoadGame.currentPlayer()?.color.ToString() + " player won!", "Game Over", MessageBoxButton.OK);
                                    NavigationService.Navigate(new Uri("GUI/StartPage.xaml", UriKind.Relative));
                                }
                                canRollDice = true;
                                HighlightNextPlayerLabel();
                            
                    }
            }
            catch (Exception exception) { }
        }
        private List<System.Windows.Shapes.Rectangle> currentPlayerHome()
        {
            switch (LoadGame.currentPlayer()?.color.ToString())
            {
                case "Red": return homeRed;
                case "Blue": return homeBlue;
                case "Green": return homeGreen;
                case "Yellow": return homeYellow;
                default: return homeRed;
            }
        }
        private void deleteEllipse(Ellipse ellipse)
        {
            MyGrid.Children.Remove(ellipse);
        }

        //da li dve figure stoje na istom mjestu, ako je iste boje vrati true ako nije jede je i vrati false
        private bool isFigureInPlace(Figure1 currentFigure)
        {
            int currentPos = currentFigure.currentPos;
            string color = currentFigure.figureColor.ToString();
            bool atFinish = currentFigure.isAtFinish;
            for (int i = 0; i < LoadGame.figures.Length; i++)
            {

                Figure1 fig = LoadGame.figures[i];
                if (currentPos == -1)
                {
                    if (currentFigure.startPos == fig.currentPos && fig.figureColor.ToString().Equals(color)) return true;
                    if (currentFigure.startPos == fig.currentPos)
                    {
                        if (currentFigure != fig && fig.isAtFinish == atFinish) //u slucaju da nekako sama na sebe stane
                        {
                            // ako je na finishu svom
                            if (fig.isAtFinish == true && fig.figureColor.ToString().Equals(color)) return true;
                            if (fig.isAtFinish == true) continue;
                            // ako je naisao na svog igraca na pocetnom polju
                            if (fig.figureColor.ToString().Equals(color)) return true;
                            /*deleteEllipse(FindEllipseByTag(i));
                            setToHome(fig, i);*/
                            return false;
                        }
                    }
                }
                if (currentPos != -1 && fig.currentPos == currentPos)
                    {
                        if (currentFigure != fig && fig.isAtFinish == atFinish) //u slucaju da nekako sama na sebe stane
                        {
                        // ako je na finishu svom
                        if (fig.isAtFinish == true && fig.figureColor.ToString().Equals(color)) return true;
                        if (fig.isAtFinish == true) continue;
                        // ako je naisao na svog igraca na pocetnom polju
                        if (fig.figureColor.ToString().Equals(color)) return true;
                            /*deleteEllipse(FindEllipseByTag(i));
                            setToHome(fig, i);*/
                            return false;
                        }
                    }
                }
                return false;
        }
        private bool eatFigure(Figure1 currentFigure)
        {
            int currentPos = currentFigure.currentPos;
            string color = currentFigure.figureColor.ToString();
            bool atFinish = currentFigure.isAtFinish;
            for (int i = 0; i < LoadGame.figures.Length; i++)
            {
                Figure1 fig = LoadGame.figures[i];
                if (currentPos == -1)
                {
                    if (currentFigure.startPos == fig.currentPos && fig.figureColor.ToString().Equals(color)) return true;
                    if (currentFigure.startPos == fig.currentPos)
                    {
                        if (currentFigure != fig && fig.isAtFinish == atFinish) //u slucaju da nekako sama na sebe stane
                        {
                            // ako je na finishu svom
                            if (fig.isAtFinish == true && fig.figureColor.ToString().Equals(color)) return true;
                            if (fig.isAtFinish == true) continue;
                            // ako je naisao na svog igraca na pocetnom polju
                            if (fig.figureColor.ToString().Equals(color)) return true;
                            deleteEllipse(FindEllipseByTag(i));
                            setToHome(fig, i);
                            return false;
                        }
                    }
                }
                if (currentPos != -1 && fig.currentPos == currentPos)
                {
                    if (currentFigure != fig && fig.isAtFinish==atFinish) //u slucaju da nekako sama na sebe stane
                    {
                        // ako je na finishu svom
                        if (fig.isAtFinish == true && fig.figureColor.ToString().Equals(color)) return true; //kada su zajedno u kucici nije dobro!
                        if (fig.isAtFinish == true) continue;
                        // ako je naisao na svog igraca na pocetnom polju
                        if (fig.figureColor.ToString().Equals(color)) return true;
                        deleteEllipse(FindEllipseByTag(i));
                        setToHome(fig, i);
                        return false;
                    }
                }
            }
            return false;

        }

        private Ellipse FindEllipseByTag(int tagValue)
        {
            foreach (UIElement element in MyGrid.Children)
            {
                if (element is Ellipse ellipse && ellipse.Tag != null && ellipse.Tag.Equals(tagValue))
                {
                    return ellipse; // Found the ellipse with the matching tag
                }
            }
            return null; // No matching ellipse found
        }
        private bool gameOver()
        {
            foreach (Figure1 fig in LoadGame.currentPlayer()?.figs)
            {
                if (!fig.isAtFinish) return false;
            }
            return true;
        }
        private void HighlightNextPlayerLabel()
        {
            LoadGame.NextPlayer();
            foreach (var child in MyGrid.Children)
            {
                if (child is System.Windows.Controls.Label label && label.Tag is string tag)
                {
                    label.Background = Brushes.Transparent;
                    if (LoadGame.currentPlayer()?.color.ToString() == tag)
                        label.Background = Brushes.White;
                    
                }
            }
        }
    }
}
