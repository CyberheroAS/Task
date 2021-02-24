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
    public class MyAnswer:IComparable
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
        static string triplet = "";                                    //текущий триплет
        static SortedSet<MyAnswer> myList = new SortedSet<MyAnswer>();
        static string[] word_mass;                                    //массив для слов файла
        static string word;                                           //текущее слово из которого берем триплеты
        static string text = "";                                     //весь текст файла
        static StreamReader sr = new StreamReader("D:\\ft2.txt");


        public static void mythread1()
        {
            for (int i = 0; i < word_mass.Length; i++)
            {
                word = word_mass[i];
                for (int j = 0; j < word.Length; j++)
                {
                    if (triplet.Length < 3)
                        triplet += word[j];
                    if ((triplet.Length == 3) && (j != word.Length))
                    {
                        int amount = new Regex(triplet).Matches(text).Count;
                        myList.Add(new MyAnswer() { triplet_answer = triplet, amount_answer = amount });
                        triplet = triplet.Remove(0, 1);
                    }
                }
                triplet = "";
                word = "";
            }
        }

        public static void mythread2()
        {
            List<MyAnswer> list = new List<MyAnswer>(myList);
            for (int i = 0; i < 10; i++)
            {
                Console.Write(list[i]);
            }
        }
        
       /* public static void mythread3()
        {
            while (sr.EndOfStream != true)
            {
                text += sr.ReadLine();
            }
            text = Regex.Replace(text, @"[^\p{L}0-9 -]", "");
            text = text.Trim();
            word_mass = text.Split(' ');
        } */


        static void Main(string[] args)
        {
            Console.ReadKey(true);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread thread1 = new Thread(mythread1);
            Thread thread2 = new Thread(mythread2);
            //Thread thread3 = new Thread(mythread3);
             while (sr.EndOfStream != true)
             {
                 text += sr.ReadLine();
             }
             text = Regex.Replace(text, @"[^\p{L}0-9 -]", "");
             text = text.Trim();                                  
             word_mass = text.Split(' ');  
           // thread3.Start();
           // thread3.Join();
            thread1.Start();
            thread1.Join();
            thread2.Start();            
            thread2.Join();
            //Parallel.Invoke(mythread1, mythread2);
            stopwatch.Stop();
            Console.WriteLine("\r \n" + stopwatch.ElapsedMilliseconds + " ms.");
            Console.ReadLine();
        }
    }
}
