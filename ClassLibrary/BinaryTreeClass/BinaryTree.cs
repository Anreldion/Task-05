using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.BinaryTreeClass
{
    /// <summary>
    /// Class describing the implementation of a binary search tree.
    /// </summary>
    /// <typeparam name="T"><see cref="Student"/></typeparam>
    public class BinaryTree<T> where T : Student
    {
        /// <summary>
        /// Root node <see cref="BinaryTreeNode{T}"/>
        /// </summary>
        private BinaryTreeNode<T> RootNode = null;
        /// <summary>
        /// The number of nodes in a binary tree. Private field for <see cref="Count"/>
        /// </summary>
        private int count_node = 0;
        /// <summary>
        /// The number of nodes in a binary tree.
        /// </summary>
        public int Count => count_node;
        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public BinaryTree()
        {
        }
        /// <summary>
        /// Clear binary tree.
        /// </summary>
        public void Clear()
        {
            RootNode = null;
            count_node = 0;
        }
        /// <summary>
        /// Recursive method for <see cref="Clear"/>
        /// </summary>
        /// <param name="node"><see cref="BinaryTreeNode{T}"/></param>
        private void Clear(BinaryTreeNode<T> node)
        {
            if (node == null)
            {
                return;
            }

            Clear(node.Left);
            Clear(node.Right);
            count_node--;
        }
        /// <summary>
        /// Create new node.
        /// </summary>
        /// <param name="data">Data.</param>
        /// <returns><see cref="BinaryTreeNode{T}"/></returns>
        private BinaryTreeNode<T> NewNode(T data)
        {
            count_node++;
            BinaryTreeNode<T> node = new BinaryTreeNode<T>(data)
            {
                Balancing = 0
            };
            return node;
        }
        /// <summary>
        /// Add data to a binary tree.
        /// </summary>
        /// <param name="data"></param>
        public void Add(T data)
        {
            if (RootNode == null)
            {
                RootNode = NewNode(data);
            }
            else
            {
                Insert(ref RootNode, data);
            }

        }
        /// <summary>
        /// Recursive method for <see cref="Add"/>
        /// </summary>
        /// <param name="root"><see cref="BinaryTreeNode{T}"/></param>
        /// <param name="data">Data item</param>
        /// <returns>true if an empty node is found, otherwise false</returns>
        private bool Insert(ref BinaryTreeNode<T> root, T data)
        {
            BinaryTreeNode<T> node = root;
            if (node == null)
            {
                root = NewNode(data);
                return true;
            }

            if (data.CompareTo(node.Data) == 0)
            {
                throw new ArgumentException("Data already exists!");
            }

            bool result;
            if (data.CompareTo(node.Data) < 0)
            {
                result = Insert(ref node.Left, data) && --node.Balancing != 0;
            }
            else
            {
                result = Insert(ref node.Right, data) && ++node.Balancing != 0;
            }
            if ((Balance(ref root) & BALANCE_ROTATE) == 1)
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Remove item from tree.
        /// </summary>
        /// <param name="data">Data item</param>
        public void Remove(T data)
        {
            Remove(ref RootNode, data);
        }
        /// <summary>
        /// Recursive method for <see cref="Remove"/>
        /// </summary>
        /// <param name="root"><see cref="BinaryTreeNode{T}"/></param>
        /// <param name="data">Data item</param>
        /// <returns></returns>
        private bool Remove(ref BinaryTreeNode<T> root, T data)
        {
            bool is_ok = false;
            BinaryTreeNode<T> node = root;
            if (node == null)
            {
                return false;
            }
            if (data.CompareTo(node.Data) > 0)
            {
                if (Remove(ref node.Right, data) && !Convert.ToBoolean(--node.Balancing))
                {
                    is_ok = true;
                }
            }
            else if (data.CompareTo(node.Data) < 0)
            {
                if (Remove(ref node.Left, data) && !Convert.ToBoolean(++node.Balancing))
                {
                    is_ok = true;
                }
            }
            else
            {
                if (node.Right == null)
                {
                    count_node--;
                    root = node.Left;
                    return true;
                }
                count_node--;
                is_ok = GetMin(ref node.Right, ref root);
                root.Right = node.Right;
                root.Left = node.Left;
                root.Balancing = node.Balancing;
                if (is_ok)
                {
                    is_ok = !Convert.ToBoolean(--node.Balancing);
                }
            }
            return (Balance(ref root, is_ok ? BALANCE_HIGHT_CHANGE : 0) & BALANCE_HIGHT_CHANGE) != 0;
        }
        /// <summary>
        /// Search smallest node.
        /// </summary>
        /// <param name="root_position"><see cref="BinaryTreeNode{T}"/></param>
        /// <param name="root"><see cref="BinaryTreeNode{T}"/></param>
        /// <returns></returns>
        private bool GetMin(ref BinaryTreeNode<T> root_position, ref BinaryTreeNode<T> root)
        {
            BinaryTreeNode<T> node = root_position;
            if (node.Left != null)
            {
                if (GetMin(ref node.Left, ref root) && !Convert.ToBoolean(++node.Balancing))
                {
                    return true;
                }
                return (Balance(ref root_position) & BALANCE_HIGHT_CHANGE) != 0;
            }
            root = node;
            root_position = node.Right;
            return true;
        }
        /// <summary>
        /// A rotation of a binary tree node occurred
        /// </summary>
        private const int BALANCE_ROTATE = 1;
        /// <summary>
        /// There was a change in the height of the balancing removal
        /// </summary>
        private const int BALANCE_HIGHT_CHANGE = 2;
        /// <summary>
        /// Balancing table for <see cref="TurnLeftToRight(ref BinaryTreeNode{T})"/ method>
        /// </summary>
        private static readonly int[,] left_to_right_table = new int[6, 4] { { -1, -1, +1, +1 }, { -1, +0, +1, +0 }, { -1, +1, +2, +0 }, { -2, -1, +0, +0 }, { -2, -2, +0, +1 }, { -2, +0, +1, -1 } };
        /// <summary>
        /// Balancing table for <see cref="TurnRightToLeft(ref BinaryTreeNode{T})"/ method>
        /// </summary>
        private static readonly int[,] right_to_left_table = new int[6, 4] { { +1, -1, -2, +0 }, { +1, +0, -1, +0 }, { +1, +1, -1, -1 }, { +2, +0, -1, +1 }, { +2, +1, +0, +0 }, { +2, +2, +0, -1 } };

        /// <summary>
        /// Balancing a binary tree.
        /// </summary>
        /// <param name="root"><see cref="BinaryTreeNode{T}"/></param>
        /// <param name="result">Balancing behavior</param>
        /// <returns>Balancing behavior</returns>
        private int Balance(ref BinaryTreeNode<T> root, int result = 0)
        {
            BinaryTreeNode<T> node = root;
            if (node.Balancing > 1)
            {
                result = BALANCE_ROTATE;
                if (node.Right.Balancing != 0)
                {
                    result |= BALANCE_HIGHT_CHANGE;
                }
                if (node.Right.Balancing < 0)
                {
                    node.Right = TurnLeftToRight(ref node.Right);
                }
                root = TurnRightToLeft(ref node);
            }
            if (node.Balancing < -1)
            {
                result = BALANCE_ROTATE;
                if (node.Left.Balancing != 0)
                {
                    result |= BALANCE_HIGHT_CHANGE;
                }
                if (node.Left.Balancing > 0)
                {
                    node.Left = TurnRightToLeft(ref node.Left);
                }
                root = TurnLeftToRight(ref node);
            }
            return result;
        }
        /// <summary>
        /// Check balance.
        /// </summary>
        /// <returns>true, otherwise false</returns>
        public bool CheckBalance()
        {
            if (CheckBalance(ref RootNode) < 0)
            {
                return false;
            }
            return true;

        }
        /// <summary>
        /// Recursive method for <see cref="CheckBalance"/>
        /// </summary>
        /// <param name="node"><see cref="BinaryTreeNode{T}"/></param>
        /// <returns></returns>
        private int CheckBalance(ref BinaryTreeNode<T> node)
        {
            int left, right;
            if (node == null)
            {
                return 0;
            }
            left = CheckBalance(ref node.Left);
            right = CheckBalance(ref node.Right);
            if (left < 0 || right < 0)
            {
                return -1;
            }
            if (node.Balancing != right - left)
            {
                return -1;
            }
            if (Math.Abs(node.Balancing) > 1)
            {
                return -1;
            }
            return Math.Max(left, right) + 1;
        }
        /// <summary>
        /// Rotate node from left to right.
        /// </summary>
        /// <param name="node"><see cref="BinaryTreeNode{T}"/></param>
        /// <returns><see cref="BinaryTreeNode{T}"/> item.</returns>
        private BinaryTreeNode<T> TurnLeftToRight(ref BinaryTreeNode<T> node)
        {
            BinaryTreeNode<T> left_node = node.Left;
            BinaryTreeNode<T> right_node = left_node.Right;
            left_node.Right = node;
            node.Left = right_node;
            for (int n = 0; n < 6; n++)
            {
                if (left_to_right_table[n, 0] == node.Balancing && left_to_right_table[n, 1] == left_node.Balancing)
                {
                    left_node.Balancing = left_to_right_table[n, 2];
                    node.Balancing = left_to_right_table[n, 3];
                    break;
                }
            }
            return left_node;
        }
        /// <summary>
        /// Rotate node from right to left.
        /// </summary>
        /// <param name="node"><see cref="BinaryTreeNode{T}"/></param>
        /// <returns><see cref="BinaryTreeNode{T}"/> item.</returns>
        private BinaryTreeNode<T> TurnRightToLeft(ref BinaryTreeNode<T> node)
        {
            BinaryTreeNode<T> right_node = node.Right;
            BinaryTreeNode<T> left_node = right_node.Left;
            right_node.Left = node;
            node.Right = left_node;
            for (int n = 0; n < 6; n++)
            {
                if (right_to_left_table[n, 0] == node.Balancing && right_to_left_table[n, 1] == right_node.Balancing)
                {
                    right_node.Balancing = right_to_left_table[n, 2];
                    node.Balancing = right_to_left_table[n, 3];
                    break;
                }
            }
            return right_node;
        }
        /// <summary>
        /// Checking if there is such data in the tree.
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>true if the value is contained in the tree. Otherwise, it returns false.</returns>
        public bool Contains(T data)
        {
            if (RootNode == null || data == null)
            {
                return false;
            }
            BinaryTreeNode<T> current_node = RootNode;
            while (current_node != null)
            {
                int result = data.CompareTo(current_node.Data);
                if (result == 0)
                {
                    return true;
                }
                if (result < 0)
                {
                    current_node = current_node.Left;
                }
                else
                {
                    current_node = current_node.Right;
                }
            }
            return false;
        }
        /// <summary>
        /// Convert <see cref="BinaryTree{T}"/> to <see cref="List{T}"/>
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="Student"/></returns>
        public List<T> ToList()
        {
            List<T> list = new List<T>();
            ToList(RootNode, list);
            list = list.OrderBy(n => n.TestScore).ToList();
            return list;
        }
        /// <summary>
        /// Recursive method for <see cref="ToList"/>
        /// </summary>
        /// <param name="root"><see cref="BinaryTreeNode{T}.RootNode"/></param>
        /// <param name="list"><see cref="List{T}"/> of <see cref="Student"/></param>
        private void ToList(BinaryTreeNode<T> root, List<T> list)
        {
            if (root == null)
            {
                return;
            }
            list.Add(root.Data);
            ToList(root.Left, list);
            ToList(root.Right, list);
        }
        /// <inheritdoc cref="object.Equals"/>
        public override bool Equals(object obj)
        {
            List<T> list = ToList();
            BinaryTree<T> other_obj = obj as BinaryTree<T>;
            List<T> obj_list = other_obj.ToList();

            return count_node == other_obj.count_node &&
                   Enumerable.SequenceEqual(list, obj_list);
        }
        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            List<T> list = ToList();
            return HashCode.Combine(RootNode, Count, list.GetHashCode());
        }
        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            List<T> list = ToList();
            string data = "";
            foreach (var item in list)
            {
                data += item.ToString() + "\n";
            }
            return data;
        }
    }
}
