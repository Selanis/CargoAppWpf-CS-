using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CargoAppWpf
{
public partial class Work_Window : Window
    {

        XElement works = XElement.Load("../../../xml-files/works.xml");
        XElement drivers = XElement.Load("../../../xml-files/drivers.xml");
        XElement ways = XElement.Load("../../../xml-files/ways.xml");

        bool newDriver = false;
        public Work_Window()
        {
            InitializeComponent();

            var result = works.Descendants("Work").Select(x => new
            {
                Код_Маршрута = x.Element("WayId").Value,
                Код_Водителя = x.Element("Drivers").Elements("DriverId").Count<XElement>() == 2 ? 
                x.Element("Drivers").Elements("DriverId").First().Value + "\n" +
                x.Element("Drivers").Elements("DriverId").Last().Value : x.Element("Drivers").Elements("DriverId").First().Value,
                Дата_Отправки = x.Element("DateStart").Value,
                Дата_Возвращения = x.Element("DateBack").Value,
                Премия = x.Element("Prize").Value
            });

            /// Заполняю табличку данными. Колонки будут названы названием переменной, так как в xaml я поставила AutoGenerateColumns="True"
            DTUsers.ItemsSource = result;

            var driverId = drivers.Descendants("Driver").Select(x => 
            x.Element("Id").Value + " (" + x.Element("Name").Value + 
            " " + x.Element("FirstName").Value + " " + x.Element("LastName").Value + ")");

            DriverId.ItemsSource = driverId;
            DriverId2.ItemsSource = driverId;

            var wayId = ways.Descendants("Way").Select(x =>
            x.Element("Id").Value + " (" + x.Element("Name").Value + ")");
            WayId.ItemsSource = wayId;
        }

        private void Button_New(object sender, RoutedEventArgs e)
        {
            ButtonNew.Visibility = Visibility.Collapsed;
            ButtonCancel.Visibility = Visibility.Visible;
            DriverId2.Visibility = Visibility.Visible;

            newDriver = true;

        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            ButtonNew.Visibility = Visibility.Visible;
            ButtonCancel.Visibility = Visibility.Collapsed;
            DriverId2.Visibility = Visibility.Collapsed;

            newDriver = false;
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            if (WayId.SelectedItem != null & DriverId.SelectedItem != null & DateStart.Text != null & !newDriver)
            {
                var driverTimeWork = from dr in drivers.Elements("Driver")
                                     where DriverId.SelectedItem.ToString().Substring(0, 8) == dr.Element("Id").Value
                                     select (float)dr.Element("TimeWork");

                var waySalary = from w in ways.Elements("Way")
                                where WayId.SelectedItem.ToString().Substring(0, 8) == w.Element("Id").Value
                                select (float)w.Element("Salary");

                float prize = waySalary.First() * (driverTimeWork.First() / 100 + 1);

                var time = from t in ways.Elements("Way")
                           where WayId.SelectedItem.ToString().Substring(0, 8) == t.Element("Id").Value
                           select (float)t.Element("Days");

                TimeSpan dayTimeRoad = new TimeSpan((int)time.First(), 0, 0, 1);

                bool wrongDate = false;
                DateTime? dt = DateStart.SelectedDate;
                try
                {
                    var newWork = new XElement("Work",
                        new XElement("WayId", WayId.SelectedItem.ToString().Substring(0, 8)),
                        new XElement("Drivers",
                            new XElement("DriverId", DriverId.SelectedItem.ToString().Substring(0, 8))),
                        new XElement("DateStart", DateStart.ToString().Substring(0, 10)),
                        new XElement("DateBack", (dt + dayTimeRoad).ToString().Substring(0, 10)),
                        new XElement("Prize", prize));
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Выберите дату!");
                    wrongDate = true;
                }
                finally
                {
                    if (!wrongDate)
                    {
                        var newWork = new XElement("Work",
                            new XElement("WayId", WayId.SelectedItem.ToString().Substring(0, 8)),
                            new XElement("Drivers",
                                new XElement("DriverId", DriverId.SelectedItem.ToString().Substring(0, 8))),
                            new XElement("DateStart", DateStart.ToString().Substring(0, 10)),
                            new XElement("DateBack", (dt + dayTimeRoad).ToString().Substring(0, 10)),
                            new XElement("Prize", prize));

                        works.Add(newWork);
                        works.Save("../../../xml-files/works.xml");
                        MessageBox.Show("Успешно!");
                    }
                }
            }
            else if ((WayId.SelectedItem != null & DriverId.SelectedItem != null & DriverId2.SelectedItem != null & DateStart.Text != null & newDriver))
            {
                var driverTimeWork = from dr in drivers.Elements("Driver")
                                     where DriverId.SelectedItem.ToString().Substring(0, 8) == dr.Element("Id").Value
                                     select (float)dr.Element("TimeWork");

                var driverTimeWork2 = from dr in drivers.Elements("Driver")
                                     where DriverId2.SelectedItem.ToString().Substring(0, 8) == dr.Element("Id").Value
                                     select (float)dr.Element("TimeWork");

                var waySalary = from w in ways.Elements("Way")
                                where WayId.SelectedItem.ToString().Substring(0, 8) == w.Element("Id").Value
                                select (float)w.Element("Salary");

                float prize = waySalary.First() * (driverTimeWork.First() / 100 + 1 + driverTimeWork2.First() / 100) * 2;

                var time = from t in ways.Elements("Way")
                           where WayId.SelectedItem.ToString().Substring(0, 8) == t.Element("Id").Value
                           select (float)t.Element("Days");

                TimeSpan dayTimeRoad = new TimeSpan((int)time.First(), 0, 0, 1);

                bool wrongDate = false;
                DateTime? dt = DateStart.SelectedDate;
                try
                {
                    var newWork = new XElement("Work",
                        new XElement("WayId", WayId.SelectedItem.ToString().Substring(0, 8)),
                        new XElement("Drivers",
                            new XElement("DriverId", DriverId.SelectedItem.ToString().Substring(0, 8)),
                            new XElement("DriverId", DriverId2.SelectedItem.ToString().Substring(0, 8))),
                        new XElement("DateStart", DateStart.ToString().Substring(0, 10)),
                        new XElement("DateBack", (dt + dayTimeRoad).ToString().Substring(0, 10)),
                        new XElement("Prize", prize));
                }
                catch (ArgumentOutOfRangeException)
                {
                    MessageBox.Show("Выберите дату!");
                    wrongDate = true;
                }
                finally
                {
                    if (!wrongDate)
                    {
                        var newWork = new XElement("Work",
                            new XElement("WayId", WayId.SelectedItem.ToString().Substring(0, 8)),
                            new XElement("Drivers",
                                new XElement("DriverId", DriverId.SelectedItem.ToString().Substring(0, 8)),
                                new XElement("DriverId", DriverId2.SelectedItem.ToString().Substring(0, 8))),
                            new XElement("DateStart", DateStart.ToString().Substring(0, 10)),
                            new XElement("DateBack", (dt + dayTimeRoad).ToString().Substring(0, 10)),
                            new XElement("Prize", prize));

                        works.Add(newWork);
                        works.Save("../../../xml-files/works.xml");
                        MessageBox.Show("Успешно!");
                    }
                }
            }
            else { MessageBox.Show("Не все ячейки заполнены!"); };
        }


        private void Button_Reload(object sender, RoutedEventArgs e)
        {
            var result = works.Descendants("Work").Select(x => new
            {
                Код_Маршрута = x.Element("WayId").Value,
                Код_Водителя = x.Element("Drivers").Elements("DriverId").Count<XElement>() == 2 ?
                x.Element("Drivers").Elements("DriverId").First().Value + "\n" +
                x.Element("Drivers").Elements("DriverId").Last().Value : x.Element("Drivers").Elements("DriverId").First().Value,
                Дата_Отправки = x.Element("DateStart").Value,
                Дата_Возвращения = x.Element("DateBack").Value,
                Премия = x.Element("Prize").Value
            });

            /// Заполняю табличку данными. Колонки будут названы названием переменной, так как в xaml я поставила AutoGenerateColumns="True"
            DTUsers.ItemsSource = result;
        }

        private void DoubleClick(object sender, RoutedEventArgs e)
        {
            var rowId = DTUsers.SelectedItem;
            System.Type type = rowId.GetType();
            var idPath = (string)type.GetProperty("Код_Маршрута").GetValue(rowId, null);
            var idDriver = (string)type.GetProperty("Код_Водителя").GetValue(rowId, null);
            var date = (string)type.GetProperty("Дата_Отправки").GetValue(rowId, null);

            var index = 0;
            foreach (var i in ways.Elements("Way"))
            {
                if (i.Element("Id").Value == idPath)
                {
                    break;
                }
                index++;
            }
            WayId.SelectedIndex = index;
            DateStart.Text = date;

            if (idDriver.Length > 8)
            {
                var idDriver1 = idDriver.Substring(0, 8);
                var idDriver2 = idDriver.Substring(9, 8);

                ButtonNew.Visibility = Visibility.Collapsed;
                ButtonCancel.Visibility = Visibility.Visible;
                DriverId2.Visibility = Visibility.Visible;

                newDriver = true;

                var index1 = 0;
                var index2 = 0;
                foreach (var i in drivers.Elements("Driver"))
                {
                    if (i.Element("Id").Value == idDriver1)
                    {
                        break;
                    }
                    index1++;
                }
                foreach (var i in drivers.Elements("Driver"))
                {
                    if (i.Element("Id").Value == idDriver2)
                    {
                        break;
                    }
                    index2++;
                }
                DriverId.SelectedIndex = index1;
                DriverId2.SelectedIndex = index2;
            } else
            {
                ButtonNew.Visibility = Visibility.Visible;
                ButtonCancel.Visibility = Visibility.Collapsed;
                DriverId2.Visibility = Visibility.Collapsed;

                newDriver = false;
                index = 0;
                foreach (var i in drivers.Elements("Driver"))
                {
                    if (i.Element("Id").Value == idDriver)
                    {
                        break;
                    }
                    index++;
                }
                DriverId.SelectedIndex = index;
            }
        }

        private void Button_Rename(object sender, RoutedEventArgs e)
        {
            IEnumerable<XElement> wrk;
            if (!newDriver)
            {
                wrk = from work in works.Elements("Work")
                    where work.Element("WayId").Value == WayId.Text.Substring(0, 8) && work.Element("Drivers").Elements("DriverId").First().Value == DriverId.Text.Substring(0, 8)
                      select work;
            } else
            {
                wrk = from work in works.Elements("Work")
                    where work.Element("WayId").Value == WayId.Text.Substring(0, 8) && work.Element("Drivers").Elements("DriverId").First().Value == DriverId.Text.Substring(0, 8) && work.Element("Drivers").Elements("DriverId").Last().Value == DriverId2.Text.Substring(0, 8)
                      select work;
            }

            var time = from t in ways.Elements("Way")
                       where WayId.SelectedItem.ToString().Substring(0, 8) == t.Element("Id").Value
                       select (float)t.Element("Days");
            TimeSpan dayTimeRoad = new TimeSpan((int)time.First(), 0, 0, 1);
            DateTime? dt = DateStart.SelectedDate;

            wrk.First().Element("DateStart").Value = DateStart.Text;
            wrk.First().Element("DateBack").Value = (dt + dayTimeRoad).ToString().Substring(0, 10);

            works.Save("../../../xml-files/works.xml");

            MessageBox.Show("Успешно!");
        }
    }
}