using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class UploadViewModel : ViewModelBase
    {
        private NavigationStore navigationStore;
        private int userId;


        private Byte[] _fileContent;
        public Byte[] FileContent
        {
            get => _fileContent;
            set
            {
                _fileContent = value;
                OnPropertyChanged(nameof(FileContent));
            }
        }

        public string _Filename;
        public string Filename
        {
            get => _Filename;
            set
            {
                _Filename = value;
                OnPropertyChanged(nameof(Filename));
            }
        }
        
        public string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _encadrant;
        public string Encadrant
        {
            get => _encadrant;
            set
            {
                _encadrant = value;
                OnPropertyChanged(nameof(Encadrant));
            }
        }

        public string _writers;
        public string Writers
        {
            get => _writers;
            set
            {
                _writers = value;
                OnPropertyChanged(nameof(Writers));
            }
        }

        public string _keywords;
        public string Keywords
        {
            get => _keywords;
            set
            {
                _keywords = value;
                OnPropertyChanged(nameof(Keywords));
            }
        }
        public string _faculty;
        public string Faculty
        {
            get => _faculty;
            set
            {
                _faculty = value;
                OnPropertyChanged(nameof(Faculty));
            }
        }

        public string _year;
        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        public string _language;
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        public string _department;
        public string Department
        {
            get => _department;
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        public string _summary;
        public string Summary
        {
            get => _summary;
            set
            {
                _summary = value;
                OnPropertyChanged(nameof(Summary));
            }
        }


        private bool _isLicence;
        public bool IsLicence
        {
            get => _isLicence;
            set
            {
                if (_isLicence != value)
                {
                    _isLicence = value;
                    if (value)
                    {
                        IsMaster = false;
                        IsDoctorat = false;
                        SelectedDegree = DegreeType.Licence;
                    }
                    OnPropertyChanged(nameof(IsLicence));
                }
            }
        }

        private bool _isMaster;
        public bool IsMaster
        {
            get => _isMaster;
            set
            {
                if (_isMaster != value)
                {
                    _isMaster = value;
                    if (value)
                    {
                        IsLicence = false;
                        IsDoctorat = false;
                        SelectedDegree = DegreeType.Master;
                    }
                    OnPropertyChanged(nameof(IsMaster));
                }
            }
        }

        private bool _isDoctorat;
        public bool IsDoctorat
        {
            get => _isDoctorat;
            set
            {
                if (_isDoctorat != value)
                {
                    _isDoctorat = value;
                    if (value)
                    {
                        IsLicence = false;
                        IsMaster = false;
                        SelectedDegree = DegreeType.Doctorat;
                    }
                    OnPropertyChanged(nameof(IsDoctorat));
                }
            }
        }

        private DegreeType _selectedDegree;
        public DegreeType SelectedDegree
        {
            get => _selectedDegree;
            set
            {
                _selectedDegree = value;
                OnPropertyChanged(nameof(SelectedDegree));
            }
        }
        public enum DegreeType
        {
            Licence,
            Master,
            Doctorat
        }
        public IEnumerable<Department> Departments => Enum.GetValues(typeof(Department)).Cast<Department>();
        public IEnumerable<Language> Languages => Enum.GetValues(typeof(Language)).Cast<Language>();

        public ObservableCollection<string> UniversityYears { get; } = new ObservableCollection<string>
        {
            "2012/2013",
            "2013/2014",
            "2014/2015",
            "2015/2016",
            "2016/2017",
            "2017/2018",
            "2018/2019",
            "2019/2020",
            "2020/2021",
            "2021/2022",
            "2022/2023",
            "2023/2024",
            "2024/2025"
        };

        public ObservableCollection<string> Faculties { get; } = new ObservableCollection<string>
        {
            "Civil Engineering",
            "Biological Sciences",
            "Earth Sciences, Geography And Territorial Planning",
            "Chemistry",
            "Computer Science",
            "Electrical Engineering",
            "Physics",
            "Process And Mechanical Engineering",
            "Mathematics"
        };



        private ArticleModel article;
        



        public ICommand ParcourirCommand { get; }



        public string Role;
        public bool isMember { get; set; }
        public bool isUser { get; set; }

        public object SideBarViewModel { get; }

        public ICommand UploadCommand { get; }
        public ICommand UploadArticleCommand { get; }



        private string _userName;
        public string Username
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _userrole;
        public string User_role
        {
            get => _userrole;
            set
            {
                _userrole = value;
                OnPropertyChanged(nameof(User_role));
            }
        }

        private ImageSource _userpic;
        public ImageSource user_profilepic
        {
            get => _userpic;
            set
            {
                _userpic = value;
                OnPropertyChanged(nameof(user_profilepic));
            }
        }


        private UserRepos userRepos;

        //entering commands
        public ICommand EnterKeyword { get;  }
        public ICommand EnterAuthor { get; }
        public ICommand EnterSup { get; }

        //keywords
        public List<string> keywords { get; set; } = new List<string>();

        //authors
        public List<string> authors { get; set; } = new List<string>();

        //supervisors
        public List<string> supervisors { get; set; } = new List<string>();

        public UploadViewModel(NavigationStore navigationStore, int userId)
        {
            userRepos = new UserRepos();
            this.navigationStore = navigationStore;
            this.userId = userId;


            

            Role = userRepos.GetUserRole(userId);
            if (Role == "Student")
            {
                isUser = true;
                isMember = false;
                SideBarViewModel = new UserSideBarViewModel(userId, navigationStore);
            }
            if (Role == "Member")
            {
                isMember = true;
                isUser = false;
                SideBarViewModel = new MemberSideBarViewModel(userId, navigationStore);
            }

            EnterKeyword = new EnterkeywordCommand(this);
            EnterAuthor = new EnterAuthorCommand(this);
            EnterSup = new EnterSupCommand(this);


            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, userId));

            _userName = userRepos.GetuserName(userId);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(userRepos.GetProfilepicFromId(userId));
            Email = userRepos.GetuserEmail(userId);
            User_role = userRepos.GetUserRole(userId);

            ParcourirCommand = new ParcourirCommand(this);

            UploadArticleCommand = new UploadThesisCommand(this, userId);
        }



    }
}
