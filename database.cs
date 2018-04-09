using System;
using System.Data;
using MySql.Data.MySqlClient;
 
namespace database
{
    public class Test
    {
        public static string databaseName="";

        public static void menuDisplay()
        {
            Console.WriteLine("This menu      :m");
            Console.WriteLine("Show records   :l");
            Console.WriteLine("Add new record :i");
            Console.WriteLine("Delete record  :d");
            Console.WriteLine("Modify record  :a");
            Console.WriteLine("Add column     :c");
            Console.WriteLine("Delete column  :r");
            Console.WriteLine("Exit           :x");
        }

        public static void showRecords(ref IDbConnection dbcon) 
        { 
            Console.Clear();
            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql = "SELECT * FROM "+Test.databaseName;
            dbcmd.CommandText = sql;
            IDataReader reader = dbcmd.ExecuteReader();
            DataTable schemaTable = reader.GetSchemaTable();
           
            bool firstRow = true;
            Console.Clear();
            try
            {
                while (reader.Read())
                {
                    if (firstRow)
                    {
                        for (int col = 0; col < reader.FieldCount; col++)
                            Console.Write("{0,-11}", reader.GetName(col).ToString() + " ");
                        Console.WriteLine();
                        for (int col = 0; col < reader.FieldCount; col++)
                        {
                            if (reader.GetDataTypeName(col).ToString() == "INT")
                                Console.Write("number     ");
                            if (reader.GetDataTypeName(col).ToString() == "DOUBLE")
                                Console.Write("decimal    ");
                            if (reader.GetDataTypeName(col).ToString() == "DATE")
                                Console.Write("date       ");
                            if (reader.GetDataTypeName(col).ToString() == "TIME")
                                Console.Write("time       ");
                            if ((reader.GetDataTypeName(col).ToString() == "VARCHAR") || (reader.GetDataTypeName(col).ToString() == "TEXT"))
                                Console.Write("text       ");
                        }
                        Console.WriteLine();
                    }
                    for (int col = 0; col < reader.FieldCount; col++)
                    {
                        Console.Write("{0,-10}", reader[reader.GetName(col).ToString()].ToString());
                    }
                    Console.WriteLine();
                    firstRow = false;
                }
                Console.WriteLine();
                menuDisplay();
                reader.Close();
                reader = null;
            }
            catch
            { 
                Console.WriteLine("ERROR: Could not read records"); 
            }
            dbcmd.Dispose();
            dbcmd = null;
        
        }

        public static void addColumn(ref IDbConnection dbcon)
        {
            Console.WriteLine();
            Console.WriteLine("Write name for new column");
            string s;
            s = Console.ReadLine();
            Console.WriteLine("Choose column type");
            Console.WriteLine("text            :t");
            Console.WriteLine("natural numbers :n");
            Console.WriteLine("decimal         :d");
            Console.WriteLine("date            :y");
            Console.WriteLine("time            :h");
            ConsoleKeyInfo keyInfo;
            bool pick = false;
            string s1="";
            while (!pick)
            {
                keyInfo = Console.ReadKey(true);

                switch (keyInfo.KeyChar)
                { 
                    case 't': s1 = " TEXT"; break;
                    case 'n': s1 = " INT"; break;
                    case 'd': s1 = " DOUBLE"; break;
                    case 'y': s1 = " DATE"; break;
                    case 'h': s1 = " TIME"; break;
                }
                if (s1!="") pick = true;
            }

            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql ="ALTER TABLE "+Test.databaseName+" ADD " +s+ s1;
            dbcmd.CommandText = sql;
            try
            {
                IDataReader reader = dbcmd.ExecuteReader();
                reader.Close();
                reader = null;
                Console.WriteLine("Column added");
            }
            catch 
            {
                Console.WriteLine("ERROR: Column not added");
            }
                
            dbcmd.Dispose();
            dbcmd = null;
        }

        public static void delColumn(ref IDbConnection dbcon)
        {
            Console.WriteLine();
            Console.WriteLine("Write name of column to delete");
            string s;
            s = Console.ReadLine();
            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql = "ALTER TABLE "+Test.databaseName+" DROP COLUMN " + s ;
            dbcmd.CommandText = sql;
            try
            {
                IDataReader reader = dbcmd.ExecuteReader();
                reader.Close();
                reader = null;
                Console.WriteLine("Column deleted");
            }
            catch 
            {
                Console.WriteLine("ERROR: Column not deleted ! ");
            }

            dbcmd.Dispose();
            dbcmd = null;
        }

