using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class UnGroupCommand : ICommand
    {
        public List<IShape> shapes;
        public IShape added;
        public UnGroupCommand(List<IShape> _selection)
        {
            shapes = _selection;
        }

        public override ICommand clone()
        {
            return new UnGroupCommand(shapes);
        }

        public override void execute(IShape shape)
        {
            added = shape;
            ICommand c = new GroupCommand(shapes, added);
            c.unexecute();
        }

        public override IShape GetShape() => added;


        public override void unexecute()
        {
            ICommand c = new GroupCommand(shapes);
            c.execute(added);
        }
    }


}
