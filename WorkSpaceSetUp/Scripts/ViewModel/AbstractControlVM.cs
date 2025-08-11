using System;
using WorkSpaceSetUp.Scripts.Model;

namespace WorkSpaceSetUp.Scripts.ViewModel
{
    abstract class AbstractControlVM<T>
    {
        #region Variables
        protected T _value;
        #endregion

        #region CallMethods
        public T ControlValue { get { return _value; }
        }
        public abstract bool IsSelected();
        public abstract TResult<int> ValidateSelected(FileGroupManager fileGroupManager);
        public abstract void RemoveSelected();
        public abstract void Fill(FileGroupManager fileGroupManager);
        public abstract void Clear();
        public abstract void Add(T value);
        #endregion


    }
}
