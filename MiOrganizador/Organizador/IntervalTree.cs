using System;
using System.Collections.Generic;


namespace Organizador
{

    public enum Color 
    {
        Red,
        Black
    }

    public class IntervalTree<T,R> where T : IComparable<T>, IIntervaleable<R>, new() where R: IComparable
    {
        #region Constructors
        public IntervalTree(T key, Color color, IntervalTree<T,R> parent, IntervalTree<T,R> left_child, IntervalTree<T,R> right_child)
        {
            this.key = key;
            this.color = color;
            if (parent != null)
            {
                this.parent = (IntervalTree<T,R>)parent.MemberwiseClone();
            }
            if (left_child != null)
            {

                if (left_child.isNILT)
                {
                    left_child.parent = this;
                }
                this.left_child = (IntervalTree<T,R>)left_child.MemberwiseClone();
            }
            if (right_child != null)
            {

                if (right_child.isNILT)
                {
                    right_child.parent = this;
                }
                this.right_child = (IntervalTree<T,R>)right_child.MemberwiseClone();
            }

        }

        /// <summary>
        /// use this constructor for root
        /// </summary>
        /// <param name="key"></param>
        /// <param name="color"></param>
        public IntervalTree(T key, Color color)
        {
            this.key = key;
            this.color = color;
            this.isNILT = false;

            this.parent = new IntervalTree<T,R>(null);
            this.left_child = new IntervalTree<T,R>(this);
            this.right_child = new IntervalTree<T,R>(this);

            this.maxInterval = GetMax();
        }

        /// <summary>
        /// use this constructor only for NIL trees
        /// </summary>
        public IntervalTree(IntervalTree<T, R> parent)
        {
            this.key = new T();
            this.color = Color.Black;
            this.parent = parent;
            this.isNILT = true;
        }

        #endregion

        #region Fields

        protected T key;
        protected Color color;
        protected IntervalTree<T,R> parent, left_child, right_child;
        protected bool isNILT;
        #endregion

        #region Properties

        public T Key
        {
            get { return this.key; }
            set { this.key = value; }
        }

        public Color MyColor
        {
            get { return this.color; }
            set { this.color = value; }
        }

        public IntervalTree<T,R> RightChild
        {
            get { return this.right_child; }
            set { this.right_child = value; }
        }

        public IntervalTree<T,R> LeftChild
        {
            get { return this.left_child; }
            set { this.left_child = value; }
        }

        public IntervalTree<T,R> Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public bool IsNILT
        {
            get { return this.isNILT; }
            set
            {
                if (value)
                {
                    this.color = Color.Black;
                }
                this.isNILT = value;
            }
        }



        public bool IsLeaf
        {
            get
            {
                return this.right_child.IsNILT && this.left_child.IsNILT;
            }
        }


        private bool HasOneDescendet
        {
            get
            {
                //Para saber si tiene un solo hijo, le hago XOR
                return this.left_child.IsNILT ^ this.right_child.IsNILT;
            }
        }


        #endregion

        #region General Methods

        private void LinkWithUniqueDescendent()
        {
            if (!this.left_child.IsNILT)
            {
                CopyThisIntoMe(this.left_child);
            }
            else
            {
                CopyThisIntoMe(this.right_child);
            }
        }

        private void CopyThisIntoMe(IntervalTree<T,R> to_copy)
        {
            #region Before

            //copy all fields
            //este tipo esta modificando NILT!!!!!!!!

            //become a normal node before it changes
            //to avoid modifing NILT
            //this.left_child = (IntervalTree<T,R>)NILT.MemberwiseClone();
            //this.right_child = (IntervalTree<T,R>)NILT.MemberwiseClone();

            this.key = to_copy.key;
            this.color = to_copy.color;
            this.isNILT = to_copy.isNILT;
            //parent
            this.parent = to_copy.parent;
            //left
            if (this.left_child == null)
            {
                this.left_child = new IntervalTree<T,R>(this);
            }
            this.left_child = to_copy.left_child;
            //right
            if (this.right_child == null)
            {
                this.right_child = new IntervalTree<T,R>(this);
            }
            this.right_child = to_copy.right_child;

            #endregion

            if (this.key.CompareTo(this.parent.key) > 0)
            {
                this.parent.right_child = to_copy;
            }
            else
            {
                this.parent.left_child = to_copy;
            }
        }

