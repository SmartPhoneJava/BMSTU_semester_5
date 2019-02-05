using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

// https://metanit.com/sharp/adonet/2.4.php

namespace lab8
{
    class Program
    {

        static void Main(string[] args)
        {
            Program launch = new Program();

            int command = 100;

            while(command > 0)
            {
                Console.WriteLine("Введите одну из следующих команд для запуска какой либо подпрограммы:");
                Console.WriteLine("1  - SearchAnimeWithWord - поиск тайтла по описанию;");
                Console.WriteLine("2  - AmountOfAnimeWithOneYearRating - узнать количество аниме в БД заданного возрастного рейтинга;");
                Console.WriteLine("3  - UpdateYearStudioWhereIdIs - обновить год основания одной из студий;");
                Console.WriteLine("4  - JustInsertInTitle - добавьте тайтл в базу данных;");
                Console.WriteLine("5  - Adder - добавьте в таблицу n-строчек, а потом наслаждайтесь их поэтапным удалением с помощью откатов;");
                Console.WriteLine("     Начиная со следующей функции и далее данные загружаются в ");
                Console.WriteLine("     локальную БД(DataSet) и дальше мы можем работать с ними без соединения.");
                Console.WriteLine("6  - GetTitlesWithEpisodesMoreThen - получить список тайтлов, где эпизодов больше указанного;");
                Console.WriteLine("7  - InsertTypeComment - записать из локального хранилища новую строку в таблицу Типов аниме;");
                Console.WriteLine("8  - InsertTitle - добавить тайтл в таблицу тайтлов с автоматическим созданием описания;");
                Console.WriteLine("9  - GuessTheStudio - веселая игра угадайка;");
                Console.WriteLine("10 - SaveToXML - сохраните себе на компьютер список тайтлов с высоким рейтингом в формате xml;");
                Console.WriteLine("<=0  - покинуть программу;");
                command = launch.get_int("Выбор за вами. Введите число:", 0, 10, "Попробуйте все таки придерживаться " +
                    " указанных правил. Укажите число от меньше 10:");
                if (command != -100)
                {
                    switch(command)
                    {
                        case 0: // Выход
                            break;
                        case 1: // Просто получение части таблицы
                            launch.SearchAnimeWithWord();
                            break;
                        case 2:  // Скалярная функция
                            launch.AmountOfAnimeWithOneYearRating();
                            break;
                        case 3:  // Вызов хранимой процедуры
                            launch.TryConnect(launch.UpdateYearStudioWhereIdIsComment,
                                   launch.UpdateYearStudioWhereIdIs);
                            break;
                        case 4: // Добавить элемент в БД
                            launch.TryConnect(launch.JustInsertInTitleComment,
                                   launch.JustInsertInTitle);
                            break;
                        case 5: // Использование транзакций
                            launch.TryConnect(launch.AdderComment, launch.Adder);
                            break;
                        // Дальше начинается использование DataSet, с помощью которого
                        // можно работать независимо от наличия подключения
                        case 6:  // Получение части таблицы
                            launch.TryConnect(launch.GetTitlesWithEpisodesMoreThenComment,
                                launch.GetTitlesWithEpisodesMoreThen);
                            break;
                        case 7: // Загрузка данных в таблицу
                            launch.TryConnect(launch.InsertTypeComment,
                                launch.InsertType);
                            break;
                        case 8:  // Использование хранимой процедуры
                            launch.TryConnect(launch.InsertTitleComment,
                                launch.InsertTitle);
                            break;
                        case 9:  // Играем в игру с delete
                            launch.TryConnect(launch.GuessTheStudioComment,
                                launch.GuessTheStudio);
                            break;
                        case 10:  // Создаем список запланированных для просмотра тайтлов...
                                  // Ладно, просто заносим в xml тайтлы с высоким рейтингом
                            launch.TryConnect(launch.SaveToXMLComment,
                                launch.SaveToXML);
                            break;
                        default:
                            Console.WriteLine("Непредвиденная ситуация, а ведь вы ввели {0}...", command);
                            break;
                    }
                }
            }
        }

        private bool repeat()
        {
            Console.WriteLine("Повторим?[1/0]");
            int continue_ = 0;
            if (!Int32.TryParse(Console.ReadLine(), out continue_))
            {
                Console.WriteLine("Будем считать, что вы отказались.");
                return false;
            }
            if (continue_ != 1)
            {
                return false;
            }
            return true;
        }

