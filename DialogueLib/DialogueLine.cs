using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DialogueLib
{
    public class DialogueLine
    {
        private string _name;
        private string _text;
        private string _portraitFile;
        private int i = 0;
        public ImageBrush PortraitBrush = new ImageBrush();
        public string Name { get { return _name; } }
        public string Text { get { return _text; } }
        public DialogueLine(string name, string text, string portraitFile)
        {
            this._name = name;
            this._text = text;
            this._portraitFile = portraitFile;
            this.PortraitBrush.ImageSource = new BitmapImage(new Uri(portraitFile + "0"));
        }

        public void Update()
        {
            this.PortraitBrush.ImageSource = new BitmapImage(new Uri(_portraitFile + i.ToString()));
            i++;
            i %= 2;
        }
    }

}
