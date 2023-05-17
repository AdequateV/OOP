using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_oop
{
    internal abstract class ShapeAfactory
    {
        public abstract IShape CreateShape(string code);
    }

    internal class wadimShapeFactory : ShapeAfactory
    {
        public override IShape CreateShape(string code)
        {
            IShape shape = null;
            shape = code switch
            {
                "Square" => new square(0, 0),
                "Circle" => new circle(0, 0),
                "Triangle" => new triangle(0, 0),
                "Section" => new square(0, 0, 1),
                "Group" => new shapeGroup(),
                _ => new circle(0, 0),
            };
            return shape;
        }
    }
}
