using Realms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleaningPic.Data
{
    public class Cleaning : RealmObject, ICloneable
    {
        private const char concatChar = '・';

        [PrimaryKey]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Id。DBのアップデートなどに利用
        private int _Place { get; set; }                            // 汚れのある場所(Placeに対応するint)
        [Ignored]
        public Place Place
        {
            get { return (Place)_Place; }
            set { _Place = (int)value; }
        }                            
        public string Dirt { get; set; }                            // 汚れの種類。
        public string Method { get; set; }                          // 掃除の手法
        public string Caution { get; set; }                         // 掃除の注意点
        private string _ToolsString { get; set; }                   // Tools保存用
        [Ignored]
        public string ToolsString                                   // ToolsStringのリスト表示用
        {
            get
            {
                var ret = string.Join(concatChar.ToString(), Tools.Take(2));
                if (Tools.Count() > 2) ret += "...";
                return ret;
            }
        }
        public IList<string> Tools                                  // 掃除の道具。入出力のインターフェースで、保存はToolsStringの方で行う
        {
            get { return _ToolsString.Split(concatChar); }
            set { _ToolsString = string.Join(concatChar.ToString(), value); }
        }
        public int CleaningTime { get; set; }
        public byte[] ImageData { get; set; }                       // 汚れの画像
        public bool Done { get; set; } = false;                     // やりたいならfalse、やったならtrue
        public DateTimeOffset Created { get; set; }                 // この手法のクラスを作成・追加・完了した日時。ソートに利用。offsetが上手く保存されないのでUTCで保存する
        public bool CanNotify { get; set; } = false;                // 通知を行うかどうか
        public DateTimeOffset NotificationDate { get; set; }        // 通知日時

        public override string ToString()
        {
            return Place.DisplayName() + "の" + Dirt;
        }

        public Cleaning Clone()
        {
            var clone = new Cleaning()
            {
                Id = Id,
                _Place = _Place,
                Dirt = Dirt,
                Method = Method,
                Caution = Caution,
                _ToolsString = _ToolsString,
                CleaningTime = CleaningTime,
                ImageData = ImageData,
                Done = Done,
                Created = Created,
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