        public static bool FindTree(T key, IntervalTree<T,R> root, ref IntervalTree<T,R> curr)
        {
            int result;
            curr = root;
            while (!curr.isNILT)
            {
                result = key.CompareTo(curr.key);
                if (result == 0)
                {
                    return true;
                }
                else if (result > 0)
                {
                    curr = curr.RightChild;
                }
                else
                {
                    curr = curr.LeftChild;
                }
            }
            return false;

        }

        public T Minimum()
        {
            if (this.left_child.IsNILT)
            {
                return this.Key;
            }
            return this.left_child.Minimum();
        }

        public T Maximum()
        {
            if (this.right_child.IsNILT)
            {
                return this.Key;
            }
            return this.right_child.Maximum();
        }

        public int Height()
        {
            if (this.IsLeaf) return 0;
            int left = 0, right = 0;
            if (!this.left_child.IsNILT)
            {
                left = this.left_child.Height();
            }
            if (!this.right_child.IsNILT)
            {
                right = this.right_child.Height();
            }
            return Math.Max(left, right) + 1;
        }

        public T Predecessor()
        {
            //Symmetric to Successor
            if (!this.left_child.IsNILT)
            {
                return this.left_child.Minimum();
            }
            IntervalTree<T,R> prnt = this.parent;
            T k = this.Key;
            while (!prnt.IsNILT && !prnt.left_child.IsNILT && k.Equals(prnt.left_child.Key))
            {
                k = prnt.Key;
                prnt = prnt.parent;
            }
            return prnt.Key;
        }

        public T Successor()
        {
            if (this.right_child.IsNILT)
            {
                return this.right_child.Minimum();
            }
            IntervalTree<T,R> prnt = this.parent;
            T k = this.Key;
            while (!prnt.IsNILT && !prnt.right_child.IsNILT && k.Equals(prnt.right_child.Key))
            {
                k = prnt.Key;
                prnt = prnt.parent;
            }
            return prnt.Key;
        }

        public IntervalTree<T,R> Clone()
        {
            IntervalTree<T,R> cloned = new IntervalTree<T,R>(default(T), Color.Black);
            if (this.isNILT)
            {
                return new IntervalTree<T,R>(this.parent);
            }
            cloned.key = this.key;
            cloned.color = this.color;
            //no need parent
            cloned.right_child = this.right_child.Clone();
            cloned.left_child = this.left_child.Clone();
            return cloned;
        }

        public IntervalTree<T,R> TreeMaximum()
        {
            if (this.right_child.isNILT)
            {
                return this;
            }
            return this.right_child.TreeMaximum();
        }

        public IntervalTree<T,R> TreeMinimum()
        {
            if (this.left_child.isNILT)
            {
                return this;
            }
            return this.left_child.TreeMinimum();
        }

        public IEnumerable<T> Inorder()
        {
            if (!this.left_child.IsNILT)
            {
                foreach (T k in this.left_child.Inorder())
                {
                    yield return k;
                }
            }
            yield return this.Key;
            if (!this.right_child.IsNILT)
            {
                foreach (T k in this.right_child.Inorder())
                {
                    yield return k;
                }
            }
        }

        public void Print()
        {
            foreach (T item in Inorder())
            {
                Console.Write(" {0}", item);
            }
            Console.WriteLine();
        }
        
        #endregion

        #region RightLeftStuff
		private static bool IsLeftChild(IntervalTree<T,R> tree)
        {
            return tree.key.CompareTo(tree.parent.left_child.key) == 0;
        }

        private static bool IsRightChild(IntervalTree<T,R> tree)
        {
            return !IsLeftChild(tree);
        }

        public static void LeftRotate(ref IntervalTree<T,R> tree)
        {
            if (tree.right_child.isNILT)
            {
                throw new InvalidOperationException("Left Rotation with NILT right_child not permited");
            }
            //store temporarely the left child of the left child
            IntervalTree<T,R> stored = (IntervalTree<T,R>)tree.left_child.MemberwiseClone();
            bool i_am_left = tree.key.CompareTo(tree.parent.key) < 0;
            //keep stored's children parent unchanged
            if (!stored.isNILT)
            {
                stored.left_child.parent = stored;
                stored.right_child.parent = stored;
            }


            //asign my key to l_f's key
            tree.left_child.Key = tree.Key;
            //asgin my color to my l_f's color
            tree.left_child.MyColor = tree.MyColor;
            //now my key is my r_c's key
            tree.key = tree.right_child.key;

            //now my color is my r_c's color
            tree.MyColor = tree.right_child.MyColor;
            //
            tree.left_child.right_child = tree.right_child.left_child;
            tree.left_child.right_child.parent = tree.left_child;
            //
            tree.left_child.left_child = stored;
            tree.left_child.left_child.parent = tree.left_child;
            //NIL value
            tree.left_child.isNILT = tree.isNILT;
            tree.right_child = tree.right_child.right_child;
            tree.right_child.parent = tree;
            tree.left_child.parent = tree;
            //linking with parent
            if (i_am_left)
            {
                tree.parent.left_child = tree;
            }
            else
            {
                tree.parent.right_child = tree;
            }
            
        }

