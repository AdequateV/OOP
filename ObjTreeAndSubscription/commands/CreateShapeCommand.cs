using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class CreateShapeCommand : ICommand
    {
        private IShape? _shape;
        private List<IShape> _shapes;
        public int _x, _y, _type, _index;
        public CreateShapeCommand(int x, int y, int type, List<IShape> shapes)
        {
            _x = x;
            _y = y;
            _type = type;
            _shapes = shapes;
        }
        public override ICommand clone()
        {
            return new CreateShapeCommand(_x, _y, _type, _shapes);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("createShapeCommand execute");
            _shape = shapeCreator.createShapeMethod(_x, _y, _type);
            _shape.IsSelected = true;
            _shapes.Add(_shape);

        }

        public override IShape GetShape() => _shape;
        public override void unexecute()
        {
            Console.WriteLine("createShapeCommand unexecute");
            _shapes.Remove(_shape);
            //_shapes.RemoveAt(_index);

        }
    }


}
