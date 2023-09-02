using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sample.WpfApp.ViewModels
{
    [ObservableRecipient]
    public partial class UserFormVm : ObservableValidator, IRecipient<UserFormEditableMessage>
    {
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        [NotifyDataErrorInfo]
        [Required]
        private string _firstName = "galo";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [NotifyPropertyChangedRecipients]
        [Required]
        private string _lastName = "S";

        [ObservableProperty]
        private string _fullName;

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        [NotifyDataErrorInfo]
        [Range(1, 100)]
        private int _age = 30;

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        [NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        [MaxLength(100)]
        [Required]
        private string _introduction;

        //[RelayCommand(CanExecute = nameof(SaveEnabled))]
        //private async Task SaveAsync()
        //{
        //    await Task.Delay(10);
        //}

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private bool _allowEdit = false;
        public void Receive(UserFormEditableMessage message)
        {
            AllowEdit = message.Editable;
        }

        [RelayCommand(CanExecute = nameof(AllowEdit))]
        private async Task SaveAsync()
        {
            await Task.Delay(10);
        }

        private bool SaveEnabled()
        {
            return !HasErrors;
        }

        public override string ToString()
        {
            return $"姓:{FirstName},名:{LastName},自我介绍：{Introduction}";
        }

    }
}
