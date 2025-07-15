using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class ItemRecieve : IAction
    {
        private BitmapImage itemImage;
        private ITEMS itemName;
        private ActionType actionType = ActionType.ITEM;
        public new ActionType Type { get { return actionType; } }
        public BitmapImage ItemImage { get { return itemImage; } }
        public ITEMS ItemName { get { return itemName; } }

        public ItemRecieve(BitmapImage itemImage, ITEMS itemName)
        {
            this.itemImage = itemImage;
            this.itemName = itemName;
        }
    }
}
