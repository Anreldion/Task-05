using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLibrary.BinaryTreeClass
{
    /// <summary>
    /// Класс описывающий реализацию бинарного дерева поиска
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTree<T> where T : Student
    {
        private BinaryTreeNode<T> RootNode = null;
        private int count_node = 0;
        public int Count => count_node;
        /// <summary>
        /// 
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
        /// <param name="node"></param>
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
        /// Создать узел
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private BinaryTreeNode<T> NewItem(T value)
        {
            count_node++;
            BinaryTreeNode<T> node = new BinaryTreeNode<T>(value)
            {
                Balancing = 0
            };
            return node;
        }

        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data"></param>
        public void Add(T data)
        {
            if (RootNode == null)
            {
                RootNode = NewItem(data);
            }
            else
            {
                Insert(ref RootNode, data);
            }

        }

        /// <summary>
        /// Recursive method for <see cref="Add"/>
        /// </summary>
        /// <param name="root"></param>
        /// <param name="data">Data item</param>
        /// <returns></returns>
        private bool Insert(ref BinaryTreeNode<T> root, T data)
        {
            BinaryTreeNode<T> node = root;
            if (node == null)
            {
                root = NewItem(data);
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
        /// <param name="value"></param>
        public void Remove(T value)
        {
            Remove(ref RootNode, value);
        }
        /// <summary>
        /// Recursive method for <see cref="Remove"/>
        /// </summary>
        /// <param name="root"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool Remove(ref BinaryTreeNode<T> root, T data)
        {
            bool ok = false;
            BinaryTreeNode<T> node = root;
            if (node == null)
            {
                return false;
            }
            if (data.CompareTo(node.Data) > 0)
            //if (node.Data < value)
            {
                //--node.BalanceFactor;
                if (Remove(ref node.Right, data) && !Convert.ToBoolean(--node.Balancing))
                {
                    ok = true;
                }
            }
            else
            //if (node.Data > value)
            if (data.CompareTo(node.Data) < 0)
            {
                if (Remove(ref node.Left, data) && !Convert.ToBoolean(++node.Balancing))
                {
                    ok = true;
                }
            }
            else
            {
                // нашли вершину которую надо удалить
                if (node.Right == null)
                {
                    // если есть возможность вырезать сразу, вырезаем
                    root = node.Left;
                    count_node--;
                    return true;
                }
                ok = GetMin(ref node.Right, ref root);   // находим вершину, которую вставляем на место удалённой
                root.Balancing = node.Balancing;       // ставим на место удалённой вершины, нашу замену
                root.Left = node.Left;
                root.Right = node.Right;
                count_node--;
                if (ok)
                {
                    //--root.BalanceFactor;
                    ok = !Convert.ToBoolean(--node.Balancing);
                }
            }
            return (Balance(ref root, ok ? BALANCE_HIGHT_CHANGE : 0) & BALANCE_HIGHT_CHANGE) != 0;
        }

        /// <summary>
        /// Search smallest node
        /// </summary>
        /// <param name="root"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        private bool GetMin(ref BinaryTreeNode<T> root, ref BinaryTreeNode<T> res)
        {
            BinaryTreeNode<T> node = root;
            if (node.Left != null)
            {
                //++node.BalanceFactor;
                if (GetMin(ref node.Left, ref res) && !Convert.ToBoolean(++node.Balancing))
                {
                    return true;
                }
                return (Balance(ref root) & BALANCE_HIGHT_CHANGE) != 0;
            }
            res = node;
            root = node.Right;
            return true;
        }


        // поведение балансировки
        const int BALANCE_ROTATE = 1;   // произошло вращение
        const int BALANCE_HIGHT_CHANGE = 2; // произошло изменение высоты балансировки удаления
        /// <summary>
        /// Таблица балансировки для <see cref="TurnLeftToRight(ref BinaryTreeNode{T})"/ method>
        /// </summary>
        private static readonly int[,] left_to_right_table = new int[6, 4] { { -1, -1, +1, +1 }, { -1, +0, +1, +0 }, { -1, +1, +2, +0 }, { -2, -1, +0, +0 }, { -2, -2, +0, +1 }, { -2, +0, +1, -1 } };
        /// <summary>
        /// Таблица балансировки для <see cref="TurnRightToLeft(ref BinaryTreeNode{T})"/ method>
        /// </summary>
        private static readonly int[,] right_to_left_table = new int[6, 4] { { +1, -1, -2, +0 }, { +1, +0, -1, +0 }, { +1, +1, -1, -1 }, { +2, +0, -1, +1 }, { +2, +1, +0, +0 }, { +2, +2, +0, -1 } };

        /// <summary>
        /// Балансировка узла
        /// </summary>
        /// <param name="root"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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
        /// <returns></returns>
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
        /// <param name="node"></param>
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
        /// Проверка, есть ли такие данные в дереве
        /// </summary>
        /// <param name="data"></param>
        /// <returns> Возвращает true если значение содержится в дереве. В противном случает возвращает false.</returns>
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
            string data="";
            foreach(var item in list)
            {
                data += item.ToString() + "\n";
            }
            return data;
        }
    }
}
