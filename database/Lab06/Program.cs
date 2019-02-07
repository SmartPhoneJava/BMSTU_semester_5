using System;
using System.Xml;
using System.IO;

namespace Lab06
{


    class XmlTool
    {
        private XmlDocument doc;

        const int NOERROR = 0;
        const int NOT_INT = -1;
        const int NO_TAG = -2;
        const int NO_ROOT = -3;
        const int NO_INSTRUCTION = -4;
        const int INPUT_INT_ERROR = -100;

        static public int get_int(string comment, int a, int b,
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

        public XmlTool()
        {
            this.doc = new XmlDocument();
        }

        //first
        public void readXml(String path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            doc.Load(file);

            file.Close();
        }

        private int getValue(XmlNode node, ref int to)
        {
            return Int32.TryParse(node.
                Attributes[0].Value, out to) 
                ? NOERROR : NOT_INT;
        }

        public void result(int code)
        {
            switch (code)
            {
                case NOERROR:
                    break;
                case NOT_INT:
                    Console.WriteLine("Обнаружены некорректные символы!\n");
                    break;
                case NO_TAG:
                    Console.WriteLine("Указанного тега не найдено!\n");
                    break;
                case NO_ROOT:
                    Console.WriteLine("Корневого тега root не найдено!\n");
                    break;
                case NO_INSTRUCTION:
                    Console.WriteLine("Заголовочной таблицы xml не найдено!\n");
                    break;
                default:
                    Console.WriteLine("Новая ошибка!\n");
                    break;
            }
        }

        public int findMaxMinDiffName(String tagName, 
            ref string max, ref string min)
        {
            XmlNodeList tagList = doc.GetElementsByTagName(tagName);
           
            if (tagList == null || tagList.Count == 0)
            {
                return NO_TAG;
            }
            
            int n = tagList.Count;

            Console.WriteLine("Список элементов \n");
            string for_print = max = min = tagList[0].Attributes[0].Value;
            
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine(for_print);
                if (max.CompareTo(for_print) < 0)
                {
                    max = for_print;
                }
                if (min.CompareTo(for_print) > 0)
                {
                    min = for_print;
                }
                for_print = tagList[i].Attributes[0].Value;
            }

            return NOERROR;
        }


        private void allAboutXmlElementComments(XmlNode elem)
        {
            for (int i = 0; i < elem.ChildNodes.Count; i++)
            {
                if (!Object.ReferenceEquals(elem.ChildNodes[i].GetType(), elem.GetType()))
                {
                    Console.WriteLine(elem.ChildNodes[i].Value);
                }
                allAboutXmlElementComments(elem.ChildNodes[i]);
            }
        }

        private void allAboutXmlElement(XmlNode elem, int n)
        {
            for (int i = 0; i < elem.ChildNodes.Count; i++)
            {
                if (Object.ReferenceEquals(elem.ChildNodes[i].GetType(), elem.GetType()))
                {
                    foreach (XmlAttribute attr in elem.ChildNodes[i].Attributes)
                    {
                        Console.WriteLine();
                        for (int j = 0; j < n * 2; j++)
                        {
                            Console.Write(" ");
                        }
                        Console.Write(attr.Name + ":" + attr.Value);
                    }
                    Console.WriteLine();
                    allAboutXmlElement(elem.ChildNodes[i], n + 1);
                }
            }
        }

