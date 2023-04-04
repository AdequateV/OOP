using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6_oop
{
    public class Container<T> : IEnumerable<T>
    {
        private Node start;
        private Node end;
        public int size { get; private set; }
        public class Node
        {
            internal Node(T value)
            {
                Console.WriteLine("Node(T _value)");
                this.Value = value;
            }
            internal Node()
            {
                Console.WriteLine("Node()");
            }
            internal T? Value { get; set; }
            internal Node? Next { get; set; } = null;
        }
        public IEnumerator<T> GetEnumerator()
        {
            Node current = start.Next;
            while (current != null)
            {
                yield return current.Value!;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public Container()
        {
            Console.WriteLine("Container()");
            start = new Node();
            end = start;
            size = 0;
        }
        public Container(params T[] values)
        {
            Console.WriteLine("Container(params T[] values)");
            start = new Node();
            end = start;
            foreach (T item in values) Add(item);
        }

        public void Add(T element)
        {
            Console.WriteLine("Add(T element) " + element?.ToString());
            end.Next = new Node(element);
            end = end.Next;
            ++size;
        }

        public void Add(T element, int index)
        {
            Console.WriteLine("Add(T element, int " + index + ") " + element?.ToString());
            Node? prev, node = new(element);
            if (index == 0)
            {
                Console.WriteLine("added at first");
                prev = start;
                if (size == 0)
                    start = node;
            }
            else prev = FindNode(index - 1);
            if (prev.Next == null)
            {
                Console.WriteLine("Add(T element, int) says out of bounds");
                return;
            }
            ++size;
            node.Next = prev.Next;
            prev.Next = node;

        }

        private Node? FindNode(int index)
        {
            Console.WriteLine("FindNode(int " + index + ")");
            if (index >= size || index < 0)
            {
                Console.WriteLine("FindNode(int) says out of range");
                return null;
            }
            Node? temp = start.Next;
            while (index-- != 0)
                temp = temp.Next;

            return temp;
        }
        
        public T? GetObject(int index)
        {
            Console.WriteLine("GetObject(int " + index + ")");
            var res = FindNode(index);
            if (res != null)
                return res.Value;
            Console.WriteLine("GetObject got null");
            return default;
        }
        public T? RemoveAt(int index)
        {
            Console.WriteLine("RemoveAt(int " + index + ")");
            Node? node, res;

            if (index == 0)
            {
                Console.WriteLine("removed at first");
                node = start;
            }
            else node = FindNode(index - 1);
            if (node == null)
            {
                Console.WriteLine("RemoveAt got null");
                return default;
            }

            res = node.Next; //to save result
            if (node.Next == null)
                return default;
            node.Next = node.Next.Next;
            --size;
            return res.Value;

        }
        public void Clear()
        {
            Console.WriteLine("Clear()");
            start.Next = null;
            size = 0;
            end = start;
        }

    }

}
