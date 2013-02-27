using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using RoDuino.SMS.Bll.Bll;

namespace RoDuino.SMS.Helpers.Collections
{
    public class ClientsCollection : ObservableCollection<Client>, INotifyPropertyChanged
    {
        private Client current;
        private bool dirty;

       

        public Client Current
        {
            get { return current; }
            set
            {
                if (current != value)
                {
                    current = value;
                    OnPropertyChanged("Current");
                    OnCurrentChanged();
                }
            }
        }

        /// <summary>
        /// Get or set if the list has been modified.
        /// </summary>
        public bool IsDirty
        {
            get { return dirty; }
            set { dirty = value; }
        }

        public Client Find(int id)
        {
            foreach (Client person in this)
            {
                if (person.Id == id)
                    return person;
            }

            return null;
        }
        /// <summary>
        /// Gets the next person in the people list.  
        /// Returns null if the current person is the last person in the list.
        /// </summary>
        public Client Next(int i)
        {
            Client p = null;

            foreach (Client person in this)
            {
                if (this.IndexOf(person) == i + 1)
                {
                    return person;
                }
            }

            return p;
        }

        /// <summary>
        /// Gets the previous person in the people list.  
        /// Returns null if the current person is the first person in the list.
        /// </summary>
        public Client Previous(int i)
        {
            Client p = null;

            foreach (Client person in this)
            {
                if (this.IndexOf(person) == i)
                {
                    return person;
                }
            }

            return p;
        }

        public event EventHandler CurrentChanged;
        protected void OnCurrentChanged()
        {
            if (CurrentChanged != null)
                CurrentChanged(this, EventArgs.Empty);
        }

        protected override event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler<ContentChangedEventArgs> ContentChanged;
        public void OnContentChanged()
        {
            dirty = true;
            if (ContentChanged != null)
                ContentChanged(this, new ContentChangedEventArgs(null));
        }

        /// <summary>
        /// The details of a person changed, and a new person was added to the collection.
        /// </summary>
        public void OnContentChanged(Client newPerson)
        {
            dirty = true;
            if (ContentChanged != null)
                ContentChanged(this, new ContentChangedEventArgs(newPerson));
        }

    }

    public class ContentChangedEventArgs : EventArgs
    {
        private Client newPerson;

        public Client NewPerson
        {
            get { return newPerson; }
        }

        public ContentChangedEventArgs(Client newPerson)
        {
            this.newPerson = newPerson;
        }

    }
}
