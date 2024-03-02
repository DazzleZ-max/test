namespace SAJMaui.CustomControls;

public partial class ShowData : ContentView
{
	
	


	
	public ShowData()
	{
		InitializeComponent();
	}

    public string ProductID { get { return productID.Text; } set { productID.Text = value; } }
    public string ProductName { get { return productName.Text; } set { productName.Text = value; } }
	public string KeysToFind { get { return keysToFind.Text; } set { keysToFind.Text = value; } }
    public string Category1 { get { return category1.Text; } set { category1.Text = value; } }
    public string Category2 { get { return category2.Text; } set { category2.Text = value; } }
    public string Category3 { get { return category3.Text; } set { category3.Text = value; } }
    public string Category4 { get { return category4.Text; } set { category4.Text = value; } }
    public string Category5 { get { return category5.Text; } set { category5.Text = value; } }
    public string Category6 { get { return category6.Text; } set { category6.Text = value; } }
}
