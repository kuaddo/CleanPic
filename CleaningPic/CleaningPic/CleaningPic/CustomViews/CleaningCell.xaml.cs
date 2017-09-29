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

        public static readonly BindableProperty RemoveCommandProperty = BindableProperty.Create(
            nameof(RemoveCommand),
            typeof(Command),
            typeof(CleaningCell),
            null,
            propertyChanged: (b, o, n) => (b as CleaningCell).RemoveCommand = n as Command);

        public static readonly BindableProperty RemoveParamProperty = BindableProperty.Create(
            nameof(RemoveParam),
            typeof(object),
            typeof(CleaningCell),
            null,
            propertyChanged: (b, o, n) => (b as CleaningCell).RemoveParam = n);

        public static readonly BindableProperty AddCommandProperty = BindableProperty.Create(
            nameof(AddCommand),
            typeof(Command),
            typeof(CleaningCell),
            null,
            propertyChanged: (b, o, n) => (b as CleaningCell).AddCommand = n as Command);

        public static readonly BindableProperty AddParamProperty = BindableProperty.Create(
            nameof(AddParam),
            typeof(object),
            typeof(CleaningCell),
            null,
            propertyChanged: (b, o, n) => (b as CleaningCell).AddParam = n);

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

        public Command RemoveCommand
        {
            get { return GetValue(RemoveCommandProperty) as Command; }
            set
            {
                SetValue(RemoveCommandProperty, value);
                if (RemoveParam != null)
                    SetRemoveRecognizer();
            }
        }

        public object RemoveParam
        {
            get { return GetValue(RemoveParamProperty); }
            set
            {
                SetValue(RemoveParamProperty, value);
                if (RemoveCommand != null)
                    SetRemoveRecognizer();
            }
        }

        public Command AddCommand
        {
            get { return GetValue(AddCommandProperty) as Command; }
            set
            {
                SetValue(AddCommandProperty, value);
                if (AddParam != null)
                    SetAddRecognizer();
            }
        }

        public object AddParam
        {
            get { return GetValue(AddParamProperty); }
            set
            {
                SetValue(AddParamProperty, value);
                if (AddCommand != null)
                    SetAddRecognizer();
            }
        }

        public bool RemoveIsVisible
        {
            set { removeImage.IsVisible = value; }
        }

        public bool ShoppingIsVisible
        {
            set { shoppingImage.IsVisible = value; }
        }

        public bool AddIsVisible
        {
            set {  addImage.IsVisible = value; }
        }

        public bool NotificationIsVisible
        {
            set { notificationImage.IsVisible = value; }
        }

        public bool DoneIsVisible
        {
            set { doneImage.IsVisible = value; }
        }

        public bool ShowsAddDate
        {
            set { addDateLabel.IsVisible = value; }
        }

        public bool ShowsDoneDate
        {
            set { doneDateLable.IsVisible = value; }
        }

        private void SetDoneRecognizer()
        {
            if (doneImage.GestureRecognizers.Count > 1) return;
            var recognizer = new TapGestureRecognizer() { Command = DoneCommand, CommandParameter = DoneParam };
            doneImage.GestureRecognizers.Add(recognizer);
        }

        private void SetRemoveRecognizer()
        {
            if (removeImage.GestureRecognizers.Count > 1) return;
            var recognizer = new TapGestureRecognizer() { Command = RemoveCommand, CommandParameter = RemoveParam };
            removeImage.GestureRecognizers.Add(recognizer);
        }

        private void SetAddRecognizer()
        {
            if (addImage.GestureRecognizers.Count > 1) return;
            var recognizer = new TapGestureRecognizer() { Command = AddCommand, CommandParameter = AddParam };
            addImage.GestureRecognizers.Add(recognizer);
        }

        public CleaningCell()
		{
			InitializeComponent();
        }
    }
}