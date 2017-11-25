using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Views;
using System;

namespace ImageSlider
{
    [Activity(Label = "ImageSlider", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private ImageView _currentImageView, _nextImageView;
        private List<string> _images;
        private int _nextImageIndex;
        private System.Timers.Timer _timer;
        private AnimationListener _listener;
        private ViewPropertyAnimator _animator;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _images = new List<string> 
            {
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild1.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild2.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild3.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild4.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild5.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild6.jpg",
                "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild7.jpg"
            };

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }

        protected override void OnResume()
        {
            base.OnResume();

            _currentImageView = FindViewById<ImageView>(Resource.Id.imageView1);
            _nextImageView = FindViewById<ImageView>(Resource.Id.imageView2);

            _nextImageIndex = 0;

            ImageLoader.LoadImage(_images[_nextImageIndex], _currentImageView);
            _nextImageIndex++;

            ImageLoader.LoadImage(_images[_nextImageIndex], _nextImageView);
            _nextImageIndex++;

            _listener = new AnimationListener(null, null, SwitchPositions);

            _timer = new System.Timers.Timer
            {
                Interval = 3000
            };

            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
        }

        protected override void OnPause()
        {
            base.OnPause();

            _timer.Enabled = false;
            _timer.Elapsed -= OnTimedEvent;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _timer = null;
            _listener = null;
            _animator = null;
            _currentImageView = null;
            _nextImageView = null;
            _images = null;
        }

        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            var width = _currentImageView.Width;

            RunOnUiThread(() => 
            {
                _nextImageView.TranslationX = width;

                _currentImageView.Animate().SetDuration(300).TranslationX(-width).Start();

                _animator = _nextImageView.Animate().SetDuration(300).TranslationX(0).SetListener(_listener);
                _animator.Start();
            });
        }

        private void SwitchPositions()
        {
            // Remove the listner
            _animator.SetListener(null);
            _animator = null;

            RunOnUiThread(() => 
            {
                var width = _currentImageView.Width;

                var tmp = _currentImageView;
                _currentImageView = _nextImageView;
                _currentImageView.TranslationX = 0;

                _nextImageView = tmp;
                _nextImageView.TranslationX = width;
                _nextImageView.SetImageBitmap(null);
                _nextImageView.DestroyDrawingCache();

                GC.Collect();

                ImageLoader.LoadImage(_images[_nextImageIndex], _nextImageView);
                _nextImageIndex++;

                if (_nextImageIndex >= _images.Count)
                {
                    _nextImageIndex = 0;
                }
            });
        }
    }
}