        public static void RightRotate(ref IntervalTree<T,R> tree)
        {
            if (tree.left_child.isNILT)
            {
                throw new InvalidOperationException("Right Rotation with NILT left_child not permited");
            }
            IntervalTree<T,R> stored = (IntervalTree<T,R>)tree.right_child.MemberwiseClone();

            bool i_am_left = tree.Equals(tree.parent.left_child);

            //keep stored's children parent unchanged
            if (!stored.isNILT)
            {
                stored.left_child.parent = stored;
                stored.right_child.parent = stored;
            }

            tree.right_child.key = tree.key;
            tree.right_child.color = tree.color;
            tree.color = tree.left_child.color;

            tree.key = tree.left_child.key;


            tree.right_child.right_child = stored;
            tree.right_child.right_child.parent = tree.right_child;

            tree.right_child.left_child = tree.left_child.right_child;
            tree.right_child.left_child.parent = tree.right_child;



            tree.right_child.isNILT = tree.isNILT;
            tree.left_child = tree.left_child.left_child;
            tree.left_child.parent = tree;
            tree.right_child.parent = tree;
            //linking with parent
            if (i_am_left)
            {
                tree.parent.left_child = tree;
            }
            else
            {
                tree.parent.right_child = tree;
            }
            
        } 
	    #endregion

        #region Override
        public override string ToString()
        {
            return string.Format("{0},{1}", this.Key, this.MyColor);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            IntervalTree<T,R> tr = obj as IntervalTree<T,R>;
            if (tr == null)
            {
                return false;
            }

            //casos sin entrar a comparar los hijos
            if (this.IsNILT && !tr.IsNILT)
            {
                return false;
            }
            else if (!this.IsNILT && tr.IsNILT)
            {
                return false;
            }
            else if (this.IsNILT && tr.IsNILT)
            {
                return true;
            }

            //comparo primero los valores y las llaves y despues recursivo los hijos
            //si ninguno de los hijos es null, entro recursivo por ellos

            if (!this.left_child.IsNILT && !tr.left_child.IsNILT && !this.right_child.IsNILT && !tr.right_child.IsNILT)
            {
                return this.ToString() == tr.ToString() && this.left_child.Equals(tr.left_child) && this.right_child.Equals(tr.right_child);
            }
            //en caso de que los hijos derechos sean null, comparo los izquierdos
            else if (!this.left_child.IsNILT && !tr.left_child.IsNILT && this.right_child.IsNILT && tr.right_child.IsNILT)
            {
                return this.ToString() == tr.ToString() && this.left_child.Equals(tr.left_child);
            }
            //en caso de que los hijos izquiedos sean null, comparo los derechos
            else if (this.left_child.IsNILT && tr.left_child.IsNILT && !this.right_child.IsNILT && !tr.right_child.IsNILT)
            {
                return this.ToString() == tr.ToString() && this.right_child.Equals(tr.right_child);
            }
            //en caso de que todos sean null, (estos nodos deben ser NIL)
            else if (this.left_child.IsNILT && tr.left_child.IsNILT && this.right_child.IsNILT && tr.right_child.IsNILT)
            {
                return this.ToString() == tr.ToString();
            }
            else
            {
                return false;
            }

        }
        #endregion
        
        #region InsertionStuff
		public static void InsertKey(T key, IntervalTree<T,R> tree)
        {
            IntervalTree<T,R> curr = new IntervalTree<T,R>(tree);

            if (FindTree(key, tree, ref curr))
            {
                return;
            }
            curr.key = key;
            curr.isNILT = false;
            curr.color = Color.Red;
            curr.left_child = new IntervalTree<T,R>(curr);
            curr.right_child = new IntervalTree<T,R>(curr);

            //link parent with curr
            if (key.CompareTo(curr.parent.key) > 0)
            {
                curr.parent.right_child = curr;
            }
            else
            {
                curr.parent.left_child = curr;
            }
            //Fix insertion violations
            FixUpInsert(ref curr);
            tree.maxInterval = tree.GetMax();
        }

