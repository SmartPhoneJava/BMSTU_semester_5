using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace lab7_2 // XML
{
    class Program
    {

        const int EXIT = 0;
        const int READ = 1;
        const int WRITE = 2;
        const int NEW = 3;

        static void Main(string[] args)
        {
            Program launch = new Program();

            string filename = @"C:\msys64\home\BMSTU_semester_5\Lab07\titles.xml";

            int command = EXIT;
            while (command != INPUT_INT_ERROR)
            {
                launch.writeInstructions();
                command = launch.get_int("Выбор за вами. Введите число:", 0, 3, "Попробуйте все таки придерживаться " +
                  " указанных правил. Укажите число меньше 4:");
                if (command != INPUT_INT_ERROR)
                {
                    switch (command)
                    {
                        case EXIT: // Выход
                            break;
                        case READ: // Считать строки
                            launch.findSeasonsWhereThereWasAnimeWithRating(filename);
                            break;
                        case WRITE:  // Изменить строки
                            launch.FixId(filename);
                            break;
                        case NEW: // Добавить строку
                            launch.addTitle(filename);
                            break;
                    }
                }
            }
        }

        const int INPUT_INT_ERROR = -100;
        void writeInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("<1 - выйти");
            Console.WriteLine("1 - получить список сезонов," +
                " в которых выходили эпизоды выбранного возрастного рейтинга");
            Console.WriteLine("2 - исправить индексацию элементов");
            Console.WriteLine("3 - добавить новый тайтл");
        }

        void findSeasonsWhereThereWasAnimeWithRating(string filename)
        {
            
            XDocument xdoc = XDocument.Load(filename);

            int old_rating = get_int("Укажите возрастной рейтинг тайтла(от 0 до 99):",
                0, 99, "Вам ведь не может быть столько лет. " +
                  "Укажите возрастное ограничение от 0 до 100 ");

            if (old_rating == INPUT_INT_ERROR)
            {
                Console.WriteLine("Освобождаем память." +
                    " Отключаем систему. Выключаем свет. Закрываем двери.");
                return;
            }
            
            string year_rating = whatRating(old_rating);

            var ratings = xdoc.Descendants("ratings");
            var namings = xdoc.Descendants("namings");
            var id_season = xdoc.Descendants("id_season");
            var query = from rat in ratings
                        where rat.Attribute("year_rating").Value == year_rating
                        join nam in namings on rat.Parent equals nam.Parent
                        join seas in id_season on rat.Parent equals seas.Parent
                        select new { Name = nam.Attribute("local").Value,
                            ID_SEASON = seas.Attribute("id_season").Value};
            Console.WriteLine("Ниже приведен список сезонов и тайтлов" +
                " где возрастной рейтинг {0}", year_rating);

            Console.WriteLine("1 - зимний, 2 - весенний, 3 - летний, 4 - осенний");

            foreach (var q in query)
            {
                Console.WriteLine("сезон {0}, тайтл: {1, 40}", q.ID_SEASON, q.Name);
            }
        }

        void xmlPrint(IEnumerable<XElement> elems, int n)
        {
            foreach (XElement elem in elems)
            {
              
               foreach (XAttribute attr in elem.Attributes())
                {
                    Console.WriteLine();
                    for (int i = 0; i < n * 2; i++)
                    {
                        Console.Write(" ");
                    }
                    Console.Write(attr.Name + ":" + attr.Value);
                }
               Console.WriteLine();
               xmlPrint(elem.Elements(), n + 1);

            }

        }

        void showAll(string filename)
        {
            XDocument xdoc = XDocument.Load(@filename);

            xmlPrint(xdoc.Elements(), 0);
        }

        int FixId(string filename)
        {
            XDocument xdoc = XDocument.Load(filename);
            XNode node = xdoc.Root.FirstNode;

            int idx = 0;

            while (node != null)
            {
                if (node.NodeType == System.Xml.XmlNodeType.Element)
                {
                    XElement el = (XElement)node;
                    el.Attribute("id").Value = idx.ToString() + "-id";
                    idx++;
                }
                node = node.NextNode;
            }
            xdoc.Save(filename);
            Console.WriteLine("Обновленная таблица:");
            showAll(filename);
            
            return idx;
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

        string getString(string write)
        {
            Console.WriteLine(write);
            return Console.ReadLine();
        }

        string whatRating(int old_rating)
        {
            string year_rating = "r21";
            if (old_rating < 14)
            {
                year_rating = "r13";
            }
            else if (old_rating < 19)
            {
                year_rating = "r18";
            }
            return year_rating;
        }

        void getSeasonInfo(XDocument xdoc, int season_num, ref int films,
            ref int serials, ref int anounced)
        {
            var seasons = xdoc.Descendants("id_season");
            foreach (XElement season in seasons)
            {
               if (Int32.Parse(season.Attribute("id_season").Value) == season_num)
               {
                    films = Int32.Parse(season.Element("types").Attribute("films").Value);
                    serials = Int32.Parse(season.Element("types").Attribute("serials").Value);
                    anounced = Int32.Parse(season.Element("types").Attribute("anounced").Value);
                    break;
                }
            }
        }

        void addTitle(string filename)
        {
            XDocument xdoc = XDocument.Load(filename);

            string origin = getString("Введите имя тайтла на английском:");
            string local = getString("Введите имя тайтла на русском:");

            int r = get_int("Укажите возрастной рейтинг(оставьте поле пустым, чтобы пропустить):",
                1, 100, "Укажите число меньше 1, чтобы пропустить " +
                  " или число от 1 до 100 чтобы продолжить.");

            if (r == INPUT_INT_ERROR)
            {
                r = 18;
            }

            int s = get_int("Укажите сезон:",
               1, 100, "Укажите число меньше 1, чтобы пропустить " +
                 " или число от 1 до 4 чтобы продолжить.");

            if (s == INPUT_INT_ERROR)
            {
                s = 1;
            }

            int films = 0, serials = 0, anounced = 0;
            getSeasonInfo(xdoc, s, ref films, ref serials, ref anounced);

            XElement id = new XElement("id",
                new XAttribute("id", "-"),
                new XElement("ratings",
                    new XAttribute("user_rating", "0"),
                    new XAttribute("year_rating", whatRating(r))),
                 new XElement("namings",
                    new XAttribute("original", origin),
                    new XAttribute("local", local)),
                new XElement("id_season",
                    new XAttribute("id_season", s),
                    new XElement("types",
                        new XAttribute("films", films),
                        new XAttribute("serials", serials),
                        new XAttribute("anounced", anounced))));
            xdoc.Root.Add(id);
            xdoc.Save(filename);

            FixId(filename);
        }
    }
}



