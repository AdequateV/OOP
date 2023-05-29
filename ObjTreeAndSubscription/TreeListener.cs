namespace Lab8_oop
{
    public class TreeListener : IMaster, IListener
    {
        public TreeView treeView;
        private List<IListener> watchers = new();
        public TreeListener(TreeView t)
        {
            treeView = t;
        }
        public void AddListener(IListener obj)
        {
            watchers.Add(obj);
        }
        public void Notify()
        {
            foreach (IListener wa in watchers)
            {
                wa.OnMasterChanged(this);
            }
        }

        public void OnMasterChanged(IMaster obj)
        {
            treeView.Nodes.Clear();
            shapeVault tmp = (shapeVault)obj;
            foreach (IShape s in tmp)
            {
                var newNode = new TreeNode(s.SimpleName);
                if (s.IsSelected)
                {
                    newNode.BackColor = Globals.TreeColorA;
                    newNode.Expand();
                }
                else newNode.Collapse();
                if (s is shapeGroup) ProcessNode(newNode, s);
                treeView.Nodes.Add(newNode);
            }
        }
        public void ProcessNode(TreeNode tr, IShape elem)
        {
            if (elem is not shapeGroup) return;
            List<IShape> tmp = ((shapeGroup)elem).shapes;
            foreach(IShape s in tmp)
            {
                TreeNode newNode = new(s.SimpleName);
                if (s.IsSelected)
                {
                    newNode.BackColor = Globals.TreeColorB;
                    newNode.Expand();
                }
                else newNode.Collapse();
                ProcessNode(newNode, s);
                tr.Nodes.Add(newNode);
            }
        }
    }
}
