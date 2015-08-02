using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TordsApplikation.Model;

namespace TordsApplikation.MainViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        #region Properties

        // The List of persons we have bounded to the Listview in MainWindow.xaml
        // I'm also setting that if the personlist isnt empty, have the first item in the list selected in the view. 
        private ObservableCollection<Person> _personList;

        public ObservableCollection<Person> PersonList
        {
            get { return _personList; }
            set
            {
                _personList = value;
                NotifyPropertyChanged("PersonList");
                if (PersonSelected == null && PersonList.Count > 0)
                {
                    PersonSelected = PersonList.First();
                }
                PersonList.CollectionChanged += (sender, args) =>
                {
                    if (PersonSelected == null && PersonList.Count > 0)
                    {
                        PersonSelected = PersonList.First();
                    }
                };
            }
        }

        /// <summary>
        ///  The NameString, AdressString and PhoneString properties exist for Textbox binding. 
        ///  I've used databinding in the TextBox Text="" to these properties so we can use whatever we wright in the Textboxes without any code behind. 
        /// </summary>
        // The property we have bound to the Textbox next to the Name Label
        private string _nameString;

        public string NameString
        {
            get { return _nameString; }
            set
            {
                _nameString = value;
                NotifyPropertyChanged("NameString");
            }
        }
        // The property we have bound to the Textbox next to the Adress Label
        private string _adress;

        public string AdressString
        {
            get { return _adress; }
            set
            {
                _adress = value;
                NotifyPropertyChanged("AdressString");
            }
        }
        // The property we have bound to the Textbox next to the Phone Label
        private string _phonestring;

        public string PhoneString
        {
            get { return _phonestring; }
            set
            {
                _phonestring = value;
                NotifyPropertyChanged("PhoneString");
            }
        }
        // The property I've bound to the SelectedValue="" inside the listview, so we can delete or even change any person we want inside our list/ListView
        private Person _personSelected;

        public Person PersonSelected
        {
            get { return _personSelected; }
            set
            {
                _personSelected = value;
                NotifyPropertyChanged("PhoneString");
            }
        }

        #endregion
        // I'm just a constructor....
        public MainWindowViewModel()
        {
            // Xml-Reader. Take whats in the xml-file, put it in  ObservableCollection<Person> deserialized.
            // savedXmlPath makes sure that the xml goes currectly in everycomputer, instead of just wrighting @"C:\MyProject\Blablabla
            var savedXmlPath = AppDomain.CurrentDomain.BaseDirectory + @"Assets\Resources\SaveContactsXml.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            ObservableCollection<Person> deserialized;
            using (StreamReader reader = new StreamReader(savedXmlPath))
            {
                deserialized = (ObservableCollection<Person>)serializer.Deserialize(reader);
            }
            // Setting PersonList to the list of persons found in the Xml-File
            PersonList = new ObservableCollection<Person>(deserialized);
        }

        #region Button Commands
        /// <summary>
        /// Using our own DelegateCommand Class instead of iCommand, Here are all of the Button actions for our (for now) three buttons in our MainWindow
        /// </summary>
        
        // Add Person To The List
        private DelegateCommand _addperson;
        public DelegateCommand AddPerson
        {
            get
            {
                return _addperson ?? (_addperson = new DelegateCommand
                {
                    Execute = () =>
                    {
                        var person = new Person
                        {
                            Name = NameString,
                            Adress = AdressString,
                            Phone = PhoneString
                        };
                        PersonList.Add(person);
                        NameString = null;
                        AdressString = null;
                        PhoneString = null;

                    }
                });
            }
        }
        // Remove the Person You've selected with the PersonSelected property from the List
        private DelegateCommand _removePerson;
        public DelegateCommand RemovePerson
        {
            get
            {
                return _removePerson ?? (_removePerson = new DelegateCommand
                {
                    Execute = () =>
                    {
                        PersonList.Remove(PersonSelected);
                    }
                });
            }
        }

        // Save All contacts you have added to the Xml
        private DelegateCommand _saveContacts;
        public DelegateCommand SaveContacts
        {
            get
            {
                return _saveContacts ?? (_saveContacts = new DelegateCommand
                {
                    Execute = () =>
                    {
                        ToXml(PersonList);
                    }
                });
            }
        }

        #endregion

        // Save Persons in PersonList to the xml-file for storage
        public string ToXml<T>(T obj)
        {
            // saveXmlPath makes sure that the xml goes currectly in everycomputer, instead of just wrighting @"C:\MyProject\Blablabla
            var saveToXmlPath = AppDomain.CurrentDomain.BaseDirectory + @"Assets\Resources\SaveContactsXml.xml";
            using (var stringWriter = new StreamWriter((saveToXmlPath)))
            {
                var xmlSerializer = new XmlSerializer(typeof(ObservableCollection<Person>));
                xmlSerializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }

        // Used for implementing the INotifyPropertyChanged interface 
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
