using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab8_oop
{
    public class Listener
    {
        public List<IShape> masters { get; } = new();
        public void addMaster(IShape s)
        {
            masters.Add(s);
        }
        public void removeMaster(IShape s)
        {
            masters.Remove(s);
        }
        public bool IsMaster(IShape s)
        {
            return masters.Contains(s);
        }
        public void Clear()
        {
            masters.Clear();
        }
        public void changeArrows(Graphics e)
        {
            foreach (IShape s in masters)
            {
                s.dungeonMaster.UpdateArrows(s.position.X, s.position.Y, e);
            }
        }
    }
}
