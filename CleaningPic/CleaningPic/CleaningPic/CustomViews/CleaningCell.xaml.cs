using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.CustomViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CleaningCell : ViewCell
	{
        private bool shoppingIsVisible     = false;
        private bool addIsVisible          = false;
        private bool notificationIsVisible = false;
        private bool doneIsVisible         = false;
        private bool showsAddDate          = false;
        private bool showsDoneDate         = false;

        public static readonly BindableProperty CreatedProperty = BindableProperty.Create(
            nameof(Created),
            typeof(DateTimeOffset),
            typeof(CleaningCell),
            DateTimeOffset.Now,
            propertyChanged: (b, o, n) => (b as CleaningCell).Created = (DateTimeOffset)n);

        public static readonly BindableProperty DirtOrPlaceProperty = BindableProperty.Create(
            nameof(DirtOrPlace),
            typeof(string),
            typeof(CleaningCell),
            "",
            propertyChanged: (b, o, n) => (b as CleaningCell).DirtOrPlace = (string)n);

        public static readonly BindableProperty CleaningTimeProperty = BindableProperty.Create(
            nameof(CleaningTime),
            typeof(int),
            typeof(CleaningCell),
            0,
            propertyChanged: (b, o, n) => (b as CleaningCell).CleaningTime = (int)n);

        public static readonly BindableProperty ToolsStringProperty = BindableProperty.Create(
            nameof(ToolsString),
            typeof(string),
            typeof(CleaningCell),
            "",
            propertyChanged: (b, o, n) => (b as CleaningCell).ToolsString = (string)n);

        public static readonly BindableProperty DoneCommandProperty = BindableProperty.Create(
            nameof(DoneCommand),
            typeof(Command),
            typeof(CleaningCell),
            null,
            propertyChanged: (b, o, n) => (b as CleaningCell).DoneCommand = n as Command);

        public static readonly BindableProperty DoneParamProperty = BindableProperty.Create(
            nameof(DoneParam),
            typeof(object),
            typeof(CleaningCell),
            null,
            propertyChanged: (b, o, n) => (b as CleaningCell).DoneParam = n);

        public DateTimeOffset Created
        {
            get { return (DateTimeOffset)GetValue(CreatedProperty); }
            set
            {
                SetValue(CreatedProperty, value);
                dateTimeLabel.Text = value.DateTime.AddHours(9).ToString();
            }
        }

        public string DirtOrPlace
        {
            get { return (string)GetValue(DirtOrPlaceProperty); }
            set
            {
                SetValue(DirtOrPlaceProperty, value);
                titleLabel.Text = value;
            }
        }

        public int CleaningTime
        {
            get { return (int)GetValue(CleaningTimeProperty); }
            set
            {
                SetValue(CleaningTimeProperty, value);
                timeLabel.Text = string.Format("{0}分", value);
            }
        }

        public string ToolsString
        {
            get { return (string)GetValue(ToolsStringProperty); }
            set
            {
                SetValue(ToolsStringProperty, value);
                toolsLabel.Text = value;
            }
        }

        public Command DoneCommand
        {
            get { return GetValue(DoneCommandProperty) as Command; }
            set
            {
                SetValue(DoneCommandProperty, value);
                if (DoneParam != null)
                    SetDoneRecognizer();
            }
        }

        public object DoneParam
        {
            get { return GetValue(DoneParamProperty); }
            set
            {
                SetValue(DoneParamProperty, value);
                if (DoneCommand != null)
                    SetDoneRecognizer();
            }
        }

        public bool ShoppingIsVisible
        {
            get { return shoppingIsVisible; }
            set
            {
                shoppingIsVisible = value;
                shoppingImage.IsVisible = value;
            }
        }

        public bool AddIsVisible
        {
            get { return addIsVisible; }
            set
            {
                addIsVisible = value;
                addImage.IsVisible = value;
            }
        }

        public bool NotificationIsVisible
        {
            get { return notificationIsVisible; }
            set
            {
                notificationIsVisible = value;
                notificationImage.IsVisible = value;
            }
        }

        public bool DoneIsVisible
        {
            get { return doneIsVisible; }
            set
            {
                doneIsVisible = value;
                doneImage.IsVisible = value;
            }
        }

        public bool ShowsAddDate
        {
            get { return showsAddDate; }
            set
            {
                showsAddDate = value;
                addDateLabel.IsVisible = value;
            }
        }

        public bool ShowsDoneDate
        {
            get { return showsDoneDate; }
            set
            {
                showsDoneDate = value;
                doneDateLable.IsVisible = value;
            }
        }

        private void SetDoneRecognizer()
        {
            if (doneImage.GestureRecognizers.Count > 1) return;
            var recognizer = new TapGestureRecognizer() { Command = DoneCommand, CommandParameter = DoneParam };
            doneImage.GestureRecognizers.Add(recognizer);
        }

        public CleaningCell()
		{
			InitializeComponent();
        }
    }
}