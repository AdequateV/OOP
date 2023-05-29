using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class DeleteShapeCommand : ICommand
    {
        private IShape? _shape;
        private List<IShape> _shapes, saveBuffer, deleteBuffer;
        public DeleteShapeCommand(List<IShape> shapes)
        {
            _shapes = shapes;
        }
        public override ICommand clone()
        {
            return new DeleteShapeCommand(_shapes);
        }

        public override void execute(IShape shape)
        {
            Console.WriteLine("execute delete shape cmd ");
            saveBuffer = new();
            deleteBuffer = new();
            foreach (IShape c in _shapes)
            {
                //saving all non-selected
                if (!c.IsSelected) saveBuffer.Add(c);
                else deleteBuffer.Add(c);
            }
            _shapes.Clear();

            foreach (IShape c in saveBuffer)
                _shapes.Add(c);
            Console.WriteLine("deleted");
        }

        public override IShape GetShape() => _shape;
        public override void unexecute()
        {
            Console.WriteLine("unexecute delete shape cmd ");
            foreach (IShape s in deleteBuffer)
                _shapes.Add(s);


        }
    }


}
