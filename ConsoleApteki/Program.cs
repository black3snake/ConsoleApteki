using System;
using System.Data.SqlClient;
using System.Text;

namespace ConsoleApteki
{
    internal class Program
    {
        static string connectionString = @$"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename = {Environment.CurrentDirectory}\TestDbA.mdf; Integrated Security = True";
        //static string connectionStringN = @$"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename = {Environment.CurrentDirectory}\MyDatabaseData.mdf; Integrated Security = True";
        //static readonly string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename = D:\DataBaseSQL\TestDbA.mdf; Integrated Security = True";
        static void Main(string[] args)
        {
            var filename = Path.Combine(Environment.CurrentDirectory+@"\", "TestDbA.mdf");
            if (!File.Exists(filename))
            {
                CreateSqlDatabase(filename);
                CreateSqlTable_Aptekis();
                CreateSqlTable_Sklads();
                Thread.Sleep(3000);
                CreateSqlTable_Goods();
                Thread.Sleep(3000);
                CreateSqlTable_Goods_Ap();
                Thread.Sleep(3000);
                CreateSqlTable_Goods_Sk();
                Thread.Sleep(3000);
                CreateSqlTable_Parties();
            }


            System.Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Подключение открыто");

                Console.WriteLine("Свойства подключения:");
                Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
                Console.WriteLine("\tБаза данных: {0}", connection.Database);
                Console.WriteLine("\tСервер: {0}", connection.DataSource);
                Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
                Console.WriteLine("\tСостояние: {0}", connection.State);
                Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);
            }
            Console.WriteLine("Подключение закрыто...");

            Aptekis aptekis = new Aptekis(connectionString);
            Sklads sklads = new Sklads(connectionString);
            Goods goods = new Goods(connectionString);
            Parties parties = new Parties(connectionString);
            ComingGA comingGA = new ComingGA(connectionString);
            ComingGS comingGS = new ComingGS(connectionString);


            int number = 0;
            while (true)
            {
                switch(number)
                {
                    case 0: number = Menu1(); break;
                    case 1: number = aptekis.Menu2A(); break;
                    case 2: number = sklads.Menu2Sk(); break;
                    case 3: number = goods.Menu2Goods(); break;
                    case 4: number = parties.Menu2Parties(); break;
                    case 5: number = comingGA.Menu2Goods_Ap(); break;
                    case 6: number = comingGS.Menu2Goods_Sk(); break;
                }
            }

            //Console.ReadKey();
        }


        static int Menu1()
        {
            bool result = false;
            int number;
            Console.WriteLine("\tMENU_1_Main");
            Console.WriteLine(("").PadRight(30, '='));
            Console.WriteLine("1.\t Аптеки");
            Console.WriteLine("2.\t Склады");
            Console.WriteLine("3.\t Товары");
            Console.WriteLine("4.\t Партии");
            Console.WriteLine("5.\t Приход Товара на Аптеку");
            Console.WriteLine("6.\t Приход Товара на Склад");
            Console.WriteLine(("").PadRight(30, '='));
            Console.WriteLine("Для выхода из программы нажмите 0");
            Console.Write("Введитe номер: ");
            string? input  = Console.ReadLine();
            result = int.TryParse(input, out number);
            if(result)
            {
                switch (number)
                {
                    case 0: Environment.Exit(0); break;
                    case 1: return 1;
                    case 2: return 2;
                    case 3: return 3;
                    case 4: return 4;
                    case 5: return 5;
                    case 6: return 6;

                    default:
                        Console.WriteLine("Ошибка, нужно выбрать цифры в представленом меню");
                        Console.WriteLine("Нажмите любую кнопку для продолжения..");
                        Console.ReadKey();
                        break;
                }

            } else {
                Console.Clear();
                Console.WriteLine("Введено не число, повторите ввод снова");
                Console.WriteLine("Нажмите любую кнопку для продолжения..");
                Console.ReadKey();
            }
            return 0;
        }
        
