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
        private double probability;
        private string toolsString;

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
            set
            {
                SetProperty(ref tools, value);
                var sb = new StringBuilder();
                foreach (var tool in tools)
                {
                    sb.Append(tool);
                    sb.Append("・");
                }
                SetProperty(ref toolsString, sb.ToString().Substring(0, sb.Length - 1));    // 最後の余分な"・"を取り除く
            }
        }

        public byte[] ImageData         // 汚れの画像
        {
            get { return imageData; }
            set { SetProperty(ref imageData, value); }
        }

        public DateTime Created         // この手法のクラスを作成した日時。ソートに利用
        {
            get { return created; }
            set { SetProperty(ref created, value); }
        }

        public double Probability         // 汚れの予想確率。0~1
        {
            get { return probability; }
            set { SetProperty(ref probability, value); }
        }

        public string ToolsString       // リスト表示用の道具リストの文字列
        {
            get { return toolsString; }
        }
    }
}
