using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ERP.Masters
{
    class ConvertTextToImage : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    if(value.ToString() == "")
                        return new BitmapImage(new Uri("/ERP;component/Images/facebook-default-no-profile-pic.jpg", UriKind.RelativeOrAbsolute));
                        else
                    return new BitmapImage(new Uri(value.ToString(), UriKind.RelativeOrAbsolute));
                }
            }
            catch (Exception)
            {
                return new BitmapImage(new Uri("/ERP;component/Images/facebook-default-no-profile-pic.jpg", UriKind.RelativeOrAbsolute));
            
            }
            return new BitmapImage(new Uri("/ERP;component/Images/facebook-default-no-profile-pic.jpg", UriKind.RelativeOrAbsolute));
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
