using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Threading.Tasks;

namespace Sample.WpfApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<string>>
    {
        public MainWindowViewModel()
        {
            IsActive = true;
        }

        [ObservableProperty]
        private UserFormVm _usr = new UserFormVm { IsActive = true };
        [ObservableProperty]
        private string _name = "default";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateNameCommand))]
        private bool updateEnabled = true;

        [ObservableProperty]
        //[NotifyPropertyChangedRecipients]
        private bool _editable = false;

        [RelayCommand(CanExecute = nameof(UpdateEnabled))]
        private async Task UpdateNameAsync()
        {
            await Task.Delay(10);
            Name = "Default updated.";
        }

        [RelayCommand]
        private async Task SetFormEditableAsync()
        {
            await Task.Delay(10);
            WeakReferenceMessenger.Default.Send(new UserFormEditableMessage(Editable));
            //WeakReferenceMessenger.Default.Send(new ValueChangedMessage<bool>(Editable));
        }

        [ObservableProperty]
        private string _info;
        public void Receive(PropertyChangedMessage<string> message)
        {
            Info = _usr.ToString();
        }
    }

    public class UserFormEditableMessage
    {
        public bool Editable { get; set; }

        public UserFormEditableMessage(bool editable)
        {
            Editable = editable;
        }
    }
}
