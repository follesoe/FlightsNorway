using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Phone.Controls.Samples
{
    public abstract class PivotItemCollectionAdaptor<T> : ObservableCollection<PivotItem>
    {
        #region Constructors
        public PivotItemCollectionAdaptor()
        {
        }

        public PivotItemCollectionAdaptor(IList<T> list)
        {
            foreach (T item in list)
            {
                this.Add(item);
            }
        }
        #endregion

        #region Abstracts
        protected abstract T OnGetItem(PivotItem pivot);
        protected abstract void OnSetItem(PivotItem pivot, T item);
        #endregion

        #region Virtuals
        protected virtual void OnCreateItem(PivotItem pivot, T item)
        {
            OnSetItem(pivot, item);
        }
        #endregion

        #region Methods
        public new IList<T> Items
        {
            get
            {
                List<T> list = new List<T>();
                foreach (PivotItem pivot in base.Items)
                {
                    T item = this.OnGetItem(pivot);
                    list.Add(item);
                }
                return list;
            }
        }

        public T GetItem(PivotItem pivot)
        {
            if (!base.Contains(pivot))
                throw new KeyNotFoundException();

            return this.OnGetItem(pivot);
        }

        public void SetItem(PivotItem pivot, T item)
        {
            if (!base.Contains(pivot))
                throw new KeyNotFoundException();

            this.OnSetItem(pivot, item);
        }

        public new T this[int index]
        {
            get { return GetItem(base[index]); }
            set { SetItem(base[index], value); }
        }

        public T this[PivotItem pivot]
        {
            get { return GetItem(pivot); }
            set { SetItem(pivot, value); }
        }

        public void InsertItem(int index, T item)
        {
            PivotItem pivot = new PivotItem();
            this.OnCreateItem(pivot, item);
            base.InsertItem(index, pivot);
        }

        public void Add(T item)
        {
            PivotItem pivot = new PivotItem();
            this.OnCreateItem(pivot, item);
            base.Add(pivot);
        }

        public void Remove(T item)
        {
            foreach (PivotItem pivot in base.Items)
            {
                T val = OnGetItem(pivot);
                if (item.Equals(val))
                {
                    base.Remove(pivot);
                    return;
                }
            }
        }
        #endregion
    }
}
