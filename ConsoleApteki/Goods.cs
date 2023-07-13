using System.Data.SqlClient;

namespace ConsoleApteki
{
    internal class Goods : ARemove_Add
    {
        string connectionString;
        bool result = false;
        int number, GoodId;
        string? GoodName;
        public Goods(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Menu2Goods()
        {
            Console.Clear();
            Console.WriteLine("\tMENU_2_Goods");
            Console.WriteLine(("").PadRight(40, '='));

            string sqlExpression = "SELECT * FROM Goods";
            /*string sqlExpression = "SELECT Goods.GoodsId, Goods.Name, Aptekis.Name, Goods_Ap.Quantity, Sklads.Name, Goods_Sk.Quantity " +
                "FROM Goods INNER JOIN Goods_Ap ON Goods.GoodsId = Goods_Ap.GaId " +
                "INNER JOIN Sklads ON Parties.SkladId = Sklads.SkladsId";*/

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0,-10}{1,-30}", reader.GetName(0), "Goods_" + reader.GetName(1));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object GoodsId = reader.GetValue(0);
                        object Name = reader.GetValue(1);

                        Console.WriteLine("{0,-10}{1,-30}", GoodsId, Name);
                    }
                }

                reader.Close();
            }

            Console.WriteLine(("").PadRight(40, '='));
            Console.WriteLine("Для перемещения в главное меню 0");
            Console.WriteLine("Для добавления Товара: 1");
            Console.WriteLine("Для удаления Товара: 2");
            Console.Write("Введитe номер: ");
            string? input = Console.ReadLine();
            result = int.TryParse(input, out number);
            if (result)
            {
                switch (number)
                {
                    case 0: return 0;

                    case 1:
                        Console.WriteLine("Введите Наименование Товара:");
                        GoodName = Console.ReadLine();
                        //Add(GoodName);
                        Add($"INSERT INTO Goods ( Name) VALUES (N'{GoodName}')", connectionString);
                        break;

                    case 2:
                        Console.WriteLine("Введите GoodId Товара для удаления из базы:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out GoodId);
                        if (result)
                        {
                            //Remove(GoodId);
                            Remove($"DELETE FROM Goods WHERE GoodsId={GoodId}", connectionString);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Введено не число, повторите ввод снова");
                            Console.WriteLine("Нажмите любую кнопку для продолжения..");
                            Console.ReadKey();
                            return 3;
                        }
                        break;

                    default:
                        Console.WriteLine("Ошибка, нужно выбрать цифры в представленом меню");
                        Console.WriteLine("Нажмите любую кнопку для продолжения..");
                        Console.ReadKey();
                        break;
                }
            }


            return 3;
        }

        /*private void Remove(int goodId)
        {
            string sqlExpression = $"DELETE FROM Goods WHERE GoodsId={goodId}";

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

        private void Add(string? goodName)
        {
            string sqlExpression = $"INSERT INTO Goods ( Name) VALUES (N'{goodName}')";
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
        }*/

        public void ShowGoodsID()
        {
            string sqlExpression = $"SELECT GoodsId, Name FROM Goods";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object GoodsId = reader.GetValue(0);
                        object Name = reader.GetValue(1);

                        Console.Write("{0}.{1} | ", GoodsId, Name);
                    }
                }

                reader.Close();
            }

        }
    }
}
