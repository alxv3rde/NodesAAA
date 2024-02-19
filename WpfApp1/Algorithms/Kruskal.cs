using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using WpfApp1.Enums;
using WpfApp1.UserControls;

namespace WpfApp1.Algorithms
{
    internal class Kruskal
    {
        private List<UCLine> _lines = MainWindow.GetLines.OrderBy(x => x.GetLineValue).ToList();
        private UCNode? _end;
        public int Run()
        {
            int sum = 0;
            foreach (var l in _lines)
            {
                _end = l.GetNodes[1];
                if (!FormsCycle(l.GetNodes[0]))
                {
                    sum += l.GetLineValue;
                    l.ChangeToWayline();
                }
                MainWindow.GetNodes.ForEach(x => x.DefaultValues());
            }
            return sum;
        }

        public bool FormsCycle(UCNode node)
        {
            node.Visited = true;
            foreach (var n in node.nodes.FindAll(x => !x.Visited && node.GetLines.Exists(y => y.ExistsConection(node, x) && y.IsWay)))
            {
                if (n == _end || FormsCycle(n))
                    return true;
            }
            return false;
        }
    }
}
