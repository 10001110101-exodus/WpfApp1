using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class DialogueLine : IAction
    {
        private string _name;
        private string _text;
        private string _portraitFile;
        private int i = 0;
        private string appPath = AppDomain.CurrentDomain.BaseDirectory;

        private ActionType actionType;

        public ImageBrush PortraitBrush = new ImageBrush();
        public string Name { get { return _name; } }
        public string Text { get { return _text; } }
        public ActionType Type { get { return actionType; } }
        public DialogueLine(string name, string text, string portraitFile)
        {
            this.actionType = ActionType.DIALOGUE;
            this._name = name;
            this._text = text;
            this._portraitFile = appPath + @"\Resources\DialoguePhotos\" + portraitFile;
            this.PortraitBrush.ImageSource = new BitmapImage(new Uri(this._portraitFile + "0" + ".png"));
        }

        public void Update()
        {
            this.PortraitBrush.ImageSource = new BitmapImage(new Uri(_portraitFile + i.ToString() + ".png"));
            i++;
            i %= 2;
        }
    }

}
