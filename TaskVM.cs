using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;
using WpfApp7_8;
using Vitvor._7_8WPF; 
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Converters;

namespace Vitvor._7_8WPF
{
    public class TaskVM : INotifyPropertyChanged
    {
        private MainWindow _mainWindow;
        private Form form;
        public ObservableCollection<Task> TODO { get; set; } = new ObservableCollection<Task>();
        public ObservableCollection<Task> TODOSearch { get; set; } = new ObservableCollection<Task>();
        private Task _todo;
        
        public Task SelectedTask
        {
            get
            {
                return _todo;
            }
            set
            {
                _todo = value;
                OnPropertyChanged("SelectedTask");
            }
        }
        private RelayCommand _add;
        public RelayCommand Add
        {
            get
            {
                return _add ??
                    (_add = new RelayCommand(obj =>
                      {
                          Form form = new Form(this);
                          this.form = form;
                          SelectedTask = new Task();
                          form.calendarStart.SelectedDate = SelectedTask.Date;
                          form.calendarEnd.SelectedDate = SelectedTask.Duration;
                          form.ShowDialog();
                      }));
            }
        }
        private RelayCommand _addNew;
        public RelayCommand AddNew
        {
            get
            {
                return _addNew ??
                    (_addNew = new RelayCommand(obj =>
                      {
                          Task task = obj as Task;
                          if (task != null)
                          {
                              task.Category = form.category.SelectedItem.ToString().Remove(0,38);
                              task.Priority = form.priority.SelectedItem.ToString().Remove(0,38);
                              task.Date = (DateTime)form.calendarStart.SelectedDate;
                              task.Duration = (DateTime)form.calendarEnd.SelectedDate;
                              TODO.Add(task);
                              form.Close();

                          }
                      }));
            }
        }
        private RelayCommand _delete;
        public RelayCommand Delete
        {
            get
            {
                return _delete ??
                    (_delete = new RelayCommand(obj =>
                     {
                         Task task = obj as Task;
                         if (task != null)
                         {
                             TODO.Remove(task);
                         }
                     }));
            }
        }
        private RelayCommand _search;
        public RelayCommand Search
        {
            get
            {
                return _search ??
                    (_search = new RelayCommand(obj =>
                      {
                          string part = obj as string;
                          if (part != null)
                          {
                              if (TODOSearch.Count < TODO.Count)
                              {
                                  foreach (var i in TODO)
                                  {
                                      TODOSearch.Add(i);
                                  }
                              }
                              if (part.Equals("Category"))
                              {

                                  if (_mainWindow.Category.SelectedIndex != -1)
                                  {
                                      TODO.Clear();
                                      var search = from i in TODOSearch where _mainWindow.Category.SelectedItem.ToString().Contains(i.Category) select i;
                                      foreach (var i in search)
                                      {
                                          TODO.Add(i);
                                      }
                                  }
                                  else
                                  {
                                      TODOSearch.Clear();
                                  }
                              }
                              else if (part.Equals("Priority"))
                              {
                                  if (_mainWindow.Priority.SelectedIndex != -1)
                                  {
                                      TODO.Clear();
                                      var search = from i in TODOSearch where _mainWindow.Priority.SelectedItem.ToString().Contains(i.Priority) select i;
                                      foreach (var i in search)
                                      {
                                          TODO.Add(i);
                                      }
                                  }
                                  else
                                  {
                                      TODOSearch.Clear();
                                  }
                              }
                              else if (part.Equals("Date"))
                              {
                                  TODO.Clear();
                                  var search = from i in TODOSearch where i.Date == _mainWindow.Date.SelectedDate select i;
                                  foreach (var i in search)
                                  {
                                      TODO.Add(i);
                                  }
                              }
                          }
                      }));
            }
        }
        private RelayCommand _reset;
        public RelayCommand Reset
        {
            get
            {
                return _reset ??
                    (_reset = new RelayCommand(obj =>
                      {
                          TODO.Clear();
                          foreach (var i in TODOSearch)
                          {
                              TODO.Add(i);
                          }
                          TODOSearch.Clear();
                          _mainWindow.Date.SelectedDate = DateTime.Today;
                          _mainWindow.Priority.SelectedIndex = -1;
                          _mainWindow.Category.SelectedIndex = -1;
                      }));
            }
        }
        private RelayCommand _save;
        public RelayCommand Save
        {
            get
            {
                return _save ??
                    (_save = new RelayCommand(obj =>
                     {
                         if(TODO.Count<TODOSearch.Count)
                         {
                             TODO.Clear();
                             foreach (var i in TODOSearch)
                             {
                                 TODO.Add(i);
                             }
                         }
                         XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Task>));
                         using(FileStream fs=new FileStream("save.xml", FileMode.OpenOrCreate))
                         {
                             xmlSerializer.Serialize(fs, TODO);
                         }
                     }));
            }
        }
        private RelayCommand _load;
        public RelayCommand Load
        {
            get
            {
                return _load ??
                    (_load = new RelayCommand(obj =>
                      {
                          XmlSerializer xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Task>));
                          using(FileStream fs=new FileStream("save.xml", FileMode.OpenOrCreate))
                          {
                              ObservableCollection<Task> tasks=(ObservableCollection<Task>)xmlSerializer.Deserialize(fs);
                              foreach(var i in tasks)
                              {
                                  if(!TODO.Any(u=>u.Category==i.Category && u.Date==i.Date && u.Duration==i.Duration && 
                                  u.FullDescription==i.FullDescription && u.Name==i.Name && u.Periodicity==i.Periodicity && u.Priority==i.Priority && u.State==i.State))
                                      TODO.Add(i);
                              }
                          }
                      }));
            }
        }
        private RelayCommand _changeLanguage;
        public RelayCommand ChangeLanguage
        {
            get
            {
                return _changeLanguage ??
                    (_changeLanguage = new RelayCommand(obj =>
                      {
                        string style1 = obj as string;
                        if (style1!=null)
                          {
                              string style = "Resourses/lang." + style1;
                              // определяем путь к файлу ресурсов
                              var uri = new Uri(style + ".xaml", UriKind.Relative);
                              // загружаем словарь ресурсов
                              ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                              ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                                            where d.Source != null && d.Source.OriginalString.Contains("Resourses/lang.")
                                                            select d).FirstOrDefault();
                              // очищаем коллекцию ресурсов приложения
                              if (oldDict != null)
                              {
                                  int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                                  Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                                  Application.Current.Resources.MergedDictionaries.Insert(ind, resourceDict);
                              }
                              else
                              {
                                  Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                              }
                          }
                      }));
            }

        }
        private RelayCommand _changeTheme;
        public RelayCommand ChangeTheme
        {
            get
            {
                return _changeTheme ??
                    (_changeTheme = new RelayCommand(obj =>
                    {
                        string style1 = "";
                        if (_mainWindow.ChangeTheme.Background.ToString().Contains("4B0082"))
                        {
                            style1 = "Dark";
                        }
                        else
                            style1 = "Light";
                        if (style1 != null)
                        {
                            string style = "Resourses/" + style1+"Theme";
                            // определяем путь к файлу ресурсов
                            var uri = new Uri(style + ".xaml", UriKind.Relative);
                            // загружаем словарь ресурсов
                            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                            ResourceDictionary oldDict = (from d in Application.Current.Resources.MergedDictionaries
                                                          where d.Source != null && d.Source.OriginalString.Contains("Theme.")
                                                          select d).FirstOrDefault();
                            // очищаем коллекцию ресурсов приложения
                            if (oldDict != null)
                            {
                                int ind = Application.Current.Resources.MergedDictionaries.IndexOf(oldDict);
                                Application.Current.Resources.MergedDictionaries.Remove(oldDict);
                                Application.Current.Resources.MergedDictionaries.Insert(ind, resourceDict);
                            }
                            else
                            {
                                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                            }
                        }
                    }));
            }
        }
        public TaskVM(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
