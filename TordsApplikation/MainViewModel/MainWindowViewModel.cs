using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TordsApplikation.Model;
using TordsApplikation.Extensions;

namespace TordsApplikation.MainViewModel
{
	public class MainWindowViewModel : MyExtansions
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
				OnPropertyChanged("PersonList");
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
		// PersonSelected for Remove
		private Person _personSelected;

		public Person PersonSelected
		{
			get { return _personSelected; }
			set
			{
				_personSelected = value;
				OnPropertyChanged("PersonSelected");
			}
		}

		// New Add Person 
		private Person _newPerson;
		public Person NewPerson
		{
			get
			{
				return _newPerson;
			}
			set
			{
				_newPerson = value;
				OnPropertyChanged("NewPerson");

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
			NewPerson = new Person();
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
						if (NewPerson != null)
							PersonList.Add(NewPerson);
						NewPerson = new Person();


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

	}
}
