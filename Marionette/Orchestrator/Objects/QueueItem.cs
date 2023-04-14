using Marionette.Orchestrator.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Marionette.Orchestrator
{
    public class QueueItem : INotifyPropertyChanged
    {
        private bool DisableNotifyPropertyChanged = true;
        public QueueItem(string assignedTo, string deferDate, string dueDate, int id, Guid itemkey, string lastProcessingOn,
            Dictionary<string,object> output, QueueItemPriority priority, string progress, string queueName, string reference,
            int retryno, string reviewStatus,
            Dictionary<string,object> specificContent, string startTransactionTime,QueueItemStatus status,
            Orchestrator orchestrator)
        {
            
            AssignedTo = assignedTo;
            DeferDate = deferDate;
            DueDate = dueDate;
            Id = id;
            ItemKey = itemkey;
            LastProcessingOn = lastProcessingOn;
            Output = output;
            Priority = priority;
            Progress = progress;
            QueueName = queueName;
            Reference = reference;
            RetryNo = retryno;
            ReviewStatus = reviewStatus;
            SpecificContent = specificContent;
            StartTransactionTime = startTransactionTime;
            Status = status;
            Orchestrator = orchestrator;
            DisableNotifyPropertyChanged = false;
        }
        
        private string _assignedTo;
        private string _deferDate;
        private string _dueDate;
        private int _id;
        private Guid _itemKey;
        private string _lastProcessingOn;
        private Dictionary<string, object> _output;
        private QueueItemPriority _priority;
        private string _progress;
        private string _queueName;
        private string _reference;
        private int _retryNo;
        private string _reviewStatus;
        private Dictionary<string, object> _specificContent;
        private string _startTransactionTime;
        private QueueItemStatus _status;

        public string AssignedTo
        {
            get { return _assignedTo; }
            set
            {
                _assignedTo = value;
                NotifyPropertyChanged(nameof(AssignedTo));
            }
        }

        public string DeferDate
        {
            get { return _deferDate; }
            set
            {
                _deferDate = value;
                NotifyPropertyChanged(nameof(DeferDate));
            }
        }

        public string DueDate
        {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                NotifyPropertyChanged(nameof(DeferDate));
            }
        }

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }

        public Guid ItemKey
        {
            get { return _itemKey; }
            set
            {
                _itemKey = value;
                NotifyPropertyChanged(nameof(ItemKey));
            }
        }

        public string LastProcessingOn
        {
            get { return _lastProcessingOn; }
            set
            {
                _lastProcessingOn = value;
                NotifyPropertyChanged(nameof(LastProcessingOn));
            }
        }

        public Dictionary<string, object> Output
        {
            get { return _output; }
            set
            {
                _output = value;
                NotifyPropertyChanged(nameof(Output));
            }
        }

        public QueueItemPriority Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                NotifyPropertyChanged(nameof(Priority));
            }
        }

        public string Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                NotifyPropertyChanged(nameof(Progress));
            }
        }

        public string QueueName
        {
            get { return _queueName; }
            set
            {
                _queueName = value;
                NotifyPropertyChanged(nameof(QueueName));
            }
        }

        public string Reference
        {
            get { return _reference; }
            set
            {
                _reference = value;
                NotifyPropertyChanged(nameof(Reference));
            }
        }

        public int RetryNo
        {
            get { return _retryNo; }
            set
            {
                _retryNo = value;
                NotifyPropertyChanged(nameof(RetryNo));
            }
        }

        public string ReviewStatus
        {
            get { return _reviewStatus; }
            set
            {
                _reviewStatus = value;
                NotifyPropertyChanged(nameof(ReviewStatus));
            }
        }

        public Dictionary<string, object> SpecificContent
        {
            get { return _specificContent; }
            set
            {
                _specificContent = value;
                NotifyPropertyChanged(nameof(SpecificContent));
            }
        }

        public string StartTransactionTime
        {
            get { return _startTransactionTime; }
            set
            {
                _startTransactionTime = value;
                NotifyPropertyChanged(nameof(StartTransactionTime));
            }
        }

        public QueueItemStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged(nameof(Status));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Call UpdateQueueItem method whenever a property is changed
            if (IsInitialized() && DisableNotifyPropertyChanged == false)
            {
                Orchestrator.UpdateQueueItem(this,QueueName);
            }

            
        }
        
        private bool IsInitialized()
        {
            return AssignedTo != null &&
                   DeferDate != null &&
                   DueDate != null &&
                   Id != null &&
                   ItemKey != null &&
                   LastProcessingOn != null &&
                   Output != null &&
                   Priority != null &&
                   Progress != null &&
                   QueueName != null &&
                   Reference != null &&
                   RetryNo != null &&
                   ReviewStatus != null &&
                   SpecificContent != null &&
                   StartTransactionTime != null &&
                   Status != null;
        }
        
        public Orchestrator Orchestrator;
    }
}