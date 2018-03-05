using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace MRMS.Helpers
{
    public class CustomGridKeyboardCommandProvider : DefaultKeyboardCommandProvider
    {
        private GridViewDataControl dataControl;

        public CustomGridKeyboardCommandProvider(GridViewDataControl dataControl)
            : base(dataControl)
        {
            this.dataControl = dataControl;
            this.ModifiersProvider = () => Keyboard.Modifiers;
        }

        internal Func<ModifierKeys> ModifiersProvider { get; private set; }
        private ModifierKeys Modifiers
        {
            get
            {
                return this.ModifiersProvider.Invoke();
            }
        }

        public override IEnumerable<ICommand> ProvideCommandsForKey(Key key)
        {
            List<ICommand> commandsToExecute = base.ProvideCommandsForKey(key).ToList();
            bool ctrlIsPressed = (this.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
            bool shiftPress = (this.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
            bool altPress = (this.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
            switch (key)
            {
                case Key.Enter:
                    {
                        commandsToExecute.Clear();
                        commandsToExecute = base.ProvideCommandsForKey(Key.Tab).ToList();
                        break;
                    }
                case Key.Left:
                case Key.Right:
                case Key.Up:
                case Key.Down:
                    {
                        commandsToExecute.Clear();
                        break;
                    }
            }
            return commandsToExecute;
        }
    }
}
