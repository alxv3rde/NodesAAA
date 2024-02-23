using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.UserControls;

namespace WpfApp1.Algorithms
{
    internal class Traveler
    {
        private List<UCNode> _Nodes = MainWindow.GetNodes;
        private List<UCLine> _Lines = MainWindow.GetLines;
        private List<string> permutation = new List<string>();
        private string[] arr;
        private List<List<string>> res = new List<List<string>>();
        private bool[] used;
        private List<(List<UCLine>, double)> lines = new List<(List<UCLine>, double)>();
        private UCNode sNode;

        public Traveler(UCNode sNode)
        {
            this.sNode = sNode;
            int i = 0;
            used = new bool[_Nodes.Count - 1];
            arr = new string[_Nodes.Count - 1];
            foreach (var n in _Nodes.FindAll(x => x != sNode))
            {
                used[i] = false;
                arr[i] = n.GetName;
                i++;
            }
            Permute();
        }
        public double Run()
        {
            var uCLinesMin = lines.Find(x=>x.Item2 == lines.Min(y=>y.Item2));
            foreach (var item in _Lines)
            {
                if (uCLinesMin.Item1.Contains(item))
                {
                    item.ChangeToWayline();
                }
                else
                {
                    item.ChangeToWrongWay();
                }
            }
            //foreach (var l in uCLinesMin.Item1)
            //{
            //    l.ChangeToWayline();
            //}
            return uCLinesMin.Item2;
        }
        private void Permute()
        {
            if (permutation.Count == arr.Length)
            {
                FindPermLines();
                res.Add(permutation.ToList());
            }
            for (int i = 0; i < arr.Length; i++)
            {
                if (!used[i])
                {
                    used[i] = true;
                    permutation.Add(arr[i]);
                    Permute();
                    used[i] = false;
                    permutation.Remove(arr[i]);
                }
            }
        }
        public static string ToString(List<string> s)
        {
            string str = "";
            foreach (var i in s)
            {
                str += i;
            }
            return str;
        }
        public static List<string> Reverse(List<string> s)
        {
            List<string> ret = new List<string>();
            for (int j = s.Count - 1; j >= 0; j--)
                ret.Add(s[j]);
            return ret;
        }
        private void FindPermLines()
        {
            List<UCLine> l = new List<UCLine>();
            double weight = 0;

            var firstConnection = _Lines.Find(x => x.ExistsConection(sNode, _Nodes.Find(y => y.GetName == permutation[0])));
            if (firstConnection != null)
            {
                l.Add(firstConnection);
                weight += firstConnection.GetLineValue;
                for (int i = 1; i < permutation.Count; i++)
                {
                    var connection = _Lines.Find(x => x.ExistsConection(_Nodes.Find(y => y.GetName == permutation[i - 1]), _Nodes.Find(y => y.GetName == permutation[i])));
                    if (connection != null)
                    {
                        l.Add(connection);
                        weight += connection.GetLineValue;
                    }
                    else
                    {
                        return;
                    }
                }
                var lastConnection = _Lines.Find(x => x.ExistsConection(_Nodes.Find(y => y.GetName == permutation[permutation.Count - 1]), sNode));
                if (lastConnection != null)
                {
                    l.Add(lastConnection);
                    weight += lastConnection.GetLineValue;
                }
                else
                {
                    return;
                }

                lines.Add((l, weight));
            }
        }
    }
}
