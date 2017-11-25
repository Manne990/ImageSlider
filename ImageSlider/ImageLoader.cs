using Android.App;
using Android.Widget;
using Square.Picasso;
using Exception = Java.Lang.Exception;

namespace ImageSlider
{
    public static class ImageLoader
    {
        public static void LoadImage(string url, ImageView imageView)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                System.Diagnostics.Debug.WriteLine("LoadImage: No Image URL!");
                return;
            }

            if (imageView == null)
            {
                throw new Exception("ImageLoader.ImageLoader: imageView is null!");
            }

            // Construct URL
            imageView.ViewTreeObserver.AddOnPreDrawListener(new PreDrawListener(() => 
            {
                Picasso.With(Application.Context)
                    .Load(url)
                    .Fit()
                    .CenterInside()
                    .Into(imageView, null, null);

                return imageView;
            }));
        }
    }
}