using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Saber.Serializer;
using System.IO;


namespace Saber.Serializer.Test
{
    [TestClass]
    public class UnitTest1
    {

        private ListRand InitList()
        {
            ListRand listRand = new ListRand();
            listRand.InsertToEnd("One");
            listRand.InsertToEnd("Two\nqTwo");
            listRand.InsertToEnd("3");
            listRand.InsertToEnd("ol|olo");
            listRand.InsertToEnd("qq||||qq");
            listRand.InsertToEnd("test:slom");
            return listRand;
        }

        [TestMethod]
        public void SerializeTest()
        {
            // Arrange
            var listRand = InitList();

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
            var listRand1 = InitList();
            ListRand listRand2 = new ListRand();


            // Act
            using (FileStream stream = new FileStream("test.txt", FileMode.Open))
            {
                listRand2.Deserialize(stream);
                stream.Close();
            }

            // Asserst
            Assert.AreEqual(listRand1.Count, listRand2.Count);
            if (listRand1.Count > 0) Assert.AreEqual(listRand1.Head.Data, listRand2.Head.Data);
        }
    }
}
