using System.Drawing.Drawing2D;

namespace Lab8_oop
{
    public class DungeonMaster
    {
        public Dictionary<IShape, bool> watchers { get; }
        public int width = 4;
        private GraphicsPath a;
        private Pen pen;
        public DungeonMaster() 
        {
            watchers = new();
            a = new();
            pen = new(Color.LightGoldenrodYellow, 1);
            a.AddEllipse(-width, -width, width * 2, width * 2);
            pen.CustomEndCap = new AdjustableArrowCap(width, width * 2, false);
            pen.CustomStartCap = new(null, a);
        }

        public void NotifyObservers(int x, int y)
        {
            foreach(var o in watchers)
            {
                if (!o.Value)
                {
                    watchers[o.Key] = true;
                    o.Key.move(x, y);
                    if (o.Key.IsOnEdge())
                        o.Key.move(-x, -y);
                }
            }
        }
        public void AddWatcher(IShape e)
        {
            watchers.Add(e, false);
        }
        public void RemoveWatcher(IShape e)
        {
            watchers.Remove(e);
        }
        public void Clear()
        {
            watchers.Clear();
        }
        public bool IsWatcher(IShape e)
        {
            return watchers.ContainsKey(e);
        }

        public void UpdateArrows(float stX, float stY, Graphics g)
        {
            foreach (var e in watchers)
            {                
                g.DrawLine(pen, stX, stY, e.Key.position.X, e.Key.position.Y);
            }
        }
        public void ToDefault()
        {
            foreach (var o in watchers)
                watchers[o.Key] = false;
        }
        ////////////////
        //public List<IShape> observers { get; } = new();
        //public List<bool> visited { get; } = new();

        //public void NotifyObservers(int x, int y)
        //{
        //    for (int i = 0; i < observers.Count; i++)
        //    {
        //        if (visited[i] == false)
        //        {
        //            visited[i] = true;
        //            observers[i].move(x, y);
        //            if (observers[i].IsOnEdge())
        //                observers[i].move(-x, -y);
        //        }
        //    }
        //}
        //public void AddListener(IShape e) 
        //{
        //    observers.Add(e);
        //    visited.Add(false);
        //}
        //public void RemoveWatcher(IShape e)
        //{
        //    observers.Remove(e);
        //}
        //public void Clear()
        //{
        //    observers.Clear();
        //    visited.Clear();
        //}
        //public bool IsWatcher(IShape e)
        //{
        //    return observers.Contains(e);
        //}
        //public void UpdateArrows(float stX, float stY, Graphics g)
        //{
        //    foreach (IShape e in observers)
        //    {
        //        Pen pen = new(Color.Black, 1.0f);
        //        pen.CustomEndCap = new AdjustableArrowCap(5, 10, false);
        //        g.DrawLine(pen, stX, stY, e.position.X, e.position.Y);
        //    }
        //}
        //internal void ToDefault()
        //{
        //    for (int i = 0; i < visited.Count; i++)
        //    {
        //        visited[i] = false;
        //    }
        //}
    }
}
