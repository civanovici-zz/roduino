using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using RoDuino.SMS.Bll.Bll.Base;

namespace RoDuino.SMS.Bll.Bll
{
    [ActiveRecord("Clients")]
    public class Client : BaseItem<Client>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IList<SmsHistory> histories = new List<SmsHistory>();
        private bool isDeleted;
        private string phone;
        private string network;
        private string name;
        private string email;

        public override string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        [Property,  ValidateNonEmpty("Phone required")]
        public string Phone
        {
            get { return phone; }
            set
            {
                phone = value;
                RaisePropertyChanged("Phone");
            }
        }


        [Property,  ValidateNonEmpty("Network required")]
        public string Network
        {
            get { return network; }
            set
            {
                network = value;
                RaisePropertyChanged("Network");
            }
        }

        public string Message { get; set; }

        [Property]
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }

        [Property]
        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                RaisePropertyChanged("IsDeleted");
            }
        }

//        [HasMany(typeof(SmsHistory))]
//        public IList<SmsHistory> History
//        {
//            get { return histories; }
//            set
//            {
//                histories=value;
//                RaisePropertyChanged("History");
//            }
//        }


        protected void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                //                Console.WriteLine("<{0} Id='{1}' Property='{2}'>", this.GetType().Name, this.Id, property);
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
