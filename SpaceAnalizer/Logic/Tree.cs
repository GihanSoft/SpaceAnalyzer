using System;
using System.Collections.Generic;

namespace Gihan.SpaceAnalizer.Logic
{
    public class TreeNode<TData>
    {
        protected readonly List<TreeNode<TData>> _children = new List<TreeNode<TData>>();

        public TData Data { get; set; }
        public TreeNode<TData> Parent { get; protected set; }
        public IReadOnlyCollection<TreeNode<TData>> Children => _children.AsReadOnly();

        public virtual TreeNode<TData> AddChild(TreeNode<TData> child)
        {
            if (child is null)
                throw new ArgumentNullException(nameof(child));
            if (child.Parent != null)
                throw new ArgumentException("child parent is not null", nameof(child));

            child.Parent = this;
            _children.Add(child);
            return child;
        }
        public virtual TreeNode<TData> AddChild(TData childData)
        {
            var node = new TreeNode<TData> { Data = childData };
            return AddChild(node);
        }

        public override string ToString()
        {
            return $"{Parent} > ({Data})";
        }
    }
}
