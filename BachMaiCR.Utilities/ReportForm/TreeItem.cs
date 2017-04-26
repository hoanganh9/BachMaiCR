using System;
using System.Collections.Generic;

namespace BachMaiCR.Utilities.ReportForm
{
    [Serializable]
    public class TreeItem
    {
        private List<TreeItem> _children;

        public string Name { get; set; }

        public int Level { get; set; }

        public string ParentName { get; set; }

        public string ParentPath { get; set; }

        public List<TreeItem> Children
        {
            get
            {
                return this._children ?? (this._children = new List<TreeItem>());
            }
            set
            {
                this._children = value;
            }
        }

        public TreeItem()
        {
        }

        public TreeItem(string val, int level, string parentName = "", List<TreeItem> children = null)
        {
            this.Level = level;
            this.Name = val;
            this.Children = children;
            this.ParentName = parentName;
        }

        public void SetLevel(TreeItem tree, int level, ref int maxLevel)
        {
            if (tree == null)
                return;
            tree.Level = level;
            if (maxLevel < level)
                maxLevel = level;
            if (tree.Children == null)
                return;
            for (int index = 0; index < tree.Children.Count; ++index)
                this.SetLevel(tree.Children[index], level + 1, ref maxLevel);
        }
    }
}