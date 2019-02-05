using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Lab7_1
{
    [Serializable]
    public class Title
    {
        public Title(int id, string name, string name_local, 
            string desr, int id_type, int episodes, int duration)
        {
            this.Id = id;
            this.Name = name;
            this.Name_local = name_local;
            this.Desr = desr;
            this.ID_type = id_type;
            this.Episodes = episodes;
            this.Duration = duration;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_local { get; set; }
        public string Desr { get; set; }
        public int ID_type { get; set; }
        public int Episodes { get; set; }
        public int Duration { get; set; }
    }

    public class Studio
    {
        public Studio(int id, string name, int year, string center_studio, string site_studio)
        {
            this.Id = id;
            this.Name = name;
            this.Year = year;
            this.Center_studio = center_studio;
            this.Site_studio = site_studio;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public string Center_studio { get; set; }
        public string Site_studio { get; set; }
    }

    public class AirTime
    {
        public AirTime(int id, int studio_id, int title_id)
        {
            this.Id = id;
            this.Studio_id = studio_id;
            this.Title_id = title_id;
        }

        public int Id { get; set; }
        public int Studio_id { get; set; }
        public int Title_id { get; set; }
    }

    public class DataBase
    {
        private static List<Title> titles;
        private static List<Studio> studios;
        private static List<AirTime> airtimes;

        public static IList<AirTime> GetAirtime()
        {
            airtimes = null;
            if (airtimes == null)
            {
                airtimes = new List<AirTime>(18);
                airtimes.Add(new AirTime(1, 15, 1));
                airtimes.Add(new AirTime(2, 1, 2));
                airtimes.Add(new AirTime(3, 14, 3));
                airtimes.Add(new AirTime(4, 13, 4));
                airtimes.Add(new AirTime(5, 1, 5));
                airtimes.Add(new AirTime(6, 1, 6));
                airtimes.Add(new AirTime(7, 16, 7));
                airtimes.Add(new AirTime(8, 16, 8));
                airtimes.Add(new AirTime(9, 2, 9));
                airtimes.Add(new AirTime(10, 13, 10));
                airtimes.Add(new AirTime(11, 6, 11));
                airtimes.Add(new AirTime(12, 12, 12));
                airtimes.Add(new AirTime(13, 6, 13));
                airtimes.Add(new AirTime(14, 8, 14));
                airtimes.Add(new AirTime(15, 5, 15));
                airtimes.Add(new AirTime(16, 2, 16));
                airtimes.Add(new AirTime(17, 6, 17));
                airtimes.Add(new AirTime(18, 8, 18));
            }
            return airtimes;
        }

        public static IList<Title> GetTitles()
        {
            titles = null;
            if (titles == null)
            {
                titles = new List<Title>(18);
                titles.Add(new Title(1, "aaa", "Аватар короля", "Крутой фильм1", 4,  2,   60));
                titles.Add(new Title(2, "bbb", "Актёры ослеплённого города", "Крутой фильм2", 3, 1,   46));
                titles.Add(new Title(3, "ccc", "Ангельские ритмы!", "Крутой фильм3", 3 , 13 , 120));
                titles.Add(new Title(4, "ddd", "Баракамон", "Крутой фильм1", 4  ,12 , 120));
                titles.Add(new Title(5, "eee", "Бездомный бог", "Крутой фильм2", 1,   8,   120));
                titles.Add(new Title(6, "fff", "Безумный азарт", "Крутой фильм3", 1, 12,  60));
                titles.Add(new Title(7,  "aaa", "Белый ящик", "Крутой фильм1", 2 ,   2,   60));
                titles.Add(new Title(8,  "bbb", "Большой магический перевал", "Крутой фильм2", 2  ,  22,  46));
                titles.Add(new Title(9,  "ccc", "Брошенный кролик", "Крутой фильм3", 4  ,22  ,46));
                titles.Add(new Title(10, "ddd", "В воскресенье даже Бог отдыхает", "Крутой фильм1", 2 ,  12,  24));
                titles.Add(new Title(11, "eee", "Вайолет Эвергарден", "Крутой фильм2", 1  ,  1  , 60));
                titles.Add(new Title(12, "fff", "Великолепный парк Амаги", "Крутой фильм3", 3  , 12,  60));
                titles.Add(new Title(13, "fff", "Волчица и пряности", "Крутой фильм1", 3   , 22,  13));
                titles.Add(new Title(14,  "aaa", "Восточный Эдем", "Крутой фильм2", 3   ,13 , 60));
                titles.Add(new Title(15,  "bbb", "Врата: Там бьются наши воины", "Крутой фильм3", 4 ,2 ,  13));
                titles.Add(new Title(16,  "ccc", "Габриэль бросает школу", "Крутой фильм1", 1 ,   13 , 13));
                titles.Add(new Title(17,  "ddd", "Геймеры!", "Крутой фильм2", 2, 8  , 46));
                titles.Add(new Title(18, "eee", "Город в котором меня нет", "Крутой фильм3", 2 , 13 , 46));
            }                             
            return titles;
        }

        public static IList<Studio> GetStudios()
        {
            studios = null;
            if (studios == null)
            {
                studios = new List<Studio>(16);
                studios.Add(new Studio(1, "IDEN FILMS", 2000, "Shizuoka", "F@gmail.ru"));
                studios.Add(new Studio(2, "Studio DEEN", 1989, "Sapporo", "Fw@gmail.ru"));
                studios.Add(new Studio(3, "8bit", 1986, "Kyoto", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(4, "A-1 Pictures Inc.", 1990, "Kyoto", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(5, "AIC", 1986, "Kawasaki", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(6, "Brains Base", 1989, "Tokyo", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(7, "Dogakobo", 1993, "Nagoya", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(8, "J.C.Staff", 1984, "Shizuoka", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(9, "Kyoto Animation", 1990, "Kawasaki","Feeeeeee@gmail.ru"));
                studios.Add(new Studio(10, "Lerche", 2018, "Nagoya", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(11, "Madhouse Studios", 1989, "Fukuoka","Feeeeeee@gmail.ru"));
                studios.Add(new Studio(12, "Normad", 1999, "Shizuoka", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(13, "P.A.Works", 1998, "Sapporo","Feeeeeee@gmail.ru"));
                studios.Add(new Studio(14, "Production I.G.", 1998, "Nagoya","Feeeeeee@gmail.ru"));
                studios.Add(new Studio(15, "Silver Link", 1999, "Fukuoka", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(16, "SIGNAL MD", 1989, "Nagoya", "Feeeeeee@gmail.ru"));
                studios.Add(new Studio(17, "feel.", 1998, "Shizuoka", "Feeeeeee@gmail.ru"));
            }
            return studios;
        }
    }





    class Program
    {
        const int INPUT_INT_ERROR = -100;
        void writeInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("<1 - выйти");
            Console.WriteLine("1 - получить тайтл с количеством эпизодов больше заданного и продолжительностью больше заданной");
            Console.WriteLine("2 - определить название студии, которая выпустила на свет выбранный тайтл");
            Console.WriteLine("3 - определить среднюю продолжительность тайтлов, выпущенных выбранной студией");
            Console.WriteLine("4 - выбрать аниме в зависимости от количества свободного времени");
            Console.WriteLine("5 - получить отсортированный список студий");
        }

        static void Main(string[] args)
        {
            Program launch = new Program();
            int command = 0;
            while (command != INPUT_INT_ERROR)
            {
                launch.writeInstructions();
                command = launch.get_int("Выбор за вами. Введите число:", 0, 5, "Попробуйте все таки придерживаться " +
                  " указанных правил. Укажите число меньше 6:");
                if (command != INPUT_INT_ERROR)
                {
                    switch (command)
                    {
                        case 0: // Выход
                            break;
                        case 1: // Просто получить выборку из таблицы
                            launch.GetAnimeLongerThen();
                            break;
                        case 2:  // Использование джоинов
                            launch.GettTitlesStudio();
                            break;
                        case 3: // Использование скалярных функций
                            launch.GetAvgDurationOfStudio();
                            break;
                        case 4: // Введение переменных
                            launch.GetSpendingTime();
                            break;
                        case 5: // Сортировка
                            launch.GetStudiosOrderBy();
                            break;
                    }
                }
            }

            Console.ReadLine();
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
            int duration = get_int("Введите продолжительность серии от 1 до 999 минут",
              1, 999, "Введено некорректное число. Введите число от " +
              "1 до 999, либо отрицательное для выхода из подпрограммы.");

            if (duration == INPUT_INT_ERROR)
            {
                Console.WriteLine("Видимо вам хочется выйти, выходим");
                return;
            }

            int episodes = get_int("Введите количество эпизодов(от 1 до 9999):",
                1, 9999, "Указанное число эпизодов за пределами моего понимания, попробуйте ещё раз:");
            ;

            if (episodes == INPUT_INT_ERROR)
            {
                Console.WriteLine("На нет суда нет");
                return;
            }
            IEnumerable<Title> i;

            i = from p in DataBase.GetTitles()
                where p.Duration >= duration && 
                p.Episodes >= episodes
                select p;

            Console.WriteLine("Таблица тайтлов, где " +
                " эпизодов больше {0}, а продолжительность" +
                " одного больше {1} минут", episodes, duration);
            foreach (Title p in i)
            {
                Console.WriteLine("{0, 40} {1, 6} {2,6}",
                    p.Name_local, p.Episodes, p.Duration);
            }
        }

        void GettTitlesStudio()
        {
            var pass = from p in DataBase.GetTitles()
                       select p;

            Console.WriteLine("Список доступных тайтлов:");
           
            foreach (var v in pass)
            {
                Console.WriteLine("{0,40}", v.Name_local);
            }

            Console.WriteLine("Введите название тайтла:");
            string name = Console.ReadLine();

            IList<Title> titles = DataBase.GetTitles();
            IList<Studio> studios = DataBase.GetStudios();
            var pass2 = from p in DataBase.GetAirtime()
                       join tit in titles on p.Title_id equals tit.Id
                       join studio in studios on p.Studio_id equals studio.Id
                       where tit.Name_local == name
                       select new { Titl = tit.Name_local, Studio = studio.Name };

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

        void GetAvgDurationOfStudio()
        {

            var pass = from p in DataBase.GetStudios()
                       select p;
            Console.WriteLine("Список доступных студий:");

            foreach (var v in pass)
            {
                Console.WriteLine("{0,40}", v.Name);
            }

            Console.WriteLine("Введите название студии:");
            string name = Console.ReadLine();

            IList<Title> titles = DataBase.GetTitles();
            IList<Studio> studios = DataBase.GetStudios();
            var pass2 = from p in DataBase.GetAirtime()
                        join tit in titles on p.Title_id equals tit.Id
                        join studio in studios on p.Studio_id equals studio.Id
                        where studio.Name == name
                        group tit by studio.Name into g
                        select new { StudioName = g.Key, AvgGross = g.Average(k => k.Duration) };

            var pass1 = from p in DataBase.GetTitles()
                       group p by p.Duration into g
                       select new { ID = g.Key, AvgGross = g.Average(k => k.Episodes) };

            int i = 0;
            foreach (var p in pass2)
            {
                i++;
                Console.WriteLine("Средняя продолжительность эпизода от " +
                    "студии {0} составляет {1} минут.", p.StudioName, p.AvgGross);
            }
            if (i == 0)
            {
                Console.WriteLine("Студия {0} малоизвестна, её сериалов нет в нашей базе данных.", name);
            }
        }

        void GetSpendingTime()
        {
            int spend = get_int("Введите количество часов(от 0 до 30):", 0, 30, " Слишком много часов, откуда" +
                  " у вас столько времени?!");
            var pass = from p in DataBase.GetTitles()
                       let time = p.Duration * p.Episodes / 60
                       where time > spend 
                       select new { NameLocal = p.Name_local , Time = time,
                           Ep = p.Episodes, Dur = p.Duration};

            Console.WriteLine("Список тайтлов, для полного просмотра которых " +
                   "требуется более {0} часов.", spend);
            int i = 0;
            foreach (var p in pass)
            {
                i++;
                Console.WriteLine("{0,40} {1,5} часов = {2} * {3} / 60", p.NameLocal, p.Time, p.Dur, p.Ep);
            }
            if (i == 0)
            {
                Console.WriteLine("Вот это да, у вас достаточно" +
                    " времени, чтобы посмотреть любой тайтл!");
            }
        }


        void GetStudiosOrderBy()
        {
            var pass = from p in DataBase.GetStudios()
                       orderby p.Center_studio ascending
                       select new { Name = p.Name, Center = p.Center_studio };

            Console.WriteLine("Просто отсортируем список студий по месту обитания. " +
                "Первый столбец - название студии, второй - название города ");
            foreach (var p in pass)
            {
                Console.WriteLine("{0,20} {1,20}", p.Name, p.Center);
            }
        }


    }

}
