using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deeprockitems.Content.Upgrades
{
    public class OverclockService
    {
        private Overclock _value;
        public Overclock ThisOverclock
        {
            get => _value;
            set
            {
                _oldValue = _value;
                _value = value;
                OnValueChanged(_value, _oldValue);
            }
        }
        private Overclock _oldValue;
        public delegate void ValueChanged(Overclock newValue, Overclock oldValue);
        public event ValueChanged OnValueChanged; 
    }
}