        protected static void FixUpInsert(ref IntervalTree<T,R> tree)
        {
            if (tree.parent.color == Color.Red)
            {

                if (/*avoid null*/ tree.parent.parent.right_child == null)
                {
                    return;
                }

                if (IsRightChild(tree))
                {
                    //parent its also right child
                    if (IsRightChild(tree.parent))
                    {
                        //ajust this colors is independent of the next conditions
                        tree.parent.color = Color.Black;
                        tree.parent.parent.color = Color.Red;
                        //my uncle is red
                        if (tree.parent.parent.left_child.color == Color.Red)
                        {
                            //adjust uncle's color
                            tree.parent.parent.left_child.color = Color.Black;
                            //as in this statement there's no rotation that make my
                            //grandparent black, could be a violation in it
                            FixUpInsert(ref tree.parent.parent);
                        }
                        //if my uncle is black it does not change uncle's color
                        else LeftRotate(ref tree.parent.parent);

                    }
                    //i'm left child of a right child
                    else
                    {
                        //become a case of the previous if statement
                        LeftRotate(ref tree.parent);
                        //now my previos parent is the left child of my parent(which is me)
                        if (tree.left_child.color == Color.Red)
                        {
                            FixUpInsert(ref tree.left_child);
                        }
                        else FixUpInsert(ref tree.parent.left_child);
                    }

                }
                //I'm left child
                else
                {
                    //my parent is left child
                    if (IsLeftChild(tree.parent))
                    {
                        //ajust this colors is independent of the next conditions
                        tree.parent.color = Color.Black;
                        tree.parent.parent.color = Color.Red;
                        //my uncle is red
                        if (tree.parent.parent.right_child.color == Color.Red)
                        {
                            //adjust uncle's color before rotation
                            tree.parent.parent.right_child.color = Color.Black;
                            FixUpInsert(ref tree.parent.parent);
                        }
                        else RightRotate(ref tree.parent.parent);


                    }
                    else
                    {
                        RightRotate(ref tree.parent);
                        //symmetric
                        if (tree.right_child.color == Color.Red)
                        {
                            FixUpInsert(ref tree.right_child);
                        }
                        else FixUpInsert(ref tree.parent.right_child);
                    }
                }
                //root must be black
                if (/*avoid null*/!tree.parent.parent.isNILT && tree.parent.parent.parent.isNILT)
                {
                    tree.parent.parent.color = Color.Black;
                }

            }



        } 
	    #endregion
                
        #region DeletionStuff
		
        public static void DeleteKey(T key, ref IntervalTree<T,R> tree)
        {
            IntervalTree<T,R> curr = new IntervalTree<T,R>(null);
            FindTree(key, tree, ref curr);
            if (curr.isNILT)
            {
                return;
            }
            if (curr.IsLeaf)//A red node is leaf
            {
                if (IsLeftChild(curr))
                {
                    curr.parent.left_child = new IntervalTree<T,R>(curr.parent);//remove links with parent

                }
                else//symmetric
                {
                    curr.parent.right_child = new IntervalTree<T,R>(curr.parent);

                }
                if (curr.color == Color.Black)
                {
                    try
                    {
                        FixUpDelete(ref curr.parent);
                    }
                    catch (Exception)
                    {

                    }


                }


            }
            else
            {
                if (curr.left_child.isNILT)
                {
                    IntervalTree<T,R> suc = curr.right_child.TreeMinimum();
                    curr.key = suc.key;
                    DeleteKey(suc.key, ref suc);
                }
                else
                {
                    IntervalTree<T,R> suc = curr.left_child.TreeMaximum();
                    curr.key = suc.key;
                    DeleteKey(suc.key, ref suc);
                }
            }
            tree.maxInterval = tree.GetMax();
        }

