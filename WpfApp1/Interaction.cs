using System.Windows.Controls;

namespace WpfApp1
{
    public enum ActionType
    {
        ITEM,
        DIALOGUE
    }
    public class Interaction
    {
        private List<IAction> _actions;
        private int _index = 0;
        private bool _done = false;

        // DialogueLine stuff
        private Grid _dialogueBox;
        private TextBlock _dialogueText;
        private TextBlock _dialogueName;
        private System.Windows.Shapes.Rectangle _dialoguePortrait;

        // ItemRecieve stuff
        private TextBlock _itemRecievedText;
        private InventorySlot[] _inventorySlots;


        public bool Done { get { return _done; } }

        public Interaction(List<IAction> actions, Grid dialogueBox, TextBlock dialogueText, TextBlock dialogueName, System.Windows.Shapes.Rectangle dialoguePortrait,
                           TextBlock itemRecievedText, InventorySlot[] inventorySlots)
        {
            this._actions = actions;
            this._dialogueText = dialogueText;
            this._dialogueName = dialogueName;
            this._dialoguePortrait = dialoguePortrait;
            this._itemRecievedText = itemRecievedText;
            this._inventorySlots = inventorySlots;
            this._dialogueBox = dialogueBox;
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
                    break;
            }
            _index++;
        }
    }
}