        private int GetSize()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;
            int size = 0;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение
                
                connection.Open();

                SqlCommand command = new SqlCommand();
                
                command.CommandText = String.Format("select COUNT(*) from studio");
                command.Connection = connection;

                object obj = command.ExecuteScalar();
                size = (int)obj;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return size;
        }

        private void ShowLast3Titles(SqlConnection connection)
        {
            string sqlExpression = "select top 3 * from title order by id_title desc";
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
                // выводим названия столбцов
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                    reader.GetName(0), reader.GetName(1), reader.GetName(2),
                    reader.GetName(3), reader.GetName(4), reader.GetName(5),
                    reader.GetName(6));

                while (reader.Read()) // построчно считываем данные
                {
                    int id = reader.GetInt32(0);
                    string name_origin = reader.GetString(1);
                    string name_local = reader.GetString(2);
                    string descrition = reader.GetString(3);
                    int id_type = reader.GetInt32(4);
                    int episodes = reader.GetInt32(5);
                    int duration = reader.GetInt32(6);

                    Console.WriteLine("{0} \t{1} \t{2} \t{3} \t{4} \t{5} \t{6}",
                        id, name_origin, name_local, descrition, id_type, episodes,
                        duration);
                }
            }
        }

        private void showStudiosWithConnection(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();

            command.CommandText = String.Format("select * from studio");
            command.Connection = connection;

            SqlDataReader reader = command.ExecuteReader();
           
            while (reader.Read())
            {
                Console.WriteLine(reader["id_studio"] + " " + reader["name_studio"] + " " + reader["year_"]);
            }
        }

        public void SaveToXMLComment()
        {
            Console.WriteLine("Сохранить таблицу видеоэфира в xml," +
                "где рейтинг ваше заданного ");
        }

        public bool SaveToXML(SqlConnection connection)
        {
            float rating = 6;
            
            while (!(rating >= 0 && rating <= 5))
            {
                if (!float.TryParse(Console.ReadLine(), out rating))
                {
                    Console.WriteLine("Мы вас услышали, выходим.");
                    return false;
                }
                if (rating < 0)
                {
                    Console.WriteLine("На нет и суда нет, назад.");
                    return false;
                }

                if (!(rating >= 0 && rating <= 5))
                {
                    Console.WriteLine("Мы ждем от вас рейтинг - число с" +
                        " плавающей точкой от 0 до 5. Можете ввести" +
                        " отрицательное число, если хотите выйти.");
                }
            }
            

            SqlCommand command = new SqlCommand();

            command.CommandText = String.Format("select * from airtime where" +
                " user_rating >= @rating");
            command.Connection = connection;

            SqlParameter rating_p = new SqlParameter("@rating", rating);
            command.Parameters.Add(rating_p);


            SqlDataAdapter adapter = new SqlDataAdapter(command);

            DataSet ds = new DataSet("Anime");
            DataTable dt = new DataTable("airtime");
            ds.Tables.Add(dt);
            adapter.Fill(ds.Tables["airtime"]);

            ds.WriteXml("airtime.xml");
            Console.WriteLine("Данные сохранены в файл");

            return false;
        }

        public void GuessTheStudioComment()
        {
            Console.WriteLine("Давайте сыграем в игру. Вы угадаваете студию" +
                " по году основания и месторасположению. Если ответите неверно, я удалю" +
                " её из таблицы!");
        }
        
        // disconnected
        public bool GuessTheStudio(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();

            command.CommandText = String.Format("select * from studio");
            command.Connection = connection;
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];
            int count = 0;
            Random rand = new Random();
            object[] arr = new object[100];
            while (count < 10)
            {
                count++;
                int i = -1;
                int work = 0;
                foreach (DataRow row in dt.Rows)
                {
                    i++;
                    if (row.RowState == DataRowState.Deleted)
                    {
                        continue;
                    } else
                    {
                        work = i;
                    }
                    if (rand.Next(0, 100) > 60)
                    {
                        work = i;
                        break;
                    }
                }
                DataRow kill = dt.Rows[work];
                var cells = dt.Rows[work].ItemArray;
                int p = 0;
                foreach (object cell in cells)
                {
                    arr[p] = cell;
                    p++;
                }

                Console.WriteLine("Год основания: {0}, город: {1}. " +
                    "Итак, о какой студии идёт речь?",
                    arr[3].ToString(), arr[4].ToString());
                string name = Console.ReadLine();
                if (name == arr[2].ToString())
                {
                    Console.WriteLine("Верно!");
                }
                else
                {
                    kill.Delete();
                    Console.WriteLine("Мимо! Удаляю студию {0}!",
                        arr[2].ToString());
                }
            }

            Console.WriteLine("Вот что осталось от локальной таблицы!");

            foreach (DataColumn column in dt.Columns)
                Console.Write("\t{0,16}", column.ColumnName);
            Console.WriteLine();
            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    continue;
                }
                // получаем все ячейки строки
                var cells = row.ItemArray;
                foreach (object cell in cells)
                    Console.Write("\t{0, 16}", cell);
                Console.WriteLine();
            }
            return false;
        }

        public void GetTitlesWithEpisodesMoreThenComment()
        {
            Console.WriteLine("Эта подпрограмма выводит тайтлы, количество эпизодов " +
                "которых выше указанного.");
        }
        
        public bool GetTitlesWithEpisodesMoreThen(SqlConnection connection)
        {
            int episodes = get_int("Введите количество эпизодов(от 1 до 9999):",
                1, 9999, "Указанное число эпизодов за пределами моего понимания, попробуйте ещё раз:");

            if (episodes == -100)
            {
                return false;
            }

            SqlCommand command = new SqlCommand();

            command.CommandText = String.Format("select name_local," +
                " episodes from title where episodes > @episodes");
            command.Connection = connection;
            
            SqlParameter episodes_p = new SqlParameter("@episodes", episodes);
            command.Parameters.Add(episodes_p);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];
            int count = 0;
            foreach (DataRow row in dt.Rows)
            {
                // получаем все ячейки строки
                var cells = row.ItemArray;
                foreach (object cell in cells)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
                count++;
            }
            if (count == 0)
            {
                Console.WriteLine("Увы, ничего не нашлось, попробуйте указать" +
                    " меньшее количество эпизодов.");
            }
            return true;
        }

        private void showStudios()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                // Открываем подключение

                connection.Open();

                SqlCommand command = new SqlCommand();

                command.CommandText = String.Format("select * from studio");
                command.Connection = connection;

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["id_studio"] + " " + reader["name_studio"] + " " + reader["year_"]);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);
            }
            finally
            {
                // закрываем подключение
                connection.Close();
            }
        }

        //https://professorweb.ru/my/csharp/charp_theory/level10/10_4.php
        public void TryConnect(Action comment,
            Func<SqlConnection, bool> action)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;
            // Создание подключения

            bool never_stop = true;

            comment();

            while (never_stop)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    // Открываем подключение
                    connection.Open();
                    never_stop = action(connection);
                    if (never_stop)
                        never_stop = repeat();
                }
                catch (SqlException ex)
                {
                    never_stop = false;
                    Console.WriteLine("Что то сломалось..." + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void InsertTitleComment()
        {
            Console.WriteLine("Вызов хранимой процедуры, которая автоматически" +
                " создает описание тайтла при его добавлении в таблицу!");
        }

        public bool InsertTitle(SqlConnection connection)
        {
            Console.WriteLine("Введите название тайтла");
            string name = Console.ReadLine();

            int type_id = get_int("Введите id типа от 1 до 4",
                1, 4, "Введено некорректное число. Введите число от " +
                "1 до 4, либо отрицательное для выхода из подпрограммы.");

            if (type_id == -100)
                return false;

            int episodes = get_int("Введите количество эпизодов от 1 до 9999",
               1, 9999, "Введено некорректное число. Введите число от " +
               "1 до 9999, либо отрицательное для выхода из подпрограммы.");

            if (episodes == -100)
                return false;

            int duration = get_int("Введите продолжительность серии от 1 до 999 минут",
              1, 999, "Введено некорректное число. Введите число от " +
              "1 до 999, либо отрицательное для выхода из подпрограммы.");

            if (duration == -100)
                return false;

            SqlCommand command = new SqlCommand();

            command.Connection = connection;
            
            command.CommandText = String.Format("select * from title");
            
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.InsertCommand = new SqlCommand("CreateTitleWithDescription",
                connection);

            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@name_origin",
                SqlDbType.NVarChar, 100, "name_origin"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@id_type",
                SqlDbType.Int, 0, "id_type"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@episodes",
                SqlDbType.Int, 0, "episodes"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@duration",
                SqlDbType.Int, 0, "duration"));
            
            SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
            parameter.Direction = ParameterDirection.Output;

            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            DataRow newRow = dt.NewRow();
            newRow["name_origin"] = name;
            newRow["id_type"] = type_id;
            newRow["episodes"] = episodes;
            newRow["duration"] = duration;
            dt.Rows.Add(newRow);

            adapter.Update(ds);
            ds.AcceptChanges();

            ShowLast3Titles(connection);

            return true;
        }

        public int get_int(string comment, int a, int b,
            string please )
        {
            Console.WriteLine(comment);
            int num = a - b;
            while (!(num >= a && num <= b))
            {
                if (!Int32.TryParse(Console.ReadLine(), out num))
                {
                    num = -100;
                    break;
                }
                if (num < 0)
                {
                    num = -100;
                    break;
                }

                if (!(num >= a && num <= b))
                {
                    Console.WriteLine(please);
                }
            }
            return num;
        }

        public void InsertTypeComment()
        {
            Console.WriteLine("Создайте свой собственный тип тайтлов!");
        }
        public bool InsertType(SqlConnection connection)
        {
            Console.WriteLine("Введите название нового типа");
            string name = Console.ReadLine();
           
            SqlCommand command = new SqlCommand();

            command.Connection = connection;

            command.CommandText = "DELETE type_anime " +
               "WHERE id_type > 4";
            command.ExecuteNonQuery();

            
            command.CommandText = String.Format("select * from type_anime");

            SqlParameter name_p = new SqlParameter("@name", name);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            DataTable dt = ds.Tables[0];

            DataRow newRow = dt.NewRow();
            newRow["name_type"] = name_p.SqlValue;
            dt.Rows.Add(newRow);

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(dt);

            ds.Clear();
            adapter.Fill(ds);

            foreach (DataColumn column in dt.Columns)
                Console.Write("\t{0}", column.ColumnName);
            Console.WriteLine();
            foreach (DataRow row in dt.Rows)
            {
                var cells = row.ItemArray;
                foreach (object cell in cells)
                    Console.Write("\t{0}", cell);
                Console.WriteLine();
            }
            return true;
        }

        public void AdderComment()
        {
            Console.WriteLine("В этой функции вам предлагается последовательно," +
                " строка за строкой, добавлять тестовые строки, а в конце" +
                " будет показано чудесное поэтапное их удаление" +
                "(откатом транзакций)!");
        }

        public bool Adder(SqlConnection connection)
        {
            SqlTransaction transaction = connection.BeginTransaction();

            SqlCommand command = connection.CreateCommand();
            command.Transaction = transaction;

            bool delete = true;
            
            if (delete)
            {
                command.CommandText = "DELETE studio " +
                "WHERE id_studio > 20";
                command.ExecuteNonQuery();
                transaction.Save("0");
            }
            
            int i = 0;
            while (delete && i < 20)
            {
                i++;
                
                command.CommandText = "INSERT studio(name_studio, year_," +
               " center_studio, site_studio) values ('Когда наступит'," +
               " 2019, '?', '!?')";
               
                command.ExecuteNonQuery();

                transaction.Save(i.ToString());
                reader(command);

                Console.WriteLine("Продолжить добавление?(1/0):");
                int cont = 1;
                if (!Int32.TryParse(Console.ReadLine(), out cont))
                {
                    delete = false;
                }
                if (cont != 1)
                {
                    delete = false;
                }
            }

            Console.WriteLine("Восстановление!");

            while (i > 0)
            {
                i--;
                transaction.Rollback(i.ToString());
                Console.WriteLine("------------------------");
                Console.WriteLine("Шаг назад");
                Console.WriteLine("------------------------");
                reader(command);
            }
            return true;
        }

        private static void reader(SqlCommand command)
        {
            command.CommandText = "select * from studio ";
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {
                // выводим названия столбцов
                Console.WriteLine("{0, 20} {1, 5} {2, 10}",
                    reader.GetName(2), reader.GetName(3),
                    reader.GetName(4));

                while (reader.Read()) // построчно считываем данные
                {
                    string name = reader.GetString(2);
                    int year = reader.GetInt32(3);
                    string center = reader.GetString(4);

                    Console.WriteLine("{0,20} {1, 5} {2, 10}",
                        name, year, center);
                }

            }
            reader.Close();
        }

        public void JustInsertInTitleComment()
        {
            Console.WriteLine("Данная функция добавляет строку в таблицу Тайтлов.");
        }
        public bool JustInsertInTitle(SqlConnection connection)
        {
            Console.WriteLine("Введите название на английском:");
            string name_origin = Console.ReadLine();

            Console.WriteLine("Введите название на русском:");
            string name_local = Console.ReadLine();

            Console.WriteLine("Опишите тайтл:");
            string description = Console.ReadLine();

            Console.WriteLine("Укажите тип(от 1 до 4):");
            int type = -1;

            while (type < 0 || type > 4)
            {
                if (!Int32.TryParse(Console.ReadLine(), out type))
                {
                    Console.WriteLine("Возвращаемся назад.");
                    return false;
                }
                if (type < 0 || type > 4)
                {
                    Console.WriteLine("Введено некорректное число. Попробуем еще раз?(Введите не число, чтобы покинуть подпрограмму)");
                }
            }

            Console.WriteLine("Укажите количество эпизодов(от 1 до 9999):");
            int episodes = -1;

            while (episodes < 1 || episodes > 9999)
            {
                if (!Int32.TryParse(Console.ReadLine(), out episodes))
                {
                    Console.WriteLine("Возвращаемся назад.");
                    return false;
                }
                if (episodes < 1 || episodes > 9999)
                {
                    Console.WriteLine("С указанным числом эпизодов явно что то не так, укажите число от 0 до 9999!");
                }
            }

            Console.WriteLine("Укажите длительность эпизода в минутах(от 1 до 999):");
            int duration = -1;

            while (duration < 1 || duration > 999)
            {
                if (!Int32.TryParse(Console.ReadLine(), out duration))
                {
                    Console.WriteLine("Возвращаемся назад.");
                    return false;
                }
                if (duration < 1 || duration > 999)
                {
                    Console.WriteLine("Пожалейте зрителей, как можно указывать подобную продолжительность?" +
                        "(Если хотите покинуть подпрограмму, введите любой символ - не цифру)");
                }
            }

            SqlCommand command = new SqlCommand("INSERT title(name_origin, name_local, description_title," +
                " id_type, episodes, duration) VALUES(@name_origin, @name_local, @description_title, @id_type," +
                " @episodes, @duration)", connection);
            // Боремся с инъекциями!
            SqlParameter name_origin_p = new SqlParameter("@name_origin", name_origin);
            command.Parameters.Add(name_origin_p);
            SqlParameter name_local_p = new SqlParameter("@name_local", name_local);
            command.Parameters.Add(name_local_p);
            SqlParameter episodes_p = new SqlParameter("@episodes", episodes);
            command.Parameters.Add(episodes_p);
            SqlParameter duration_p = new SqlParameter("@duration", duration);
            command.Parameters.Add(duration_p);
            SqlParameter type_p = new SqlParameter("@id_type", type);
            command.Parameters.Add(type_p);
            SqlParameter description_p = new SqlParameter("@description_title", description);
            command.Parameters.Add(description_p);

            command.ExecuteNonQuery();

            Console.WriteLine("Данные успешно отправлены. Перед вами последние три записи таблицы.");
            ShowLast3Titles(connection);

            return true;
        }

        public void UpdateYearStudioWhereIdIsComment()
        {
            Console.WriteLine("Данная функция обновляет год студии. Сначала выберите студию, а потом" +
                            "укажите новое значение года.");
        }

        public bool UpdateYearStudioWhereIdIs(SqlConnection connection)
        {
            //int max_size = GetSize();
            string f = string.Format("Введите id студии(от 1 до {0}):", 20);
            int id = get_int(f, 0, 20, "Обнаружено некорректное id, давайте попробуем ещё раз:");

            if (id == -100)
            {
                return false;
            }
            
            int year = get_int("Введите новый год основания(от 1900 до 2019):",
                1900, 2019, "Долой путешествия во времени," +
                " мы живём в настоящем! Укажите год с 1900 по 2019:");

            if (year == -100)
            {
                return false;
            }

            SqlParameter yearParam = new SqlParameter
            {
                ParameterName = "@year",
                Value = year
            };

            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = id
            };
            SqlCommand command = new SqlCommand("exec updateStudio @year, @id", connection);
            command.Parameters.Add(yearParam);
            command.Parameters.Add(idParam);
            var result = command.ExecuteScalar();
            showStudios();

            return true;
        }

        public void SearchAnimeWithWord()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;

            bool never_stop = true;

            while (never_stop)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    // Открываем подключение
                    connection.Open();
                    /*
                    Console.WriteLine("Свойства подключения:");
                    Console.WriteLine("\tСтрока подключения: {0}", connection.ConnectionString);
                    Console.WriteLine("\tБаза данных: {0}", connection.Database);
                    Console.WriteLine("\tСервер: {0}", connection.DataSource);
                    Console.WriteLine("\tВерсия сервера: {0}", connection.ServerVersion);
                    Console.WriteLine("\tСостояние: {0}", connection.State);
                    Console.WriteLine("\tWorkstationld: {0}", connection.WorkstationId);
                    */
                    SqlCommand command = new SqlCommand();
                    
                    Console.WriteLine("Введите ключевое слово(или фразу) для поиска:");
                    string word = Console.ReadLine();

                    command.CommandText = String.Format("select * from Title" +
                        " where description_title like '%{0}%'", word);
                    command.Connection = connection;
                    SqlDataReader reader = command.ExecuteReader();
                    Console.WriteLine("Вот что удалось найти:");
                    int i = 0;
                    while (reader.Read())
                    {
                        i++;
                        Console.WriteLine(reader["name_local"]);
                        Console.WriteLine(reader["description_title"]);
                    }
                    if (i == 0)
                    {
                        Console.WriteLine("Ан нет, не судьба, ничего не нашлось, попробовать еще раз?[1/0]");
                        int continue_ = 0;
                        if (!Int32.TryParse(Console.ReadLine(), out continue_))
                        {
                            Console.WriteLine("Будем считать, что вы отказались.");
                        }
                        if (continue_ != 1)
                        {
                            never_stop = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Итого: {0} совпадений!", i);
                        never_stop = false;
                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Что то сломалось..." + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AmountOfAnimeWithOneYearRating()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["try"].ConnectionString;

            bool never_stop = true;

            while (never_stop)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    Console.WriteLine("Выбрана функция определения количества" +
                        " тайтлов определенного возрастного рейтинга." +
                        " Выберите один из предложенных ниже вариантов");
                    Console.WriteLine("0 - 13+");
                    Console.WriteLine("1 - 18+");
                    Console.WriteLine("2 - 21+");
                    Console.WriteLine("3 - назад к другим функциям");

                    int age = 0;
                    if (!Int32.TryParse(Console.ReadLine(), out age))
                    {
                        Console.WriteLine("Предположим вы хотели вернуться к прошлым вопросам");
                        age = 3;
                    }
                    if (age < 0)
                    {
                        Console.WriteLine("Странный шаг с вашей стороны," +
                            " наверное вы хотели узнать общее количество тайтлов!");
                    }
                    else if (age > 3 && age < 18)
                    {
                        Console.WriteLine("Необходимо было написать число от 0" +
                            " до 3, но ладно уж, округлю {0} до 13+", age);
                        age = 0; 
                    }
                    else if (age >= 18 && age < 21)
                    {
                        Console.WriteLine("Необходимо было написать число" +
                            " от 0 до 3, но ладно уж, округлю {0} до 18+", age);
                        age = 1;
                    }
                    else if (age >= 21)
                    {
                        Console.WriteLine("Необходимо было написать число" +
                            " от 0 до 3, но ладно уж, округлю {0} до 21+", age);
                        age = 2;
                    }

                    string rating;
                    if (age != 3)
                    {
                        switch(age)
                        {
                            case -1:
                                rating = "r";
                                break;

                            case 0:
                                rating = "r13";
                                break;

                            case 1:
                                rating = "r18";
                                break;

                            default:
                                rating = "r21";
                                break;
                        }
                        command.CommandText = String.Format("select COUNT(year_rating)" +
                            " from airtime where year_rating like '%{0}%'", rating);
                        command.Connection = connection;
                        object count = command.ExecuteScalar();
                        Console.WriteLine("Ответ:" + count);

                        never_stop = repeat();
                    } else
                    {
                        never_stop = false;
                    }
                    
                }
                catch (SqlException ex)
                {
                    never_stop = false;
                    Console.WriteLine("Что то сломалось..." + ex.Message);
                }
                finally
                {
                    // закрываем подключение
                    connection.Close();
                }
            }
        }
    }
}
