using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int squareSize = 128;
        private System.Windows.Threading.DispatcherTimer _frameTimer;

        private Birb birb;
        private Worm worm;

        private int cawCounter = 0;
        private int stepSize = 8;

        private bool left;
        private bool right;
        private bool up;
        private bool down;

        private Random rnd = new Random();
        private Point[] points;

        private List<ISpriteSheet> sprites = new List<ISpriteSheet>();

        public MainWindow()
        {
            birb = new Birb("C:\\Users\\pcardwell\\source\\repos\\WpfApp1\\WpfApp1\\sprites", 6);
            worm = new Worm("C:\\Users\\pcardwell\\source\\repos\\WpfApp1\\WpfApp1\\sprites");
            sprites.Add(worm);
            sprites.Add(birb);
            _frameTimer = new System.Windows.Threading.DispatcherTimer();
            _frameTimer.Tick += onFrame;
            _frameTimer.Interval = TimeSpan.FromMilliseconds(75);

            InitializeComponent();
            //birb.UiElement = new Rectangle
            //{
                //Height = squareSize,
                //Width = squareSize,
                //Fill = playerImage,
                //StrokeThickness = 2
            //};
            //worm.UiElement = new Rectangle
            //{
                //Height = squareSize,
                //Width = squareSize,
                //Fill = wormImage,
                //StrokeThickness = 2
            //};
            _frameTimer.Start();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

            foreach (var sprite in sprites)
            {
                GameArea.Children.Add(sprite.UiElement);
                sprite.Update();
                Canvas.SetTop(sprite.UiElement, sprite.LocationY);
                Canvas.SetLeft(sprite.UiElement, sprite.LocationX);
            }
            drawCharacter();
        }

        private void drawArea()
        {
            ImageBrush backgroundTile = new ImageBrush();

            for (int i = 0; i < this.Width / squareSize; i++)
            {
                for (int j = 0; j < this.Height / squareSize; j++)
                {

                    //int temp = (j + i) % 2;
                    backgroundTile.ImageSource = new BitmapImage(new Uri("C:\\Users\\pcardwell\\source\\repos\\WpfApp1\\WpfApp1\\backgroundTile" + 0.ToString() + ".png"));

                    Rectangle rect = new Rectangle
                    {
                        Height = squareSize,
                        Width = squareSize,
                        Fill = backgroundTile,
                        StrokeThickness = 2
                    };
                    GameArea.Children.Add(rect);
                    Canvas.SetTop(rect, squareSize * i);
                    Canvas.SetLeft(rect, j * squareSize);
                }

            }
            ImageBrush rockBrush = new ImageBrush();
            rockBrush.ImageSource = new BitmapImage(new Uri("C:\\Users\\pcardwell\\source\\repos\\WpfApp1\\WpfApp1\\rock.png"));
            points = new Point[5];

            for (int i = 0; i < points.Length; i++)
            {
                int rockX = rnd.Next(0, (int)this.Width);
                int rockY = rnd.Next(0, (int)this.Height);
                points[i] = new Point(rockX, rockY);

                Rectangle rect = new Rectangle
                {
                    Height = squareSize,
                    Width = squareSize,
                    Fill = rockBrush,
                    StrokeThickness = 2
                };
                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, rockY);
                Canvas.SetLeft(rect, rockX);
            }
        }

        private void drawCharacter()
        {
            if (birb.Walking)
            {
                if (up)
                {
                    birb.LocationY -= stepSize;
                }
                else if (down)
                {
                    birb.LocationY += stepSize;
                }
                if(right)
                {
                    birb.facingRight = true;
                    birb.LocationX += stepSize;
                }
                else if(left)
                {
                    birb.facingRight = false;
                    birb.LocationX -= stepSize;
                }
            }

            Canvas.SetTop(birb.UiElement, birb.LocationY);
            Canvas.SetLeft(birb.UiElement, birb.LocationX);

            if (cawCounter > 0)
            {
                cawCounter--;
            }
            else birb.Caw = false;
        }

        private void onFrame(object send, EventArgs e)
        {
            foreach(var sprite in sprites)
            {
                sprite.Update();
            }
            drawCharacter();
        }

        private void showWorm()
        {
            worm.Talk_Bool = true;
            UIElement rect = new Rectangle
            {
                Height = 256,
                Width = 640,
                Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(200, 113, 150, 157))
            };
            GameArea.Children.Add(rect);
            GameArea.Children.Add(worm.UiTalk);
            Canvas.SetLeft(worm.UiTalk, 128);
            //GameArea.Children.Add(worm.UiElement);
            //Canvas.SetTop(worm.UiElement, 128);
            //Canvas.SetLeft(worm.UiElement, 512);
        }  
        private void hideWorm()
        {
            GameArea.Children.Remove(worm.UiTalk);
            //worm.Talk_Bool = false;
            //GameArea.Children.Clear();

            //GameArea.Children.Add(birb.UiElement);

            //GameArea.Children.Add(worm.UiElement);

            //birb.Update();
            //worm.Update();
            //Canvas.SetTop(worm.UiElement, 64);
            //Canvas.SetLeft(worm.UiElement, 448);
            //drawCharacter();
        }
        private bool checkHitBox(ISpriteSheet sprite)
        {
            int hitboxleft = sprite.LocationX + sprite.HitBoxLeft;
            int hitboxright = hitboxleft + sprite.HitBoxWidth;
            int hitboxtop = sprite.LocationY + sprite.HitBoxTop;
            int hitboxbottom = hitboxtop + sprite.HitBoxHeight;

            int birbboxleft = birb.LocationX + birb.HitBoxLeft;
            int birbboxright = birbboxleft + birb.HitBoxWidth;
            int birbboxtop = birb.LocationY + birb.HitBoxTop;
            int birbboxbottom = birbboxtop + birb.HitBoxHeight;

            return (birbboxright > hitboxleft
                && birbboxleft < hitboxright
                && birbboxtop < hitboxbottom
                && birbboxbottom > hitboxtop);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                cawCounter = 8;
                birb.Caw = true;
                if (worm.Talk_Bool)
                {
                    hideWorm();
                }
                else if (checkHitBox(worm))
                {
                    showWorm();
                }
            }

            else if(!e.IsRepeat)
            {
                birb.Walking = true;
                switch (e.Key)
                {
                    case Key.A:
                        right = false;
                        left = true;
                        break;
                    case Key.W:
                        down = false;
                        up = true;
                        break;
                    case Key.D:
                        left = false;
                        right = true;
                        break;
                    case Key.S:
                        up = false;
                        down = true;
                        break;
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
            {
                switch (e.Key)
                {
                    case Key.A:
                        left = false;
                        break;
                    case Key.W:
                        up = false;
                        break;
                    case Key.D:
                        right = false;
                        break;
                    case Key.S:
                        down = false;
                        break;
                }
                if(!(left ||  up || down || right))
                {
                    birb.Walking = false;
                }
            }
        }

    }
}