        public int getXMLInfo() {
            XmlNodeList tagList = doc.GetElementsByTagName("id");
            Console.WriteLine();
            int t_count = tagList.Count;
            int choose = get_int("Введите id тайтла (Доступны от 1 до " + t_count + ")",
                1, t_count, "Попробуем еще раз. Если хотите выйти введите число меньше 0");
            if (choose == INPUT_INT_ERROR)
            {
                return NOERROR;
            }
            XmlElement id = doc.GetElementById(tagList[choose - 1].Attributes[0].InnerText);
            
            allAboutXmlElement(id, 0);
            Console.WriteLine();
            Console.WriteLine("'Сырой' xml данного тайтла:" + id.InnerXml);
            
            XmlNode x = doc.SelectSingleNode("/root");
            if (x == null)
                return NO_ROOT;

            Console.WriteLine();
            Console.WriteLine("Спрятанные комментарии:");
            allAboutXmlElementComments(id);

            Console.WriteLine();
            Console.WriteLine("Инструкции:");

            XmlProcessingInstruction instruction = doc.SelectSingleNode("processing-instruction('xml-stylesheet')") as XmlProcessingInstruction;

            if (instruction == null)
                return NO_INSTRUCTION;
            Console.WriteLine(instruction.InnerText);

            return NOERROR;
        }

        public int findAllInfoAboutTitle()
        {
            XmlNodeList tagList = doc.GetElementsByTagName("id");
           int i_max = 0;
           foreach(XmlNode node in tagList)
            {
                Console.WriteLine(node.Attributes[0].Value);
                i_max++;
            }
           if (i_max == 0)
            {
                return NO_TAG;
            }
            Console.WriteLine();
            int choose = get_int("Введите номер строки от 1 до " + i_max,
                1, i_max, "Попробуем еще раз. Если хотите выйти введите число меньше 0");
            if (choose == INPUT_INT_ERROR)
            {
                return NOERROR;
            }
            
            XmlElement id = doc.GetElementById(tagList[choose-1].Attributes[0].Value);

            if (id == null)
                return NO_TAG;

            Console.WriteLine();
            Console.WriteLine(id.Name + ":" + id.Attributes[0].Value);
            allAboutXmlElement(id, 1);

            return NOERROR;
        }

        public XmlNodeList into(XmlNodeList nodes, string attr)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlAttributeCollection arr = nodes[i].Attributes;
                for (int j = 0; j < arr.Count; j++)
                {
                   if (attr == arr[j].Name)
                    {
                        Console.WriteLine("{0} is {1}", arr[j].Name, arr[j].Value);
                    }
                }
                into(nodes[i].ChildNodes, attr);
            }
               
