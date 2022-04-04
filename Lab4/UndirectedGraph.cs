using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace Lab4
{
	public class UndirectedGraph
	{
		public List<Node> Nodes { get; set; }

		public UndirectedGraph()
		{
			Nodes = new List<Node>();
		}

		public UndirectedGraph(string path)
		{
			Nodes = new List<Node>();

			List<string> lines = new List<string>();

			try
			{
				using (StreamReader streamReader = new StreamReader(path))
				{

					string line;
					while ((line = streamReader.ReadLine()) != null)
					{
						line = line.Trim();
						if (line == "" || line[0] == '#')
						{
							continue;
						}

						lines.Add(line);
					}
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return;
			}

			// process the lines

			if (lines.Count < 1)
			{
				Console.WriteLine("Empty graph file");
				return;
			}

			string[] nodeNames = Regex.Split(lines[0], @"\W+");
			foreach (var name in nodeNames)
			{
				Nodes.Add(new Node(name));
			}

			for (int i = 1; i < lines.Count; i++)
            {
				// extract the node names

				 nodeNames = Regex.Split(lines[i], @"\W+");
				if( nodeNames.Length < 2)
                {
					throw new Exception("Two nodes are required for each edge.");
                }

				// add edge between those nodes
				AddEdge(nodeNames[0], nodeNames[1]);
			}

		}

		public void AddEdge(string node1name, string node2name)
        {
			// find the 2 node objects
			Node node1 = FindNode(node1name);
			Node node2 = FindNode(node2name);

			if( node1 == null || node2 ==null)
            {
				throw new Exception("Invalid node name");
            }

			// add node2 as a neighbor to node 1
			node1.Neighbors.Add(node2);

			// add node 1 as a neighbor to node 2
			node2.Neighbors.Add(node1);

		}

		protected Node FindNode(string name)
        {
			var node = Nodes.Find(node => node.Name == name);

			return node;
        }

		public bool IsReachable(string node1name, string node2name)
        {
			// find the 2 node objects
			Node node1 = FindNode(node1name);
			Node node2 = FindNode(node2name);

			if (node1 == null || node2 == null)
			{
				throw new Exception("Invalid node name");
			}

			ResetNodeColor();

			var result = IsReachableExplore(node1, node2);

            Console.WriteLine("result: " + result);

			//return node2.Color != Color.White;

			ResetNodeColor();

			return result;
        }

		protected bool IsReachableExplore(Node currentNode, Node endingNode)
        {
			currentNode.Color = Color.Gray;

			if (currentNode==endingNode)
            {
				return true;
            }

			foreach (var neighbor in currentNode.Neighbors)
			{
				if(neighbor.Color == Color.White)
                {
					var result = IsReachableExplore(neighbor, endingNode);
					if (result)
                    {
						return result;
                    }
                }
			}

			currentNode.Color = Color.Black;

			return false;
		}


		// TODO
		/**
         * <summary> Returns the number of connecxted components in the graph .</summary>
         */
		public int ConnectedComponents
		{
			get
			{
				int connectedComponents = 0;

				foreach (Node node in Nodes)
                {
					if (node.Color == Color.White)
					{
						connectedComponents++;
						foreach (Node x in node.Neighbors)
							if (x.Color == Color.White)
							{
								x.Color = Color.Gray;
							}
						node.Color = Color.Black;
					}
				}

				// for all the nodes
				//     if node is white
				//        connectedComponents++
				//        explore the neighbors


				return connectedComponents;
			}
		}

		// TODO
		/**
		 *  <summary> Returns a predessor dictionary with the DFS paths starting DFS 
		 *  at the given starting node</summary>
		 */

		public Dictionary<Node, Node> DFS(Node startingNode)
		{
			Dictionary<Node, Node> predecessorDictionary = new Dictionary<Node, Node>();

			// foreach v in V do
			//    pred[v] = -1
			//    color[v] = white

			foreach (Node v in Nodes)
            {
				predecessorDictionary[v] = null;
				v.Color = Color.White;
			}
			foreach (Node u in Nodes)
            {
				DFSVisit(u, predecessorDictionary);
			}
			//    
			// dfsVisit(startingNode)

			return predecessorDictionary;
		}

		// TODO
		private void DFSVisit(Node node, Dictionary<Node, Node> pred)
		{
			node.Color = Color.Gray;
			foreach (Node v in node.Neighbors)
            {
				if (v.Color == Color.White)
                {
					pred[v] = node;
					DFSVisit(v, pred);
				}
            }
			node.Color = Color.Black;
			// color[node] = gray
			// foreach neighbor v of node
			//    if color[v] = white then
			//        pred[v] = node
			//        dfsVisit(v, pred)
            // color[node] = black


		}


		// TODO
		/**
		 *  <summary> Returns a predessor dictionary with the BFS paths starting BFS 
		 *  at the given starting node</summary>
		 */
		public Dictionary<Node, (Node pred, int dist)> BFS(Node startingNode)
		{
			var predecessorDictionary = new Dictionary<Node, (Node pred, int dist)>();
			Queue<Node> queue = new Queue<Node>();

			// init
			// foreach v in V do
			//    pred[v] = -1
			//    dist[v] = infinity
			//    color[v] = white
			foreach (Node node in Nodes)
            {
				predecessorDictionary[node] = (null, 0);
				node.Color = Color.White;
            }

			// startingNode.color = gray
			// dist[startingNode] = 0
			startingNode.Color = Color.Gray;
			predecessorDictionary[startingNode] = (null, 0);

			// queue = empty Queue
			// queuue.enqueue(startingNode)
			queue.Enqueue(startingNode);

			// while( queue is not empty ) do
			//   u = head(Q)
			//   foreach neighbor v of u do
			//      if v.color = white then
			//        dist[v] = dist[u] + 1
			//        pred[v] = u
			//        color[v] = gray
			//        enqueue(v)
			//   queue.dequeue()
			//   color[u] = black

			while (queue.Count != 0)
            {
				Node u = queue.Peek();
				foreach(Node v in u.Neighbors)
                {
					if (v.Color == Color.White)
                    {
						(Node y, int pre) = predecessorDictionary[u];
						predecessorDictionary[v] = (u, pre);
						v.Color = Color.Gray;
						queue.Enqueue(v);
                    }
                }
				queue.Dequeue();
				u.Color = Color.Black;
            }

			
			return predecessorDictionary;
		}


		public void ResetNodeColor()
		{
			foreach(var node in Nodes)
            {
				node.Color = Color.White;
            }

		}

		public string ReadDFSDict(Dictionary<Node, Node> Nodes)
        {
			string str = "";
			foreach ((Node key, Node value) in Nodes) {
				if (key == null)
                {
					str += "none";
				}
				else
                {
					str += key.Name;
				}
				str += ": ";
				if (value == null)
				{
					str += "none";
				}
				else
				{
					str += value.Name;
				}
				str += Environment.NewLine;
			}

			return str;
        }



        public override string ToString()
        {
			string str="";


			foreach( Node node in Nodes)
            {
				str += node.Name;
				if (node.Neighbors.Count == 0)
                {
					str += " has no neighbors";
				}
				else
                {
					str += " has neighbors: ";

					foreach (Node neighbor in node.Neighbors)
					{
						str += neighbor.Name;
						str += ", ";
					}
					str = str.Remove(str.Length - 2);
				}
				str += ".";
				str += Environment.NewLine;

            }


            return str;
        }

    }
}

