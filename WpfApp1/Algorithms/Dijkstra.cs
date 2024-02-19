using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using WpfApp1.UserControls;
using WpfApp1.Enums;
using System.Data;

namespace WpfApp1.Algorithms.Dijkstra
{
    internal class Dijkstra
    {
        public Dijkstra()
        {

        }
        public static int? Run(UCNode initNode, UCNode finalNode)
        {
            List<UCNode> _Nodes = MainWindow.GetNodes;
            List<UCNode> way = new List<UCNode>();
            UCNode? actualNode;
            int? value = 0;
            do
            {
                if (_Nodes.Exists(x => x == initNode && x.VFinal == null)) actualNode = _Nodes.Find(x => x == initNode && x.VFinal == null);
                else actualNode = _Nodes.Find(x => x.VFinal == null && x.VTemp == _Nodes.Min(y => y.VTemp));

                actualNode.VFinal = actualNode == initNode ? 0 : actualNode.VTemp;
                actualNode.VTemp = null;

                //------------------------------------------------------------------
                if (!_Nodes.Any(x => x.VFinal == null))
                {
                    way.Add(finalNode);
                    List<UCNode> nodosAd = new List<UCNode>();
                    while (way.Last() != initNode)
                    {
                        nodosAd = way.Last().nodes.FindAll(x => way.Last().VFinal - x.GetLines.Find(y => y.ExistsConection(x, way.Last())).GetLineValue == x.VFinal && !way.Contains(x));
                        if (nodosAd.Any())
                        {
                            UCLine line = way.Last().GetLines.Find(x => x.ExistsConection(nodosAd[0], way.Last()));
                            line.ChangeToWayline();
                            way.Add(nodosAd[0]);
                        }
                    }
                    value = finalNode.VFinal;
                    break;
                }
                //------------------------------------------------------------------

                foreach (var n in actualNode.nodes.FindAll(x => x.VFinal == null))
                {
                    UCLine line = n.GetLines.Find(x => x.ExistsConection(n, actualNode));
                    if ((n.VTemp > actualNode.VFinal + line.GetLineValue) || n.VTemp == null)
                        n.VTemp = actualNode.VFinal + line.GetLineValue;
                }
            } while (true);
            return value;
        }
    }
}
