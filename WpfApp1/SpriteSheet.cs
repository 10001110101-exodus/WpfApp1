using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Shapes;
using System.Data;

namespace WpfApp1
{
    public interface ISpriteSheet
    {
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int HitBoxLeft { get; set; }
        public int HitBoxTop { get; set; }
        public int HitBoxWidth { get; set; }
        public int HitBoxHeight { get; set; }
        public System.Windows.Shapes.Rectangle UiElement { get; set; }
        public Rect hitbox { get; set; }
        public void Update();
        public void Talk();
        public bool IsOnScreen { get; set; }

        public Rect BottomHitBox { get; }
    }

    abstract class spriteSheet
    {
        private string _spriteSheetSource;
        cellCount standing;
        private int locationx;
        private int locationy;

    }
    public class Birb : ISpriteSheet
    {
        private string _spriteSheetSource;

        private bool walking = false;
        private bool facingright = false;
        private bool caw = false;

        cellCount cawIdx;
        cellCount walkIdx;

        private int locationx = 0;
        private int locationy = 0;
        private Rect _hitbox;
        private bool _isOnScreen = false;

        public int LocationX { get { return locationx; } set{ locationx = value; _hitbox = new Rect(HitBoxLeft + locationx, HitBoxTop + locationy, HitBoxWidth, HitBoxHeight); } }

        public int LocationY { get { return locationy; } set { locationy = value;
                _hitbox = new Rect(HitBoxLeft + locationx, HitBoxTop + locationy, HitBoxWidth, HitBoxHeight); } }

        public System.Windows.Shapes.Rectangle UiElement { get; set; }
        private ImageBrush img = new ImageBrush();
        public int HitBoxLeft { get; set; }
        public int HitBoxTop { get; set; }
        public int HitBoxWidth { get; set; }
        public int HitBoxHeight { get; set; }
        public Rect hitbox { get { return _hitbox; } set { this._hitbox = value; } }
        public bool IsOnScreen { get { return _isOnScreen; } set { _isOnScreen = value; } }

        private int _bottomLeft = 0 * 4;
        private int _bottomTop = 55 * 4;
        private int _bottomWidth = 64 * 4;
        private int _bottomHeight = 9 * 4;
        public Rect BottomHitBox
        {
            get
            {
                return new Rect(_bottomLeft + locationx, _bottomTop + locationy, _bottomWidth, _bottomHeight);
            }
        }
        public Birb(string spriteSheetSource, int numFrames)
        {
            _spriteSheetSource = spriteSheetSource;

            this.HitBoxLeft = 4 * -4;
            this.HitBoxWidth = 4 * 64;
            this.HitBoxTop = 4 * 5;
            this.HitBoxHeight = 4 * 58;
            this._hitbox = new Rect(HitBoxLeft + locationx, HitBoxTop + locationy, HitBoxWidth, HitBoxHeight);

            this.walkIdx = new cellCount(8);
            this.cawIdx = new cellCount(8);
            this.UiElement = new System.Windows.Shapes.Rectangle
            {
                Height = 256,
                Width = 256,
                Fill = img,
            };
        }

        public bool Walking { get { return walking; } set { /*if (value == true) walkIdx.index = 0; */walking = value; } }
        public bool facingRight {
            get { return facingright; }
            set { facingright = value; }
        }
        public bool Caw { get { return caw; } set { if  (value == true)  { cawIdx.index = 0; walking = false; } caw = value; } }


        public void Update()
        {
            cellCount cells;
            string filePrefix = "\\cloakedBird";
            if (caw)
            {
                filePrefix += "Caw";
                cells = cawIdx;
            }
            else
            {
                cells = walkIdx;
                if (walking) filePrefix += "Walk";
            }
            if (facingright) filePrefix += "Right";


            BitmapImage cell = new BitmapImage(new Uri(_spriteSheetSource + filePrefix + "_" + cells.index.ToString() + ".png"));
            cells.Increment();
            img.ImageSource = cell;
        }
        public void Talk()
        {

        }
    }

