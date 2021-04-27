using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    /// <summary>
    /// Batuhan Gülbudaklı
    /// </summary>

    internal static class Program
    {
        public static Dictionary<int, bool> PrimeCache = new Dictionary<int, bool>();

        private static void Main(string[] args)
        {
            var result = GetInput()
                .TransformTo2Darray()
                .ResetAllPrimeNumbers()
                .WalkThroughTheNode();
            Console.WriteLine($"Maximum Toplam:  {result}");
            Console.ReadKey();
        }
        private static string[] GetInput()
        {

            /// DOSYA OKUMADA 1. ÇÖZÜM
            /// TÜM SATIRLARI BİR DİZİYE ATAR.
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\batuhan\Desktop\CEE\Yazılım\C#- Console\ConsoleApp1\ConsoleApp1\bin\Debug\netcoreapp3.1\metinbelgesi.txt");
            /// DOSYA KONUMU

            return lines;
        }
        private static string GetInput2()
        {
            /// DOSYA KONUMU
            string dosya_yolu = @"C:\Users\batuhan\Desktop\CEE\Yazılım\C#- Console\ConsoleApp1\ConsoleApp1\bin\Debug\netcoreapp3.1\metinbelgesi.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);

            /// DOSYA OKUMADA 2. ÇÖZÜM SATIRLARI BİR STRİNG DEĞERE ATAR 
            /// VE BU STRİNG DEĞERİNİ OKUMASI VE ARRAY'E DÖNÜŞTÜRMESİ İÇİN 
            /// TransformInputToArray METODUNA İHTİYAÇ DUYAR.
            string input = sw.ReadLine();
            string allInputs = null;

            while (input != null)
            {

                Console.WriteLine(input);
                input = sw.ReadLine();
                allInputs += input;
            }

            sw.Close();
            fs.Close();

            return allInputs;
        }

        private static string[] TransformInputToArray(this string input)
        {
            /// STRİNG OLARAK GÖNDERİLEN DEĞERİ SATIRLARA AYIRARAK ARRAY HALİNE DÖNÜŞTÜRÜR.
            return input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static int[,] TransformTo2Darray(this string[] arrayOfRowsByNewlines)
        {
            ///ARRAY HALİNDE GELEN STRİNG DEĞERLER SÜTUNLAR BAZINDA 2 BOYUTLU ARRAY HALİNDE AYRILIR.
            var tableHolder = new int[arrayOfRowsByNewlines.Length, arrayOfRowsByNewlines.Length + 1];

            for (var row = 0; row < arrayOfRowsByNewlines.Length; row++)
            {
                var eachCharactersInRow = arrayOfRowsByNewlines[row].ExtractNumber();

                for (var column = 0; column < eachCharactersInRow.Length; column++)
                    tableHolder[row, column] = eachCharactersInRow[column];
            }
            return tableHolder;
        }


        private static int[] ExtractNumber(this string rows)
        {
            /// SATIRLARDAN NUMARALARI AYIRIR.
            return
                Regex
                    .Matches(rows, "[0-9]+")
                    .Cast<Match>()
                    .Select(m => int.Parse(m.Value)).ToArray();
        }


        private static int[,] ResetAllPrimeNumbers(this int[,] tableHolder)
        {
            /// TÜM PRIME NUMARALARI SIFIRLAR.
            var length = tableHolder.GetLength(0);
            for (var i = 0; i < length; i++)
            {
                for (var j = 0; j < length; j++)
                {
                    if (tableHolder[i, j] == 0) continue;
                    if (IsPrime(tableHolder[i, j]))
                        tableHolder[i, j] = 0;
                }
            }
            return tableHolder;
        }


        private static int WalkThroughTheNode(this int[,] tableHolder)
        {
            /// PRIME OLMAYAN NUMARALAR ÜZERİNDE GEZİNME
            var tempresult = tableHolder;
            var length = tableHolder.GetLength(0);



            for (var i = length - 2; i >= 0; i--)
            {
                for (var j = 0; j < length; j++)
                {
                    var c = tempresult[i, j];
                    var a = tempresult[i + 1, j];
                    var b = tempresult[i + 1, j + 1];
                    if ((!IsPrime(c) && !IsPrime(a)) || (!IsPrime(c) && !IsPrime(b)))
                        tableHolder[i, j] = c + Math.Max(a, b);
                }
            }
            return tableHolder[0, 0];
        }


        public static bool IsPrime(this int number)
        {
            /// PRIME NUMARA KONTROLÜ
            if (PrimeCache.ContainsKey(number))
            {
                bool value;
                PrimeCache.TryGetValue(number, out value);
                return value;
            }
            if ((number & 1) == 0)
            {
                if (number == 2)
                {
                    if (!PrimeCache.ContainsKey(number)) PrimeCache.Add(number, true);
                    return true;
                }
                if (!PrimeCache.ContainsKey(number)) PrimeCache.Add(number, false);
                return false;
            }

            for (var i = 3; i * i <= number; i += 2)
            {
                if (number % i == 0)
                {
                    if (!PrimeCache.ContainsKey(number)) PrimeCache.Add(number, false);
                    return false;
                }
            }
            var check = number != 1;
            if (!PrimeCache.ContainsKey(number)) PrimeCache.Add(number, check);
            return check;
        }
    }
}