using System.Data.SqlClient;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace ConsoleApteki
{
    internal class Aptekis : ARemove_Add
    {
        string connectionString;
        int number;
        string? AptekaName;
        string? AptekaAdress;
        string? AptekaPhone;
        int AptekisId;
        bool result = false;

        /*public Aptekis() : this("")
        {  }*/
        public Aptekis(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int Menu2A()
        {
            Console.Clear();
            Console.WriteLine("\tMENU_2_Aptekis");
            Console.WriteLine(("").PadRight(67, '='));

            string sqlExpression = "SELECT * FROM Aptekis";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    // выводим названия столбцов
                    Console.WriteLine("{0,-10}{1,-15}{2,-30}{3,-12}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));

                    while (reader.Read()) // построчно считываем данные
                    {
                        object AptekisId = reader.GetValue(0);
                        object Name = reader.GetValue(1);
                        object Adress = reader.GetValue(2);
                        object Phone = reader.GetValue(3);

                        Console.WriteLine("{0,-10}{1,-15}{2,-30}{3,-12}", AptekisId, Name, Adress, Phone);
                    }
                }

                reader.Close();
            }

            Console.WriteLine(("").PadRight(67, '='));
            Console.WriteLine("Для перемещения в главное меню: 0");
            Console.WriteLine("Для добавления Аптеки: 1");
            Console.WriteLine("Для удаления Аптеки: 2");
            Console.WriteLine("Список товаров в Аптеки: 3");
            Console.Write("Введитe номер: ");
            string? input = Console.ReadLine();
            result = int.TryParse(input, out number);
            if (result)
            {
                switch (number)
                {
                    case 0: return 0;

                    case 1:
                        Console.WriteLine("Введите Наименование Аптеки:");
                        AptekaName = Console.ReadLine();
                        Console.WriteLine("Введите Адрес Аптеки:");
                        AptekaAdress = Console.ReadLine();
                        Console.WriteLine("Введите Номер телефона Аптеки:");
                        AptekaPhone = Console.ReadLine();

                        Add($"INSERT INTO Aptekis (Name, Adress, Phone) VALUES (N'{AptekaName}',N'{AptekaAdress}', N'{AptekaPhone}')", connectionString);
                        break;

                    case 2:
                        Console.Write("Введите AptekisId Аптеки для удаления из базы:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out AptekisId);
                        if (result)
                        {
                            Remove($"DELETE FROM Aptekis WHERE AptekisId={AptekisId}", connectionString);
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

                    case 3:
                        Console.Write("Введите AptekisId Аптеки:");
                        input = Console.ReadLine();
                        result = int.TryParse(input, out AptekisId);
                        if (result)
                        {
                            Rezult(AptekisId);
                            Rezult2(AptekisId); // Связь Товары на Складе, Товары , Аптека, Склад.

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
            }

            return 1;
        }

        private void Rezult2(int aptekisId)
        {
            string sqlExpression = "SELECT Goods.Name, Goods_Sk.Quantity FROM Goods_Sk " +
                "INNER JOIN Goods ON Goods_Sk.GoodId = Goods.GoodsId WHERE Goods_Sk.SkladId = " +
                $"(SELECT Sklads.SkladsId FROM Sklads WHERE Sklads.AptekisID = {aptekisId})";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    Console.WriteLine("Товары на Складах приязаных к Аптеке");
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

        

        private void Rezult(int aptekisId)
        {
            string sqlExpression = "SELECT Goods.Name, Goods_Ap.Quantity, Aptekis.Name FROM Goods_Ap INNER JOIN Goods ON Goods_Ap.GoodId = Goods.GoodsId " +
                "INNER JOIN Aptekis ON Goods_Ap.AptekaId = Aptekis.AptekisId " +
                $"WHERE AptekaId = {aptekisId}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    Console.WriteLine();
                    Console.Write($"Выбранная Аптека: ");
                    ShowNameApteka(aptekisId);
                    Console.WriteLine("Товары находящиеся в Аптеке");
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
            /*Console.WriteLine("Нажмите любую кнопку для продолжения..");
            Console.ReadKey();*/
        }

        public void ShowAptekisID()
        {
            string sqlExpression = $"SELECT AptekisId, Name FROM Aptekis";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        object AptekisId = reader.GetValue(0);
                        object Name = reader.GetValue(1);

                        Console.Write("{0}.{1} | ", AptekisId, Name);
                    }
                }

                reader.Close();
            }

        }

        private void ShowNameApteka(int aptekaid)
        {
            string sqlExpression = $"SELECT Name FROM Aptekis WHERE AptekisId = {aptekaid}";

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
