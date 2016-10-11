using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.Commanding;

namespace LogoFX.Tools.Templates.Wizard.ViewModel
{
    public sealed class WaitEventArgs : EventArgs
    {
        public Exception Error { get; set; }

        public bool Cancelled { get; set; }
    }

    public sealed class WaitViewModel : PropertyChangedBase, IProgress<double>
    {
        private CancellationTokenSource _cancellationTokenSource;

        public EventHandler<WaitEventArgs> Completed = delegate { };

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand =
                           ActionCommand
                               .When(() => !IsCancelling && _cancellationTokenSource != null)
                               .Do(() =>
                               {
                                   IsCancelling = true;
                                   _cancellationTokenSource.Cancel();
                               })
                               .RequeryOnPropertyChanged(this, () => IsCancelling));
            }
        }

        private bool _isCancelling;

        public bool IsCancelling
        {
            get { return _isCancelling; }
            private set
            {
                if (_isCancelling == value)
                {
                    return;
                }

                _isCancelling = value;
                NotifyOfPropertyChange();
            }
        }

        private int _progress;

        public int Progress
        {
            get { return _progress; }
            private set
            {
                if (_progress == value)
                {
                    return;
                }

                _progress = value;
                NotifyOfPropertyChange();
            }
        }

        private string _caption;

        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption == value)
                {
                    return;
                }

                _caption = value;
                NotifyOfPropertyChange();
            }
        }

        public void Run(Action<IProgress<double>, CancellationToken> action)
        {
            Debug.Assert(_cancellationTokenSource == null);

            _cancellationTokenSource = new CancellationTokenSource();

            action.BeginInvoke(this, _cancellationTokenSource.Token, OnCallback, null);
        }

        private void OnCallback(IAsyncResult ar)
        {
            _cancellationTokenSource = null;

            Completed(this,
                new WaitEventArgs
                {
                    Error = null,
                    Cancelled = IsCancelling || !ar.IsCompleted
                });

            IsCancelling = false;
        }

        public void Report(double value)
        {
            Progress = (int) Math.Round(value*100.0);
        }
    }
}