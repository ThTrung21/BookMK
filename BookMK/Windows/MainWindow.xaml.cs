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
using BookMK.Models;
using BookMK.Commands;
using BookMK.Commands.DummyData;
using System.Security.Cryptography.X509Certificates;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;

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

            




            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase database = client.GetDatabase("BookMK");


            //// Create an admin account on first loadup
            var staffDataProvider = new DataProvider<Staff>("staffs");
            //// Check if any accounts exist in the database with the username "admin"

            bool staffExists = Staff.IsExisted("admin");
            if (staffExists)
            {
               
            }
            else
            {
                var adminAccount = new Staff
                {

                    ID = Staff.CreateID(),
                    Role = "admin",
                    Username = "admin",
                    PasswordHash = Staff.HashPassword("12345"),
                    Phone = "",
                    FullName = "admin",
                    Email = "",
                    IsVerified = false
                };

                // Insert the admin account into the database
                staffDataProvider.Insert(adminAccount);
            }




            //===================================================================================

            // Create a customer instance for unregistered customer purchase
            var customerProvider = new DataProvider<Customer>("customers");
            // Check if any accounts exist in the database with the username "admin"
            bool customerExists = Customer.IsExisted("Walk-in Customers");
            if (!customerExists)
            {

                // Seed a customer for annonymous
                var Walk_in = new Customer
                {
                    ID = 0,
                    Phone = "",
                    FullName = "Walk-in Customers",
                    Email = "",
                    Address = "",
                    IsLoyalDiscountReady = false,
                    PurchasePoint = 0
                    
                };

                // Insert the admin account into the database
                customerProvider.Insert(Walk_in);
            }
            //===================================================================================
            var authorprovider = new DataProvider<Author>("author");
            bool authorexist = Author.IsExisted(1);
            if(!authorexist)
            {
                Author a = new Author()
                {
                    ID = 1,
                    Name = "Test",
                    Note = "An example author"
                };
                authorprovider.Insert(a);
            }
            bool authorexist2 = Author.IsExisted(2);
            if (!authorexist2)
            {
                Author a2 = new Author()
                {
                    ID = 2,
                    Name = "J.K.Rowling",
                    Note = "Another author"
                };
                authorprovider.Insert(a2);
            }

            //===================================================================================
            var bookprovider = new DataProvider<Book>("books");
            bool bookexist = Book.IsExisted(1);
            ICommand addbook = new AddBookData();
            if (!bookexist)
            {
                addbook.Execute(this);
            }
            //===================================================================================




            var loyaldiscountProvider = new DataProvider<Discount>("discounts");
            bool discountexists = Discount.IsExistedLoyal();
            if(!discountexists)
            {
                var loyal = new Discount
                {
                    ID = 0,
                    Type="Loyal",
                    Value = 0,
                    PointMileStone = 100,
                    Time=DateTime.Now
                };
                loyaldiscountProvider.Insert(loyal);
            }
            
        }

        private void BookMK_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }  
}

