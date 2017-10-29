using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleaningPic.Data
{
    public class Cleaning : RealmObject, ICloneable
    {
        private const char concatChar = '・';

        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Id。DBのアップデートなどに利用
        public string Dirt { get; set; }                            // 汚れの種類。TODO: 後でDirtクラスを作る
        public string Method { get; set; }                          // 掃除の手法
        public string ToolsString { get; private set; }             // リスト表示用の道具リストの文字列
        public IList<string> Tools                                  // 掃除の道具。入出力のインターフェースで、保存はToolsStringの方で行う
        {
            get { return ToolsString.Split(concatChar); }
            set { ToolsString = string.Join(concatChar.ToString(), value); }
        }
        public int CleaningTime { get; set; }
        public byte[] ImageData { get; set; }                       // 汚れの画像
        public bool Done { get; set; } = false;                     // やりたいならfalse、やったならtrue
        public DateTimeOffset Created { get; set; }                 // この手法のクラスを作成・追加・完了した日時。ソートに利用。offsetが上手く保存されないのでUTCで保存する
        public double Probability { get; set; }                     // 汚れの予想確率。0~1
        public bool CanNotify { get; set; } = false;                // 通知を行うかどうか
        public DateTimeOffset NotificationDate { get; set; }        // 通知日時

        public override string ToString()
        {
            // TODO: Place実装時にPlaceの情報も追加する
            return Dirt;
        }

        public Cleaning Clone()
        {
            var clone = new Cleaning()
            {
                Id = Id,
                Dirt = Dirt,
                Method = Method,
                ToolsString = ToolsString,
                CleaningTime = CleaningTime,
                ImageData = ImageData,
                Done = Done,
                Created = Created,
                Probability = Probability,
                CanNotify = CanNotify,
                NotificationDate = NotificationDate
            };
            return clone;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
