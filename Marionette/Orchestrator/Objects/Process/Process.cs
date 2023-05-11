using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Marionette.Orchestrator.Operations.Helpers;

namespace Marionette.Orchestrator;

public partial class Process : INotifyPropertyChanged
{
    private Orchestrator _orchestrator;
    private string _name;
    private string _prevName;
    private string _version;
    private string _prevVersion;
    private string _description;
    private string _prevDescription;
    private string _path;
    private string _prevPath;

    public string Name
    {
        get => _name;
        set
        {
            _prevName = _name;
            _name = value;
            NotifyPropertyChanged(nameof(Name));
        }
    }
    public string Version
    {
        get => _version;
        set
        {
            _prevVersion = _version;
            _version = value;
            NotifyPropertyChanged(nameof(Version));
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _prevDescription = _description;
            _description = value;
            NotifyPropertyChanged(nameof(Description));
        }
    }

    public string Path
    {
        get => _path;
        set
        {
            _prevPath = _path;
            _path = value;
            NotifyPropertyChanged(nameof(Path));
        }
    }

    
    public Process(string name, string version, string description, string path, Orchestrator orchestrator,
        bool isBeingPulled = false)
    {
        _name = name;
        _version = version;
        _description = description;
        _path = path;
        _orchestrator = orchestrator;
        
        _prevName = name;
        _prevVersion = version;
        _prevDescription = description;
        _prevPath = path;

        if (isBeingPulled == false)
        {
            if (RecordBase.RecordExists("processes", this.ToPublicDictionary(), _orchestrator) == false)
            {
                RecordBase.CreateRecord("processes", this.ToPublicDictionary(), _orchestrator);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        RecordBase.UpdateRecord("processes", this.ToPublicDictionary(), this.ToPrivateDictionary(), _orchestrator);
    }
}