        // Создание Базы данных mdf файла и Таблиц
        private static void CreateSqlDatabase(string filename)
        {
            string connectionStringDB = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = master; Integrated Security = True";

            string databaseName = Path.GetFileNameWithoutExtension(filename);
            using (SqlConnection connection = new SqlConnection(connectionStringDB))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        String.Format("CREATE DATABASE {0} ON PRIMARY (NAME={0}, FILENAME='{1}')", databaseName, filename);
                    command.ExecuteNonQuery();

                    command.CommandText =
                        String.Format("EXEC sp_detach_db '{0}', 'true'", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }
        private static void CreateSqlTable_Aptekis()
        {
            string sqlExpression = "CREATE TABLE [dbo].[Aptekis] ([AptekisId] INT IDENTITY (1, 1) NOT NULL, [Name] NVARCHAR (100) NULL, [Adress] NVARCHAR (200) NULL, [Phone] NVARCHAR (50) NULL, CONSTRAINT [PK_Aptekis] PRIMARY KEY CLUSTERED ([AptekisId] ASC));";

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
        private static void CreateSqlTable_Goods()
        {
            string sqlExpression = "CREATE TABLE [dbo].[Goods] ([GoodsId] INT IDENTITY (1, 1) NOT NULL, [Name] NVARCHAR (100) NULL, CONSTRAINT [PK_Goods] PRIMARY KEY CLUSTERED ([GoodsId] ASC));";

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
        private static void CreateSqlTable_Goods_Ap()
        {
            string sqlExpression = "CREATE TABLE [dbo].[Goods_Ap] ([GaId] INT IDENTITY (1, 1) NOT NULL, [GoodId] INT NOT NULL, [Quantity] INT DEFAULT ((0)) NOT NULL, [AptekaId] INT NOT NULL, CONSTRAINT [PK_Goods_Ap] PRIMARY KEY CLUSTERED ([GaId] ASC), CONSTRAINT [FK_Goods_Ap_To_Goods] FOREIGN KEY ([GoodId]) REFERENCES [dbo].[Goods] ([GoodsId]) ON DELETE CASCADE, CONSTRAINT [FK_Goods_Ap_To_Aptekis] FOREIGN KEY ([AptekaId]) REFERENCES [dbo].[Aptekis] ([AptekisId]) ON DELETE CASCADE);";

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
        private static void CreateSqlTable_Goods_Sk()
        {
            string sqlExpression = "CREATE TABLE [dbo].[Goods_Sk] ([SkId] INT IDENTITY (1, 1) NOT NULL, [GoodId] INT NOT NULL, [Quantity] INT DEFAULT ((0)) NOT NULL, [SkladId] INT NOT NULL, CONSTRAINT [PK_Goods_Sk] PRIMARY KEY CLUSTERED ([SkId] ASC), CONSTRAINT [FK_Goods_Sk_To_Goods] FOREIGN KEY ([GoodId]) REFERENCES [dbo].[Goods] ([GoodsId]) ON DELETE CASCADE, CONSTRAINT [FK_Goods_Sk_To_Sklads] FOREIGN KEY ([SkladId]) REFERENCES [dbo].[Sklads] ([SkladsId]) ON DELETE CASCADE);";

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
        private static void CreateSqlTable_Parties()
        {
            string sqlExpression = "CREATE TABLE [dbo].[Parties] ([PartiesId] INT NOT NULL, [GoodId] INT NOT NULL, [SkladId] INT NOT NULL, [QuantityP] INT DEFAULT ((0)) NOT NULL, CONSTRAINT [PK_Parties] PRIMARY KEY CLUSTERED ([PartiesId] ASC), CONSTRAINT [FK_Parties_To_Goods] FOREIGN KEY ([GoodId]) REFERENCES [dbo].[Goods] ([GoodsId]) ON DELETE CASCADE, CONSTRAINT [FK_Parties_To_Sklads] FOREIGN KEY ([SkladId]) REFERENCES [dbo].[Sklads] ([SkladsId]) ON DELETE CASCADE);";

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
        private static void CreateSqlTable_Sklads()
        {
            string sqlExpression = "CREATE TABLE [dbo].[Sklads] ([SkladsId] INT IDENTITY (1, 1) NOT NULL, [AptekisID] INT NOT NULL, [Name] NVARCHAR (100) NULL, CONSTRAINT [PK_Sklads] PRIMARY KEY CLUSTERED ([SkladsId] ASC), CONSTRAINT [FK_Sklads_To_Aptekis] FOREIGN KEY ([AptekisID]) REFERENCES [dbo].[Aptekis] ([AptekisId]) ON DELETE CASCADE);";

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