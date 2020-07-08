using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlayFabAPICallAnalyzer.Model
{
    public class MessageModel : INotifyPropertyChanged
    {
        private string _message;
        private Brush _messageBrush;

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public Brush MessageBrush
        {
            get => _messageBrush;
            set => SetProperty(ref _messageBrush, value);
        }

        public MessageModel(string msg, MessageType mt=MessageType.Success)
        {
            _message = msg;
            switch (mt)
            {
                case (MessageType.Success): _messageBrush = Brushes.Green; break;
                case (MessageType.Error): _messageBrush = Brushes.Red; break;
                case (MessageType.Warning): _messageBrush = Brushes.Orange; break;
            }
        }
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning
    }
}
