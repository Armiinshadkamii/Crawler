using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crawler.DataSets;
public class SingleList
{
    public string Parent;

    public string Node;

    public SingleList() { }

    public SingleList(string node, string parent)
    {
        Parent = parent;
        Node = node;
    }
}

