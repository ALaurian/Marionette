using System.ComponentModel;
using Marionette.Orchestrator.Operations.Helpers;

namespace Marionette.Orchestrator;

public class Asset : INotifyPropertyChanged
{
    private string _name;
    private string _prevName;
    private string _value;
    private string _prevValue;
    private Orchestrator _orchestrator;
    
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

    public string Value
    {
        get => _value;
        set
        {
            _prevValue = _value;
            _value = value;
            NotifyPropertyChanged(nameof(Value));
        }
    }
    
    public Asset(string name, string value, Orchestrator orchestrator, bool isBeingPulled = false)
    {
        _name = name;
        _value = value;
        _orchestrator = orchestrator;
        
        _prevName = name;
        _prevValue = value;

        if (isBeingPulled == false)
        {
            if (RecordBase.RecordExists("assets", this.ToPublicDictionary(), _orchestrator) == false)
            {
                RecordBase.CreateRecord("assets", this.ToPublicDictionary(), _orchestrator);
            }
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        RecordBase.UpdateRecord("assets", this.ToPublicDictionary(), this.ToPrivateDictionary(), _orchestrator);
    }
}