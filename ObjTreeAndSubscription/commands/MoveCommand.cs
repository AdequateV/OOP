using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class MoveCommand : ICommand
    {
        private IShape? _shape;
        private int _x, _y;
        public MoveCommand(int x, int y)
        {
            _x = x;
            _y = y;
            _shape = null;
        }

        public override ICommand clone()
        {
            return new MoveCommand(_x, _y);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute move");
            _shape = shape;
            _shape?.move(_x, _y);
        }

        public override IShape GetShape() => _shape;


        public override void unexecute()
        {
            Console.WriteLine("unexec move");
            _shape?.move(-_x, -_y);
        }
    }


}
