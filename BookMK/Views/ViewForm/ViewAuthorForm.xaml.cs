﻿using BookMK.Models;
using BookMK.ViewModels;
using BookMK.ViewModels.ViewForm;
using BookMK.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace BookMK.Views.ViewForm
{
    /// <summary>
    /// Interaction logic for ViewAuthorForm.xaml
    /// </summary>
    public partial class ViewAuthorForm : Window
    {
        Author au { get; set; }
        public ViewAuthorForm()
        {
            InitializeComponent();
        }

        public ViewAuthorForm(Author a)
        {
            
            
            InitializeComponent();

            DashBoardWindow main = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive) as DashBoardWindow;
            DashBoardViewModel vm = main.DataContext as DashBoardViewModel;
            Staff loggedinS = vm.CurrentStaff;
            if(loggedinS.Role!="admin")
            {
                this.InsertBtn.Visibility = Visibility.Collapsed;
            }
            this.DataContext = new ViewAuthorViewModel(a);

        }

        private void PackIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        

        private void InsertBtn_Click_1(object sender, RoutedEventArgs e)
        {
            ViewAuthorViewModel vm = this.DataContext as ViewAuthorViewModel;
            if (vm.UpdateAuthor != null)
            {
                vm.UpdateAuthor.Execute(this);
            }
        }
    }
}
