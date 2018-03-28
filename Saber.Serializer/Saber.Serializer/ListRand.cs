using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Saber.Serializer
{
    public class ListRand
    {
        public ListNode Head;

        public ListNode Tail;

        public int Count;

        public void InsertToEnd(String data)
        {
            if (Head == null)
            {
                ListNode node = new ListNode()
                {
                    Data = data,
                    Prev = null,
                    Next = null,
                    Rand = null
                };
                Head = node;
                Tail = node;
            }
            else
            {
                ListNode node = new ListNode()
                {
                    Data = data,
                    Prev = Tail,
                    Next = null,
                    Rand = Head // TODO : Сделать случано
                };

                Tail.Next = node;
                Tail = node;
            }

            Count++;
        }

        public void Serialize(FileStream s)
        {
            using (StreamWriter writer = new StreamWriter(s))
            {
                // Записываем информацию о списке
                writer.WriteLine("Head:" + Head.GetHashCode());
                writer.WriteLine("Tail:" + Tail.GetHashCode());
                writer.WriteLine("Count:" + Count);

                var cur = Head; // Выбираем начальный элемент
                do
                {
                    String data = GetWriterString(cur); // Получаем данные
                    writer.WriteLine(data); // Записываем в файл

                    // Переходим на следующий элемент
                    cur = cur.Next;
                } while (cur != null);
            }
        }

        public void Deserialize(FileStream s)
        {
            using (StreamReader reader = new StreamReader(s))
            {
                // Считываем информацию
                int head = Convert.ToInt32(reader.ReadLine().Split(':')[1]);
                int tail = Convert.ToInt32(reader.ReadLine().Split(':')[1]);
                Count = Convert.ToInt32(reader.ReadLine().Split(':')[1]);             
                
                Dictionary<int, String> FileInfo = reader.ReadToEnd().Split('|').ToDictionary((x) => Convert.ToInt32(x.Split(':')[0])); // Считываем файл
                Dictionary<int, ListNode> WorkedBlocks = new Dictionary<int, ListNode>(); // Словарь для обработанных блоков

                Head = GetNode(head, WorkedBlocks, FileInfo); // Собираем инфу о первом блоке (по цепочке дойдём до следующих)            
            }
        }

        private String GetWriterString(ListNode cur)
        {
            String data = "";

            data += cur.GetHashCode() + ":"; // Хэш элемента

            data += StringToSave(cur.Data) + ":"; // Данные

            // Хэш прошлого элемента
            if (cur.Prev != null) data += cur.Prev.GetHashCode() + ":";
            else data += "-:";

            // Хэш следующего элемента
            if (cur.Next != null) data += cur.Next.GetHashCode() + ":";
            else data += "-:";

            // Хэш случайного элемента
            if (cur.Rand != null) data += cur.Rand.GetHashCode() + ":";
            else data += "-:";

            // Разделитель
            if (cur.Next != null) data += "|";

            return data;
        }
        private ListNode GetNode(int key, Dictionary<int, ListNode> dictionary, Dictionary<int, String> file)
        {
            if (key == -1) return null;

            // Если элемент не обработан
            if (!dictionary.ContainsKey(key))
            {
                String[] nodeData = file[key].Split(':'); // Разделяем инфу о блкое

                int prev = nodeData[2] == "-" ? -1 : Convert.ToInt32(nodeData[2]);
                int next = nodeData[3] == "-" ? -1 : Convert.ToInt32(nodeData[3]);
                int rand = nodeData[4] == "-" ? -1 : Convert.ToInt32(nodeData[4]);

                ListNode node = new ListNode();
                dictionary.Add(key, node); // Добавляем в обработанные

                node.Data = StringToData(nodeData[1]);
                node.Prev = GetNode(prev, dictionary, file);
                node.Rand = GetNode(rand, dictionary, file);
                node.Next = GetNode(next, dictionary, file);

                if (node.Next == null) Tail = node;
            }
            return dictionary[key];
        }

        private String StringToSave(String msg)
        {
            msg = msg.Replace(":", "&#58");
            msg = msg.Replace("|", "&#124");

            return msg;
        }

        private String StringToData(String msg)
        {
            msg = msg.Replace("&#58", ":");
            msg = msg.Replace("&#124", "|");

            return msg;
        }
    }
}
