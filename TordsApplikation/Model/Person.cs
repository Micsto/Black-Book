using System;
using System.Xml.Serialization;

namespace TordsApplikation.Model
{
    [Serializable]
    public class Person
    {
        /// <summary>
        /// Not the biggest Model in the history of Wpf, but it's all we need really. 
        /// We have standard Name,Adress and Phone Properties. 
        /// I've made the Phone property to a string becouse it had no need for being an int, makes it easier to wright your area code
        /// </summary>
        [XmlAttribute()]
        public string Name { get; set; }

        [XmlAttribute()]
        public string Adress { get; set; }

        [XmlAttribute()]
        public string Phone { get; set; }
    }
}
