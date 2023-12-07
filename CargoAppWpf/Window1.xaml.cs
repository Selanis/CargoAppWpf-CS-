using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1(IEnumerable<XElement> userLogIn) /// Получаю userLogIn с авторизации
        {
            InitializeComponent();

            /// userLogIn — это тип IEnumerable, т.е. неисчисляемый (по типу массива). Поэтому я методом First() беру первый элемент оттуда — элемент xml-файлика User, в котором ищу Имя зашедшего
            string nameUser = userLogIn.First().Element("FirstName").Value; /// Беру имя пользователя
            mainText.Content = ("Добро пожаловать, " + nameUser).ToUpper(); /// Вывожу в Label текст «Добро пожаловать, Имя». Все буквы заглавные с помощью метода. 

            /// Подключаю файлик
            XElement users = XElement.Load("../../../xml-files/users.xml");

            
        }

        /// Дополнительная фича, которая отвечает за то, чтобы напоминать пользователю заполнить информацию о себе полностью
        private void Button_Later(object sender, RoutedEventArgs e)
        {
            /// Скрывает элементы от пользователя при нажатии на кнопку «Позже»
            ImageLater.Opacity = 0;
            TextLater.Opacity = 0;
            ButtonLater.Opacity = 0;
        }
    }
}
