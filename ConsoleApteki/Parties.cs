using System.Data.SqlClient;

namespace ConsoleApteki
{
    internal class Parties
    {
        string connectionString;
        bool result = false;
        int number;
        int PartiesId, GoodId, SkladId, QuantityP;
        public Parties(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal int Menu2Parties()
        {
            Console.Clear();
            Console.WriteLine("\tMENU_2_Goods");
            Console.WriteLine(("").PadRight(60, '='));

            string sqlExpression = "SELECT Parties.PartiesId, Goods.Name, Parties.QuantityP, Sklads.Name FROM Parties INNER JOIN Goods ON Parties.GoodId = Goods.GoodsId " +
                "INNER JOIN Sklads ON Parties.SkladId = Sklads.SkladsId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0,-10}{1,-20}{2,-15}Sklad_{3,-15}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object PartiesId = reader.GetValue(0);
                        object Name = reader.GetValue(1);
                        object QuantityP = reader.GetValue(2);
                        object SkladName = reader.GetValue(3);

                        Console.WriteLine("{0, -10}{1,-20}{2,-15}{3,-15}", PartiesId, Name, QuantityP, SkladName);
                    }
                }
                reader.Close();
            }

            Console.WriteLine(("").PadRight(60, '='));
            Console.WriteLine("Для перемещения в главное меню 0");
            Console.WriteLine("Для добавления Партии: 1");
            Console.WriteLine("Для удаления Партии: 2");
            Console.Write("Введитe номер: ");
            string? input = Console.ReadLine();
            result = int.TryParse(input, out number);
            if (result)
            {
                switch (number)
                {
                    case 0: return 0;

                    case 1:
                        Console.WriteLine("Введите уникальный ID номер партии:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out PartiesId);
                        if (!result)
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 4;
                        }
                        
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
                            return 4;
                        }
                        
                        Console.WriteLine("Введите ID Склада, который занесен в таблице Склады:");
                        Sklads sklads = new Sklads(connectionString);
                        sklads.ShowSkladsID();

                        input = Console.ReadLine();
                        result = int.TryParse(input, out SkladId);
                        if (!result)
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 4;
                        }
                        Console.WriteLine("Введите Количество товаров в партии:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out QuantityP);
                        if (result)
                        {
                            Add(PartiesId, GoodId, SkladId, QuantityP);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 4;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Введите PartiesId Партии для удаления из базы:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out PartiesId);
                        if (result)
                        {
                            Remove(PartiesId);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 4;
                        }
                        break;

                    default:
                        Console.WriteLine("Ошибка, нужно выбрать цифры в представленом меню");
                        Console.WriteLine("Нажмите любую кнопку для продолжения..");
                        Console.ReadKey();
                        break;

                }

            }
            return 4;
        }

        private void Remove(int partiesId)
        {
            string sqlExpression = $"DELETE FROM Parties WHERE PartiesId={partiesId}";

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

        private void Add(int partiesId, int goodId, int skladId, int quantityP)
        {
            string sqlExpression = $"INSERT INTO Parties ( PartiesId, GoodId, SkladId, QuantityP) VALUES (N'{partiesId}', N'{goodId}', N'{skladId}', N'{quantityP}')";

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
