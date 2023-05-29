using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{

    public class GroupCommand : ICommand
    {
        public List<IShape> shapes;
        public IShape added;

        public GroupCommand(List<IShape> shapes, IShape added = null)
        {
            this.shapes = shapes;
            this.added = added;
        }

        public override ICommand clone()
        {
            return new GroupCommand(shapes);
        }

        public override void execute(IShape shape)
        {
            if (shape == null) return;
            added = shape;
            ((shapeGroup)added).shapes.Clear();
            foreach (var s in shapes)
                if (s.IsSelected) ((shapeGroup)added).AddShape(s);
            List<IShape> b = new();

            foreach (var s in shapes)
                if (s.IsSelected) b.Add(s);
            foreach (var s in b)
                shapes.Remove(s);
            if (((shapeGroup)added).shapes.Count > 0)
            {
                shapes.Add(added);
            }

        }

        public override IShape GetShape() => added;

        public override void unexecute()
        {
            List<IShape> grElems = new();
            ((shapeGroup)added).Ungroup(grElems);
            shapes.Remove(added);
            foreach (var s in grElems)
                shapes.Add(s);

        }
    }


}
