using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRMS.Interfaces
{
    public delegate bool CommitEditOp(object data);
    public interface IEditorFactory
    {
        bool ShowNewDialog(object dataContext, CommitEditOp commitEditOp);
        bool ShowEditDialog(object dataContext, CommitEditOp commitEditOp);
        bool ShowDeleteDialog(string msg);
    }
}
