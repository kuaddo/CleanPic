using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CleaningPic.Data
{
    // Clone()を利用して、StandaloneObjectとManagedObjectを区別する
    public class DataSource : IDisposable
    {
        private Realm realm;

        public DataSource()
        {
            realm = Realm.GetInstance();
        }

        // 全てのCleaningのリストを取得
        public List<Cleaning> ReadAllCleaning() => 
            realm.All<Cleaning>()
                .ToList()
                .Select(c => c.Clone())
                .ToList();

        // 特定のidのCleaningを取得
        public Cleaning ReadCleaning(string id) =>
            realm.All<Cleaning>()
                .Where(c => c.Id == id)
                .FirstOrDefault()
                ?.Clone();

        // 特定のidのCleaningを取得。一致判定はidのみで行う
        public Cleaning ReadCleaning(Cleaning data) => ReadCleaning(data.Id);

        // 特定のidのCleaningが存在するかを判定
        public bool Exists(string id) =>
            realm.All<Cleaning>()
                .Where(c => c.Id == id)
                .Count() > 0;

        // 特定のidのCleaningが存在するかを判定。一致判定はidのみで行う
        public bool Exists(Cleaning data) => Exists(data.Id);

        // Cleaningを追加する
        public void AddCleaning(Cleaning data) => realm.Write(() => realm.Add(data.Clone()));

        // Cleaningを更新する
        public void UpdateCleaning(Cleaning data) => realm.Write(() => realm.Add(data.Clone(), update: true));

        // Cleaningを削除する。削除するオブジェクトはManagedObjectでなければならない
        public void RemoveCleaning(Cleaning data) => realm.Write(() => realm.Remove(
            realm.All<Cleaning>()
                .Where(c => c.Id == data.Id)
                .FirstOrDefault()));

        // Cleaningを全て削除する
        public void RemoveAllCleaning() => realm.Write(() => realm.RemoveAll<Cleaning>());

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    realm?.Dispose();
                disposedValue = true;
            }
        }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
        }
        #endregion
    }
}
