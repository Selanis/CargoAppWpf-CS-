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
using System.Xml;
using System.Xml.Linq;

namespace CargoAppWpf
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        /// Кнопочка для перехода на страничку логина
        private void Button_LogIn(object sender, RoutedEventArgs e)
        {
            MainWindow logIn = new MainWindow();
            logIn.Show();
            this.Close();
        }

        /// Кнопочка для внечения изменений в xml файл и регистрации
        private void Button_Register(object sender, RoutedEventArgs e)
        {
            /// Получаю все переменные из xaml файла
            var firstName = FirstName.Text;
            var login = Login.Text;
            var password = Password.Text;
            var secondPassword = SecondPassword.Text;
            var age = Age.Text;

            var radioMale = RadioMale.IsChecked;
            var radioFemale = RadioFemale.IsChecked;
            var radioOther = RadioOther.IsChecked;

            /// Переменная для проверки повтора логина
            bool wrongLogin = false;

            /// Сюда будем вкладывать пол 
            var gender = "—";

            /// Подключаю файлик
            XElement users = XElement.Load("../../../xml-files/users.xml");

            /// Проверка на повторяемость логина
            foreach (var user in users.Elements("User"))
            {
                if (user.Element("Login").Value == login) {
                    wrongLogin = true;
                    MessageBox.Show("Пользователь с таким логином уже зарегестрирован!");
                }
            }

            /// Вычисляю id для пользователя
            var id = 1;
            foreach (var user in users.Elements("User"))
            {
                id++;
            };

            /// Проверяю совпадение пароля и повторения пароля
            if (password == secondPassword)
            {
                /// Проверяю, не повторяется ли логин
                if (!wrongLogin)
                {
                    /// Вычисляю гендер по кнопочке
                    if (radioMale == true)
                    {
                        gender = "Мужской";
                    } else if (radioFemale == true) {
                        gender = "Женский";
                    } else if (radioOther == true)
                    {
                        gender = "Другое";
                    };

                    /// Формирую нового User в XML дерево
                    var userTreeXML = new XElement("User",
                        new XElement("Id", id),
                        new XElement("Name", "—"),
                        new XElement("FirstName", firstName),
                        new XElement("LastName", "—"),
                        new XElement("Gender", gender),
                        new XElement("Login", login),
                        new XElement("Password", password),
                        new XElement("Work", "—"),
                        new XElement("Mail", "—"),
                        new XElement("Telephone", "—"),
                        new XElement("Date", DateTime.Today.ToString().Substring(0, 10))
                    );

                    /// Добавляю в дерево и сохраняю
                    users.Add(userTreeXML);
                    users.Save("../../../xml-files/users.xml");

                    /// Открываю окно с логином
                    MainWindow logIn = new MainWindow();
                    logIn.Show();
                    this.Close();
                }
            } else
            {
                MessageBox.Show("Пароли не совпадают");
            }
        }
    }
}
