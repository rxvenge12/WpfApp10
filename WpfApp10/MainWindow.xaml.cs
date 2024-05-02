using System;
using System.IO;
using System.Reflection; // Импортируем пространство имен для работы с рефлексией
using System.Windows;
using Microsoft.Win32; // Импортируем пространство имен для работы с диалоговыми окнами

namespace WpfApp10
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Инициализируем компоненты окна
        }

        // Метод, вызываемый при нажатии кнопки "Запустить рефлектор"
        private void btnRunReflector_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Получаем имя класса и путь к файлу из текстовых полей
                string className = txtClassName.Text;
                string outputPath = txtOutputPath.Text;

                // Проверяем, что поля не пустые
                if (string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(outputPath))
                {
                    MessageBox.Show("Пожалуйста, введите имя класса и путь к файлу.");
                    return;
                }

                // Запускаем рефлектор и отображаем сообщение об успехе
                Reflector.PrintClassToFile(className, outputPath);
                txtResultMessage.Text = "Информация о классе успешно записана в файл output.txt";
            }
            catch (Exception ex)
            {
                // В случае ошибки выводим сообщение об ошибке
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        // Метод, вызываемый при нажатии кнопки "Обзор"
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Позволяет пользователю выбрать файл для сохранения результата
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                txtOutputPath.Text = openFileDialog.FileName; // Устанавливаем выбранный путь к файлу в текстовое поле
            }
        }
    }

    // Класс, выполняющий рефлексию и записывающий информацию о классе в файл
    public class Reflector
    {
        public static void PrintClassToFile(string className, string outputPath)
        {
            // Получаем тип класса по его имени
            Type type = Type.GetType(className);
            if (type == null)
            {
                throw new ArgumentException($"Тип {className} не найден.");
            }

            // Записываем информацию о классе в файл
            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                writer.WriteLine($"Информация о классе {className}:");
                writer.WriteLine();

                // Вывод информации о полях класса
                writer.WriteLine("Поля:");
                foreach (FieldInfo field in type.GetFields())
                {
                    writer.WriteLine($"{field.FieldType} {field.Name}");
                }
                writer.WriteLine();

                // Вывод информации о свойствах класса
                writer.WriteLine("Свойства:");
                foreach (PropertyInfo property in type.GetProperties())
                {
                    writer.WriteLine($"{property.PropertyType} {property.Name}");
                }
                writer.WriteLine();

                // Вывод информации о методах класса
                writer.WriteLine("Методы:");
                foreach (MethodInfo method in type.GetMethods())
                {
                    writer.WriteLine($"{method.ReturnType} {method.Name}()");
                }
            }
        }
    }
}
