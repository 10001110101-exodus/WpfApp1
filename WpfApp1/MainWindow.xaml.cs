using System.Diagnostics.Eventing.Reader;
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
using System.Drawing;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region DEFS
        private const int squareSize = 256;
        public System.Windows.Threading.DispatcherTimer _frameTimer;
        string appPath = AppDomain.CurrentDomain.BaseDirectory;

        private Birb birb;
        private Worm worm;
        private Worm freakWorm;

        private int cawCounter = 0;
        private int stepSize = 12;

        private bool left;
        private bool right;
        private bool up;
        private bool down;

        private InventorySlot[] InventorySlots;

        private Random rnd = new Random();
        private System.Windows.Point[] points;

        private List<ISpriteSheet> sprites = new List<ISpriteSheet>();

        private System.Windows.Shapes.Rectangle eyeballStatue;
        private ImageBrush eyeballBrush;
        private System.Windows.Controls.Image eyeballImage; 

        private Interaction wormInteraction1;
        private Interaction wormInteraction2;
        private Interaction freakWormInteraction;
        private Interaction freakWormInteraction1;
        private Interaction freakWormInteraction2;
        private Interaction wormInteraction3;
        private Interaction statueInteraction;

        private Interaction curInteraction;

        private Rect eyeballStatueHitbox = new Rect((74 + 13) * 4, (14 + 16) * 4, 36 * 4, 25 * 4);
        private bool behindFreak = false;
        private bool behindWorm = false;
        private bool behindStatue = false;

        private LOCATION location = LOCATION.AREA1;
        private enum LOCATION
        {
            AREA1,
            AREA2
        }
        #endregion

        #region INIT
        public MainWindow()
        {
            birb = new Birb(appPath + @"\sprites", 6);
            worm = new Worm(appPath + @"\sprites", @"\spriteWorm");
            freakWorm = new Worm(appPath + @"\sprites", @"\freakWorm");

            eyeballBrush = new ImageBrush(new BitmapImage(new Uri(appPath + @"\eyeballStatue0.png")));
            eyeballStatue = new System.Windows.Shapes.Rectangle
            {
                Width = 64 * 4,
                Height = 64 * 4,
                Fill = eyeballBrush
            };

            //eyeballImage = new System.Windows.Controls.Image();
            //eyeballImage.Height = 64 * 4;
            //eyeballImage.Width = 64 * 4;
            //eyeballImage.Source = new BitmapImage(new Uri("C:\\Users\\pcardwell\\source\\repos\\bird time\\WpfApp1\\eyeballStatue0.png"));

            sprites.Add(worm);
            sprites.Add(freakWorm);
            sprites.Add(birb);


            worm.LocationX = 448;
            worm.LocationY = 64;

            freakWorm.LocationY = 300;
            freakWorm.LocationX = 10;

            _frameTimer = new System.Windows.Threading.DispatcherTimer();
            _frameTimer.Tick += onFrame;
            _frameTimer.Interval = TimeSpan.FromMilliseconds(75);

            InitializeComponent();

            InventorySlots = new InventorySlot[] { new InventorySlot(Slot0), new InventorySlot(Slot1), new InventorySlot(Slot2) };
            ItemAquired.Visibility = Visibility.Hidden;

            InitializeInteraction();
            //drawArea();
            _frameTimer.Start();
        }
        private void InitializeInteraction()
        {
            ItemRecieve itemrec = new ItemRecieve(new BitmapImage(new Uri(appPath + @"\meat.png")), ITEMS.MEAT);
            SpriteMove freakMove = new SpriteMove(freakWorm, new int[] { 896 }, new DIR[] { DIR.RIGHT }, 12);

            wormInteraction1 = new Interaction(new List<IAction>() { new DialogueLine("???", "erm", "wormDialogue_"),
                                                                          new DialogueLine("???", "would you like this meat?", "wormDialogue_"), itemrec,
                                                                          new DialogueLine("???", "Watch out for my peculiar cousin over there,", "wormDialogue_"),
                                                                          new DialogueLine("???", "He really likes meat...", "wormDialogue_")}, Dialogue, dialogueText, dialogueName, 
                                                                          DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

            wormInteraction2 = new Interaction(new List<IAction>() { new DialogueLine("???", "Watch out for my freaky cousin over there,", "wormDialogue_"),
                                                                          new DialogueLine("???", "He really likes meat...", "wormDialogue_")}, Dialogue, dialogueText, dialogueName,
                                                                          DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

            freakWormInteraction = new Interaction(new List<IAction>() { new DialogueLine("Freak Worm", "HI DO YOU HAVE MEAT", "freakWormDialogue_"),
                                                                          new DialogueLine("Freak Worm", "PLEASE OH PLEASE OH PLEASE", "freakWormDialogue_")}, Dialogue, dialogueText, dialogueName,
                                                                          DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

            freakWormInteraction1 = new Interaction(new List<IAction>() { new DialogueLine("Freak Worm", "YES YES THANK YOU YES", "freakWormDialogue_"), 
                                                                          new DialogueLine("???", "...", "wormDialogue_"),
                                                                          freakMove }, Dialogue, dialogueText, dialogueName,
                                                                          DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

            freakWormInteraction2 = new Interaction(new List<IAction>() { new DialogueLine("Freak Worm", "BRING ME MORE\n F L E S H\nFOR MY EFFIGY", "freakWormDialogue_") }, Dialogue, dialogueText, dialogueName,
                                                              DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

            wormInteraction3 = new Interaction(new List<IAction>() { new DialogueLine("???", "Oh god...", "wormDialogue_"),
                                                                     new DialogueLine("???", "What have you done?", "wormDialogue_")}, Dialogue, dialogueText, dialogueName,
                                                                     DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

            statueInteraction = new Interaction(new List<IAction>() { new DialogueLine("Birb", "Hm, what a strange statue...", "birbDialogue_"),
                                                                      new DialogueLine("Birb", "Oh well, surely this won't become plot relevant later.", "birbDialogue_"),
                                                                     }, Dialogue, dialogueText, dialogueName,
                                                                     DialoguePortrait, ItemAquired, InventorySlots, _frameTimer);

        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            foreach (var sprite in sprites)
            {
                GameArea.Children.Add(sprite.UiElement);
                sprite.Update();
                Canvas.SetTop(sprite.UiElement, sprite.LocationY);
                Canvas.SetLeft(sprite.UiElement, sprite.LocationX);
                sprite.IsOnScreen = true;
            }
            drawCharacter();
        }

        private void drawArea()
{ 
            ImageBrush meatBrush = new ImageBrush();
            meatBrush.ImageSource = new BitmapImage(new Uri(appPath + @"\meat.png"));
            points = new System.Windows.Point[5];
            for (int i = 0; i < points.Length; i++)
            {
                int meatX = rnd.Next(0, (int)this.GameArea.Width) % 160 * 4;
                int meatY = rnd.Next(0, (int)this.GameArea.Height) % 64 * 4;
                points[i] = new System.Windows.Point(meatX, meatY);

                System.Windows.Shapes.Rectangle meat = new System.Windows.Shapes.Rectangle
                {
                    Height = 128,
                    Width = 128,
                    Fill = meatBrush,
                    StrokeThickness = 2
                };
                GameArea.Children.Add(meat);
                Canvas.SetTop(meat, meatY);
                Canvas.SetLeft(meat, meatX);

            }
}
        #endregion

        #region GRAPHICS
        private void drawCharacter()
        {
            if (birb.Walking && !birb.Caw)
            {
                if (up)
                {
                    birb.LocationY -= stepSize;
                }
                else if (down)
                {
                    birb.LocationY += stepSize;
                }
                if (right)
                {
                    birb.facingRight = true;
                    birb.LocationX += stepSize;
                }
                else if (left)
                {
                    birb.facingRight = false;
                    birb.LocationX -= stepSize;
                }
            }

            if (birb.LocationX + 256 < 0 && location == LOCATION.AREA2)
            {
                renderArea1();
                return;
            }
            else if (birb.LocationX > GameArea.Width && location == LOCATION.AREA1)
            {
                renderArea2();
                return;
            }

            checkBehind();

            Canvas.SetTop(birb.UiElement, birb.LocationY);
            Canvas.SetLeft(birb.UiElement, birb.LocationX);

            if (cawCounter > 0)
            {
                cawCounter--;
            }
            else birb.Caw = false;

            Canvas.SetTop(freakWorm.UiElement, freakWorm.LocationY);
            Canvas.SetLeft(freakWorm.UiElement, freakWorm.LocationX);

            void checkBehind()
            {
                if (checkHitBoxBottom(worm.BottomHitBox))
                {
                    if (!behindWorm)
                    {
                        GameArea.Children.Remove(worm.UiElement);
                        GameArea.Children.Add(worm.UiElement);
                    }
                    behindWorm = true;
                }
                else
                {
                    if (behindWorm)
                    {
                        GameArea.Children.Remove(birb.UiElement);
                        GameArea.Children.Add(birb.UiElement);
                    }
                    behindWorm = false;
                }

                if (checkHitBoxBottom(freakWorm.BottomHitBox))
                {
                    if (!behindFreak)
                    {
                        GameArea.Children.Remove(freakWorm.UiElement);
                        GameArea.Children.Add(freakWorm.UiElement);
                    }
                    behindFreak = true;
                }
                else
                {
                    if (behindFreak)
                    {
                        GameArea.Children.Remove(birb.UiElement);
                        GameArea.Children.Add(birb.UiElement);
                    }
                    behindFreak = false;
                }

                if (location == LOCATION.AREA2)
                {

                    if (checkHitBoxBottom(eyeballStatueHitbox))
                    {
                        if (!behindStatue)
                        {
                            GameArea.Children.Remove(eyeballStatue);
                            GameArea.Children.Add(eyeballStatue);
                        }
                        behindStatue = true;
                    }
                    else
                    {
                        if(behindStatue)
                        {
                            GameArea.Children.Remove(birb.UiElement);
                            GameArea.Children.Add(birb.UiElement);
                        }
                        behindStatue = false;
                    }
                }
            }
        }

        private void renderArea1()
        {
            clearSprites();

            GameArea.Children.Remove(eyeballStatue);

            location = LOCATION.AREA1;

            backgroundBrush.ImageSource = new BitmapImage(new Uri(appPath + @"\background0.png"));

            birb.LocationX = (int)GameArea.Width - (128 / 2);
            GameArea.Children.Add(birb.UiElement);
            birb.IsOnScreen = true;

            GameArea.Children.Add(worm.UiElement);
            worm.IsOnScreen = true;

            if(!freakWormInteraction1.Done)
            {
                GameArea.Children.Add(freakWorm.UiElement);
                freakWorm.IsOnScreen = true;
            }

            setSpriteLocations();
        }

        private void renderArea2()
        {
            clearSprites();

            location = LOCATION.AREA2;

            string eyeballImageSrc = freakWormInteraction1.Done ? (appPath + @"\eyeballStatue1.png") : (appPath + @"\eyeballStatue0.png");
            backgroundBrush.ImageSource = new BitmapImage(new Uri(appPath + @"\background1.png"));
            eyeballBrush.ImageSource = new BitmapImage(new Uri(eyeballImageSrc));

            GameArea.Children.Add(eyeballStatue);
            Canvas.SetLeft(eyeballStatue, 74 * 4);
            Canvas.SetTop(eyeballStatue, 16 * 4);

            birb.LocationX = -(128 / 2);
            GameArea.Children.Add(birb.UiElement);
            birb.IsOnScreen = true;

            if (freakWormInteraction1.Done)
            {
                GameArea.Children.Add(freakWorm.UiElement);
                freakWorm.LocationY = 192;
                freakWorm.LocationX = 500;
                freakWorm.IsOnScreen = true;
            }

            setSpriteLocations();
        }

        private void setSpriteLocations()
        {
            foreach (var sprite in sprites)
            {
                if (GameArea.Children.Contains(sprite.UiElement))
                {
                    Canvas.SetTop(sprite.UiElement, sprite.LocationY);
                    Canvas.SetLeft(sprite.UiElement, sprite.LocationX);
                }
            }
        }

        private void clearSprites()
        {
            foreach (var sprite in sprites)
            {
                if(GameArea.Children.Contains(sprite.UiElement))
                {
                    GameArea.Children.Remove(sprite.UiElement);
                    sprite.IsOnScreen = false;
                }
            }
        }

        private void onFrame(object send, EventArgs e)
        {
            foreach (var sprite in sprites)
            {
                sprite.Update();
            }
            drawCharacter();
        }
        #endregion

        #region CHECKING FUNCTIONS 
        private bool checkHitBox(ISpriteSheet sprite)
        {
            return sprite.hitbox.IntersectsWith(birb.hitbox);
        }
        private bool checkHitBoxClick(System.Windows.Point click, ISpriteSheet sprite)
        {
            return sprite.hitbox.Contains(click); 
        }
        private bool checkHitBox(Rect obj)
        {
            return birb.hitbox.IntersectsWith(obj);
        }
        private bool checkHitBoxBottom(ISpriteSheet sprite)
        {
            return birb.BottomHitBox.IntersectsWith(sprite.BottomHitBox);
        }
        private bool checkHitBoxBottom(Rect hitbox)
        {
            return birb.BottomHitBox.IntersectsWith(hitbox);
        }
        #endregion 

        #region INPUT EVENTS

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                cawCounter = 8;
                birb.Caw = true;
                ItemAquired.Visibility = Visibility.Hidden;
                if (curInteraction != null)
                {
                    curInteraction.next();
                    if (curInteraction.Done)
                        curInteraction = null;
                }
                else
                {
                    if (worm.IsOnScreen && checkHitBox(worm))
                    {
                        if (!wormInteraction1.Done)
                        {
                            curInteraction = wormInteraction1;
                            wormInteraction1.next();
                        }
                        else if (!freakWormInteraction1.Done)
                        {
                            curInteraction = wormInteraction2;
                            wormInteraction2.next();
                        }
                        else
                        {
                            curInteraction = wormInteraction3;
                            wormInteraction3.next();
                        }
                    }
                    else if (freakWorm.IsOnScreen && checkHitBox(freakWorm))
                    {
                        if (!freakWormInteraction1.Done)
                        {
                            curInteraction = freakWormInteraction;
                            freakWormInteraction.next();
                        }
                        else
                        {
                            curInteraction = freakWormInteraction2;
                            freakWormInteraction2.next();
                        }
                    }
                    else if(location == LOCATION.AREA2 && checkHitBox(eyeballStatueHitbox))
                    {
                        curInteraction = statueInteraction;
                        statueInteraction.next();
                    }
                }
            }


            else if (!e.IsRepeat && curInteraction == null)
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
                if (!(left || up || down || right))
                {
                    birb.Walking = false;
                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(this);
            double x = p.X;
            double y = p.Y;
            
            if(checkHitBoxClick(p, freakWorm))
            {
                foreach(InventorySlot slot in InventorySlots)
                {
                    if(slot.Selected = true && slot.Item == ITEMS.MEAT)
                    {
                        slot.Empty();
                        curInteraction = freakWormInteraction1;
                        curInteraction.next();
                    }
                }
            }
            else
            {
                foreach (InventorySlot slot in InventorySlots)
                {
                    if (slot.Selected == true)
                    {
                        slot.Selected = false; 
                    }
                }
            }
        }
        #endregion

        #region INVENTORY SLOTS
        private void Slot0_Click(object sender, RoutedEventArgs e)
        {
            if (InventorySlots[0].Selected)
            {
                InventorySlots[0].Selected = false;
            }
            else
            {
                InventorySlots[0].Selected = true;
            }
        }
        private void Slot1_Click(object sender, RoutedEventArgs e)
        {
            if (InventorySlots[1].Selected)
            {
                InventorySlots[1].Selected = false;
            }
            else
            {
                InventorySlots[1].Selected = true;
            }
        }

        private void Slot2_Click(object sender, RoutedEventArgs e)
        {
            if (InventorySlots[2].Selected)
            {
                InventorySlots[2].Selected = false;
            }
            else
            {
                InventorySlots[2].Selected = true;
            }
        }

        #endregion

    }

}
