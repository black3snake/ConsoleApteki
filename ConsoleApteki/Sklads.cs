using System.Data.SqlClient;

namespace ConsoleApteki
{
    internal class Sklads
    {
        string connectionString;
        bool result = false;
        int number;
        int SkladsId, AptekisId;
        string? SkladName;

        public Sklads(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public int Menu2Sk()
        {
            Console.Clear();
            Console.WriteLine("\tMENU_2_Sklads");
            Console.WriteLine(("").PadRight(40, '='));

            //string sqlExpression = "SELECT * FROM Sklads";
            string sqlExpression = "SELECT Sklads.SkladsId, Sklads.Name, Aptekis.Name FROM Sklads INNER JOIN Aptekis ON Sklads.AptekisId = Aptekis.AptekisId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0,-10}{1,-15}{2,-15}", reader.GetName(0), "Skads_" + reader.GetName(1), "Apteka_" + reader.GetName(2));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object SkladsId = reader.GetValue(0);
                        object AptekisId = reader.GetValue(1);
                        object Name = reader.GetValue(2);

                        Console.WriteLine("{0,-10}{1,-15}{2,-15}", SkladsId, AptekisId, Name);
                    }
                }

                reader.Close();
            }

            Console.WriteLine(("").PadRight(40, '='));
            Console.WriteLine("Для перемещения в главное меню 0");
            Console.WriteLine("Для добавления Склада: 1");
            Console.WriteLine("Для удаления Склада: 2");
            Console.WriteLine("Список товаров на Складе: 3");
            Console.Write("Введитe номер: ");
            string? input = Console.ReadLine();
            result = int.TryParse(input, out number);
            if (result)
            {
                switch (number)
                {
                    case 0: return 0;

                    case 1:
                        Console.WriteLine("Введите Наименование Склада:");
                        SkladName = Console.ReadLine();
                        Console.WriteLine("Введите ID Аптеки, которая обслудивается этим складом:");
                        Aptekis aptekis = new Aptekis(connectionString);
                        aptekis.ShowAptekisID();
                        input = Console.ReadLine();
                        result = int.TryParse(input, out AptekisId);
                        if (result)
                        {
                            Add(SkladName, AptekisId);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 2;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Введите SkladsId Склада для удаления из базы:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out SkladsId);
                        if (result)
                        {
                            Remove(SkladsId);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 2;
                        }
                        break;
                    
                    case 3:
                        Console.Write("Введите SkladsId Склада:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out SkladsId);
                        if (result)
                        {
                            Rezult(SkladsId);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 1;
                        }
                        break;

                    default:
                        Console.WriteLine("Ошибка, нужно выбрать цифры в представленом меню");
                        Console.WriteLine("Нажмите любую кнопку для продолжения..");
                        Console.ReadKey();
                        break;
                }

            }
            else
            {
                Console.Clear();
                Console.WriteLine("Введено не число, повторите ввод снова");
                Console.WriteLine("Нажмите любую кнопку для продолжения..");
                Console.ReadKey();
                return 2;
            }
            return 2;
        }

        private void Rezult(int skladId)
        {
            string sqlExpression = "SELECT Goods.Name, Goods_Sk.Quantity FROM Goods_Sk INNER JOIN Goods ON Goods_Sk.GoodId = Goods.GoodsId " +
                "INNER JOIN Sklads ON Goods_Sk.SkladId = Sklads.SkladsId " +
                $"WHERE SkladId = {skladId}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    Console.Write($"Выбранный Склад: ");
                    ShowNameSklad(skladId);

                    Console.WriteLine("{0,-20}{1,-10}", "Goods_" + reader.GetName(0), reader.GetName(1));
                    Console.WriteLine(("").PadRight(30, '-'));
                    while (reader.Read()) // построчно считываем данные
                    {
                        object GoodsName = reader.GetValue(0);
                        object Quantity = reader.GetValue(1);

                        Console.WriteLine("{0,-20}{1,-10}", GoodsName, Quantity);
                    }
                }

                reader.Close();
                Console.WriteLine(("").PadRight(30, '-'));
            }
            Console.WriteLine("Нажмите любую кнопку для продолжения..");
            Console.ReadKey();
        }

        private void Add(string? skladname, int aptekisId)
        {
            string sqlExpression = $"INSERT INTO Sklads ( AptekisId, Name) VALUES (N'{aptekisId}', N'{skladname}')";

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

        private void Remove(int id)
        {
            string sqlExpression = $"DELETE FROM Sklads WHERE SkladsId={id}";

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

        public void ShowSkladsID()
        {
            string sqlExpression = $"SELECT SkladsId, Name FROM Sklads";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object SkladsId = reader.GetValue(0);
                        object Name = reader.GetValue(1);

                        Console.Write("{0}.{1} | ", SkladsId, Name);
                    }
                }

                reader.Close();
            }
        }
        private void ShowNameSklad(int skladId)
        {
            string sqlExpression = $"SELECT Name FROM Sklads WHERE SkladsId = {skladId}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object Name = reader.GetValue(0);

                        Console.WriteLine("{0}", Name);
                    }
                }

                reader.Close();
            }


        }
    }
}
