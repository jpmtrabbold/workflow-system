using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Service.Models.Helpers
{
    public class LazyLoadedData<T> where T: class, new()
    {
        public LazyLoadedData()
        {
            State = LazyLoadedDataStateEnum.NotLoaded;
            Data = new T();
        }

        public LazyLoadedData(T data)
        {
            State = LazyLoadedDataStateEnum.Ready;
            Data = data;
        }

        public T Data { get; set; }
        public LazyLoadedDataStateEnum State { get; set; }
    }

    public enum LazyLoadedDataStateEnum
    {
        NotLoaded = 0,
        Loading = 1,
        Ready = 2,
    }
}
