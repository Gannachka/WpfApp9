using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vitvor._7_8WPF;

namespace Vitvor._7_8WPF
{
    [Serializable]
    public class Task : INotifyPropertyChanged
    {
        private string _fullDescription;
        public string FullDescription
        {
            get
            {
                return _fullDescription;
            }
            set
            {
                _fullDescription = value;
                OnPropertyChanged("FullDescription");
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        private string _priority;
        public string Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                OnPropertyChanged("Priority");
            }
        }
        private string _periodicity;
        public string Periodicity
        {
            get
            {
                return _periodicity;
            }
            set
            {
                _periodicity = value;
                OnPropertyChanged("Periodicity");
            }
        }
        private DateTime _duration;
        public DateTime Duration
        {
            get
            {
                return _duration.Date;
            }
            set
            {
                if (value > _startDate)
                {
                    _duration = value;
                    OnPropertyChanged("Duration");
                }
                else
                {
                    MessageBox.Show("Нет возможности установить такую дату окончания");
                }
            }
        }
        private string _category;
        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }
        private string _state;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged("State");
            }
        }
        private DateTime _startDate;
        public DateTime Date
        {
            get
            {
                return _startDate.Date;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged("Date");
            }
        }
        public Task()
        {
            Date = DateTime.Today;
            Duration = DateTime.Today.AddDays(1);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
