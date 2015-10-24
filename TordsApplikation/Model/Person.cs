using System;
using System.Xml.Serialization;
using TordsApplikation.Extensions;

namespace TordsApplikation.Model
{
	[Serializable]
	public class Person : MyExtansions
	{
		private string _name;
		private string _adress;
		private string _phone;
		/// <summary>
		/// Not the biggest Model in the history of Wpf, but it's all we need really. 
		/// We have standard Name,Adress and Phone Properties. 
		/// I've made the Phone property to a string becouse it had no need for being an int, makes it easier to wright your area code
		/// </summary>
		[XmlAttribute()]
		public string Name
		{
			get { return _name; }
			set { _name = value; OnPropertyChanged("Name"); }
		}

		[XmlAttribute()]
		public string Adress
		{
			get { return _adress; }
			set { _adress = value; OnPropertyChanged("Adress"); }
		}

		[XmlAttribute()]
		public string Phone
		{
			get { return _phone; }
			set { _phone = value; OnPropertyChanged("Phone"); }
		}
	}
}