        private static void FixUpDelete(ref IntervalTree<T,R> tree)
        {

            //2 easy cases

            if (tree.color == Color.Red)
            {
                //then tree has a child who is a black leaf(not  necessarily)
                //tree.color = Color.Black;
                if (tree.left_child.isNILT)
                {
                    if (tree.right_child.IsLeaf)
                    {
                        tree.color = Color.Black;
                        tree.right_child.color = Color.Red;
                        return;
                    }

                    else
                    {
                        if (tree.right_child.right_child.isNILT)
                        {
                            tree.right_child.left_child.color = Color.Black;
                            tree.right_child.color = Color.Red;
                            RightRotate(ref tree.right_child);
                            LeftRotate(ref tree);
                        }
                        else if (tree.right_child.left_child.isNILT)
                        {
                            LeftRotate(ref tree);
                        }
                        else
                        {
                            tree.color = Color.Black;
                            tree.right_child.color = Color.Red;
                            tree.right_child.right_child.color = Color.Black;
                            LeftRotate(ref tree);
                        }
                        return;
                    }


                }
                else if (tree.right_child.isNILT)
                {
                    if (tree.left_child.IsLeaf)
                    {
                        tree.color = Color.Black;
                        tree.left_child.color = Color.Red;
                        return;

                    }

                    else
                    {
                        tree.color = Color.Red;
                        if (tree.left_child.right_child.isNILT)
                        {
                            //tree.left_child.left_child.color = Color.Black;
                            //tree.left_child.color = Color.Red;
                            RightRotate(ref tree);
                            //LeftRotate(ref tree);

                        }
                        else if (tree.left_child.left_child.isNILT)
                        {
                            tree.left_child.color = Color.Red;
                            tree.left_child.right_child.color = Color.Black;
                            LeftRotate(ref tree.left_child);
                            RightRotate(ref tree);

                        }
                        else
                        {
                            tree.color = Color.Black;
                            tree.left_child.color = Color.Red;
                            tree.left_child.left_child.color = Color.Black;
                            RightRotate(ref tree);

                        }
                        return;
                    }

                }

            }
            else
            {
                if (tree.left_child.isNILT)
                {
                    if (tree.right_child.color == Color.Red)
                    {
                        //sibling is red
                        tree.color = Color.Red;
                        tree.right_child.color = Color.Black;
                        LeftRotate(ref tree);
                        LeftRotate(ref tree.left_child);
                        return;
                    }
                }
                else
                {
                    if (tree.left_child.color == Color.Red)
                    {
                        //a
                        tree.color = Color.Red;
                        tree.left_child.color = Color.Black;
                        RightRotate(ref tree);
                        RightRotate(ref tree.right_child);
                        return;
                    }
                }


            }


            FourToughCases(ref tree);

        }

        private static void Rep(ref IntervalTree<T,R> tree)
        {
            if (tree.right_child.isNILT)
            {
                if (tree.left_child.IsLeaf)
                {
                    tree.left_child.color = Color.Red;
                }
                else
                {
                    if (tree.left_child.left_child.isNILT)
                    {
                        tree.color = Color.Red;
                        tree.left_child.right_child.color = Color.Black;
                        LeftRotate(ref tree.left_child);
                        RightRotate(ref tree);
                    }
                    else if (tree.left_child.right_child.isNILT)
                    {
                        tree.color = Color.Red;
                        RightRotate(ref tree);
                    }
                    else
                    {
                        tree.left_child.left_child.color = Color.Black;
                        RightRotate(ref tree);
                    }
                }

            }
            else
            {
                if (tree.right_child.IsLeaf)
                {
                    tree.right_child.color = Color.Red;
                }
                else
                {
                    if (tree.right_child.right_child.isNILT)
                    {
                        tree.color = Color.Red;
                        tree.right_child.left_child.color = Color.Black;
                        tree.right_child.color = Color.Red;
                        RightRotate(ref tree.right_child);
                        LeftRotate(ref tree);
                    }
                    else if (tree.right_child.left_child.isNILT)
                    {
                        tree.color = Color.Red;
                        LeftRotate(ref tree);
                    }
                    else
                    {
                        tree.right_child.right_child.color = Color.Black;
                        LeftRotate(ref tree);
                    }

                }

            }
        }

