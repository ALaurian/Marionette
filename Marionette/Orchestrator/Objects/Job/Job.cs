using System.ComponentModel;
using System.Runtime.CompilerServices;
using Marionette.Orchestrator.Enums;
using MySqlConnector;

namespace Marionette.Orchestrator;

public partial class Job : INotifyPropertyChanged
{
    // Define the properties of the Job object
    private string _process;
    private string _machine;
    private string _hostname;
    private string _hostIdentity;
    private JobType _jobType;
    private RuntimeType _runtimeType;
    private JobState _state;
    private JobPriority _priority;
    private string _started;
    private string _ended;
    public Orchestrator Orchestrator;
    
    // Get and set the properties for INotifyPropertyChanged
    public string Process
    {
        get { return _process; }
        set
        {
            _process = value;
            NotifyPropertyChanged(nameof(Process));
        }
    }
    public string Machine
    {
        get { return _machine; }
        set
        {
            _machine = value;
            NotifyPropertyChanged(nameof(Machine));
        }
    }
    public string Hostname
    {
        get { return _hostname; }
        set
        {
            _hostname = value;
            NotifyPropertyChanged(nameof(Hostname));
        }
    }
    public string HostIdentity
    {
        get { return _hostIdentity; }
        set
        {
            _hostIdentity = value;
            NotifyPropertyChanged(nameof(HostIdentity));
        }
    }
    public JobType JobType
    {
        get { return _jobType; }
        set
        {
            _jobType = value;
            NotifyPropertyChanged(nameof(JobType));
        }
    }
    public RuntimeType RuntimeType
    {
        get { return _runtimeType; }
        set
        {
            _runtimeType = value;
            NotifyPropertyChanged(nameof(RuntimeType));
        }
    }
    public JobState State
    {
        get { return _state; }
        set
        {
            _state = value;
            NotifyPropertyChanged(nameof(State));
        }
    }
    public JobPriority Priority
    {
        get { return _priority; }
        set
        {
            _priority = value;
            NotifyPropertyChanged(nameof(Priority));
        }
    }
    public string Started
    {
        get { return _started; }
        set
        {
            _started = value;
            NotifyPropertyChanged(nameof(Started));
        }
    }
    public string Ended
    {
        get { return _ended; }
        set
        {
            _ended = value;
            NotifyPropertyChanged(nameof(Ended));
        }
    }
    
    
    public Job(string Process, string Machine, string Hostname, string HostIdentity, JobType JobType,
        RuntimeType RuntimeType, JobState State, JobPriority Priority, string Started, string Ended)
    {
        _process = Process;
        _machine = Machine;
        _hostname = Hostname;
        _hostIdentity = HostIdentity;
        _jobType = JobType;
        _runtimeType = RuntimeType;
        _state = State;
        _priority = Priority;
        _started = Started;
        _ended = Ended;
    }
    
    
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        Orchestrator.UpdateJob(this);
    }
}