
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Baza
{
    class Serialization
    {
        public static void SaveUserListToFile(List<User> usersList, string usersPath)                                       
        {

            using (Stream fs = new FileStream(usersPath, FileMode.Create, FileAccess.Write, FileShare.None))   // zapis do pliku
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                serializer.Serialize(fs, usersList);
            }
        }

        public static List<User> LoadUserFromFile(string usersPath)                                           // odczyt z pliku
        {
            List<User>usersList = new List<User>();                                                           // tworzy nową listę dla użytkowników
             
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));                                 // deserializacja

            using (FileStream fs2 = File.OpenRead(usersPath))                                                // odczyt z pliku
            {
                return usersList = (List<User>)serializer.Deserialize(fs2);                                  // wczytanie użytkowników z pliku do listy
            }
        }
    }
}
