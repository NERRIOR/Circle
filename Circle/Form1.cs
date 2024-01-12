using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Circle
{
    public partial class Form1 : Form
    {
        // глобальные переменные 
        private Point PreviousPoint, point;
        private Bitmap bmp;
        private Pen blackPen;
        private Graphics g;
        public Form1()
        {
            InitializeComponent();
        }
        // действие при нажатии кнопки загрузки изображения
        private void Open_Button_Click(object sender, EventArgs e)
        {
            // описываем объект класса OpenFileDialog
            OpenFileDialog dialog = new OpenFileDialog();
            // задаем расширения файлов
            dialog.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.PNG)|*.bmp; *.jpg; *.png; *.gif";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Загружаем изображение из выбранного файла
                Image image = Image.FromFile(dialog.FileName);
                int width = image.Width;
                int height = image.Height;
                pictureBox1.Height = height;
                pictureBox1.Width = width;
                // создаем и загружаем изображение в формате bmp
                bmp = new Bitmap(image, width, height);
                // записываем изображение в pictureBox1
                pictureBox1.Image = bmp;
                // подготавливаем объект Graphics для рисования
                g = Graphics.FromImage(pictureBox1.Image);

            }
        }
        // действие при нажатии кнопки сохранения файла
        private void Save_Button_Click(object sender, EventArgs e)
        {
            // описываем и порождаем объект savedialog
            SaveFileDialog savedialog = new SaveFileDialog();
            // задаем свойства для savedialog
            savedialog.Title = "Сохранить картинку как....";
            savedialog.OverwritePrompt = true;
            savedialog.CheckPathExists = true;
            savedialog.Filter = "Bitmap File(*.bmp)|*.bmp|" +
                                "GIF File(*.gif)|*.gif|" +
                                "JPEG File(*.jpg)|*.jpg|" +
                                "PNG File(*.png)|*.png";
            // показываем диалог и провверяем задано ли имя результата
            if (savedialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = savedialog.FileName;
                // Убираем из имени расширение файла
                string strFilExtn = fileName.Remove(0, fileName.Length - 3);
                // сохраняем файл в нужном формате
                switch (strFilExtn)
                {
                    case "bmp":
                        bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case "jpg":
                        bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case "gif":
                        bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case "png":
                        bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case "tif":
                        bmp.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    default:
                        break;
                }
            }
        }
        // действие при нажатии мышки в pictureBox1
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            PreviousPoint.X = e.X;
            PreviousPoint.Y = e.Y;
        }
        // действие при перемещении мышки
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // запоминаем текущее положение курсора мыши
                point.X = e.X;
                point.Y = e.Y;
                // соединяем прошлое и текущее положение точки
                g.DrawLine(blackPen, PreviousPoint, point);
                // текущее положение курсора в PreviousPoint
                PreviousPoint.X = point.X;
                PreviousPoint.Y = point.Y;
                // принудительно вызываем перерисовку
                pictureBox1.Invalidate();
            }
        }
        // зарисовываем круг
        private void Circle_Button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < bmp.Width; i++)
            {
                // счетчик для того чтобы смотреть были ли пересечения с кругом
                int counter = 0;
                // флажок для отслеживания входа и выхода из круга
                bool p_flag = false;
                // переменные для отслеживания входа и выхода из круга
                int entrance = 0;
                int exit = 0;
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color pixelColor = bmp.GetPixel(i, j);
                    // первый раз наткнулись на границу круга
                    if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0 && p_flag == false)
                    {
                        p_flag = true;
                        entrance = j;
                        counter = 1;
                        continue;
                    }
                    // второй раз наткнулись на границу круга
                    if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0 && p_flag == true)
                    {
                        counter = 2;
                        exit = j;
                    }
                }
                if (counter == 1)
                {
                    // если вход равняется выходу закрашиваем только один пиксель
                    bmp.SetPixel(i, entrance, Color.Red);
                }
                else if (counter == 2)
                {
                    // если вход не равняется выходу закрашиваем ряд пикселей
                    for (int c = entrance; c <= exit; c++)
                    {
                        bmp.SetPixel(i, c, Color.Red);
                    }
                }
            }
            // перерисовываем окно
            Refresh();
        }
        // действие при загрузки формы
        private void Form1_Load(object sender, EventArgs e)
        {
            // подготавливаем перо для рисования
            blackPen = new Pen(Color.Black, 4);
        }
    }
}
