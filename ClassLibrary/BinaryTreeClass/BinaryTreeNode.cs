﻿using System;

namespace ClassLibrary
{
    /// <summary>
    /// The BinaryTreeNode represents a single node in the tree.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryTreeNode<T> : IComparable<T> where T : Student
    {
        /// <summary>
        /// Node data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Left node. (<see cref="BinaryTreeNode{T}"/>)
        /// </summary>
        public BinaryTreeNode<T> Left;

        /// <summary>
        /// Right node. (<see cref="BinaryTreeNode{T}"/>)
        /// </summary>
        public BinaryTreeNode<T> Right;

        /// <summary>
        /// Balance factor
        /// 0 - Левая и правая ветвь имеют одинаковую высоту.
        /// -1 - Левая ветка на еденицу выше чем правая ветка.
        /// +1 - Правая ветка на еденицу выше чем левая ветка.
        /// </summary>
        public int Balancing { get; set; }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="data"></param>
        public BinaryTreeNode(T data)
        {
            Data = data;
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="Data">Node data.</param>
        /// <param name="Left">Left node.</param>
        /// <param name="Right">Right node.</param>
        public BinaryTreeNode(T Data, BinaryTreeNode<T> Left, BinaryTreeNode<T> Right)
        {
            this.Data = Data;
            this.Left = Left;
            this.Right = Right;
        }

        /// <inheritdoc cref="IComparable{T}.CompareTo(T)"/>
        public int CompareTo(T other)
        {
            return Data.CompareTo(other);
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return Data.ToString();
        }
    }
}