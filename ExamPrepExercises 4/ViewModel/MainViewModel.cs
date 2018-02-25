using ExamPrepExercises_4.Communication;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;

namespace ExamPrepExercises_4.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        #region Collections
        private ObservableCollection<ItemVM> items;
        private ObservableCollection<string> types = new ObservableCollection<string>(){ "Engine", "Gears", "Body" };

        public ObservableCollection<string> Types
        {
            get { return types; }
            set { types = value; }
        }

        public ObservableCollection<ItemVM> Items
        {
            get { return items; }
            set { items = value; }
        }
        #endregion

        #region Communication props
        public Server Server { get; set; }
        public Client Client { get; set; }
        #endregion

        #region Buttons
        private RelayCommand addBtnClickedCmd;
        private RelayCommand serverBtnClickedCmd;
        private RelayCommand clientBtnClickedCmd;
        #endregion

        public RelayCommand ClientBtnClickedCmd
        {
            get { return clientBtnClickedCmd; }
            set { clientBtnClickedCmd = value; }
        }

        public RelayCommand ServerBtnClickedCmd
        {
            get { return serverBtnClickedCmd; }
            set { serverBtnClickedCmd = value; }
        }

        public RelayCommand AddBtnClickedCmd
        {
            get { return addBtnClickedCmd; }
            set { addBtnClickedCmd = value; }
        }

        #region Variables
        private string prodId;
        private string name;
        private float price;
        private string selectedType;
        private bool actAsServer;
        private bool actAsClient;

        public bool ActAsClient
        {
            get { return actAsClient; }
            set { actAsClient = value; RaisePropertyChanged(); }
        }

        public bool ActAsServer
        {
            get { return actAsServer; }
            set { actAsServer = value; RaisePropertyChanged(); }
        }

        public string SelectedType
        {
            get { return selectedType; }
            set { selectedType = value; RaisePropertyChanged(); }
        }

        public float Price
        {
            get { return price; }
            set { price = value; RaisePropertyChanged(); }
        }

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string ProdId
        {
            get { return prodId; }
            set { prodId = value; RaisePropertyChanged(); }
        }
        #endregion

        public MainViewModel()
        {
            Items = new ObservableCollection<ItemVM>();

            AddBtnClickedCmd = new RelayCommand(()=>
            {
                Items.Add(new ItemVM(ProdId,Name,Price,SelectedType));
                if(Server != null) Server.InformAllCients(ProdId + "@"+ Name + "@"+ Price +"@"+ SelectedType + "\r\n");
                if (Client != null) Client.Send(ProdId + "@" + Name + "@" + Price + "@" + SelectedType + "\r\n");
                ProdId = null;
                Name = "";
                Price = 0;
                SelectedType = null;
            }, ()=>
            {
                return (ProdId != null && Name != null && Price != 0 && SelectedType != null);
            });

            ServerBtnClickedCmd = new RelayCommand(()=>
            {
                Server = new Server(GuiUpdate);
                ActAsServer = true;
                ActAsClient = false;
            },()=> {
                return !ActAsServer;
            });

            ClientBtnClickedCmd = new RelayCommand(()=> {
                Client = new Client(GuiUpdate);
                ActAsServer = false;
                ActAsClient = true;
            },()=> {
                return !ActAsClient;
            });
        }

        private void GuiUpdate(string message)
        {
            string[] temp = message.Split('@');
            App.Current.Dispatcher.Invoke(() =>
            {
                Items.Add(new ItemVM(temp[0], temp[1], float.Parse(temp[2]), temp[3]));
            });
        }
    }
}