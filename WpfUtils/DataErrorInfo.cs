using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WpfUtils
{

    public class Employee : INotifyPropertyChanged, IDataErrorInfo
    {

        private ValidationHandler validationHandler = new ValidationHandler();


        private string _FirstName;

        public string FirstName
        {

            get { return _FirstName; }

            set
            {

                _FirstName = value;

                NotifyPropertyChanged("FirstName");

                bool valid = validationHandler.ValidateRule("FirstName", "First Name must be at least 5 letters!", () => (value.Length >= 5));

            }

        }


        private float _TaxPercent;

        public float TaxPercent
        {

            get { return _TaxPercent; }

            set
            {

                if (_TaxPercent != value)
                {

                    if (value >= 1)

                        value /= 100;

                    _TaxPercent = value;

                    NotifyPropertyChanged("TaxPercent");

                    bool valid = validationHandler.ValidateRule("TaxPercent", "The tax has to be positive!", () => (value > 0));

                }

            }

        }


        protected void NotifyPropertyChanged(string PropertyName)
        {

            if (null != PropertyChanged)

                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));

        }


        public event PropertyChangedEventHandler PropertyChanged;


        public string Error
        {

            get { return null; }
        }


        public string this[string columnName]
        {

            get
            {

                if (this.validationHandler.BrokenRuleExists(columnName))
                {

                    return this.validationHandler[columnName];

                }

                return null;

            }

        }


    }



    public class ValidationHandler
    {

        private Dictionary<string, string> BrokenRules { get; set; }


        public ValidationHandler()
        {

            BrokenRules = new Dictionary<string, string>();

        }


        public string this[string property]
        {

            get
            {

                return this.BrokenRules[property];

            }

        }


        public bool BrokenRuleExists(string property)
        {

            return BrokenRules.ContainsKey(property);

        }


        public bool ValidateRule(string property, string message, Func<bool> ruleCheck)
        {

            if (!ruleCheck())
            {

                this.BrokenRules.Add(property, message);

                return false;

            }

            else
            {

                RemoveBrokenRule(property);

                return true;

            }

        }


        public void RemoveBrokenRule(string property)
        {

            if (this.BrokenRules.ContainsKey(property))
            {
                this.BrokenRules.Remove(property);
            }
        }
    }
}
