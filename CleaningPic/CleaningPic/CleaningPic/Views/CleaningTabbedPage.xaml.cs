﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CleaningPic.Views
{
    public partial class CleaningTabbedPage : TabbedPage
    {
        public CleaningTabbedPage(bool startWantToDoPage)
        {
            InitializeComponent();
            if (startWantToDoPage)
                CurrentPage = wantToDoPage;
            else
                CurrentPage = donePage;

            // やりたい、やったのタイトル切り替え
            CurrentPageChanged += (sender, e) =>
            {
                Title = CurrentPage.Title;
            };
        }
    }
}