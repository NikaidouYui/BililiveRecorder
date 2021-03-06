using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Data;
using BililiveRecorder.Core;
using BililiveRecorder.Core.Config;

namespace BililiveRecorder.WPF.Converters
{
    internal class RoomListInterceptConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is RecorderWrapper ? value : value is IRecorder recorder ? new RecorderWrapper(recorder) : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        private class RecorderWrapper : ObservableCollection<IRecordedRoom>
        {
            private readonly IRecorder recorder;

            public RecorderWrapper(IRecorder recorder) : base(recorder)
            {
                this.recorder = recorder;
                Add(null);

                recorder.CollectionChanged += (sender, e) =>
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            if (e.NewItems.Count != 1) throw new NotImplementedException("Wrapper Add Item Count != 1");
                            InsertItem(e.NewStartingIndex, e.NewItems[0] as IRecordedRoom);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            if (e.OldItems.Count != 1) throw new NotImplementedException("Wrapper Remove Item Count != 1");
                            if (!Remove(e.OldItems[0] as IRecordedRoom)) throw new NotImplementedException("Wrapper Remove Item Sync Fail");
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            throw new NotImplementedException("Wrapper Replace Item");
                        case NotifyCollectionChangedAction.Move:
                            throw new NotImplementedException("Wrapper Move Item");
                        case NotifyCollectionChangedAction.Reset:
                            ClearItems();
                            Add(null);
                            break;
                        default:
                            break;
                    }
                };
            }
        }
    }
}