    public class Worm : ISpriteSheet
    {
        private string _spriteSheetSource;

        private bool talk = false;

        private int locationx = 0;
        private int locationy = 0;
        private bool _isOnScreen = false;

        private ImageBrush img = new ImageBrush();
        public ImageBrush talkimg = new ImageBrush();

        cellCount bob;
        cellCount talkCells;

        private Rect _hitbox;
        public int LocationX 
        {
            get { return locationx; } 
            set 
            { 
                locationx = value; 
                _hitbox = new Rect(HitBoxLeft + locationx, HitBoxTop + locationy, HitBoxWidth, HitBoxHeight);
            }
        }
        public int LocationY { 
            get { return locationy; } 
            set 
            {
                locationy = value; 
                _hitbox = new Rect(HitBoxLeft + locationx, HitBoxTop + locationy, HitBoxWidth, HitBoxHeight); 
            } 
        }
        public int HitBoxLeft { get; set; }
        public int HitBoxTop { get; set; }
        public int HitBoxWidth { get; set; }
        public int HitBoxHeight { get; set; }
        public System.Windows.Shapes.Rectangle UiElement { get; set; }
        public System.Windows.Shapes.Rectangle UiTalk { get; set; }
        public Rect hitbox { get { return _hitbox; } set { _hitbox = value; } }
        public bool IsOnScreen { get { return _isOnScreen; } set { _isOnScreen = value; } }

        public Rect BottomHitBox
        {
            get
            {
                return new Rect(_bottomLeft + locationx, _bottomTop + locationy, _bottomWidth, _bottomHeight);
            }
        }

        private int _bottomLeft = 0 * 4;
        private int _bottomTop = 0 * 4;
        private int _bottomWidth = 32 * 4;
        private int _bottomHeight = 14 * 4;

        private string filePrefix;
        public bool Talk_Bool { get { return talk; } set { talk = value; } }
        public Worm(string spriteSheetsource, string filePrefix)
        {
            this.HitBoxLeft = 4 * 3;
            this.HitBoxWidth = 4 * 27;
            this.HitBoxTop = 4 * 2;
            this.HitBoxHeight = 4 * 25;

            this._hitbox = new Rect(HitBoxLeft + LocationX, HitBoxTop + LocationY, HitBoxWidth, HitBoxHeight);

            this._spriteSheetSource = spriteSheetsource;
            this.filePrefix = filePrefix;

            this.bob = new cellCount(8);
            this.talkCells = new cellCount(2, 4);
            this.Update();

            this.UiElement = new System.Windows.Shapes.Rectangle
            {
                Height = 128,
                Width = 128,
                Fill = img,
             };

            this.UiTalk = new System.Windows.Shapes.Rectangle
            {
                Height = 256,
                Width = 512,
                Fill = talkimg
            };
        }

        public void Update()
        {
            //string filePrefix = "\\spriteWorm";
            cellCount cells;
            if (talk)
            {
                string filePrefixTalk = filePrefix + "Close";
                talkimg.ImageSource = new BitmapImage(new Uri(_spriteSheetSource + filePrefixTalk + "_" + talkCells.index.ToString() + ".png"));
                talkCells.Increment();
            }
            BitmapImage cell = new BitmapImage(new Uri(_spriteSheetSource + filePrefix + "_" + bob.index.ToString() + ".png"));
            bob.Increment();
            img.ImageSource = cell;
        }

        public void Talk()
        {
            
        }
    }
    public class cellCount
    {
        public cellCount(int numCells)
        {
            this.numCells = numCells;
            this.index = 0;
            this.frameRate = 1;
        }
        public cellCount(int numCells, int frameRate)
        {
            this.numCells = numCells;
            this.frameRate = frameRate;
            this.index = 0;
        }
        public int index { get; set; }
        public double secondIdx = 0;
        public int numCells { get; set; }
        public double frameRate;

        public void Increment()
        {
            this.secondIdx += 1 / frameRate;
            this.index = ((int)(this.secondIdx) % this.numCells);
        }
    }

}
