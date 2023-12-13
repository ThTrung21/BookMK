using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BookMK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("BookMK");
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("Author");
            BsonDocument document = new BsonDocument
            {
                { "Ma",1},
                { "Ten","Ernest Hermingway"},
                { "Note","Greatest author of his time"}
            };
            collection.InsertOne(document);
            MessageBox.Show("Đã lưu thành công");
        }
    }
}
