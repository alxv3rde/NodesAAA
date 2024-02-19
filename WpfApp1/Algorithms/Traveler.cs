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
        private List<(List<UCLine>, int)> lines = new List<(List<UCLine>, int)>();
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
        public int Run()
        {
            var uCLines = lines.Find(x=>x.Item2 == lines.Min(y=>y.Item2));
            foreach (var l in uCLines.Item1)
            {
                l.ChangeToWayline();
            }
            return uCLines.Item2;
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
            int weight = 0;

            // Verifica la conexión entre el nodo inicial y el primer nodo permutado
            var firstConnection = _Lines.Find(x => x.ExistsConection(sNode, _Nodes.Find(y => y.GetName == permutation[0])));
            if (firstConnection != null)
            {
                l.Add(firstConnection);
                weight += firstConnection.GetLineValue;

                // Verifica las conexiones entre nodos permutados
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
                        // Si no hay conexión, puedes manejar esto según tus necesidades (por ejemplo, lanzar una excepción, ignorar, etc.).
                        // Aquí simplemente detenemos el proceso y no agregamos la permutación.
                        return;
                    }
                }

                // Verifica la conexión entre el último nodo permutado y el nodo inicial
                var lastConnection = _Lines.Find(x => x.ExistsConection(_Nodes.Find(y => y.GetName == permutation[permutation.Count - 1]), sNode));
                if (lastConnection != null)
                {
                    l.Add(lastConnection);
                    weight += lastConnection.GetLineValue;
                }
                else
                {
                    // Similar al caso anterior, puedes manejar la falta de conexión según tus necesidades.
                    return;
                }

                lines.Add((l, weight));
            }
        }
    }
}
