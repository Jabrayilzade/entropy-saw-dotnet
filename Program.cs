using System;

namespace Entropy_SAW
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            double[,] firstTable = new Double[4, 4]
                {{600, 20, -70, 20}, {800, 30, -40, 50}, {500, 15, -80, 30}, {1500, 40, -30, 10}};
            double[] secondTable = calculate(firstTable);

            printTables(firstTable, secondTable);
        }

        public static void printTables(double[,] before, double[] after)
        {
            int initAscii = 65;
            Console.WriteLine("Alternatifler                Kriterler");
            Console.WriteLine("                 X1(Fayda)  X2(Fayda)  X3(Fayda)  X4(Maliyet)");
            for (int i = 0; i < before.GetLength(0); i++)
            {
                Console.Write(Convert.ToChar(initAscii) + "                ");
                initAscii++;
                for (int j = 0; j < before.GetLength(1); j++)
                {
                    Console.Write(before[i, j] + "        ");
                }

                Console.WriteLine();
            }

            for (int i = 0; i < after.Length; i++)
                Console.WriteLine(after[i]);
        }

        public static double[] calculate(double[,] table)
        {
            double[,] normalizedTable = new Double[table.GetLength(0), table.GetLength(1)];
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    normalizedTable[i, j] = table[i, j] / columnSum(table, j);
            }

            double[] eValues = new double[table.GetLength(0)];
            double kValue = 1 / Math.Log(Math.E, table.GetLength(0));
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    normalizedTable[i, j] = normalizedTable[i, j] * Math.Log(Math.E, normalizedTable[i, j]);
            }

            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                    eValues[i] = kValue * columnSum(normalizedTable, j);
            }

            double[] wValues = new Double[table.GetLength(0)];
            double sumValue = 0;
            for (int d = 0; d < table.GetLength(0); d++) sumValue += 1 - eValues[d];
            for (int j = 0; j < eValues.Length; j++) wValues[j] = 1 - eValues[j] / sumValue;

            double[,] rValues = new double[normalizedTable.GetLength(0), normalizedTable.GetLength(1)];
            for (int i = 0; i < rValues.GetLength(0); i++)
            {
                for (int j = 0; j < rValues.GetLength(1); j++)
                {
                    if (j == table.GetLength(0) - 1)
                    {
                        rValues[i, j] = normalizedTable[i, j] / columnMin(normalizedTable, j);
                        rValues[i, j] *= wValues[j];
                        continue;
                    }

                    rValues[i, j] = normalizedTable[i, j] / columnMax(normalizedTable, j);
                    rValues[i, j] *= wValues[j];
                }
            }

            double[] finalValues = new Double[rValues.GetLength(1)];
            for (int i = 0; i < finalValues.Length; i++) finalValues[i] = rowSum(rValues, i);

            return finalValues;
        }

        public static double columnSum(double[,] table, int column)
        {
            double sum = 0;
            for (int i = 0; i < table.GetLength(0); i++) sum += table[i, column];
            return sum;
        }

        public static double rowSum(double[,] table, int row)
        {
            double sum = 0;
            for (int i = 0; i < table.GetLength(1); i++) sum += table[row, i];
            return sum;
        }

        public static double columnMax(double[,] table, int column)
        {
            double max = table[0, column];
            for (int i = 0; i < table.GetLength(1); i++)
                if (max <= table[i, column])
                    max = table[i, column];
            return max;
        }

        public static double columnMin(double[,] table, int column)
        {
            double min = table[table.GetLength(0) - 1, column];
            for (int i = 0; i < table.GetLength(1); i++)
                if (min >= table[i, column])
                    min = table[i, column];
            return min;
        }
    }
}