            return nodes;
        }



        public string lookInside(String Xpath)
        {
            XmlNode node = doc.SelectSingleNode(Xpath);
           
            int count = node.ChildNodes.Count;
            if (count == 0)
            {
                int attr_count = node.Attributes.Count;
                Console.WriteLine("Текущий путь - " + Xpath + "." +
               "Список возможных атрибутов");
                foreach (XmlAttribute attr in node.Attributes)
                {
                    Console.WriteLine(attr.Name);
                }
                int с = get_int("Введите число от 1 до " + attr_count,
                    1, attr_count, "Попробуем еще раз!");
                if (с == INPUT_INT_ERROR)
                {
                    с = 1;
                }
                return Xpath + @"/@" + @node.Attributes[с - 1].Name;
            }
            Console.WriteLine("Текущий путь - " + Xpath + "." +
               "Список возможных тегов");
            foreach (XmlNode child in node.ChildNodes)
            {
                Console.WriteLine(child.Name);
            }

            int choose = get_int("Введите число от 1 до " + count, 1, count,
                "Попробуем еще раз!");
            if (choose == INPUT_INT_ERROR)
            {
                choose = 1;
            }
            return lookInside(@Xpath + @"/" + @node.ChildNodes[choose - 1].Name);
        }
        

        public int selectSingleNode()
        {
            string Xpath = lookInside("/root/id");
            XmlNode res = doc.SelectSingleNode(Xpath);

            if (res == null)
                return NO_TAG;
            
            Console.WriteLine(Xpath + "->" + res.Value);
            
            return NOERROR;
        }

        public int selectNodes()
        {
            string Xpath = lookInside("/root/id");
            XmlNodeList list = doc.SelectNodes(Xpath);
            if (list == null)
            {
                return NO_TAG;
            }
            XmlNode max = list[0];
            foreach (XmlNode element in list)
            {
                if (max.Value.CompareTo(element.Value) < 0) {
                    max = element;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Максимальный элемент, заданный с помощью путя"
                + Xpath + " - " + max.Value);

            return NOERROR;
        }

        //third
        public XmlElement getXmlElementById(String Id)
        {
            XmlElement res = doc.GetElementById(Id);

            if (res == null)
                throw new Exception("Not found any Id!");

            return res;
        }

        public XmlElement getXmlElementByPath(String Xpath)
        {
            XmlNode res = doc.SelectSingleNode(Xpath);

            if (res == null)
                throw (new Exception("Can't find nodes by this path!\n"));

            return (XmlElement)res;
        }

        public String getElemName(XmlElement elem)
        {
            return  elem.Name;
        }

        public String getComments()
        {
            String res = "";

            foreach (XmlComment comment in doc.SelectNodes("//comment()"))
            {
                res += comment.Value + "\n";
            }

            return res;
        }

        public String getProcessingInstr()
        {
            XmlProcessingInstruction instruction = doc.SelectSingleNode("processing-instruction('xml-stylesheet')") as XmlProcessingInstruction;

            if (instruction == null)
                throw new Exception("Can't find processing instructions\n");

            return instruction.Data;
        }

        public String getElemAttrs(XmlElement elem)
        {
            String res = "";

            foreach (XmlAttribute n in elem.ChildNodes[0].Attributes)
                res += n.Value + ' ';

            return res;
        }
        
        public void deleteLastNode(String output)
        {
            XmlElement root = doc.DocumentElement;
            XmlElement toDelete = (XmlElement)root.ChildNodes[root.ChildNodes.Count - 1];

            root.RemoveChild(toDelete);

            doc.Save(output);
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

        //XPath
        public void changeYearRating(String output)
        {
            XmlElement elem = getXmlElementByPath("/root/id/ratings[@year_rating = 'r13']");

            int old_rating = 18;

            string year_rating = whatRating(old_rating);

            elem.Attributes[1].Value = year_rating;

            doc.Save(output);
        }

        public void addTitle(String output)
        {
            XmlElement id = doc.CreateElement("id");
            XmlElement ratings = doc.CreateElement("ratings");
            XmlElement namings = doc.CreateElement("namings");
            XmlElement id_season = doc.CreateElement("id_season");
            XmlElement types = doc.CreateElement("types");

            XmlAttribute id_a = doc.CreateAttribute("id");
            XmlAttribute user_rating_a = doc.CreateAttribute("user_rating");
            XmlAttribute year_rating_a = doc.CreateAttribute("year_rating");
            XmlAttribute original_a = doc.CreateAttribute("original");
            XmlAttribute local_a = doc.CreateAttribute("local");
            XmlAttribute id_season_a = doc.CreateAttribute("id_season");
            XmlAttribute films_a = doc.CreateAttribute("films");
            XmlAttribute serials_a = doc.CreateAttribute("serials");
            XmlAttribute anounced_a = doc.CreateAttribute("anounced");

            id_a.Value = "19";
            user_rating_a.Value = "5";
            year_rating_a.Value = "r13";
            original_a.Value = "something is here";
            local_a.Value = "здесь что то есть";
            id_season_a.Value = "1";
            films_a.Value = "490";
            serials_a.Value = "95";
            anounced_a.Value = "49";
            

            id.Attributes.Append(id_a);
            ratings.Attributes.Append(user_rating_a);
            ratings.Attributes.Append(year_rating_a);
            namings.Attributes.Append(original_a);
            namings.Attributes.Append(local_a);
            id_season.Attributes.Append(id_season_a);
            types.Attributes.Append(films_a);
            types.Attributes.Append(serials_a);
            types.Attributes.Append(anounced_a);
            
            id.AppendChild(ratings);
            id.AppendChild(namings);
            id_season.AppendChild(types);
            id.AppendChild(id_season);
   
            doc.DocumentElement.AppendChild(id);

            doc.Save(output);
        }
    }


    class consoleApp
    {
        const int INPUT_INT_ERROR = -100;

        static public int get_int(string comment, int a, int b,
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

        static string getString(string write)
        {
            Console.WriteLine(write);
            return Console.ReadLine();
        }

        static void writeInstructions()
        {
            Console.WriteLine();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("<1 - выйти");
            Console.WriteLine("1 - получить наибольшее и наименьшее значение первого атрибута выбранного тега");
            Console.WriteLine("2 - получить информацию о выбранном тайтле");
            Console.WriteLine("3 - получить первое значение выбранного атрибута");
            Console.WriteLine("4 - получить максимальное значение выбранного атрибута");
            Console.WriteLine("5 - получить всю информацию о тайтле, включая xml код и вложенные" +
                " комментарии, а также шапку таблицы");
            Console.WriteLine("6 - удалить последний элемент");
            Console.WriteLine("7 - изменить рейтинги");
            Console.WriteLine("8 - добавить новый элемент");
        }
        
        const int EXIT = 0;
        const int READ = 1;
        const int WRITE = 2;
        const int NEW = 3;

        const int NOERROR = 0;

        static void findMaxMinByName(XmlTool tool)
        {
            string tagName = getString("Введите имя элемента для поиска:");
            string min = "", max = "";
            int code = tool.findMaxMinDiffName(tagName, ref max, ref min);
            if (code == NOERROR)
            {
                Console.WriteLine("Минимальное значение:" + min +
                    ", максимальное:" + max);
            }
            else
            {
                tool.result(code);
            }
        }

        static void findAllInfoAboutTitle(XmlTool tool)
        {
            tool.result(tool.findAllInfoAboutTitle());
        }

        static void selectNodes(XmlTool tool)
        {
            tool.result(tool.selectNodes());
        }

        static void selectNode(XmlTool tool)
        {
            tool.result(tool.selectSingleNode());
        }

        static void xmlInfo(XmlTool tool)
        {
            tool.result(tool.getXMLInfo());
        }

        static void Main(string[] args)
        {
            try
            {
                XmlTool tool = new XmlTool();

                tool.readXml(@"C:\msys64\home\BMSTU_semester_5\Lab06\titles.xml");

                int command = EXIT;
                while (command != INPUT_INT_ERROR)
                {
                    writeInstructions();
                    command = get_int("Выбор за вами. Введите число:", 0, 8, "Попробуйте все таки придерживаться " +
                      " указанных правил. Укажите число меньше 9:");
                    if (command != INPUT_INT_ERROR)
                    {
                        switch (command)
                        {
                            case EXIT: // Выход
                                Console.WriteLine("Выходим!");
                                break;
                            case 1: // Получить наибольшее и наименьшее значения
                                findMaxMinByName(tool);
                                break;
                           case 2: // Получить информацию о тайтле
                                findAllInfoAboutTitle(tool);
                                break;
                            case 3: // Получить первый подходящий элемент
                                selectNode(tool);
                                break;
                            case 4: // получить наибольший подходящий элемент
                                selectNodes(tool);
                                break;
                            case 5: // получить служебную информацию
                                xmlInfo(tool);
                                break;
                            case 6: // удалить последний элемент
                                tool.deleteLastNode(@"C:\msys64\home\BMSTU_semester_5\Lab06\deleted.xml");
                                break;
                            case 7: // обновить рейтинги
                                tool.changeYearRating(@"C:\msys64\home\BMSTU_semester_5\Lab06\changed.xml");
                                break;
                            case 8: // добавить новые элементы
                                tool.addTitle(@"C:\msys64\home\BMSTU_semester_5\Lab06\added.xml");
                                break;
                            default:
                                break;
                        }
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
        }
    }
}