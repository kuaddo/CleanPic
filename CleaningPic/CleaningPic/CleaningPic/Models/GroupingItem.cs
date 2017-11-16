using CleaningPic.Data;
using System.Collections.ObjectModel;

namespace CleaningPic.Models
{
    public class GroupingItem : ObservableCollection<Cleaning>
    {
        public string PlaceLabel { get; set; }
    }
}
