using System.Data.SqlClient;

namespace ConsoleApteki
{
    internal interface IRemove_Add
    {
        void Remove(string sqlEx, string conn);

        void Add(string sqlEx, string conn);
    }

    abstract class ARemove_Add : IRemove_Add
    {
        public void Remove(string sqlExpression, string connectionString)
        {
            //string sqlExpression = $"DELETE FROM Sklads WHERE SkladsId={id}";
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

        public void Add(string sqlExpression, string connectionString)
        {
            //string sqlExpression = $"INSERT INTO Sklads ( AptekisId, Name) VALUES (N'{aptekisId}', N'{skladname}')";
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
