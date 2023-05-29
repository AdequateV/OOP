using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop.scommands
{
    public abstract class ICommand
    {
        public abstract void execute(IShape shape);
        public abstract void unexecute();
        public abstract ICommand clone();
        public abstract IShape GetShape();

    }

}