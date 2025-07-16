using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public enum DIR
    {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }
    public class SpriteMove : IAction
    {
        private ISpriteSheet _sprite;
        private int[] _numSteps;
        private DIR[] _dir;
        public int i = 0;
        public int stepSize;

        public ISpriteSheet Sprite { get { return _sprite; } }
        public int NumSteps { get { int steps = _numSteps[i]; _numSteps[i] -= stepSize; return steps; } }
        public DIR Dir { get { return _dir[i]; } }
        public int StepsLength { get { return _numSteps.Length; } }
        public ActionType Type { get { return ActionType.MOVE; } }
        public SpriteMove(ISpriteSheet sprite, int[] numSteps, DIR[] dir, int stepSize)
        {
            this._sprite = sprite;
            this._numSteps = numSteps;
            this._dir = dir;
            this.stepSize = stepSize;
        }

    }
}
