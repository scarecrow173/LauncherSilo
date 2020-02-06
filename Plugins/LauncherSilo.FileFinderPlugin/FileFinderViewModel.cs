using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Media;

using Misc;

namespace LauncherSilo.FileFinderPlugin
{
    public class FileFinderDisplayItemViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (value != _Path)
                {
                    _Path = value;
                    OnPropertyChanged("Path");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("IconSource");
                }
            }
        }
        private string _Path { get; set; } = string.Empty;
        public ImageSource IconSource
        {
            get
            {
                return Misc.IconImageStorage.FindSystemIconImage(_Path);
            }
        }
        public bool IsFile
        {
            get
            {
                return _IsFile;
            }
            set
            {
                if (value != _IsFile)
                {
                    _IsFile = value;
                    OnPropertyChanged("IsFile");
                }
            }
        }
        private bool _IsFile = false;

        public bool IsDirectory
        {
            get
            {
                return _IsDirectory;
            }
            set
            {
                if (value != _IsDirectory)
                {
                    _IsDirectory = value;
                    OnPropertyChanged("IsDirectory");
                }
            }
        }
        private bool _IsDirectory = false;

        public FileFinderDisplayItemViewModel(string path)
        {
            Path = path;
            IsFile = System.IO.File.Exists(path);
            IsDirectory = System.IO.Directory.Exists(path);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    public class FileFinderViewModel : INotifyPropertyChanged
    {
        private CancellationTokenSource _cancellation = null;
        private CancellationToken _cancellation_token;
        private System.Timers.Timer SearchQueryTimer = new System.Timers.Timer(100) { AutoReset = false };

        public float SearchRange
        {
            get
            {
                return _SearchRange;
            }
            set
            {
                if (value != _SearchRange)
                {
                    _SearchRange = value;
                    OnPropertyChanged("SearchRange");
                    SearchQueryTimer.Start();
                }
            }
        }
        public float _SearchRange = 0.6f;
        public int SearchMax
        {
            get
            {
                return _SearchMax;
            }
            set
            {
                if (value != _SearchMax)
                {
                    _SearchMax = value;
                    OnPropertyChanged("SearchMax");
                    SearchQueryTimer.Start();
                }
            }
        }
        public int _SearchMax = 64;
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
            set
            {
                if (value != _SearchText)
                {
                    _SearchText = value;
                    OnPropertyChanged("SearchText");
                    SearchQueryTimer.Start();
                }
            }
        }
        private string _SearchText = string.Empty;
        public bool IsSearchFile
        {
            get
            {
                return _IsSearchFile;
            }
            set
            {
                if (value != _IsSearchFile)
                {
                    _IsSearchFile = value;
                    RefreshDisplayItemVisibility();
                    OnPropertyChanged("IsSearchFile");
                }
            }
        }
        private bool _IsSearchFile = true;
        public bool IsSearchDirectory
        {
            get
            {
                return _IsSearchDirectory;
            }
            set
            {
                if (value != _IsSearchDirectory)
                {
                    _IsSearchDirectory = value;
                    RefreshDisplayItemVisibility();
                    OnPropertyChanged("IsSearchDirectory");
                }
            }
        }
        private bool _IsSearchDirectory = true;

        public bool IsReady
        {
            get
            {
                return Finder.IsReady;
            }
        }
        public string StatusText
        {
            get
            {
                return _StatusText;
            }
            set
            {
                if (value != _StatusText)
                {
                    _StatusText = value;
                    OnPropertyChanged("StatusText");
                }
            }
        }
        private string _StatusText = string.Empty;
        private ObservableCollection<FileFinderDisplayItemViewModel> _SearchResults = new ObservableCollection<FileFinderDisplayItemViewModel>();


        public ObservableCollection<FileFinderDisplayItemViewModel> DisplayItems
        {
            get
            {
                return _DisplayItems;
            }
            set
            {
                if (value != _DisplayItems)
                {
                    _DisplayItems = value;
                    OnPropertyChanged("DisplayItems");
                }
            }
        }
        private ObservableCollection<FileFinderDisplayItemViewModel> _DisplayItems = new ObservableCollection<FileFinderDisplayItemViewModel>();


        public FileFinderDisplayItemViewModel SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
            set
            {
                if (value != _SelectedItem)
                {
                    _SelectedItem = value;
                    OnPropertyChanged("SelectedItem");
                }
            }
        }
        public FileFinderDisplayItemViewModel _SelectedItem = null;

        private FileFinder Finder { get; set; } = new FileFinder();

        public FileFinderViewModel()
        {

        }
        public void BeginInitialize()
        {
            if (Finder.State == FileFinder.eState.None)
            {
                StatusText = "準備中...";
                Finder.OnInitializeComplate += Finder_OnInitializeComplate;
                Task.Run(() => { Finder.Initialize(); });
                SearchQueryTimer.Elapsed += SearchQueryTimer_Elapsed;
            }
        }

        public void RunSelectedItem()
        {
            string FilePath = SelectedItem?.Path;
            if (!System.IO.File.Exists(FilePath) && !System.IO.Directory.Exists(FilePath))
            {
                return;
            }
            Process.Start(FilePath);
        }

        private async void SearchAsync(string text)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            if (Finder.IsReady)
            {
                _cancellation?.Cancel();
                _cancellation = new CancellationTokenSource();
                _cancellation_token = _cancellation.Token;
                try
                {
                    StatusText = "検索中...";
                    var SearchedList = await Finder.SearchFileAsync(text, SearchRange, SearchMax, _cancellation_token);
                    _SearchResults = new ObservableCollection<FileFinderDisplayItemViewModel>(SearchedList.Select(x => new FileFinderDisplayItemViewModel(x)));
                    RefreshDisplayItemVisibility();
                    StatusText = "検索完了";
                }
                catch (OperationCanceledException)
                {
                }
            }
            stopwatch.Stop();
            LogStatics.Debug(string.Format("call Search at {0:F3} s", (float)stopwatch.Elapsed.TotalSeconds));
        }
        private void RefreshDisplayItemVisibility()
        {
            DisplayItems = new ObservableCollection<FileFinderDisplayItemViewModel>(_SearchResults.Where(x => (IsSearchFile && x.IsFile) || (IsSearchDirectory && x.IsDirectory)));
        }








        private void Finder_OnInitializeComplate(object sender, EventArgs e)
        {
            StatusText = "準備完了";
            OnPropertyChanged("IsReady");
        }

        private void SearchQueryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() => { SearchAsync(_SearchText); });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

    }
}
