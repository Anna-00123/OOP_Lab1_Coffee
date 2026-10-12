using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoffeeApp
{
    class Program
    {
        static List<Coffee> coffees = new List<Coffee>();
        static int maxCount;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Програма обліку кавових напоїв ===");
            maxCount = ReadMaxCount();

            bool running = true;
            while (running)
            {
                PrintMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddCoffee(); break;
                    case "2": ViewAll(); break;
                    case "3": FindCoffee(); break;
                    case "4": DemonstrateBehavior(); break;
                    case "5": DeleteCoffee(); break;
                    case "0":
                        running = false;
                        Console.WriteLine("Роботу програми завершено. До побачення!");
                        break;
                    default:
                        Console.WriteLine("Error: невірний пункт меню, спробуйте ще раз!");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("----- МЕНЮ -----");
            Console.WriteLine("1 - Додати об'єкт");
            Console.WriteLine("2 - Переглянути всі об'єкти");
            Console.WriteLine("3 - Знайти об'єкт");
            Console.WriteLine("4 - Продемонструвати поведінку");
            Console.WriteLine("5 - Видалити об'єкт");
            Console.WriteLine("0 - Вийти з програми");
            Console.Write("Ваш вибір: ");
        }

        static int ReadMaxCount()
        {
            while (true)
            {
                Console.Write("Введіть максимальну кількість об'єктів N (N > 0): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int n) && n > 0)
                    return n;
                Console.WriteLine("Error: N має бути цілим додатним числом!");
            }
        }

        static void AddCoffee()
        {
            if (coffees.Count >= maxCount)
            {
                Console.WriteLine($"Error: досягнуто максимальної кількості об'єктів ({maxCount})!");
                return;
            }

            Console.Write("Режим додавання: 1 - вручну, 2 - автоматично: ");
            string mode = Console.ReadLine();

            Coffee c = (mode == "2") ? GenerateRandomCoffee() : ReadCoffeeManually();

            coffees.Add(c);
            Console.WriteLine("Об'єкт успішно додано!");
        }

        static Coffee ReadCoffeeManually()
        {
            string orderCode = ReadOrderCode();
            string name = ReadName();
            CoffeeType type = ReadCoffeeType();
            double price = ReadPrice();
            DateTime roastDate = ReadRoastDate();
            bool isDecaf = ReadBool("Без кофеїну? (так/ні): ");
            int volumeMl = ReadVolume();

            return new Coffee(orderCode, name, type, price, roastDate, isDecaf, volumeMl);
        }

        static Coffee GenerateRandomCoffee()
        {
            string code = rnd.Next(100000, 999999).ToString();
            string[] names = { "Ranok Blend", "Sunny Roast", "Dark Wave", "Milk Dream", "Velvet Cup" };
            string name = names[rnd.Next(names.Length)];
            CoffeeType type = (CoffeeType)rnd.Next(Enum.GetValues(typeof(CoffeeType)).Length);
            double price = Math.Round(rnd.NextDouble() * 480 + 10, 2);
            DateTime minDate = new DateTime(2020, 1, 1);
            DateTime roastDate = minDate.AddDays(rnd.Next((DateTime.Now - minDate).Days));
            bool isDecaf = rnd.Next(2) == 0;
            int volumeMl = rnd.Next(30, 1001);

            Console.WriteLine("Згенеровано випадковий об'єкт.");
            return new Coffee(code, name, type, price, roastDate, isDecaf, volumeMl);
        }

        static string ReadOrderCode()
        {
            while (true)
            {
                Console.Write("Код замовлення (рівно 6 символів, літери/цифри): ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) { Console.WriteLine("Error: відсутнє значення!"); continue; }
                if (input.Length != 6) { Console.WriteLine("Error: має бути рівно 6 символів!"); continue; }

                bool allValid = true;
                foreach (char ch in input)
                    if (!char.IsLetterOrDigit(ch)) { allValid = false; break; }

                if (!allValid) { Console.WriteLine("Error: дозволені тільки літери та цифри!"); continue; }
                return input;
            }
        }

        static string ReadName()
        {
            while (true)
            {
                Console.Write("Назва напою (2-30 символів): ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) { Console.WriteLine("Error: відсутнє значення!"); continue; }
                if (input.Length < 2 || input.Length > 30) { Console.WriteLine("Error: довжина має бути від 2 до 30 символів!"); continue; }
                return input;
            }
        }

        static CoffeeType ReadCoffeeType()
        {
            var values = Enum.GetValues(typeof(CoffeeType));
            while (true)
            {
                Console.WriteLine("Оберіть тип кави:");
                int i = 0;
                foreach (var v in values) { Console.WriteLine($"  {i} - {v}"); i++; }
                Console.Write("Ваш вибір: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int idx) && idx >= 0 && idx < values.Length)
                    return (CoffeeType)idx;
                Console.WriteLine("Error: невірний номер типу кави!");
            }
        }

        static double ReadPrice()
        {
            while (true)
            {
                Console.Write("Ціна (0 - 500 грн): ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) { Console.WriteLine("Error: відсутнє значення!"); continue; }
                if (!double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
                { Console.WriteLine("Error: має бути числом!"); continue; }
                if (price <= 0 || price > 500) { Console.WriteLine("Error: ціна має бути в діапазоні (0; 500]!"); continue; }
                return price;
            }
        }

        static DateTime ReadRoastDate()
        {
            DateTime minDate = new DateTime(2020, 1, 1);
            while (true)
            {
                Console.Write("Дата обжарки (dd.MM.yyyy): ");
                string input = Console.ReadLine();
                if (!DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                { Console.WriteLine("Error: невірний формат дати! Використовуйте dd.MM.yyyy"); continue; }
                if (date < minDate || date > DateTime.Now)
                { Console.WriteLine($"Error: дата має бути в діапазоні від 01.01.2020 до {DateTime.Now:dd.MM.yyyy}!"); continue; }
                return date;
            }
        }

        static bool ReadBool(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine()?.Trim().ToLower();
                if (input == "так" || input == "yes" || input == "true") return true;
                if (input == "ні" || input == "no" || input == "false") return false;
                Console.WriteLine("Error: введіть 'так' або 'ні'!");
            }
        }

        static int ReadVolume()
        {
            while (true)
            {
                Console.Write("Об'єм напою в мл (30 - 1000): ");
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int volume)) { Console.WriteLine("Error: має бути цілим числом!"); continue; }
                if (volume < 30 || volume > 1000) { Console.WriteLine("Error: об'єм має бути в діапазоні [30; 1000]!"); continue; }
                return volume;
            }
        }

        static void ViewAll()
        {
            if (coffees.Count == 0) { Console.WriteLine("Об'єктів немає."); return; }
            PrintTable(coffees);
        }

        static void PrintTable(List<Coffee> list)
        {
            Console.WriteLine($"{"№",-4}{"Код",-8}{"Назва",-20}{"Тип",-12}{"Ціна",-10}{"Дата обжарки",-14}{"Декаф",-7}{"Об'єм",-8}{"Темп."}");
            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"{i + 1,-4}{list[i]}");
        }

        static void FindCoffee()
        {
            if (coffees.Count == 0) { Console.WriteLine("Об'єктів немає."); return; }

            Console.Write("Пошук за характеристикою: 1 - тип кави, 2 - без кофеїну (так/ні): ");
            string choice = Console.ReadLine();
            List<Coffee> result;

            if (choice == "1")
            {
                CoffeeType type = ReadCoffeeType();
                result = coffees.FindAll(c => c.type == type);
            }
            else if (choice == "2")
            {
                bool isDecaf = ReadBool("Без кофеїну? (так/ні): ");
                result = coffees.FindAll(c => c.isDecaf == isDecaf);
            }
            else
            {
                Console.WriteLine("Error: невірний вибір характеристики!");
                return;
            }

            if (result.Count == 0)
                Console.WriteLine("Об'єкт(и) із заданою характеристикою не знайдено.");
            else
                PrintTable(result);
        }

        static void DemonstrateBehavior()
        {
            if (coffees.Count == 0) { Console.WriteLine("Об'єктів немає."); return; }

            PrintTable(coffees);
            Console.Write("Введіть номер об'єкта для демонстрації поведінки: ");
            if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > coffees.Count)
            { Console.WriteLine("Error: невірний номер об'єкта!"); return; }

            Coffee c = coffees[num - 1];
            Console.WriteLine($"--- Поведінка об'єкта \"{c.name}\" ---");
            c.Reheat(10);
            c.Cool(15);
            c.ApplyDiscount(10);
            c.AddVolume(50);
            Console.WriteLine("Поточний стан об'єкта:");
            Console.WriteLine(c.ToString());
        }

        static void DeleteCoffee()
        {
            if (coffees.Count == 0) { Console.WriteLine("Об'єктів немає."); return; }

            Console.Write("Видалення: 1 - за порядковим номером, 2 - за характеристикою: ");
            string mode = Console.ReadLine();

            if (mode == "1")
            {
                PrintTable(coffees);
                Console.Write("Введіть номер об'єкта для видалення: ");
                if (!int.TryParse(Console.ReadLine(), out int num) || num < 1 || num > coffees.Count)
                { Console.WriteLine("Error: невірний номер об'єкта!"); return; }

                coffees.RemoveAt(num - 1);
                Console.WriteLine("Об'єкт успішно видалено!");
            }
            else if (mode == "2")
            {
                Console.Write("Видалення за характеристикою: 1 - тип кави, 2 - без кофеїну (так/ні): ");
                string choice = Console.ReadLine();
                int removed;

                if (choice == "1")
                {
                    CoffeeType type = ReadCoffeeType();
                    removed = coffees.RemoveAll(c => c.type == type);
                }
                else if (choice == "2")
                {
                    bool isDecaf = ReadBool("Без кофеїну? (так/ні): ");
                    removed = coffees.RemoveAll(c => c.isDecaf == isDecaf);
                }
                else
                {
                    Console.WriteLine("Error: невірний вибір характеристики!");
                    return;
                }

                if (removed == 0)
                    Console.WriteLine("Об'єктів для видалення із заданою характеристикою не знайдено.");
                else
                    Console.WriteLine($"Видалено об'єктів: {removed}");
            }
            else
            {
                Console.WriteLine("Error: невірний вибір режиму видалення!");
            }
        }
    }
}