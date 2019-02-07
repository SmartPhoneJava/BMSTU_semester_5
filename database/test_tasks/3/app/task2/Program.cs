using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace task2
{
    [Table(Name = "Filial")]
    public class Filial
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int f_id { get; set; }
        [Column(Name = "name_")]
        public string name_ { get; set; }
        [Column(Name = "adress")]
        public string adress { get; set; }
        [Column(Name = "number")]
        public string number { get; set; }
    }

    [Table(Name = "Sotrudniki")]
    public class Sotrudniki
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int s_id { get; set; }
        [Column(Name = "name_")]
        public string name_ { get; set; }
        [Column(Name = "date_")]
        public DateTime date_ { get; set; }
        [Column(Name = "otdel_name")]
        public DateTime otdel_name { get; set; }
        [Column(Name = "f_id")]
        public int f_id { get; set; }
    }

    class Program
    {
        void linq_first()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["path_to_bd"].ConnectionString;
            DataContext db = new DataContext(connectionString);
            var query = from s in db.GetTable<Sotrudniki>()
                        join F in db.GetTable<Filial>() on s.f_id equals F.f_id
                        where (F.adress.Contains("Герцена"))
                        select new { name = s.name_, street = F.adress };
            query = query.Distinct();
            foreach (var em in query)
            {
                Console.WriteLine("{0,40} \t{1,30}", em.name, em.street);
            }
            Console.Read();
        }

        void linq_second()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["path_to_bd"].ConnectionString;
            DataContext db = new DataContext(connectionString);

            var f1 = from fil in db.GetTable<Filial>()
                        select new { name = fil.name_,
                           fid = fil.f_id };

            foreach (var f in f1)
            {
                var f2 = from sot in db.GetTable<Sotrudniki>()
                             where sot.f_id == f.fid
                             select new { FIO = sot.name_ };
                if (f2.Count() >= 4 && f2.Count() <= 20)
                    Console.WriteLine("{0,5} {1,5}", f.name, f2.Count());
            }
            Console.Read();
        }

        void linq_third()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["path_to_bd"].ConnectionString;
            DataContext db = new DataContext(connectionString);

            var query = from fil in db.GetTable<Filial>()
                        join sot in db.GetTable<Sotrudniki>() on
                        fil.f_id equals sot.f_id
                        where fil.name_.Contains("Московский Филиал") &&
                        (DateTime.Now.Year - sot.date_.Year == 25)
                        select new { name = sot.name_ };
            
            foreach (var em in query)
            {
                Console.WriteLine("{0,15}n", em.name);
            }

            Console.Read();
        }

        void dotnet_first()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["path_to_bd"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.CommandText = String.Format("select * from Sotrudniki as S join Filial" +
                    " as F On F.f_id = S.f_id where F.adress like '%Герцена%'");
                command.Connection = connection;
                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine("Результат поиска:");
                Console.WriteLine("\t{0,40} \t{1,30}", "ФИО сотрудника", "Адрес работы");
                while (reader.Read())
                {
                    Console.WriteLine("\t{0,40} \t{1,30}", reader["name_"], reader["adress"]);
                }


            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);

            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }

        void dotnet_second()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["path_to_bd"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.CommandText = String.Format("select FS.name_ from Filial as FS  where " +
                            "(select COUNT(*) from Sotrudniki as S where S.f_id = FS.f_id) >= 4 and " +
                            "(select COUNT(*) from Sotrudniki as S where S.f_id = FS.f_id) <= 20"
                            );
                command.Connection = connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                Console.WriteLine("Отделы в которых от 4 до 20 человек:");
                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);

            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }

        void dotnet_third()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["path_to_bd"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand();

                command.CommandText = String.Format("select SM.name_ from Filial as FM join Sotrudniki as SM ON SM.f_id = FM.f_id and " +
                                                    "FM.name_ like '%Московский Филиал%' and DATEDIFF(YEAR, SM.date_, GETDATE()) = 25"
                                                        );
                command.Connection = connection;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];
                Console.WriteLine("ФИО сотрудников из Москвы в возрастве 25  лет:");
                int min = 0;
                foreach (DataRow row in dt.Rows)
                {
                    var cells = row.ItemArray;

                    foreach (object cell in cells)
                        Console.Write("\t{0}", cell);
                    Console.WriteLine();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Что то сломалось..." + ex.Message);

            }
            finally
            {
                Console.ReadLine();
                connection.Close();
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();

            //p.dotnet_first();
            //p.dotnet_second();
            //p.dotnet_third();

            //p.linq_first();
            //p.linq_second();
            p.linq_third();
        }
    }
}
