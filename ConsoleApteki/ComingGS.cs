using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApteki
{
    internal class ComingGS
    {
        string connectionString;
        bool result = false;
        int number;
        int SkId, GoodId, SkladId, Quantity;
        public ComingGS(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal int Menu2Goods_Sk()
        {
            Console.Clear();
            Console.WriteLine("\tMENU_2_Goods_Sklad");
            Console.WriteLine(("").PadRight(60, '='));

            string sqlExpression = "SELECT Goods_Sk.SkId, Goods.Name, Goods_Sk.Quantity, Sklads.Name FROM Goods_Sk INNER JOIN Goods ON Goods_Sk.GoodId = Goods.GoodsId " +
                "INNER JOIN Sklads ON Goods_Sk.SkladId = Sklads.SkladsId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0,-10}{1,-20}{2,-15}Sklad_{3,-20}", reader.GetName(0), "Goods_" + reader.GetName(1), reader.GetName(2), "Sklad_" + reader.GetName(3));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object SkId = reader.GetValue(0);
                        object Name = reader.GetValue(1);
                        object Quantity = reader.GetValue(2);
                        object SkladName = reader.GetValue(3);

                        Console.WriteLine("{0, -10}{1,-20}{2,-15}{3,-20}", SkId, Name, Quantity, SkladName);
                    }
                }
                reader.Close();
            }

            Console.WriteLine(("").PadRight(60, '='));
            Console.WriteLine("Для перемещения в главное меню 0");
            Console.WriteLine("Для добавления Товара на Склад: 1");
            Console.WriteLine("Для удаления Товара из Склада: 2");
            Console.Write("Введитe номер: ");
            string? input = Console.ReadLine();
            result = int.TryParse(input, out number);
            if (result)
            {
                switch (number)
                {
                    case 0: return 0;

                    case 1:
                        Console.WriteLine("Введите ID Товара, который занесен в таблице Товары:");
                        Goods goods = new Goods(connectionString);
                        goods.ShowGoodsID();

                        input = Console.ReadLine();
                        result = int.TryParse(input, out GoodId);
                        if (!result)
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 6;
                        }

                        Console.WriteLine("Введите Количество Товара на Складе :");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out Quantity);
                        if (!result)
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 6;
                        }

                        Console.WriteLine("Введите ID Склада, который занесен в таблице Склады:");
                        Sklads sklads = new Sklads(connectionString);
                        sklads.ShowSkladsID();

                        input = Console.ReadLine();
                        result = int.TryParse(input, out SkladId);
                        if (result)
                        {
                            Add(GoodId, Quantity, SkladId);

                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 6;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Введите GaId Товара на Складе для удаления из списка:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out SkId);
                        if (result)
                        {
                            Remove(SkId);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 6;
                        }
                        break;

                    default:
                        Console.WriteLine("Ошибка, нужно выбрать цифры в представленом меню");
                        Console.WriteLine("Нажмите любую кнопку для продолжения..");
                        Console.ReadKey();
                        break;

                }

            }
            return 6;
        }

        private void Remove(int skId)
        {
            string sqlExpression = $"DELETE FROM Goods_Sk WHERE SkId={skId}";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    Console.WriteLine("Удалено объектов: {0}", number);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Возникла ошибка записи в БД, проверте вводимые данные");
                Console.WriteLine("Нажмите любую кнопку для продолжения..");
                Console.ReadKey();
            }
        }

        private void Add(int goodId, int quantity, int skladId)
        {
            string sqlExpression = $"INSERT INTO Goods_Sk ( GoodId, Quantity, SkladId) VALUES (N'{goodId}', N'{quantity}', N'{skladId}')";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    int number = command.ExecuteNonQuery();
                    Console.WriteLine("Добавлено объектов: {0}", number);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Возникла ошибка записи в БД, проверте вводимые данные");
                Console.WriteLine("Нажмите любую кнопку для продолжения..");
                Console.ReadKey();
            }
        }
    }
}
