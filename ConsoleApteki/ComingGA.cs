using System.Data.SqlClient;

namespace ConsoleApteki
{
    internal class ComingGA
    {
        string connectionString;
        bool result = false;
        int number;
        int GaId, GoodId, AptekaId, Quantity;
        public ComingGA(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal int Menu2Goods_Ap()
        {
            Console.Clear();
            Console.WriteLine("\tMENU_2_Goods_Apteka");
            Console.WriteLine(("").PadRight(65, '='));

            string sqlExpression = "SELECT Goods_Ap.GaId, Goods.Name, Goods_Ap.Quantity, Aptekis.Name FROM Goods_Ap INNER JOIN Goods ON Goods_Ap.GoodId = Goods.GoodsId " +
                "INNER JOIN Aptekis ON Goods_Ap.AptekaId = Aptekis.AptekisId";

            //string sqlExpression = "SELECT * FROM Goods_Ap";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0,-10}{1,-20}{2,-15}{3,-20}", reader.GetName(0), "Goods_" + reader.GetName(1), reader.GetName(2), "Apteka_" + reader.GetName(3));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object GaId = reader.GetValue(0);
                        object Name = reader.GetValue(1);
                        object Quantity = reader.GetValue(2);
                        object AptekaName = reader.GetValue(3);

                        Console.WriteLine("{0,-10}{1,-20}{2,-15}{3,-20}", GaId, Name, Quantity, AptekaName);
                    }
                }
                reader.Close();
            }

            Console.WriteLine(("").PadRight(65, '='));
            Console.WriteLine("Для перемещения в главное меню 0");
            Console.WriteLine("Для добавления Товара на Аптеку: 1");
            Console.WriteLine("Для удаления Товара из Аптеки: 2");
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
                            return 5;
                        }

                        Console.WriteLine("Введите Количество Товара в Аптеки:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out Quantity);
                        if (!result)
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 5;
                        }
                        
                        Console.WriteLine("Введите ID Аптеки, который занесен в таблице Аптекм:");
                        Aptekis aptekis = new Aptekis(connectionString);
                        aptekis.ShowAptekisID();

                        input = Console.ReadLine();
                        result = int.TryParse(input, out AptekaId);
                        if (result)
                        {
                            Add(GoodId, Quantity, AptekaId);

                        } else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 5;
                        }
                        break;

                    case 2:
                        Console.WriteLine("Введите GaId Товара в Аптеки для удаления из списка:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out GaId);
                        if (result)
                        {
                            Remove(GaId);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 5;
                        }
                        break;

                    default:
                        Console.WriteLine("Ошибка, нужно выбрать цифры в представленом меню");
                        Console.WriteLine("Нажмите любую кнопку для продолжения..");
                        Console.ReadKey();
                        break;

                }

            }
            return 5;
        }

        private void Remove(int gaId)
        {
            string sqlExpression = $"DELETE FROM Goods_Ap WHERE GaId={gaId}";

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

        private void Add(int goodId, int quantity, int aptekaId)
        {
            string sqlExpression = $"INSERT INTO Goods_Ap (GoodId, Quantity, AptekaId) VALUES (N'{goodId}', N'{quantity}', N'{aptekaId}')";

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
