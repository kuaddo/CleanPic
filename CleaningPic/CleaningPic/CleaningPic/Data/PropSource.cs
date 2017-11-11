using Xamarin.Forms;

namespace CleaningPic.Data
{
    public static class PropSource
    {
        private const string useOffline = "useOffline";

        // ネット接続無しで動作するか。デフォルトはfalse
        public static bool UseOffline   
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(useOffline))
                    return (bool)Application.Current.Properties[useOffline];
                else
                    return false;
            }
            set => Application.Current.Properties[useOffline] = value;
        }
    }
}
