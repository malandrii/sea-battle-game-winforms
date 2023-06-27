using System.Windows.Forms;

namespace SeaBattle
{
    public class FormControlManager
    {
        public MainForm _mainForm;

        public FormControlManager(MainForm mainForm) => _mainForm = mainForm;

        public T GetSelectedShipControl<T>() where T : Control
            => GetControlByIndex<T>(_mainForm.PreGameController.ChosenShipSize);

        public T GetControlByIndex<T>(int index) where T : Control
        {
            return (T)_mainForm.Controls[GetControlKey(typeof(T).Name, index)];
        }

        private static string GetControlKey(string controlName, int index)
        {
            return (controlName + (index) + "x").ToString();
        }
    }
}
