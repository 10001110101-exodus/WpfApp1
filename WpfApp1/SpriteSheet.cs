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
    interface ISpriteSheet
    {
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int HitBoxLeft { get; set; }
        public int HitBoxTop { get; set; }
        public int HitBoxWidth {  get; set; }
        public int HitBoxHeight { get; set; }
        public UIElement UiElement { get; set; }
        public void Update();
        public void Talk();
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

        public int LocationX { get { return locationx; } set{ locationx = value; } }
        public int LocationY { get { return locationy; } set {  locationy = value; } }
        public UIElement UiElement { get; set; }
        private ImageBrush img = new ImageBrush();
        public int HitBoxLeft { get; set; }
        public int HitBoxTop { get; set; }
        public int HitBoxWidth { get; set; }
        public int HitBoxHeight { get; set; }
        public Birb(string spriteSheetSource, int numFrames)
        {
            _spriteSheetSource = spriteSheetSource;

            this.HitBoxLeft = 4 * 6;
            this.HitBoxWidth = 4 * 22;
            this.HitBoxTop = 4 * 9;
            this.HitBoxHeight = 4 * 14;

            this.walkIdx = new cellCount(6);
            this.cawIdx = new cellCount(8);
            this.UiElement = new System.Windows.Shapes.Rectangle
            {
                Height = 128,
                Width = 128,
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
            string filePrefix = "\\sprite";
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

        private int locationx = 448;
        private int locationy = 64;

        private ImageBrush img = new ImageBrush();
        private ImageBrush talkimg = new ImageBrush();

        cellCount bob;
        cellCount talkCells;

        public int LocationX { get { return locationx; } set { locationx = value; } }
        public int LocationY { get { return locationy; } set { locationy = value; } }
        public int HitBoxLeft { get; set; }
        public int HitBoxTop { get; set; }
        public int HitBoxWidth { get; set; }
        public int HitBoxHeight { get; set; }
        public UIElement UiElement { get; set; }
        public UIElement UiTalk { get; set; }

        public bool Talk_Bool { get { return talk; } set { talk = value; } }
        public Worm(string spriteSheetsource)
        {
            this.HitBoxLeft = 4 * 3;
            this.HitBoxWidth = 4 * 27;
            this.HitBoxTop = 4 * 4;
            this.HitBoxHeight = 4 * 22;

            this._spriteSheetSource = spriteSheetsource;
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
            string filePrefix = "\\spriteWorm";
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
