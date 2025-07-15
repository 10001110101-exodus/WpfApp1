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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region DEFS
        private const int squareSize = 256;
        private System.Windows.Threading.DispatcherTimer _frameTimer;

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
        private Point[] points;

        private List<ISpriteSheet> sprites = new List<ISpriteSheet>();

        private Interaction wormInteraction1;
        private Interaction wormInteraction2;
        private Interaction freakWormInteraction;
        private Interaction freakWormInteraction1;
        private Interaction freakWormInteraction2;

        private Interaction curInteraction;

        private DIALOGUE_STATES dialogueState = DIALOGUE_STATES.OUT_DIALOGUE;

        private enum DIALOGUE_STATES
        {
            OUT_DIALOGUE,
            BUFFER,
            IN_DIALOGUE
        }

        #endregion

        #region INIT
        public MainWindow()
        {
            birb = new Birb("C:\\Users\\pcardwell\\source\\repos\\bird time\\WpfApp1\\sprites", 6);
            worm = new Worm("C:\\Users\\pcardwell\\source\\repos\\bird time\\WpfApp1\\sprites", "\\spriteWorm");
            freakWorm = new Worm("C:\\Users\\pcardwell\\source\\repos\\bird time\\WpfApp1\\sprites", "\\freakWorm");

            sprites.Add(worm);
            sprites.Add(birb);
            sprites.Add(freakWorm);

            worm.LocationX = 448;
            worm.LocationY = 64;

            freakWorm.LocationY = 300;
            freakWorm.LocationX = 10;

            _frameTimer = new System.Windows.Threading.DispatcherTimer();
            _frameTimer.Tick += onFrame;
            _frameTimer.Interval = TimeSpan.FromMilliseconds(75);

            InitializeComponent();

            InventorySlots = new InventorySlot[] { new InventorySlot(Slot0), new InventorySlot(Slot1), new InventorySlot(Slot2) };

            InitializeInteraction();
            //drawArea();

            _frameTimer.Start();
        }
        private void InitializeInteraction()
        {
            ItemRecieve itemrec = new ItemRecieve(new BitmapImage(new Uri("C:\\Users\\pcardwell\\source\\repos\\bird time\\WpfApp1\\meat.png")), ITEMS.MEAT);
            wormInteraction1 = new Interaction(new List<IAction>() { new DialogueLine("???", "erm", "wormDialogue_"),
                                                                          new DialogueLine("???", "would you like this meat?", "wormDialogue_"), itemrec,
                                                                          new DialogueLine("???", "Watch out for my freaky cousin over there,", "wormDialogue_"),
                                                                          new DialogueLine("???", "He really likes meat...", "wormDialogue_")}, Dialogue, dialogueText, dialogueName, 
                                                                          DialoguePortrait, ItemAquired, InventorySlots);

            wormInteraction2 = new Interaction(new List<IAction>() { new DialogueLine("???", "Watch out for my freaky cousin over there,", "wormDialogue_"),
                                                                          new DialogueLine("???", "He really likes meat...", "wormDialogue_")}, Dialogue, dialogueText, dialogueName,
                                                                          DialoguePortrait, ItemAquired, InventorySlots);

            freakWormInteraction = new Interaction(new List<IAction>() { new DialogueLine("Freak Worm", "HI DO YOU HAVE MEAT", "freakWormDialogue_"),
                                                                          new DialogueLine("Freak Worm", "PLEASE OH PLEASE OH PLEASE", "wormDialogue_")}, Dialogue, dialogueText, dialogueName,
                                                                          DialoguePortrait, ItemAquired, InventorySlots);

            freakWormInteraction1 = new Interaction(new List<IAction>() { new DialogueLine("Freak Worm", "YES YES THANK YOU YES", "freakWormDialogue_") }, Dialogue, dialogueText, dialogueName,
                                                                          DialoguePortrait, ItemAquired, InventorySlots);

            freakWormInteraction2 = new Interaction(new List<IAction>() { new DialogueLine("Freak Worm", "I LOVE MEAT!!", "freakWormDialogue_") }, Dialogue, dialogueText, dialogueName,
                                                              DialoguePortrait, ItemAquired, InventorySlots);
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
            ImageBrush meatBrush = new ImageBrush();
            meatBrush.ImageSource = new BitmapImage(new Uri("C:\\Users\\pcardwell\\source\\repos\\bird time\\WpfApp1\\meat.png"));
            points = new Point[5];
            for (int i = 0; i < points.Length; i++)
            {
                int meatX = rnd.Next(0, (int)this.GameArea.Width) % 160 * 4;
                int meatY = rnd.Next(0, (int)this.GameArea.Height) % 64 * 4;
                points[i] = new Point(meatX, meatY);

                Rectangle meat = new Rectangle
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
        private bool checkHitBoxClick(Point click, ISpriteSheet sprite)
        {
            return sprite.hitbox.Contains(click); 
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
                else if (checkHitBox(worm))
                {
                    if (!wormInteraction1.Done)
                    {
                        curInteraction = wormInteraction1;
                        wormInteraction1.next();
                    }
                    else
                    {
                        curInteraction = wormInteraction2;
                        wormInteraction2.next();
                    }
                }
                else if (checkHitBox(freakWorm))
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
            Point p = e.GetPosition(this);
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