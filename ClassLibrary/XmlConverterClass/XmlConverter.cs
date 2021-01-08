using ClassLibrary.BinaryTreeClass;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ClassLibrary.XmlConverterClass
{
    /// <summary>
    /// Class for working with xml files.
    /// </summary>
    public class XmlConverter
    {
        /// <summary>
        /// Serializing the <see cref="BinaryTree{Student}"/>
        /// </summary>
        /// <param name="file_name">File name</param>
        /// <param name="tree"><see cref="BinaryTree{Student}"/></param>
        public static void Serialization(string file_name, BinaryTree<Student> tree)
        {
            List<Student> list = tree.ToList();
            XmlSerializer formatter = new XmlSerializer(typeof(List<Student>));
            using FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate);
            formatter.Serialize(fs, list);
        }
        /// <summary>
        /// Deserializing the <see cref="BinaryTree{Student}"/>
        /// </summary>
        /// <param name="file_name">File name</param>
        /// <returns><see cref="BinaryTree{Student}"/></returns>
        public static BinaryTree<Student> Deserialization(string file_name)
        {
            BinaryTree<Student> tree = new BinaryTree<Student>();
            List<Student> list;
            XmlSerializer formatter = new XmlSerializer(typeof(List<Student>));

            using (FileStream fs = new FileStream(file_name, FileMode.Open))
            {
                list = formatter.Deserialize(fs) as List<Student>;
            }
            foreach (var item in list)
            {
                tree.Add(item);
            }

            return tree;
        }
    }
}
