using ClassLibrary;
using ClassLibrary.BinaryTreeClass;
using ClassLibrary.XmlConverterClass;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace NUnitTests
{
    /// <summary>
    /// Testing method of <see cref="BinaryTree{T}"/> class
    /// </summary>
    [TestFixture]
    class BinaryTreeTests
    {
        List<Student> listData;
        BinaryTree<Student> Tree;

        [OneTimeSetUp]
        public void SetUp()
        {
            listData = new List<Student>();
            //Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                listData.Add(new Student(
                    name: "Name " + i,
                    surname: "Surname " + i,
                    testName: "ÎÊÐ",
                    //testScore: (uint)random.Next(0, 100),
                    testScore: (uint)i,
                    dateOfCompletion: DateTime.Now));
            }
        }
        /// <summary>
        /// Testing methods: <see cref="BinaryTree.Add(T)"/>, <see cref="BinaryTree{T}.Contains(T)"/>, <see cref="BinaryTree{T}.Remove(T)"/>
        /// </summary>
        /// <param name="count">Count nodes</param>
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [Description("Testing methods: Add, Contains.")]
        public void AddTest(int count)
        {
            // Arrange
            Tree = new BinaryTree<Student>();
            if (count > listData.Count)
            {
                count = listData.Count;
            }

            // Act
            for (int i = 0; i < count; i++)
            {
                Tree.Add(listData[i]);
            }

            // Assert
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(Tree.Contains(listData[i]));
            }

            Assert.AreEqual(count, Tree.Count);
        }
        /// <summary>
        /// Test <see cref="BinaryTree{T}.Add(T)"/ exception>
        /// </summary>
        /// <param name="student_index">Student index in the list</param>
        [Description("Checking. Is it possible to add existing data.")]
        [TestCase(2)]
        public void AddTest_ArgumentExcpetion(int student_index)
        {
            // Arrange
            Tree = new BinaryTree<Student>();

            // Act
            for (int i = 0; i < listData.Count; i++)
            {
                Tree.Add(listData[i]);
            }

            // Assert
            Assert.Throws<ArgumentException>(() => Tree.Add(listData[student_index]));
        }

        /// <summary>
        /// Testing method: <see cref="BinaryTree{T}.Remove(T)"/>
        /// </summary>
        /// <param name="student_index">Student index in the list</param>
        [TestCase(2)]
        [TestCase(7)]
        [TestCase(9)]
        [Description("Testing Remove method")]
        public void RemoveTest(int student_index)
        {
            // Arrange
            Tree = new BinaryTree<Student>();

            // Act
            for (int i = 0; i <= 10; i++)
            {
                Tree.Add(listData[i]);
            }
            Tree.Remove(listData[student_index]);

            // Assert
            Assert.IsFalse(Tree.Contains(listData[student_index]));
        }

        /// <summary>
        /// Testing method: <see cref="BinaryTree{T}.CheckBalance"/>
        /// </summary>
        /// <param name="count">Number of nodes</param>
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [Description("Testing Remove method")]
        public void BalanceTest(int count)
        {
            // Arrange
            Tree = new BinaryTree<Student>();

            // Act
            for (int i = 0; i < count; i++)
            {
                Tree.Add(listData[i]);
            }

            // Assert
            Assert.IsTrue(Tree.CheckBalance());
        }

        /// <summary>
        /// Testing method: <see cref="BinaryTree{T}.ToList"/>
        /// </summary>
        /// <param name="count">Number of nodes</param>
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(100)]
        [Description("Testing ToList method")]
        public void ToListTest(int count)
        {
            // Arrange
            Tree = new BinaryTree<Student>();

            // Act
            for (int i = 0; i < count; i++)
            {
                Tree.Add(listData[i]);
            }

            // Assert
            List<Student> list = Tree.ToList();
            Assert.AreEqual(count, list.Count);
        }
    }

    /// <summary>
    /// Testing method of <see cref="XmlConverter"/> class
    /// </summary>
    [TestFixture]
    class XmlConverterTests
    {
        List<Student> listData;
        BinaryTree<Student> Tree;

        [OneTimeSetUp]
        public void SetUp()
        {
            listData = new List<Student>();
            for (int i = 0; i < 100; i++)
            {
                listData.Add(new Student(
                    name: "Name " + i,
                    surname: "Surname " + i,
                    testName: "ÎÊÐ",
                    testScore: (uint)i,
                    dateOfCompletion: DateTime.Now));
            }

            Tree = new BinaryTree<Student>();

            for (int i = 0; i < listData.Count; i++)
            {
                Tree.Add(listData[i]);
            }
        }

        /// <summary>
        /// Testing <see cref="XmlConverter.Serialization(string, BinaryTree{Student})"/ method and <see cref="XmlConverter.Deserialization(string)"/>>
        /// </summary>
        /// <param name="count">Count nodes</param>
        [Test]
        [Description("Testing Serialization and Deserialization method")]
        public void SerializationTest()
        {
            XmlConverter.Serialization("BinaryTree.xml", Tree);
            BinaryTree<Student> des_tree = XmlConverter.Deserialization("BinaryTree.xml");
            Assert.AreEqual(des_tree, Tree);
        }

    }

}