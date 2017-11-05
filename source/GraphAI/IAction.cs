using System;
using System.Collections.Generic;
using System.Text;

namespace GraphAI
{
    public interface IAction
    {
        IState GetResult();
    }
}
