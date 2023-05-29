using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class RecolorCommand : ICommand
    {
        private IShape? _shape;
        private Color _oldColor;
        private Color _newColor;
        public RecolorCommand(Color c)
        {
            _newColor = c;
            _shape = null;
        }

        public override ICommand clone()
        {
            return new RecolorCommand(_newColor);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute recolor");
            _shape = shape;
            _oldColor = _shape.colorMain;
            _shape?.recolor(_newColor);
        }

        public override IShape GetShape() => _shape;

        public override void unexecute()
        {
            Console.WriteLine("unexecute recolor");
            _shape?.recolor(_oldColor);
        }
    }


}
