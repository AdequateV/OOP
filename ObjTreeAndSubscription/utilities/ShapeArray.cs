using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab8_oop;

namespace Lab8_oop.utilities
{

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

    internal class ShapeArray
    {
        private List<IShape> shapes = new();
        public List<IShape> GetShapes()
        {
            return shapes;
        }
        public void LoadShapes(string filename, ShapeAfactory factory)
        {
            StreamReader streamReader = null;
            int count;
            IShape shape;
            try
            {
                streamReader = new StreamReader(filename);
                count = int.Parse(streamReader.ReadLine());
                for (int i = 0; i < count; i++)
                {
                    string code = streamReader.ReadLine();
                    shape = factory.CreateShape(code);

                    if (shape is shapeGroup group)
                    {
                        streamReader = LoadGroup(filename, streamReader, factory, group);
                        shapes.Add(shape);
                    }
                    else if (shape != null)
                    {
                        shape.Load(streamReader);
                        shapes.Add(shape);
                    }
                }
            }
            finally
            {
                streamReader?.Close();
            }
        }

        public StreamReader LoadGroup(string filename, StreamReader streamReader, ShapeAfactory factory, shapeGroup group)
        {
            int count = int.Parse(streamReader.ReadLine());
            IShape shape;
            for (int j = 0; j < count; j++)
            {
                string code = streamReader.ReadLine();
                shape = factory.CreateShape(code);
                if (shape is shapeGroup cGroup)
                {
                    streamReader = LoadGroup(filename, streamReader, factory, cGroup);
                }
                shape.Load(streamReader);
                group.AddShape(shape);
            }
            return streamReader;
        }
    }
}
