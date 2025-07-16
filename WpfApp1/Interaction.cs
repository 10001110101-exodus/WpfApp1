using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp1
{
    public enum ActionType
    {
        ITEM,
        DIALOGUE,
        MOVE
    }
    public class Interaction
    {
        private List<IAction> _actions;
        private int _index = 0;
        private bool _done = false;
        private bool onTickAdded = false;
        // DialogueLine stuff
        private Grid _dialogueBox;
        private TextBlock _dialogueText;
        private TextBlock _dialogueName;
        private System.Windows.Shapes.Rectangle _dialoguePortrait;

        // ItemRecieve stuff
        private TextBlock _itemRecievedText;
        private InventorySlot[] _inventorySlots;
        private DispatcherTimer _frameTimer;

        private SpriteMove spriteMove;

        public bool Done { get { return _done; } }

        public Interaction(List<IAction> actions, Grid dialogueBox, TextBlock dialogueText, TextBlock dialogueName, System.Windows.Shapes.Rectangle dialoguePortrait,
                           TextBlock itemRecievedText, InventorySlot[] inventorySlots, DispatcherTimer frameTimer)
        {
            this._actions = actions;
            this._dialogueText = dialogueText;
            this._dialogueName = dialogueName;
            this._dialoguePortrait = dialoguePortrait;
            this._itemRecievedText = itemRecievedText;
            this._inventorySlots = inventorySlots;
            this._dialogueBox = dialogueBox;
            this._frameTimer = frameTimer;
        }

        public void next()
        {
            bool ret = false;
            _itemRecievedText.Visibility = System.Windows.Visibility.Hidden;
            if (_index >= _actions.Count)
            {
                _index = 0;
                _dialogueBox.Visibility = System.Windows.Visibility.Hidden;
                _done = true;
                return;
            }
            switch (_actions[_index].Type)
            {
                case ActionType.DIALOGUE:
                    _dialogueBox.Visibility = System.Windows.Visibility.Visible;
                    DialogueLine line = _actions[_index] as DialogueLine;
                    _dialogueText.Text = line.Text;
                    _dialogueName.Text = line.Name;
                    _dialoguePortrait.Fill = line.PortraitBrush;
                    _index++;

                    break;
                case ActionType.ITEM:
                    ItemRecieve item = _actions[_index] as ItemRecieve;
                    for(int i = 0; i < _inventorySlots.Length; i++)
                    {
                        if (_inventorySlots[i].Item == ITEMS.EMPTY)
                        {
                            _inventorySlots[i].FillSlot(item.ItemImage, item.ItemName);
                            _itemRecievedText.Visibility = System.Windows.Visibility.Visible;
                            break;
                        }
                    }
                    _index++;
                    break;

                case ActionType.MOVE:
                    if (!onTickAdded)
                    {
                        spriteMove = _actions[_index] as SpriteMove;
                        _frameTimer.Tick += onTick;
                        onTickAdded = true;
                    }

                    break;
            }
        }
        
        private void onTick(object sender, EventArgs e) 
        {

            if (spriteMove.NumSteps <= 0)
            {
                spriteMove.i++;
            }
            if (spriteMove.i == spriteMove.StepsLength)
            {
                _index++;
                onTickAdded = false;
                _frameTimer.Tick -= onTick;
                return;
            }
            int stepsize = spriteMove.stepSize;
            switch (spriteMove.Dir)
            {
                case DIR.UP:
                    spriteMove.Sprite.LocationY -= stepsize;
                    break;
                case DIR.DOWN:
                    spriteMove.Sprite.LocationY += stepsize;
                    break;
                case DIR.LEFT:
                    spriteMove.Sprite.LocationX -= stepsize;
                    break;
                case DIR.RIGHT:
                    spriteMove.Sprite.LocationX += stepsize;
                    break;
            }
        }
    }
}
