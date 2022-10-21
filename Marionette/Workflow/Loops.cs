using System.Collections;
using MoreLinq;

namespace Marionette.Workflow;

public partial class Workflow
{
    private int _collectionIndex = 0;

    public bool Next(string label)
    {
        _actions.ForEach(x =>
        {
            if (x.Method.GetParameters()[0].Name == label)
            {
                if (_currentAction < _iterator.Cast<object>().ToArray().Length)
                {
                    _currentAction = Array.IndexOf(_actions, x) - 1;
                    _collectionIndex += 1;
                }
                else
                {
                    _iterator = null;
                    _currentAction = Array.IndexOf(_actions, x);
                    _collectionIndex = 0;
                }
            }
        });

        return true;
    }
}