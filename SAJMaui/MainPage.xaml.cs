namespace SAJMaui;
using System.IO;
using MySqlConnector;
using System;
using System.Threading.Tasks;
using SAJMaui.CustomControls;
using System.Diagnostics;
using System.Data;
using System.ComponentModel;
using Windows.System.Profile;

public class W2FProductData 
{
    public W2FProductData() 
    { }
    public string ProductID { get; set; }
    public string ProductName { get; set; }
    public string KeysToFind { get; set; }
    public string Category1 { get; set; }
    public string Category2 { get; set; }
    public string Category3 { get; set; }
    public string Category4 { get; set; }
    public string Category5 { get; set; }
    public string Category6 { get; set; }


}

public partial class MainPage : ContentPage
{
    //playcube.ddns.net
    int count = -1;
    private MySqlConnection connectToDatabase = new MySqlConnection("server=192.168.178.51; port=3307; uid=admin;pwd=stmael1234;database=W2FProducts");
    private MySqlCommand commandToDataBase;
    private List<W2FProductData> dataBaseData = new List<W2FProductData>();
    private Random randomNumber = new Random();
    private string[] test = {"Federball", "Voleyball", "Schraubendreher",
                            "Schrauben Mutter","Nvidia Geforce GTX", "Radeon 5700XT"};
    private Stopwatch stopTime = new Stopwatch();
    public  MainPage()
	{
        InitializeComponent();
        commandToDataBase = connectToDatabase.CreateCommand();
        connectToDatabase.Open();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        connectToDatabase.Close();
    }

    private async Task SelectFromMysql()
    {
        try
        {
            dataBaseData.Clear();
            stopTime.Reset();
            stopTime.Start();
            using MySqlDataReader reader = commandToDataBase.ExecuteReader();
            while (reader.Read())
            {
                W2FProductData data = new W2FProductData()
                {
                    ProductID = reader.GetInt32("productID").ToString(),
                    ProductName = reader.GetString("productName"),
                    Category1 = reader.GetString("categoryOne"),
                    Category2 = reader.GetString("categoryTwo"),
                    Category3 = reader.GetString("categoryThree"),
                    Category4 = reader.GetString("categoryFour"),
                    Category5 = reader.GetString("categoryFive"),
                    Category6 = reader.GetString("categorySix"),
                    KeysToFind = reader.GetString("keyWordsToFind")
                };
                dataBaseData.Add(data);
            }
            stopTime.Stop();
        }
        catch (Exception)
        {

            throw;
        }

    }


    private void CreateInsertCommand(int id, string name)
    {
        commandToDataBase.CommandText = "INSERT INTO productList(productID, productName, " +
                            "lowestPrice, highestPrice, sellers, categoryOne, categoryTwo, categoryThree, categoryFour, " +
                            "categoryFive, categorySix, keyWordsToFind, locations, pictureProduct) VALUES (@pID,@pName,@lowestPrice,@highestPrice,@sellers," +
                            "@catOne,@catTwo,@catThree,@catFour,@catFive,@catSix,@keysToFind,@locations,@pictureProducts)";


        commandToDataBase.Parameters.Clear();
        commandToDataBase.Parameters.AddWithValue("@pID", id);
        commandToDataBase.Parameters.AddWithValue("@pName",name);
        commandToDataBase.Parameters.AddWithValue("@lowestPrice", 100);
        commandToDataBase.Parameters.AddWithValue("@highestPrice", 200);
        commandToDataBase.Parameters.AddWithValue("@sellers", 10);
        commandToDataBase.Parameters.AddWithValue("@catOne", "Elektronik");
        commandToDataBase.Parameters.AddWithValue("@catTwo", "Kleinteile");
        commandToDataBase.Parameters.AddWithValue("@catThree", "Widerstände");
        commandToDataBase.Parameters.AddWithValue("@catFour", "Heißleiter");
        commandToDataBase.Parameters.AddWithValue("@catFive", "NTC100");
        commandToDataBase.Parameters.AddWithValue("@catSix", "medium");
        commandToDataBase.Parameters.AddWithValue("@keysToFind", 20);
        commandToDataBase.Parameters.AddWithValue("@locations", 20);
        commandToDataBase.Parameters.AddWithValue("@pictureProducts", 20);
    }
    private void CreateSearchCommand(string row)
    {
        commandToDataBase.CommandText = "SELECT * FROM productList WHERE productName like @wert";
        commandToDataBase.Parameters.Clear();
        commandToDataBase.Parameters.AddWithValue("@wert","%"+row+"%");
    }

        private async Task WriteToMysql() 
    {
        try
        {
            commandToDataBase.ExecuteNonQuery();
        }
        catch (Exception)
        {

            throw;
        }
    }


    private async void OnCounterClicked(object sender, EventArgs e)
	{
        count++;
        CreateInsertCommand(randomNumber.Next(), test[count]);
        dataStack.Children.Clear();

       /* if (count == 0)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";*/

        await  WriteToMysql();

        if (count == 5)
            count = -1;
    }

    private async void SearchButtonPressed(object sender, EventArgs e)
    {
        CreateSearchCommand(((SearchBar)sender).Text);
        await SelectFromMysql();

        int test = dataBaseData.Capacity;
        CounterBtn.Text=stopTime.ElapsedMilliseconds.ToString();
        dataStack.Children.Clear();

        dataBaseData.GetRange(0, 10).ForEach(i => 
        {

            ShowData showData = new ShowData()
            {
                ProductID = i.ProductID,
                ProductName = i.ProductName,
                Category1 = i.Category1,
                Category2 = i.Category2,
                Category3 = i.Category3,             
                Category4 = i.Category4,
                Category5 = i.Category5,
                Category6 = i.Category6,               
            };
            dataStack.Children.Add(showData);
        
        } );


      /* foreach (var item in dataBaseData)
        {
            dataStack.Children.Add(item);
        }*/
    }
}

