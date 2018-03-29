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

            var cur1 = listRand1.Head;
            var cur2 = listRand1.Head;

            while ((cur1 != null) || (cur2 != null))
            {
                Assert.AreEqual(cur1.Data, cur2.Data);

                if ((cur1.Prev != null) && (cur2.Prev != null))
                {
                    Assert.AreEqual(cur1.Prev.Data, cur2.Prev.Data);
                }

                if ((cur1.Next != null) && (cur2.Next != null))
                {
                    Assert.AreEqual(cur1.Next.Data, cur2.Next.Data);
                }

                if ((cur1.Rand != null) && (cur2.Rand != null))
                {
                    Assert.AreEqual(cur1.Rand.Data, cur2.Rand.Data);
                }

                cur1 = cur1.Next;
                cur2 = cur2.Next;
            }
        }
    }
}
