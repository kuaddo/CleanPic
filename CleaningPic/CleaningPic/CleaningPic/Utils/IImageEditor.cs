namespace CleaningPic.Utils
{
    public interface IImageEditor
    {
        byte[] Square(byte[] data);
        byte[] Resize(byte[] data, int size);
        byte[] SquareAndResize(byte[] data, int size);
    }
}
