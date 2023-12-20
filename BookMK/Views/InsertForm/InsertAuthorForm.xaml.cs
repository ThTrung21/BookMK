﻿using BookMK.ViewModels;
using BookMK.ViewModels.InsertFormViewModels;

using BookMK.Views.InsertForm;
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
using System.Windows.Shapes;

namespace BookMK.Views.InsertForm
{
    /// <summary>
    /// Interaction logic for InsertAuthorForm.xaml
    /// </summary>
    public partial class InsertAuthorForm : Window
    {
        public InsertAuthorForm()
        {
            InitializeComponent();
            
        }

        private void CloseBtn_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }


        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataContext = await AuthorFormViewModel.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing ViewModel: {ex.Message}");
            }
        }

        private void DoneBtn_Click(object sender, RoutedEventArgs e)
        {
            AuthorFormViewModel vm = this.DataContext as AuthorFormViewModel;
            if (vm.InsertAuthor != null)
            {
                vm.InsertAuthor.Execute(this);
            }

        }


        

        //Mouse cursor for close btn
        private void PackIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void PackIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }
    }
    
}