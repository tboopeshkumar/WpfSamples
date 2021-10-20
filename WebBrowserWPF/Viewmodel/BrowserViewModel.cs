using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WebBrowser.Model;
using WebBrowser.View;

namespace WebBrowser.Viewmodel
{
    internal class BrowserViewModel : BaseViewModel
    {
        public ICommand AddFavouriteCommand { get; set; }
        public ICommand MenuClickCommand { get; set; }      

        internal BrowserViewModel()
        {
            AddFavouriteCommand = new DelegateCommand(AddFavouriteCommandExecute);
            MenuClickCommand = new DelegateCommand<MenuItem>(MenuClickExecute);            
            FavouriteMenuItems = new ObservableCollection<MenuItem>();
        }

        private ObservableCollection<MenuItem> _favouriteMenuItems;
        public ObservableCollection<MenuItem> FavouriteMenuItems
        {
            get {  return _favouriteMenuItems; }
            set 
            { 
                _favouriteMenuItems = value;
                OnPropertyChanged(nameof(FavouriteMenuItems));
            }
        }

        private string _pageSource = "https://www.microsoft.com";
        public string PageSource
        {
            get { return _pageSource; }
            set {  _pageSource = value; OnPropertyChanged(nameof(PageSource)); }
        }

        private void AddFavouriteCommandExecute()
        {
            var newFaDialog = new ModifyFavouriteDialog();
            var newFavouriteViewModel = new UpdateFavouriteViewModel();
            newFaDialog.DataContext = newFavouriteViewModel;
            newFaDialog.Owner = Application.Current.MainWindow;
            newFavouriteViewModel.UpdateFavourite += e =>
            {
                if (!string.IsNullOrEmpty(e.Name))
                {
                    if (Uri.IsWellFormedUriString(e.Url, UriKind.Absolute))
                    {
                        var menuItem = new MenuItem() { Name = e.Name, Url = e.Url };
                        menuItem.Items = GetDefaultFavouriteMenuOptions(menuItem);
                        FavouriteMenuItems.Add(menuItem);
                        newFaDialog.Close();
                    } 
                    else
                    {
                        MessageBox.Show("Please enter valid absolute url. (For eg. :  https://www.google.com");
                    }
                }
                else
                {
                    MessageBox.Show("Name is required");
                }
            };
            newFavouriteViewModel.Cancel += (s,e) =>
            {
                newFaDialog.Close();
            };
            newFaDialog.ShowDialog();
           
        }

        private void MenuClickExecute(MenuItem input)
        {
            if (input == null)
                return;
            if(input.OperationType == OperationType.Open)
            {
                this.PageSource = input?.Parent.Url;
            }
            if(input.OperationType == OperationType.Delete)
            {
                FavouriteMenuItems.Remove(input.Parent);
            }
            if(input.OperationType == OperationType.Modify)
            {
                if (input.Parent == null)
                    return;
                var updateFaDialog = new ModifyFavouriteDialog();
                var updateFavouriteViewModel = new UpdateFavouriteViewModel() { Name = input.Parent?.Name, Url = input.Parent?.Url };
                updateFaDialog.DataContext = updateFavouriteViewModel;
                updateFaDialog.Owner = Application.Current.MainWindow;
                updateFavouriteViewModel.UpdateFavourite += e =>
                {
                    input.Parent.Name = e.Name;
                    input.Parent.Url = e.Url;
                    updateFaDialog.Close();
                };
                updateFavouriteViewModel.Cancel += (s, e) =>
                {
                    updateFaDialog.Close();
                };
                updateFaDialog.ShowDialog();
            }
        }       

        private List<MenuItem> GetDefaultFavouriteMenuOptions(MenuItem parent)
        {
            var open = new MenuItem() {  Name = "Open", Parent = parent, OperationType = OperationType.Open };
            var modify = new MenuItem() { Name = "Modify", Parent = parent, OperationType = OperationType.Modify };
            var delete = new MenuItem() {  Name = "Delete", Parent = parent, OperationType = OperationType.Delete };
            var menuItems = new List<MenuItem>
            {
                open,
                modify,
                delete
            };
            return menuItems;
        }
        
    }
}
