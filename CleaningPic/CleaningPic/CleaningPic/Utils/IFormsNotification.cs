using System;

namespace CleaningPic.Utils
{
    public interface IFormsNotification
    {
        void Notify(string title, string message, DateTime time);
    }
}
