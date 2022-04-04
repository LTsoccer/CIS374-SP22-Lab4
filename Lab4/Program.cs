using System;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph1 = new UndirectedGraph("../../../graphs/graph1.txt");


            Console.WriteLine(graph1);


            Console.WriteLine(graph1.ConnectedComponents);
            Console.WriteLine(graph1.ReadDFSDict(graph1.DFS(graph1.Nodes[0])));
            Console.WriteLine(graph1.ReadBFSDict(graph1.BFS(graph1.Nodes[0])));


        }
    }
}

