using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crawler.DataSets
{
    public class Tree
    {
        public string Parent;

        public string? Node;

        public HashSet<Tree> trees = new HashSet<Tree>();

        public Tree(string parent, string node)
        {
            Parent = parent;
            Node = node;
        }

        public Tree(string _parent, string _node, HashSet<Tree> linksTree)
        {
            Parent = _parent;
            Node = _node;
            trees = linksTree;
        }
    }
}
