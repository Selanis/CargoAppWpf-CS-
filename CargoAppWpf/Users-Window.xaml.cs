using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace CargoAppWpf
{
    /// <summary>
    /// Логика взаимодействия для Users_Window.xaml
    /// </summary>
    public partial class Users_Window : Window
    {


        public Users_Window()
        {
            InitializeComponent();

            /// Подключаю файлик
            XElement users = XElement.Load("../../../xml-files/users.xml");

            /// Беру из каждого User следующие данные.
            var result = users.Descendants("User").Select(x => new
            {
                Фамилия = x.Element("Name").Value,
                Имя = x.Element("FirstName").Value,
                Возраст = x.Element("Age").Value,
                Должность = x.Element("Work").Value,
                Почта = x.Element("Mail").Value
            });

            /// Заполняю табличку данными. Колонки будут названы названием переменной, так как в xaml я поставила AutoGenerateColumns="True"
            DTUsers.ItemsSource = result;

        }

        /// Функция обновления данных
        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            /// Подключаю файлик
            XElement users = XElement.Load("../../../xml-files/users.xml");

            /// Беру из каждого User следующие данные.
            var result = users.Descendants("User").Select(x => new
            {
                Фамилия = x.Element("Name").Value,
                Имя = x.Element("FirstName").Value,
                Возраст = x.Element("Age").Value,
                Должность = x.Element("Work").Value,
                Почта = x.Element("Mail").Value
            });

            /// Заполняю табличку данными. Колонки будут названы названием переменной, так как в xaml я поставила AutoGenerateColumns="True"
            DTUsers.ItemsSource = result;
        }
    }
}
