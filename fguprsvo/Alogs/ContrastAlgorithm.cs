using System.Drawing;
using System.Drawing.Imaging;

namespace fguprsvo.Alogs
{
    public class ContrastAlgorithm : IAlgo
    {
        public IFormFile File { get; set; }

        public string StrContrast { get; set; }

        public bool IsGrayscale { get; set; }

        public async Task<string> RunAsync()
        {
            double contrast = Convert.ToDouble(StrContrast);
            using (var memoryStream = new MemoryStream())
            {
                await File.CopyToAsync(memoryStream);
                using (Image img = Image.FromStream(memoryStream))
                {
                    // Обработка картинки
                    Bitmap input_picture = new Bitmap(img);
                    int red, green, blue;

                    for (int y = 0; y < input_picture.Height; y++)
                    {
                        for (int x = 0; x < input_picture.Width; x++)
                        {
                            var old_color = input_picture.GetPixel(x, y);
                            double temp = ((((old_color.R / 255.0) - 0.5) * contrast) + 0.5 * 255.0);
                            red = (int)temp;
                            temp = ((((old_color.G / 255.0) - 0.5) * contrast) + 0.5 * 255.0);
                            green = (int)temp;
                            temp = ((((old_color.B / 255.0) - 0.5) * contrast) + 0.5 * 255.0);
                            blue = (int)temp;

                            if (red > 255) red = 255;
                            if (red < 0) red = 0;

                            if (green > 255) green = 255;
                            if (green < 0) green = 0;

                            if (blue > 255) blue = 255;
                            if (blue < 0) blue = 0;

                            input_picture.SetPixel(x, y, Color.FromArgb(old_color.A, red, green, blue));
                        }
                    }

                    // Черно-белое
                    if (IsGrayscale)
                    {
                        Bitmap d = new Bitmap(input_picture.Width, input_picture.Height);

                        for (int i = 0; i < input_picture.Width; i++)
                        {
                            for (int x = 0; x < input_picture.Height; x++)
                            {
                                Color oc = input_picture.GetPixel(i, x);
                                int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
                                Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
                                d.SetPixel(i, x, nc);
                            }
                        }

                        input_picture = d;
                    }

                    // Перевод картинки в массив байтов
                    byte[] bytes;
                    using (var ms = new MemoryStream())
                    {
                        input_picture.Save(ms, ImageFormat.Jpeg);
                        bytes = ms.ToArray();
                    }

                    // Перевод изображения в base64
                    return Convert.ToBase64String(bytes);
                }
            }
        }
    }
}
