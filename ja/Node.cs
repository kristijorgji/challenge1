using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ja
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
        public void deleteNullChildren()
        {
            children.RemoveAll(n => n == null);
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
}
