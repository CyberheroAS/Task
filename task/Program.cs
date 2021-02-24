using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace task
{
    public class MyAnswer : IComparable
    {
        public int amount_answer;
        public string triplet_answer;
        

        public override string ToString()
        {
            return " " + triplet_answer + " " + amount_answer + " раз, ";
        }

        public int CompareTo(object obj)
        {
            MyAnswer compared = obj as MyAnswer;
            if (compared != null)
            {
                int a = Desc(this.amount_answer.CompareTo(compared.amount_answer));
                if (a == 0)
                {
                    return this.triplet_answer.CompareTo(compared.triplet_answer);
                }
                else return a;
            }
            else
                throw new ArgumentException("object is not MyAnswer");
        }

        private int Desc(int b)
        {
            if (b == -1)
                return 1;
            if (b == 1)
                return -1;
            return 0;
        }
    }
    class Program
    {
        //private static SortedSet<MyAnswer> myList = new SortedSet<MyAnswer>();
        private static string text = "";
        private static StreamReader sr = new StreamReader("D:\\fortask.txt");


        public static SortedSet<MyAnswer> FindTriplets(string txt)
        {          
            string word;
            SortedSet<MyAnswer> result = new SortedSet<MyAnswer>();
            txt = Regex.Replace(txt, @"[^\p{L}0-9 -]", "");
            txt = txt.Trim();
            string[] word_mass = txt.Split(' ');
            string triplet = "";
            for (int i = 0; i < word_mass.Length; i++)
            {
                word = word_mass[i];
                for (int j = 0; j < word.Length; j++)
                {
                    if (triplet.Length < 3)
                        triplet += word[j];
                    if ((triplet.Length == 3) && (j != word.Length))
                    {
                        int amount = new Regex(triplet).Matches(txt).Count;
                        //lock (myList)
                        result.Add(new MyAnswer() { triplet_answer = triplet, amount_answer = amount });
                        triplet = triplet.Remove(0, 1);
                    }
                }
                triplet = "";                
            }
            return result;
        }

        public static async List<MyAnswer> MergeSets(List<Task<SortedSet<MyAnswer>>> sets)
        {
            SortedSet<MyAnswer> result = new SortedSet<MyAnswer>();
            foreach(Task<SortedSet<MyAnswer>> tsk in sets)
            {
                SortedSet<MyAnswer> set = await tsk;
                foreach(MyAnswer answer in set)
                {
                    foreach(MyAnswer res in result)
                    {
                        if (res.triplet_answer.Equals(answer))
                        {
                            res.amount_answer += answer.amount_answer;
                        }
                    }
                } 
            }
            
        }

        static void Main(string[] args)
        {
            Console.ReadKey(true);
            List<Task<SortedSet<MyAnswer>>> result_list = new List<Task<SortedSet<MyAnswer>>>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (sr.EndOfStream != true)
            {
                text = sr.ReadLine();
                Task<SortedSet<MyAnswer>> task = Task<SortedSet<MyAnswer>>.Factory.StartNew(() => FindTriplets(text));
                task.Start();
                result_list.Add(task);
                text = "";
            }
            List<MyAnswer> list = new List<MyAnswer>(result_list);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(list[i]);
            }
            stopwatch.Stop();
            Console.WriteLine("\r \n" + stopwatch.ElapsedMilliseconds + " ms.");
            Console.ReadLine();
        }
    }
}

