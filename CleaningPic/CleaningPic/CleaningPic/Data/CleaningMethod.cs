using CleaningPic.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleaningPic.Data
{
    public class CleaningMethod : BindableBase
    {
        private string dirt;        // TODO: 後でDirtクラスを作る
        private string method;
        private List<string> tools;
        private byte[] imageData;
        private DateTime created;

        public string Dirt              // 汚れの種類
        {
            get { return dirt; }
            set { SetProperty(ref dirt, value); }
        }
        
        public string Method            // 掃除の手法
        {
            get { return method; }
            set { SetProperty(ref method, value); }
        }

        public List<string> Tools       // 掃除の道具
        {
            get { return tools; }
            set { SetProperty(ref tools, value); }
        }

        public byte[] ImageData             // 汚れの画像
        {
            get { return imageData; }
            set { SetProperty(ref imageData, value); }
        }

        public DateTime Created         // この手法のクラスを作成した日時。ソートに利用
        {
            get { return created; }
            set { SetProperty(ref created, value); }
        }
    }
}
