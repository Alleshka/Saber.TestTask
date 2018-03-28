using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saber.Serializer;
using System.IO;


namespace Saber.Serializer.Test
{
    [TestClass]
    public class UnitTest1
    {

        private ListRand listRand = new ListRand();

        private void InitList()
        {
            listRand.InsertToEnd("One");
            listRand.InsertToEnd("Two");
            listRand.InsertToEnd("3");
            listRand.InsertToEnd("ol|olo");
            listRand.InsertToEnd("qqqq");
            listRand.InsertToEnd("test:slom");
        }

        [TestMethod]
        public void SerializeTest()
        {
            // Arrange
            InitList();

            // Act
            using (FileStream stream = new FileStream("test.txt", FileMode.Create))
            {
                listRand.Serialize(stream);
                stream.Close();
            }
        }

        [TestMethod]
        public void DeSerializeTest()
        {
            // Arrange
            InitList();

            int count = listRand.Count;

            // Act
            using (FileStream stream = new FileStream("test.txt", FileMode.Open))
            {
                listRand.Deserialize(stream);
                stream.Close();
            }

            // Asserst
            Assert.AreEqual(count, listRand.Count);
        }
    }
}
