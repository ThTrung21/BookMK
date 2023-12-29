using BookMK.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace BookMK.Converters
{
    public class AbsoluteFileNameToBitmapImage: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StringBuilder filepath = value as StringBuilder;
            string filename = filepath.ToString();
            if (!String.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = new Uri(filename, UriKind.Absolute);
                bitmapImage.EndInit();
                return bitmapImage;
            }
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.UriSource = new Uri(Path.Combine(ImageStorage.CurrentSolutionLocation, "Images", "NoImage.png"), UriKind.Absolute);
            bmp.EndInit();
            return bmp;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

