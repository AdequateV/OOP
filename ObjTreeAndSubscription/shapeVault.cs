using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab8_oop
{
    public class shapeVault : List<IShape>, IMaster, IListener
    {
        private List<IListener> watchers = new();
        public shapeVault() : base()
        {

        }
        public shapeVault(List<IShape> s)
        {
            this.Clear();
            foreach(var item in s)
            {
                this.Add(item);
            }
        }
        public void AddListener(IListener obj)
        {
            watchers.Add(obj);
        }

        public void Notify()
        {
            foreach(IListener o in watchers)
            {
                o.OnMasterChanged(this);
            }
        }

        public void OnMasterChanged(IMaster obj)
        {
            DeselectAll();
            TreeListener tmp = (TreeListener)obj;
            for(int i = 0, j = 0; i < base.Count; i++,j++)
                if (tmp.treeView.Nodes[j].BackColor == Globals.TreeColorB)
                    base[i].IsSelected = true;
            Notify();
        }
        public void SelectShape(IShape s)
        {
            s.IsSelected = true;
            Notify();
        }
        public void DeselectShape(IShape s)
        {
            s.IsSelected = false;
            Notify();
        }
        public void DeselectAll()
        {
            foreach(IShape s in this)
            {
                s.IsSelected = false;
            }
        }
        bool WasRemoved = false;
        public new bool Remove(IShape o)
        {
            WasRemoved = base.Remove(o);
            o.dungeonMaster.Clear();
            o.watcher.Clear();
            foreach(IShape s in this)
                if(s.dungeonMaster.IsWatcher(o))
                    s.dungeonMaster.RemoveWatcher(o);
            o.watcher.Clear();
            if (WasRemoved) Notify();
            return WasRemoved;
        }
        public new void Add(IShape s)
        {
            base.Add(s);
            Notify();
        }

    }
}
