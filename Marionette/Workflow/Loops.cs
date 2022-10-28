using MoreLinq;
using WinRT;

namespace Marionette.Workflow;

public partial class Workflow
{
    private int _iteratorIndex = 0;
    private object _currentItem;
    
    public bool Next(string label)
    {
        _actions.ForEach(x =>
        {
            if (x.Method.GetParameters()[0].Name == label)
            {
                if (_iterator is not null)
                {
                    try
                    {
                        _iteratorIndex++;
                        _currentItem = _iterator[_iteratorIndex];
                        _currentAction = Array.IndexOf(_actions, x) - 1;
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine("Enumerator has reached the end of the collection.");
                        _iterator = null;
                        _iteratorIndex = 0;
                    }
                }
            }
        });

        return true;
    }

    public TInterface ReturnIterable<TInterface>()
    {
        return _currentItem.As<TInterface>();
    }
}