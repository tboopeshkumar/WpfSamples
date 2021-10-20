using Prism.Commands;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace WebBrowser.Viewmodel
{
    public class UpdateFavouriteEvent : EventArgs
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public delegate void UpdateFavouriteEventHandler(UpdateFavouriteEvent e); 

    internal class UpdateFavouriteViewModel : BaseViewModel
    {
        public ICommand OkCommand { get; set; }
        public ICommand CancelCommand {  get; set; }

        public event UpdateFavouriteEventHandler UpdateFavourite;
        public event EventHandler Cancel;
        public UpdateFavouriteViewModel()
        {
            OkCommand = new DelegateCommand(OkCommandExecute);
            CancelCommand = new DelegateCommand(CancelCommandExecute);
        }

        private void OkCommandExecute()
        {
            UpdateFavourite?.Invoke(new UpdateFavouriteEvent() { Name = this.Name, Url = this.Url });
        }

        private void CancelCommandExecute()
        {
            Cancel?.Invoke(this, new EventArgs());
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }
    
    }
}
