using System.Collections;
using MoreLinq;
using WinRT;

namespace Marionette.Workflow;

public partial class Workflow : Dictionary<string, object>
{
    private Func<object, object>[] _actions;
    private bool _isRunning;
    private int _currentAction;
    private object[] _iterator;


    public Workflow()
    {
        _actions = Array.Empty<Func<object, object>>();
        _isRunning = false;
        _currentAction = 0;
    }

    public Workflow CreateSequence(Func<object, object>[] actions)
    {
        _actions = actions;
        _actions.ForEach(x => { Add(x.Method.GetParameters()[0].Name, null); });
        return this;
    }

    public Workflow JumpTo(string label)
    {
        _actions.ForEach(x =>
        {
            if (x.Method.GetParameters()[0].Name == label)
            {
                _currentAction = Array.IndexOf(_actions, x);
            }
        });

        return this;
    }

    public Workflow Execute()
    {
        if (_isRunning)
            throw new Exception("Cannot execute workflow while it is already running.");

        try
        {
            _isRunning = true;
            for (; _currentAction < _actions.Length; _currentAction++)
            {
                var action = _actions[_currentAction];
                var output = action.Invoke(null);
                this[action.Method.GetParameters()[0].Name] = output;

                if (output is IEnumerable && _iterator is null)
                {
                    _iterator = (output as IEnumerable<object>).ToArray();
                    _currentItem = _iterator[0];
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        _isRunning = false;
        return this;
    }

    public TInterface Return<TInterface>(string alias)
    {
        return this[alias].As<TInterface>();
    }
}