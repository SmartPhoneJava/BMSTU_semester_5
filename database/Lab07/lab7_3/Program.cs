using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace lab7_3
{
    [Table(Name = "studio")]
    public class studio
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id_studio { get; set; }
        [Column(Name = "helper_id")]
        public int helper_id { get; set; }
        [Column(Name = "name_studio")]
        public string name_studio { get; set; }
        [Column(Name = "year_")]
        public int year_ { get; set; }
        [Column(Name = "center_studio")]
        public string center_studio { get; set; }
        [Column(Name = "site_studio")]
        public string site_studio { get; set; }
    }

    [Table(Name = "type_anime")]
    public class type_anime
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id_type { get; set; }
        [Column(Name = "name_type")]
        public string name_type { get; set; }
    }

    [Table(Name = "title")]
    public class title
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int id_title { get; set; }
        [Column(Name = "name_origin")]
        public string name_origin { get; set; }
        [Column(Name = "name_local")]
        public string name_local { get; set; }
        [Column(Name = "description_title")]
        public string description_title { get; set; }
        [Column(Name = "id_type")]
        public int id_type { get; set; }
        [Column(Name = "episodes")]
        public int episodes { get; set; }
        [Column(Name = "duration")]
        public int duration { get; set; }
    }

    [Table(Name = "airtime")]
    public class airtime
    {
        [Column(Name = "id_studio")]
        public int id_studio { get; set; }
        [Column(Name = "id_season")]
        public int id_season { get; set; }
        [Column(Name = "id_title")]
        public int id_title { get; set; }
        [Column(Name = "status")]
        public string status { get; set; }
        [Column(Name = "begin_date")]
        public DateTime begin_date { get; set; }
        [Column(Name = "finish_date")]
        public DateTime finish_date { get; set; }
        [Column(Name = "user_rating")]
        public double user_rating { get; set; }
        [Column(Name = "year_rating")]
        public string year_rating { get; set; }

    }

    class MDbContext : DataContext
    {
        public MDbContext(string dbname) : base(dbname) { }

        [Function(Name = "dbo.updateStudio")]
        [return: Parameter(DbType = "Int")]
        public int updateStudio([Parameter(Name = "id", DbType = "Int")] int id,
            [Parameter(Name = "year", DbType = "Int")] int year)
        {
            IExecuteResult result = this.ExecuteMethodCall(this,
                ((MethodInfo)(MethodInfo.GetCurrentMethod())), id, year);
            return ((int)(result.ReturnValue));
        }
    }

    class Program
    {
        static void Main(string[] args) 
        {
            Program launch = new Program();

            int command = 0;
            while (command != INPUT_INT_ERROR)
            {
                launch.writeInstructions();
                command = launch.get_int("Выбор за вами. Введите число:", 0, 7, "Попробуйте все таки придерживаться " +
                  " указанных правил. Укажите число меньше 8:");
                if (command != INPUT_INT_ERROR)
                {
                    switch (command)
                    {
                        case 0: // Выход
                            break;
                        case 1: // Просто получить выборку из таблицы
                            launch.GetAnimeLongerThen();
                            break;
                        case 2: // Использование джоинов
                            launch.GettTitlesStudio();
                            break;
                        case 3: // Обновление
                            launch.UpdateYearStudioWhereIdIs();
                            break;
                        case 4: // Добавление
                            launch.addNewTypesOfAnime();
                            break;
                        case 5: // Удаление
                            launch.deleteNewTypes();
                            break;
                        case 6: // Вызов хранимой процедуры
                            launch.updateStudioAgain();
                            break;
                    }
                }
            }

            Console.Read();
        }

        const int INPUT_INT_ERROR = -100;
        void writeInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("<1 - выйти");
            Console.WriteLine("1 - получить тайтл с количеством эпизодов больше заданного и продолжительностью больше заданной");
            Console.WriteLine("2 - определить название студии, которая выпустила на свет выбранный тайтл");
            Console.WriteLine("3 - обновить год основания выбранной студии");
            Console.WriteLine("4 - создать новый тип аниме");
            Console.WriteLine("5 - удалить последний новый тип");
            Console.WriteLine("6 - обновить год основания выбранной студии с помощью процедуры");
        }

        public int get_int(string comment, int a, int b,
           string please)
        {
            Console.WriteLine(comment);
            int num = a - b;
            while (!(num >= a && num <= b))
            {
                if (!Int32.TryParse(Console.ReadLine(), out num))
                {
                    num = INPUT_INT_ERROR;
                    break;
                }
                if (num < 0)
                {
                    num = INPUT_INT_ERROR;
                    break;
                }

                if (!(num >= a && num <= b))
                {
                    Console.WriteLine(please);
                }
            }
            return num;
        }

        void GetAnimeLongerThen()
        {
            string connectionString = @"Data Source=DESKTOP-L6KLS3C\SQLEXPRESS01;Initial Catalog=anime;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            int duration = get_int("Введите продолжительность серии от 1 до 999 минут",
              1, 999, "Введено некорректное число. Введите число от " +
              "1 до 999, либо отрицательное для выхода из подпрограммы.");

            if (duration == INPUT_INT_ERROR)
            {
                Console.WriteLine("Видимо вам хочется выйти, выходим");
            }

            int episodes = get_int("Введите количество эпизодов(от 1 до 9999):",
                1, 9999, "Указанное число эпизодов за пределами моего понимания, попробуйте ещё раз:");
            ;

            if (episodes == INPUT_INT_ERROR)
            {
                Console.WriteLine("На нет суда нет");
            }

            var query = from p in db.GetTable<title>()
                        where p.duration >= duration &&
                        p.episodes >= episodes
                        select p;

            Console.WriteLine("Таблица тайтлов, где " +
                " эпизодов больше {0}, а продолжительность" +
                " одного больше {1} минут", episodes, duration);
            foreach (var p in query)
            {
                Console.WriteLine("{0, 60} {1, 6} {2,6}",
                    p.name_local, p.episodes, p.duration);
            }
        }

        void GettTitlesStudio()
        {
            string connectionString = @"Data Source=DESKTOP-L6KLS3C\SQLEXPRESS01;Initial Catalog=anime;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            var pass = from p in db.GetTable<title>()
                       select p;

            Console.WriteLine("Список доступных тайтлов:");

            foreach (var v in pass)
            {
                Console.WriteLine("{0,40}", v.name_local);
            }

            Console.WriteLine("Введите название тайтла:");
            string name = Console.ReadLine();

            Table<title> titles = db.GetTable<title>();
            Table<studio> studios = db.GetTable<studio>();
            Table<airtime> airtimes = db.GetTable<airtime>();
            var pass2 = from p in airtimes
                        join tit in titles on p.id_title equals tit.id_title
                        join studio in studios on p.id_studio equals studio.id_studio
                        where tit.name_local == name
                        select new { Titl = tit.name_local, Studio = studio.name_studio };

            int i = 0;
            foreach (var v in pass2)
            {
                i++;
                Console.WriteLine("Данный шедевр({0}) создан студией {1}", v.Titl, v.Studio);
            }
            if (i == 0)
            {
                Console.WriteLine("Не удалось найти тайтл '{0}'", name);
            }

            Console.WriteLine();
        }
        

        void UpdateYearStudioWhereIdIs()
        {
            string connectionString = @"Data Source=DESKTOP-L6KLS3C\SQLEXPRESS01;Initial Catalog=anime;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            Console.WriteLine("Список доступных студий");
            var query1 = from t in db.GetTable<studio>()
                        select t;
            foreach (var q in query1) {
                Console.WriteLine("{0,3} {1,20} {2,4}", q.id_studio, q.name_studio, q.year_);
            }

            string f = string.Format("Введите id студии(от 1 до {0}):", 20);
            int id = get_int(f, 0, 20, "Обнаружено некорректное id, давайте попробуем ещё раз:");

            if (id == INPUT_INT_ERROR)
            {
                Console.WriteLine("Аварийное отключение.");
                return;
            }

            int year = get_int("Введите новый год основания(от 1900 до 2019):",
                1900, 2019, "Долой путешествия во времени," +
                " мы живём в настоящем! Укажите год с 1900 по 2019:");

            if (year == -100)
            {
                Console.WriteLine("Возвращаемся обратно в меню.");
                return;
            }

            var query = from t in db.GetTable<studio>()
                        where t.id_studio == id
                        select t;
            foreach (var q in query) q.year_ = year;
            db.SubmitChanges();

            Console.WriteLine("Обновлённый список студий");
            var query2 = from t in db.GetTable<studio>()
                         select t;
            foreach (var q in query2)
            {
                Console.WriteLine("{0,3} {1,20} {2,4}", q.id_studio, q.name_studio, q.year_);
            }
            
        }

        void addNewTypesOfAnime()
        {
            string connectionString = @"Data Source=DESKTOP-L6KLS3C\SQLEXPRESS01;Initial Catalog=anime;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            int id_type;

            Console.WriteLine("Cписок типов");
            var old_types = from t in db.GetTable<type_anime>()
                         select t;
            foreach (var q in old_types)
            {
                Console.WriteLine("{0,4} {1,20}", q.id_type, q.name_type);
            }

            var pass2 = from p in db.GetTable<type_anime>()
                        group p by p.name_type into g
                        select new { id_type = g.Max(k => k.id_type) };
            id_type = pass2.First().id_type;



            Console.WriteLine("Придумайте название своего нового типа аниме!");
            string name = Console.ReadLine();

            db.GetTable<type_anime>().InsertOnSubmit(new type_anime()
            {
                id_type = id_type + 1,
                name_type = name
            });

            db.SubmitChanges();

            Console.WriteLine();
            Console.WriteLine("Обновленный список типов");
            var new_types = from t in db.GetTable<type_anime>()
                            select t;
            foreach (var q in new_types)
            {
                Console.WriteLine("{0,4} {1,20}", q.id_type, q.name_type);
            }
        }

        void deleteNewTypes()
        {
            string connectionString = @"Data Source=DESKTOP-L6KLS3C\SQLEXPRESS01;Initial Catalog=anime;Integrated Security=True";
            DataContext db = new DataContext(connectionString);

            Console.WriteLine("Список типов");
            var old_types = from t in db.GetTable<type_anime>()
                            select t;
            foreach (var q in old_types)
            {
                Console.WriteLine("{0,4} {1,20}", q.id_type, q.name_type);
            }

            var item = (from t in db.GetTable<type_anime>() where t.id_type > 5 select t).First();
            db.GetTable<type_anime>().DeleteOnSubmit(item);
            db.SubmitChanges();

            Console.WriteLine("Обновленный список типов");
            var new_types = from t in db.GetTable<type_anime>()
                            select t;
            foreach (var q in new_types)
            {
                Console.WriteLine("{0,4} {1,20}", q.id_type, q.name_type);
            }
        }
        
        void updateStudioAgain()
        {
            string connectionString = @"Data Source=DESKTOP-L6KLS3C\SQLEXPRESS01;Initial Catalog=anime;Integrated Security=True";
            MDbContext db = new MDbContext(connectionString);
            
            Console.WriteLine("Список доступных студий");
            var query1 = from t in db.GetTable<studio>()
                         select t;
            foreach (var q in query1)
            {
                Console.WriteLine("{0,3} {1,20} {2,4}", q.id_studio, q.name_studio, q.year_);
            }

            string f = string.Format("Введите id студии(от 1 до {0}):", 20);
            int id = get_int(f, 0, 20, "Обнаружено некорректное id, давайте попробуем ещё раз:");

            if (id == INPUT_INT_ERROR)
            {
                Console.WriteLine("Аварийное отключение.");
                return;
            }

            int year = get_int("Введите новый год основания(от 1900 до 2019):",
                1900, 2019, "Долой путешествия во времени," +
                " мы живём в настоящем! Укажите год с 1900 по 2019:");

            if (year == -100)
            {
                Console.WriteLine("Возвращаемся обратно в меню.");
                return;
            }

            db.updateStudio(id, year);
            db.SubmitChanges();

            Console.WriteLine("Обновлённый список студий");
            var query2 = from t in db.GetTable<studio>()
                         select t;
            foreach (var q in query2)
            {
                Console.WriteLine("{0,3} {1,20} {2,4}", q.id_studio, q.name_studio, q.year_);
            }

        }
    }
}
