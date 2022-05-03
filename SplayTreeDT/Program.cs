using System;

namespace SplayTreeDT
{

    class Program
    {
        static void Main(string[] args)
        {
            Node<int> root = new Node<int>(20);
            root.left = new Node<int>(10);
            root.right = new Node<int>(30);
            root.left.left = new Node<int>(5);
            root.left.left.left = new Node<int>(3);
            root.left.left.left.left = new Node<int>(1);

            splayTreee<int> st = new splayTreee<int>();

            Console.WriteLine("\nbefore splay\n");
            st.PreOrder(root);

            Node<int> testNode = st.SearchSplay(root, 25);

            Console.WriteLine("\nAfter splay\n");
            st.PreOrder(testNode);

            //Testin the add method
            //st.Add(99);

            Console.WriteLine("\nAfter splay\n");
            //st.PreOrder(st.root);
            //Console.WriteLine(st.root);

        }
    }

    class Node<T>
    {
        public T value { get; set; }
        public Node<T> left { get; set; }
        public Node<T> right { get; set; }

        public Node(T t)
        {
            value = t;
        }
    }

    class splayTreee<T> where T : IComparable
    {
        public Node<T> root { get; set; }
        public int Count { get; private set; } //number of nodes

        public Node<T> RotateRight(Node<T> n) // rotates right side
        {
            Node<T> parent = n.left; // give the left value to the new node
            n.left = parent.right;  // change the traversal as left side to right side
            parent.right = n;   // left one moves to the right side
            return parent;
        }

        public Node<T> RotateLeft(Node<T> n) // rotates left side
        {
            Node<T> parent = n.right;
            n.right = parent.left;
            parent.left = n;
            return parent;
        }

        public Node<T> calculateSplay(Node<T> n, T newValue) // if there's no node or only root, return the first value
        {
            if (n == null || n.value.CompareTo(newValue) == 0)
            {
                return n;
            }

            if (n.value.CompareTo(newValue) > 0)
            {
                if (n.left == null)
                {
                    return n;
                }

                if (n.left.value.CompareTo(newValue) > 0) // zig zig
                {
                    n.left.left = calculateSplay(n.left.left, newValue); // recursive - n.left.left = root, keep doing

                    n = RotateRight(n);
                }
                else if (n.left.value.CompareTo( newValue) < 0)  // zig zag
                {
                    n.left.right = calculateSplay(n.left.right, newValue);

                    if (n.left.right != null)
                    {
                        n.left = RotateLeft(n);
                    }

                }

                return (n.left == null) ? n : RotateRight(n); // if it has not child on left, it returns the target or moves right again for second position
            }
            else
            {
                if (n.right == null)
                {
                    return n;
                }

                if (n.right.value.CompareTo(newValue) > 0) // zag zig
                {
                    n.right.left = calculateSplay(n.right.left, newValue);

                    if (n.right.left != null)
                    {
                        n.right = RotateRight(n.right);
                    }
                }
                else if (n.right.value.CompareTo(newValue)<0) // zag zag
                {
                    n.right.right = calculateSplay(n.right.right, newValue);

                    n = RotateLeft(n);
                }

                return (n.right == null) ? n : RotateLeft(n);
            }
        }
        

        //Search Method for the Spllay Tree
        public Node<T> SearchSplay(Node<T> n, T t) //O(logn)
        {
            return calculateSplay(n, t);
        }

        //preorder of splay tree
        public void PreOrder(Node<T> n)
        {
            if (n != null) // until it has no child
            {
                Console.Write(n.value + "   "); // Print
                PreOrder(n.left); // left
                PreOrder(n.right); // right
            }
        }

        //Print preorder of splay tree
        public void printPreorder()
        {
            PreOrder(root);
        }


        //Add mthod for spaly
        public void Add(T newValue) //O(logn)
        {
            //step 1: create a new node
            Node<T> newNode = new Node<T>(newValue);
            Count++;

            //step2: link in the new node
            if (IsEmpty())
                root = newNode;
            else
            {
                //traverse to the point where to link the newNode to the tree
                Node<T> finger = root;

                while (finger != null) //does the traversal
                {
                    if (newValue.CompareTo(finger.value) <= 0) //<= move left
                    {
                        //is there a left?
                        if (finger.left != null)  //if there is  left
                            finger = finger.left;   //move left
                        else  //there is no left - add new node to the left
                        {
                            finger.left = newNode;//link the new node to the left
                            break;
                        }
                    }
                    else //> move right
                    {
                        //is there a right?
                        if (finger.right != null)  //if there is  right
                            finger = finger.right;   //move right
                        else  //there is no right - add new node to the right
                        {
                            finger.right = newNode;//link the new node to the left
                            break;
                        }
                    }
                }
            }

            //after adding do the splaying
            calculateSplay(root, newValue);
        }

        //check empty
        public bool IsEmpty()
        {
            return root == null;
        }


    }
}
