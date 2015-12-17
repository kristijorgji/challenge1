/* @Kristi Jorgji */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ja
{
    class Program
    {
        /* class which will hold all the information needed about a node to build the json string such as children, brothers (nodes on same level) and parent */
        class Node : IEquatable<Node>
        {
            string name;
            Node parent;
            Node rightBrother, leftBrother;
            List<Node> children;

            public Node(string name, Node parent = null) 
            {
                this.name = name;
                this.parent = parent;
                children = new List<Node>();
            }

            public void addChild(Node child)
            {
                children.Add(child);
            }

            public void setChild(Node child, int position)
            {
                children[position] = child;
            }

            public void setBrothers(Node leftBrother, Node rightBrother)
            {
                this.leftBrother = leftBrother;
                this.rightBrother = rightBrother;
            }

            public void deleteChild(int position)
            {
                children.RemoveAt(position);
            }

            public void setParent(Node parent)
            {
                this.parent = parent;
            }

            public List<Node> getChildren()
            {
                return children;
            }

            public Node getRightBrother()
            {
                return rightBrother;
            }

            public Node getLeftBrother()
            {
                return leftBrother;
            }
            public Node getParent()
            {
                return parent;
            }

            public string getName()
            {
                return name;
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Node objAsNode = obj as Node;
                if (objAsNode == null) return false;
                else return Equals(objAsNode);
            }

            public bool Equals(Node other)
            {
                if (other == null) return false;
                return (this.name == other.name && this.parent.getName() == other.getParent().getName());
            }

            public override int GetHashCode()
            {
                return name.GetHashCode() + parent.getName().GetHashCode();              
            }
          
        }
        
        static void Main(string[] args)
        {    
            string[] input = System.IO.File.ReadAllLines("input.txt");
            Node root = new Node("root"); /* add a root node to link all the beggining nodes to this */
            buildGraph(input,  root); /* link all nodes with each other, and the beggining nodes with the root */
            transformGraph(root); /* attach same nodes to same point, thus avoiding 2 lines starting with same nodes seqeuence */
            printAllNodes(root);    /* print only in debug mode to check transformed graph */
            linkBrothers(root);     /* link up and down nodes corrensponding to the same level, right means down, left up */
            StringBuilder r = new StringBuilder("{" + Environment.NewLine);
            //StringBuilder r = new StringBuilder();
            r.Append(parseToJson(root, 2, root));   /* traverse the graph and get the json string */
            r.Append("}");
            System.IO.File.WriteAllText("out.txt", r.ToString());
        }

        static StringBuilder parseToJson(Node root, int spaces, Node croot)
        {
            StringBuilder result = new StringBuilder();
            List<Node> children = root.getChildren();
            
            if (children.Count == 0)
            {
                if (root.getRightBrother() != null)
                {
                    result.Append(root.getName().PadLeft(spaces) + ": \"\"" + Environment.NewLine + ",".PadLeft(spaces) + Environment.NewLine);
                }
                else
                {
                    //backtrack to last parent thas has right brother
                    int b = 0;
                    bool reachedStartRoot = false;
                    Node p = root;
                    do
                    {
                        p = p.getParent();
                        if (p == null)
                        { 
                            reachedStartRoot = true; 
                            break; 
                        }
                        b++;
                        if (p.getRightBrother() != null)
                            break;
                        
                    } while (true);

                    if (reachedStartRoot) //this is the final node of the entire graph
                    {
                        result.Append(root.getName().PadLeft(spaces) + ": \"\"");
                        for (int x = 0; x < b-1; x++)
                        {
                            result.Append(Environment.NewLine + "}".PadLeft(spaces - x));
                        }
                        result.Append(Environment.NewLine);
                    }
                    else
                    {
                        result.Append(root.getName().PadLeft(spaces) + ": \"\"");
                        int x = 0;
                        for (x = 0; x < b-1; x++)
                        {
                            result.Append(Environment.NewLine + "}".PadLeft(spaces - x));
                        }
                        result.Append(Environment.NewLine + "},".PadLeft(spaces - x) + Environment.NewLine);
                    }
                }
            }
            else
            {
                if (root != croot) result.Append(root.getName().PadLeft(spaces) + ": {" + Environment.NewLine);
                for (int i = 0; i < children.Count; i++)
                {
                    //Debug.Assert(children[i].getName() != "d");
                    result.Append(parseToJson(children[i], spaces + 2, croot));
                }
            }

            return result;
        }


        static void linkBrothers(Node root)
        {
            if (root == null)
                return;

            List<Node> children = root.getChildren();
            if (children.Count == 1)
                linkBrothers(children[0]);
            if (children.Count == 1)
                return;

            for (int i = 0; i < children.Count; i++)
            {
                if (i == 0)
                {
                    children[i].setBrothers(null, children[i + 1]);
                }
                else if (i != children.Count - 1)
                {
                    children[i].setBrothers(children[i - 1], children[i + 1]);
                }
                else
                {
                    children[i].setBrothers(children[i - 1], null);
                }
            }

            foreach (Node n in children)
            {
                linkBrothers(n);
            }
        }
        
        static void transformGraph(Node root)
        {
            if (root == null)
                return;

            List<Node> children = root.getChildren();
            int changedLinks = 0;
            for (int i = 0; i < children.Count; i++)
            {
                //if (children[i] == null) continue;

                for (int j = 0; j < children.Count; j++)
                {
                    if (i == j || children[j] == null) continue; /* don't want to check if equal with himself or with deleted node */

                    if (children[i].Equals(children[j]))
                    {
                        /* attach to one link thus avoiding two starting nodes in different lines */
                        foreach (Node c in children[j].getChildren())
                        {
                            children[i].addChild(c);
                            c.setParent(children[i]);
                        }
                        //root.setChild(null, j);
                        root.deleteChild(j);
                        changedLinks++;
                        //printAllNodes(root);
                    }
                }   
            }
            
            foreach (Node n in root.getChildren())
            {
                transformGraph(n);
            }
        }

        static void buildGraph(string[] lines,  Node root)
        {
           foreach (string line in lines)
           {
               string[] nodesC = line.Split('/');
               int nrNodes = nodesC.Length;
               Node[] nodes = new Node[nrNodes];
              
               for (int i = 0; i < nrNodes; i++)
               {
                   nodes[i] = new Node(nodesC[i]);
               }

               nodes[0].setParent(root);
               root.addChild(nodes[0]);

               for (int i = 0; i < nrNodes - 1; i++)
               {
                   nodes[i].addChild(nodes[i + 1]);
                   nodes[i + 1].setParent(nodes[i]);
               }

               if (nrNodes != 1)
               {
                   nodes[nrNodes - 1].setParent(nodes[nrNodes - 2]);
               }
           }
        }

        [Conditional("DEBUG")]
        static void printAllNodes(Node root, int spaces = 0)
        {
            foreach(Node n in root.getChildren())
            {
                Console.Write(n.getName().PadLeft(spaces) + Environment.NewLine);
                printAllNodes(n, spaces + 4);
            }
        } 
    }
}
