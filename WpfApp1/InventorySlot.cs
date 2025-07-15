using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class InventorySlot
    {
        private bool selected = false;
        private Button btn;
        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                if (selected)
                {
                    Btn.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 222, 92));
                    Btn.BorderThickness = new Thickness(3);
                }
                else
                {
                    Btn.BorderThickness = new Thickness(0);
                }
            }
        }
        public ITEMS Item { get; set; }
        public Button Btn { get { return btn; } set { btn = value; } }

        public InventorySlot(Button btn)
        {
            this.btn = btn;
            Item = ITEMS.EMPTY;
        }
        public void FillSlot(BitmapImage itemImage, ITEMS item)
        {
            Item = item;
            btn.Background = new ImageBrush(itemImage);
        }
        public void Empty()
        {
            Item = ITEMS.EMPTY;
            btn.Background = null;
            Selected = false;
            btn.BorderThickness = new Thickness(0);
        }
    }

    public enum ITEMS
    {
        EMPTY,
        MEAT
    }
}
