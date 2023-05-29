using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class ResizeCommand : ICommand
    {
        private IShape? _shape;
        public int _x, _y;
        public ResizeCommand(int x, int y)
        {
            _x = x;
            _y = y;
            _shape = null;
        }

        public override ICommand clone()
        {
            return new ResizeCommand(_x, _y);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute resize");
            _shape = shape;
            _shape?.resize(_x, _y);
        }

        public override void unexecute()
        {
            Console.WriteLine("unexec resize");
            _shape?.resize(-_x, -_y);
        }

        public override IShape GetShape() => _shape;
    }

}
