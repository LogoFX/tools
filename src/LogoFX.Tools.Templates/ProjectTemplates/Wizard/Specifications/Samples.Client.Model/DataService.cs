using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LogoFX.Core;
using $saferootprojectname$.Client.Data.Contracts.Providers;
using $safeprojectname$.Contracts;
using $safeprojectname$.Mappers;
using System.Windows.Threading;
using Caliburn.Micro;

namespace $safeprojectname$
{
    [UsedImplicitly]
    internal sealed class DataService : PropertyChangedBase, IDataService
    {
        private readonly IWarehouseProvider _warehouseProvider;
        private readonly IEventsProvider _eventsProvider;
        private readonly RangeObservableCollection<IWarehouseItem> _warehouseItems = new RangeObservableCollection<IWarehouseItem>();

        private readonly DispatcherTimer _timer;
        private DateTime _lastEvenTime;
        private readonly RangeObservableCollection<IEvent> _events =
            new RangeObservableCollection<IEvent>();

        public DataService(IWarehouseProvider warehouseProvider, IEventsProvider eventsProvider)
        {
            _warehouseProvider = warehouseProvider;
            _eventsProvider = eventsProvider;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += OnTimer;
        }

        private async void OnTimer(object sender, EventArgs e)
        {
            var events = (await _eventsProvider.GetLastEvents(_lastEvenTime))
                .Select(EventMapper.MapToEvent)
                .ToList();

            if (events.Count == 0)
            {
                return;
            }

            var max = events.Max(x => x.Time);
            if (max > _lastEvenTime)
            {
                _lastEvenTime = max;
            }

            _events.AddRange(events);
        }

        private async Task GetWarehouseItemsInternal()
        {
            var warehouseItems = (await _warehouseProvider.GetWarehouseItems()).Select(WarehouseMapper.MapToWarehouseItem);
            _warehouseItems.Clear();
            _warehouseItems.AddRange(warehouseItems);
        }

        IEnumerable<IWarehouseItem> IDataService.WarehouseItems
        {
            get { return _warehouseItems; }
        }

        async Task IDataService.GetWarehouseItemsAsync()
        {
            await ServiceRunner.RunAsync(GetWarehouseItemsInternal);
        }

        async Task<IWarehouseItem> IDataService.NewWarehouseItemAsync()
        {
            await Task.Delay(1000);
            return new WarehouseItem("", 0d, 1);
        }

        async Task IDataService.SaveWarehouseItemAsync(IWarehouseItem item)
        {
            var dto = WarehouseMapper.MapToWarehouseDto(item);
            await _warehouseProvider.SaveWarehouseItem(dto);
        }

        async Task IDataService.DeleteWarehouseItemAsync(IWarehouseItem item)
        {
            await _warehouseProvider.DeleteWarehouseItem(item.Id);
            _warehouseItems.Remove(item);
        }

        void IDataService.StartEventMonitoring()
        {
            _lastEvenTime = DateTime.Now;
            _timer.Start();
            NotifyOfPropertyChange(() => EventMonitoringStarted);
        }

        void IDataService.StopEventMonitoring()
        {
            _timer.Stop();
            NotifyOfPropertyChange(() => EventMonitoringStarted);
        }

        async Task IDataService.ClearEventsAsync()
        {
            await Task.Delay(400);
            _events.Clear();
            await Task.Delay(300);
        }

        IEnumerable<IEvent> IDataService.Events
        {
            get { return _events; }
        }

        public bool EventMonitoringStarted
        {
            get { return _timer.IsEnabled; }
        }
    }
}
