using Android.Graphics;

namespace AntoOrNot.Interface
{
    public interface IImageClassifier
    {
        string RecognizeImage(Bitmap bitmap);
    }
}