        public static void addRecord(ref IDbConnection dbcon)
        {
            IDbCommand dbcmd = dbcon.CreateCommand();
            string sql = "SELECT * FROM "+Test.databaseName;
            dbcmd.CommandText = sql;
            IDataReader reader = dbcmd.ExecuteReader();
            DataTable schemaTable = reader.GetSchemaTable();
            reader.Read();
            string s = "";
            string s1="";
            for (int col = 0; col < reader.FieldCount; col++)
            {
                Console.WriteLine(reader.GetName(col).ToString());
                Console.WriteLine("Data type {0}", reader.GetDataTypeName(col).ToString());
                Console.WriteLine("Data type {0}", reader.GetFieldType(col).ToString());
                s = Console.ReadLine();
                s1 = s1 + "'" + s + "',";
            }
            reader.Close();
            reader = null;

            s1=s1.Remove(s1.Length - 1);
            dbcmd = dbcon.CreateCommand();
            sql = "INSERT INTO "+Test.databaseName+" VALUES (" + s1 + ")";
            dbcmd.CommandText = sql;
            try
            {
                 reader = dbcmd.ExecuteReader();
                reader.Close();
                reader = null;
                Console.WriteLine("Record added");
            }
            catch 
            {
                Console.WriteLine("ERROR: Record  not added ! ");
            }
            dbcmd.Dispose();
            dbcmd = null; 
        }
        public static void delRecord(ref IDbConnection dbcon)
        {
            Console.WriteLine("Enter column name to match value");
            string s;
            s = Console.ReadLine();
            Console.WriteLine("Enter value");
            string sql;
            sql = Console.ReadLine();
            IDbCommand dbcmd = dbcon.CreateCommand();
            sql = "DELETE FROM "+Test.databaseName+" WHERE "+s+"="+sql;
            dbcmd.CommandText = sql;
            try
            {
                IDataReader reader = dbcmd.ExecuteReader();
                reader.Close();
                reader = null;
                Console.WriteLine("Record deleted");
            }
            catch 
            {
                Console.WriteLine("ERROR: Record  not deleted ! ");
            }

            dbcmd.Dispose();
            dbcmd = null;
        }

        public static void addData(ref IDbConnection dbcon)
        {
            Console.WriteLine("Enter column name to match recod by value");
            string s;
            s = Console.ReadLine();
            Console.WriteLine("Enter value to match record");
            s = s+ " = '" +Console.ReadLine()+ "'";
            Console.WriteLine("Enter column name for modification");
            string sql= Console.ReadLine();
            Console.WriteLine("Enter new value");
            sql = "UPDATE "+Test.databaseName+" SET " +sql + " = ' " + Console.ReadLine() + "' WHERE "+s;
            Console.WriteLine(sql);
            IDbCommand dbcmd = dbcon.CreateCommand();
            dbcmd.CommandText = sql;
            try
            {
                IDataReader reader = dbcmd.ExecuteReader();
                reader.Close();
                reader = null;
                Console.WriteLine("Value updated");
            }
            catch
            {
                Console.WriteLine("ERROR: Value not updated ! ");
            }

            dbcmd.Dispose();
            dbcmd = null;
        }

        public static void Main(string[] args)
        {
            string readConsole = "";
            Console.WriteLine("Enter IP address of SQL server (default localhost)");
            readConsole = Console.ReadLine();
            if (readConsole == "") readConsole = "localhost";
            string connectionString = "Server=" + readConsole + ";";
            Console.WriteLine("Enter database name (default test)");
            readConsole = Console.ReadLine();
            if (readConsole == "") readConsole = "test";
            Test.databaseName = readConsole;
            connectionString = connectionString + "Database=" + readConsole + ";";
            Console.WriteLine("Enter user ID (default test)");
            readConsole = Console.ReadLine();
            if (readConsole == "") readConsole = "test";
            connectionString = connectionString + "User ID=" + readConsole + ";";
            Console.WriteLine("Enter password (default no authentication)");
            readConsole = Console.ReadLine();
            if (readConsole != "") connectionString = connectionString + "Password=" + readConsole + ";";
            connectionString = connectionString + "Pooling=false; Convert Zero Datetime=True; Allow Zero Datetime=True;";
       
            IDbConnection dbcon;
            dbcon = new MySqlConnection(connectionString);
            try
            { 
                dbcon.Open();
            }
            catch
            {
                Console.WriteLine("ERROR: Can not open connection ! ");
                Console.ReadLine();
                System.Environment.Exit(1);
            }
      
            menuDisplay();
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                while (keyInfo.KeyChar != 'x')
                {
                    keyInfo = Console.ReadKey(true);
                    switch (keyInfo.KeyChar)
                    {
                       case 'm': menuDisplay(); break;
                        case 'l': showRecords(ref dbcon); break;
                               case 'i': addRecord(ref dbcon); break;
                        case 'd': delRecord(ref dbcon); break;
                        case 'c':
                            addColumn(ref dbcon);
                            break;
                        case 'r': delColumn(ref dbcon); break;
                        case 'a':addData(ref dbcon); break;
                    }

                }

                if (keyInfo.KeyChar == 'x') break;
            }
            dbcon.Close();
            dbcon = null;
        }
    }
}
