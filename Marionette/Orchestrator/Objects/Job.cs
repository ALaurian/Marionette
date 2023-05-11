using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Marionette.Orchestrator.Enums;
using Marionette.Orchestrator.Operations.Helpers;
using MoreLinq.Extensions;
using MySqlConnector;
using Newtonsoft.Json;

namespace Marionette.Orchestrator;

public class Job : INotifyPropertyChanged
{
    // Define the properties of the Job object
    private Process _process;
    private Machine _machine;
    private JobType _jobType;
    private RuntimeType _runtimeType;
    private JobState _state;
    private JobPriority _priority;
    private string _started;
    private string _ended;
    private Orchestrator _orchestrator;

    private Process _prevProcess;
    private Machine _prevMachine;
    private JobType _prevJobType;
    private RuntimeType _prevRuntimeType;
    private JobState _prevState;
    private JobPriority _prevPriority;
    private string _prevStarted;
    private string _prevEnded;

    // Get and set the properties for INotifyPropertyChanged
    public Process Process
    {
        get { return _process; }
        set
        {
            _prevProcess = _process;
            _process = value;
            NotifyPropertyChanged(nameof(Process));
        }
    }

    public Machine Machine
    {
        get { return _machine; }
        set
        {
            _prevMachine = _machine;
            _machine = value;
            NotifyPropertyChanged(nameof(Machine));
        }
    }

    public JobType JobType
    {
        get { return _jobType; }
        set
        {
            _prevJobType = _jobType;
            _jobType = value;
            NotifyPropertyChanged(nameof(JobType));
        }
    }

    public RuntimeType RuntimeType
    {
        get { return _runtimeType; }
        set
        {
            _prevRuntimeType = _runtimeType;
            _runtimeType = value;
            NotifyPropertyChanged(nameof(RuntimeType));
        }
    }

    public JobState State
    {
        get { return _state; }
        set
        {
            _prevState = _state;
            _state = value;
            NotifyPropertyChanged(nameof(State));
        }
    }

    public JobPriority Priority
    {
        get { return _priority; }
        set
        {
            _prevPriority = _priority;
            _priority = value;
            NotifyPropertyChanged(nameof(Priority));
        }
    }

    public string Started
    {
        get { return _started; }
        set
        {
            _prevStarted = _started;
            _started = value;
            NotifyPropertyChanged(nameof(Started));
        }
    }

    public string Ended
    {
        get { return _ended; }
        set
        {
            _prevEnded = _ended;
            _ended = value;
            NotifyPropertyChanged(nameof(Ended));
        }
    }


    public Job(Process Process, Machine Machine, JobType JobType,
        RuntimeType RuntimeType, JobState State, JobPriority Priority, string Started, string Ended,
        Orchestrator orchestrator,
        bool isBeingPulled = false)
    {
        _process = Process;
        _machine = Machine;
        _jobType = JobType;
        _runtimeType = RuntimeType;
        _state = State;
        _priority = Priority;
        _started = Started;
        _ended = Ended;
        _orchestrator = orchestrator;

        _prevProcess = Process;
        _prevMachine = Machine;
        _prevJobType = JobType;
        _prevRuntimeType = RuntimeType;
        _prevState = State;
        _prevPriority = Priority;
        _prevStarted = Started;
        _prevEnded = Ended;

        if (isBeingPulled == false)
        {

            if (RecordBase.RecordExists("jobs", this.ToPublicDictionary(), _orchestrator) == false)
			{
				RecordBase.CreateRecord("jobs", this.ToPublicDictionary(), _orchestrator);
			}
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		RecordBase.UpdateRecord("jobs", this.ToPublicDictionary(), this.ToPrivateDictionary(), _orchestrator);
	}
    
}

