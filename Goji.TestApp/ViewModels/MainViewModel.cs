
namespace Goji.TestApp.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Markup;
    using Timer = System.Timers.Timer;

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly RelayCommand genElementsCommand;

        private readonly RelayCommand gcCommand;        

        private uint elementCount = 100;


        public MainViewModel()
        {
            if (!DesignerProperties.GetIsInDesignMode(new FrameworkElement()))
            {
                Timer t1 = new Timer();
                t1.Interval = 1000;
                t1.Elapsed += (s, e) => RaiseCurrentTimePropertyChanged();
                t1.Start();
            }

            genElementsCommand = new RelayCommand(GenElements);
            gcCommand = new RelayCommand((o) => GC.Collect());
            Elements = Enumerable.Empty<GCListEntry>();

            GenElements(null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                return new CultureInfo[] { new CultureInfo("en"), new CultureInfo("de") };

                //return Application.Current.GetTranslator().AvailableLanguages.Where(x => x.IsNeutralCulture);
            }
        }

        public ICommand GenElementsCommand
        {
            get { return genElementsCommand; }
        }
        public ICommand GCCommand
        {
            get { return gcCommand; }
        }

        public DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        public float ANumericValue
        {
            get { return 935.3341f; }
        }

        public uint ElementCount
        {
            get { return elementCount; }
            set
            {
                elementCount = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ElementCount"));
                }
            }
        }

        public IEnumerable<GCListEntry> Elements { get; private set; }


        public CultureInfo Language
        {
            get
            {
                return Application.Current.GetCurrentUICulture();
            }
            set
            {
                Application.Current.SetCurrentUICulture(value);
            }
        }
        private void GenElements(object obj)
        {
            foreach (var item in Elements)
            {
                item.Dispose();
            }

            Elements = Enumerable.Range(0, (int)elementCount).Select(x => new GCListEntry()).ToList();

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Elements"));
            }
        }

        private void RaiseCurrentTimePropertyChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentTime"));
            }
        }

        public class GCListEntry : INotifyPropertyChanged, IDisposable
        {
            readonly Timer t1;

            public GCListEntry()
            {
                if (!DesignerProperties.GetIsInDesignMode(new FrameworkElement()))
                {
                    t1 = new Timer();
                    t1.Interval = 1000;
                    t1.Elapsed += (s, e) => RaiseCurrentTimePropertyChanged();
                    t1.Start();
                }
            }

            private bool val;

            public event PropertyChangedEventHandler PropertyChanged;

            public string Value
            {
                get
                {
                    return val ? "_aBindingTranslatedValue1" : "_aBindingTranslatedValue2";
                }
            }

            private void RaiseCurrentTimePropertyChanged()
            {
                val = !val;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }

            public void Dispose()
            {
                t1.Dispose();
            }
        }

    }
}
