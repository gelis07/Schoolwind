using System;
using MySql.Data.MySqlClient;
public class GetSqlData
{
    public static string sqlkey = "ogli-oglia";
    static Dictionary<string, dynamic> Errors = new Dictionary<string, dynamic>();
    static MySqlConnection conn;
    static string myConnectionString;
    public static void AddErrors(){
        myConnectionString = "";
        conn = new MySql.Data.MySqlClient.MySqlConnection();
        try
        {
            conn.ConnectionString = myConnectionString;
            conn.Open();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        //Errors.Add("-2147467259", "An account with this email already exists");
    }
    public static dynamic GetData(string SqlCommand)
    {
        Dictionary<string, dynamic> HashData = new Dictionary<string, dynamic>();
        List<dynamic> AllData = new List<dynamic>();
        try{
            Dictionary<string, dynamic> ReturnedData = new Dictionary<string, dynamic>();
            using(var cmd=new MySqlCommand(SqlCommand,conn))
            {
                var reader = cmd.ExecuteReader();
                int j=0;
                while(reader.Read())
                {
                    List<dynamic> data = new List<dynamic>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        DateTime dateValue;
                        if(DateTime.TryParse(reader.GetValue(i).ToString(), out dateValue))
                        {
                            string date = dateValue.ToString("yyyy-MM-dd");
                            data.Add(date);
                            AllData.Add(date);
                        }else
                        {
                            data.Add(reader.GetValue(i));
                            AllData.Add(reader.GetValue(i));
                        }
                    }
                    ReturnedData.Add("Row"+j, data.ToArray());
                    j++;
                }
                HashData.Add("Columns", reader.FieldCount);
                reader.Close();
            }
            HashData.Add("Data", AllData.ToArray());
            HashData.Add("NewData", ReturnedData);
            HashData.Add("Response", "Success");
            return HashData;
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            HashData.Add("Error", "Error: " + ex.Message);
            HashData.Add("Response", "Error");
            return HashData;
        }
    }
}
