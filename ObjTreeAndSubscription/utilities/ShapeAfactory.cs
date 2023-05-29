using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop
{
    internal abstract class ShapeAfactory
    {
        public abstract IShape CreateShape(string code);
    }

}
