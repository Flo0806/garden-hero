using System;
using System.ComponentModel;

namespace SD.Controls.Controls
{
    public class CalendarEvent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private int _id;
        private string _title = string.Empty;
        private DateTime _startDate;
        private DateTime _endDate;
        private string _colorKey = "DefaultEventColor";

        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); OnPropertyChanged(nameof(Duration)); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); OnPropertyChanged(nameof(Duration)); }
        }

        public string ColorKey
        {
            get => _colorKey;
            set { _colorKey = value; OnPropertyChanged(nameof(ColorKey)); }
        }

        public int Duration => (EndDate - StartDate).Days + 1;

        // Startposition im Grid
        public int StartColumn => (int)StartDate.DayOfWeek == 0 ? 6 : (int)StartDate.DayOfWeek - 1;
        public int StartRow => (StartDate.Day - 1) / 7;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