        private static void FourToughCases(ref IntervalTree<T,R> tree)
        {
            if (tree.parent.isNILT)
            {
                Rep(ref tree);
                return;
            }
            //four tough cases
            if (IsLeftChild(tree))
            {
                //my parent's sibling is black
                if (tree.parent.right_child.color == Color.Black)
                {
                    //my parent's sibling right_child is red
                    if (tree.parent.right_child.right_child.color == Color.Red)
                    {
                        //Case 4
                        tree.parent.right_child.color = tree.parent.parent.color;
                        tree.parent.color = Color.Black;
                        tree.parent.right_child.right_child.color = Color.Black;
                        LeftRotate(ref tree.parent);
                        Rep(ref tree.left_child);

                    }
                    //my parent's sibling right_child is black and my parent's sibling left_child is red
                    else if (tree.parent.right_child.right_child.color == Color.Black && tree.parent.right_child.left_child.color == Color.Red)
                    {
                        //Case3
                        tree.parent.right_child.color = Color.Red;
                        tree.parent.right_child.left_child.color = Color.Black;
                        RightRotate(ref tree.parent.right_child);
                        FixUpDelete(ref tree);

                    }
                    //my parent's sibling right_child is black and my parent's sibling left_child is black
                    else if (tree.parent.right_child.color == Color.Black && tree.parent.right_child.left_child.color == Color.Black)
                    {
                        //Case 2
                        Case2(ref tree);
                    }

                }
                else
                {

                    //Case 1
                    tree.parent.right_child.color = Color.Black;
                    tree.parent.color = Color.Red;
                    LeftRotate(ref tree.parent);
                    FixUpDelete(ref tree.left_child);//supongo que aqui se quede mi padre******
                }

            }
            //Symmetric
            else
            {
                if (tree.parent.left_child.color == Color.Black)
                {
                    if (tree.parent.left_child.right_child.color == Color.Red)
                    {
                        //Case 4
                        tree.parent.left_child.color = tree.parent.color;
                        tree.parent.color = Color.Black;
                        tree.parent.left_child.right_child.color = Color.Black;
                        RightRotate(ref tree.parent);
                        Rep(ref tree.right_child);

                    }
                    else if (tree.parent.left_child.right_child.color == Color.Black && tree.parent.left_child.left_child.color == Color.Red)
                    {
                        //Case3
                        tree.parent.left_child.color = Color.Red;
                        tree.parent.left_child.left_child.color = Color.Black;
                        LeftRotate(ref tree.parent.left_child);
                        FixUpDelete(ref tree);
                    }
                    else if (tree.parent.left_child.right_child.color == Color.Black && tree.parent.left_child.left_child.color == Color.Black)
                    {
                        //Case 2
                        Case2(ref tree);

                    }

                }
                else
                {
                    //Case 1
                    tree.parent.left_child.color = Color.Black;
                    tree.parent.color = Color.Red;
                    RightRotate(ref tree.parent); //***
                    FixUpDelete(ref tree.right_child);//supongo que aqui se quede mi padre****** 
                }
            }
        }

        private static void Case2(ref IntervalTree<T,R> tree)
        {
            //case grandparent is red (2a)
            if (tree.parent.color == Color.Red)
            {
                tree.parent.color = Color.Red;
            }
            //2b
            else
            {
                if (IsLeftChild(tree))
                {
                    tree.parent.right_child.color = Color.Red;

                }
                else
                {
                    tree.parent.left_child.color = Color.Red;
                }
                if (tree.parent.parent.isNILT)
                {
                    Rep(ref tree);
                }
                else FixUpDelete(ref tree.parent);
            }
        } 
	    
        #endregion

        #region IntervalStuff
        private R GetMax()
        {
            if (this.left_child.isNILT && this.right_child.isNILT)
            {
                return this.key.End;
            }
            else if (this.right_child.isNILT)
            {
                return Max(this.key.End, this.left_child.GetMax());
            }
            else if(this.left_child.isNILT)
            {
                return Max(this.key.End, this.right_child.GetMax());
            }
            return Max(key.End, this.left_child.GetMax(), this.right_child.GetMax());
        }

        private R Max(params R[] intervals)
        {
            Array.Sort(intervals);
            return intervals[intervals.Length-1];
        }

        R maxInterval;
        public R MaxInterval
        {
            get{return this.maxInterval;}
        }

        public IEnumerable<T> Overlap(T choosen) 
        {
            Queue<IntervalTree<T,R>> q = new Queue<IntervalTree<T,R>>();
            q.Enqueue(this);
            while (q.Count>0)
            {
                if (q.Peek().MaxInterval.CompareTo(choosen.Start)>0)
                {
                    if (!q.Peek().left_child.isNILT)
                    {
                        q.Enqueue(q.Peek().left_child);
                    }
                    if (!q.Peek().right_child.isNILT)
                    {
                        q.Enqueue(q.Peek().right_child);
                    }
                    if (q.Peek().key.Start.CompareTo(choosen.End)<0 && q.Peek().key.End.CompareTo(choosen.Start)>0)
                    {
                        yield return q.Dequeue().key;
                    }
                    
                    
                }
                if (q.Count>0)
                {
                    q.Dequeue();
                }
                
                
            }
        }

       

        #endregion
    }

    public interface IIntervaleable<R>
    {


        R Start
        {
            get;
            set;
        }

        R End
        {
            get;
            set;
        }


    }
}
