using CleaningPic.Utils;
using System;
using Xamarin.Forms;

namespace CleaningPic.CustomViews
{
	public partial class TopCleaningView : ContentView
    {
        public static readonly BindableProperty CreatedProperty = BindableProperty.Create(
            nameof(Created),
            typeof(DateTimeOffset),
            typeof(TopCleaningView),
            DateTimeOffset.Now,
            propertyChanged: (b, o, n) => (b as TopCleaningView).Created = (DateTimeOffset)n);

        public static readonly BindableProperty DirtOrPlaceProperty = BindableProperty.Create(
            nameof(DirtOrPlace),
            typeof(string),
            typeof(TopCleaningView),
            "",
            propertyChanged: (b, o, n) => (b as TopCleaningView).DirtOrPlace = (string)n);

        public static readonly BindableProperty CleaningTimeProperty = BindableProperty.Create(
            nameof(CleaningTime),
            typeof(int),
            typeof(TopCleaningView),
            0,
            propertyChanged: (b, o, n) => (b as TopCleaningView).CleaningTime = (int)n);
        public static readonly BindableProperty ImageDataProperty = BindableProperty.Create(
            nameof(ImageData),
            typeof(byte[]),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).ImageData = (byte[])n);

        public static readonly BindableProperty ToolsStringProperty = BindableProperty.Create(
            nameof(ToolsString),
            typeof(string),
            typeof(TopCleaningView),
            "",
            propertyChanged: (b, o, n) => (b as TopCleaningView).ToolsString = (string)n);

        public static readonly BindableProperty CanNotifyProperty = BindableProperty.Create(
            nameof(CanNotify),
            typeof(bool),
            typeof(TopCleaningView),
            false,
            propertyChanged: (b, o, n) => (b as TopCleaningView).CanNotify = (bool)n,
            defaultBindingMode: BindingMode.TwoWay);    // TwoWayにしないと動かない。

        public static readonly BindableProperty DoneCommandProperty = BindableProperty.Create(
            nameof(DoneCommand),
            typeof(Command),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).DoneCommand = n as Command);

        public static readonly BindableProperty DoneParamProperty = BindableProperty.Create(
            nameof(DoneParam),
            typeof(object),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).DoneParam = n);

        public static readonly BindableProperty RemoveCommandProperty = BindableProperty.Create(
            nameof(RemoveCommand),
            typeof(Command),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).RemoveCommand = n as Command);

        public static readonly BindableProperty RemoveParamProperty = BindableProperty.Create(
            nameof(RemoveParam),
            typeof(object),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).RemoveParam = n);

        public static readonly BindableProperty AddCommandProperty = BindableProperty.Create(
            nameof(AddCommand),
            typeof(Command),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).AddCommand = n as Command);

        public static readonly BindableProperty AddParamProperty = BindableProperty.Create(
            nameof(AddParam),
            typeof(object),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).AddParam = n);

        public static readonly BindableProperty NotificationCommandProperty = BindableProperty.Create(
            nameof(NotificationCommand),
            typeof(Command),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).NotificationCommand = n as Command);

        public static readonly BindableProperty NotificationParamProperty = BindableProperty.Create(
            nameof(NotificationParam),
            typeof(object),
            typeof(TopCleaningView),
            null,
            propertyChanged: (b, o, n) => (b as TopCleaningView).NotificationParam = n);

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
                timeLabel.Text = timeLabel.Text + "　" + string.Format("{0}分", value);
            }
        }

        public byte[] ImageData
        {
            get { return (byte[])GetValue(ImageDataProperty); }
            set
            {
                SetValue(ImageDataProperty, value);
                dirtImage.Source = new ImageConverter().Convert(value, null, null, null) as ImageSource;
            }
        }

        public string ToolsString
        {
            get { return (string)GetValue(ToolsStringProperty); }
            set
            {
                SetValue(ToolsStringProperty, value);
                toolsLabel.Text = toolsLabel.Text + "　" + value;
            }
        }

        public bool CanNotify
        {
            get { return (bool)GetValue(CanNotifyProperty); }
            set
            {
                SetValue(CanNotifyProperty, value);
                if (value)
                    notificationImage.Source = "ic_notification_on.png";
                else
                    notificationImage.Source = "ic_notification_off.png";
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

        public Command NotificationCommand
        {
            get { return GetValue(NotificationCommandProperty) as Command; }
            set
            {
                SetValue(NotificationCommandProperty, value);
                if (NotificationParam != null)
                    SetNotificationRecognizer();
            }
        }

        public object NotificationParam
        {
            get { return GetValue(NotificationParamProperty); }
            set
            {
                SetValue(NotificationParamProperty, value);
                if (NotificationCommand != null)
                    SetNotificationRecognizer();
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
            set { addImage.IsVisible = value; }
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

        public bool ChangesAddColor
        {
            set { if (value) SetAddFinishRecognizer(); }
        }

        private void SetDoneRecognizer()
        {
            var recognizer = new TapGestureRecognizer() { Command = DoneCommand, CommandParameter = DoneParam };
            doneImage.GestureRecognizers.Add(recognizer);
        }

        private void SetRemoveRecognizer()
        {
            var recognizer = new TapGestureRecognizer() { Command = RemoveCommand, CommandParameter = RemoveParam };
            removeImage.GestureRecognizers.Add(recognizer);
        }

        private void SetAddRecognizer()
        {
            var recognizer = new TapGestureRecognizer() { Command = AddCommand, CommandParameter = AddParam };
            addImage.GestureRecognizers.Add(recognizer);
        }

        private void SetAddFinishRecognizer()
        {
            var recognizer = new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    addImage.IsVisible = false;
                    addFinishImage.IsVisible = true;
                })
            };
            addImage.GestureRecognizers.Add(recognizer);
        }

        private void SetNotificationRecognizer()
        {
            var recognizer = new TapGestureRecognizer() { Command = NotificationCommand, CommandParameter = NotificationParam };
            notificationImage.GestureRecognizers.Add(recognizer);
        }

        public TopCleaningView()
        {
            InitializeComponent();
        }
    }
}