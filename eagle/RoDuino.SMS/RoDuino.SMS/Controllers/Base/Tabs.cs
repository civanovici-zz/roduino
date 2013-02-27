using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using RoDuino.SMS.Bll.Attributes;
using RoDuino.SMS.Bll.Util;

namespace RoDuino.SMS.Controllers.Base
{
    public class Tabs
    {
        private static Tabs instance;
        private Tab current;
        private ObservableCollection<Tab> itemTabs = new ObservableCollection<Tab>();


        public delegate void TabClose(Tab tab);
        public delegate void ChangeTab(Tab fromTab, Tab toTab);
        public event ChangeTab ChangedTab;
        public event TabClose TabClosing;

        private Tabs()
        {
            Tab tab = new Tab { Name = "Index", Id = Guid.NewGuid(), HorizontalAlignment = HorizontalAlignment.Stretch };
            itemTabs.Add(tab);
            //            itemTabs.Add(new Tab() { Name = "Detail", Id = Guid.NewGuid(), HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch });
            Current = tab;
        }

        /// <summary>
        /// the tabs
        /// </summary>
        public ObservableCollection<Tab> ItemTabs
        {
            get { return itemTabs; }
            set { itemTabs = value; }
        }

        public Tab Current
        {
            get { return current; }
            set
            {
                if (current != null)
                {
                    Tab oldTab = current;
                    current.Hide();
                    this.RaiseChangedTab(oldTab, value);
                }
                current = value;
                current.Show();
            }
        }

        public static Tabs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Tabs();
                }
                return instance;
            }
            set { instance = value; }
        }

        public void RaiseChangedTab(Tab fromTab, Tab toTab)
        {
            if (ChangedTab != null)
                ChangedTab(fromTab, toTab);
        }

        public void RaiseTabClosing(Tab tab)
        {
            if (TabClosing != null)
                TabClosing(tab);
        }

        /// <summary>
        /// closes a tab
        /// </summary>
        /// <param name="tab"></param>
        public void Close(Tab tab)
        {
            //if more then one tab
            if (itemTabs.Count > 1)
            {
                //close and remove
                this.RaiseTabClosing(tab);
                tab.Close();
                itemTabs.Remove(tab);
                //set current the first
                Current = itemTabs[0];
                tab = null;
            }
            //othewise do nothing as the last tab can't be closed
        }

        /// <summary>
        /// opens a new tab navigating at uri
        /// </summary>
        /// <param name="uri"></param>
        public void NewTab(string uri)
        {
            NewTab(uri, null, null, null);
        }

        /// <summary>
        /// opens a new tab navigating at uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="propertybag"></param>
        public void NewTab(string uri, IDictionary propertybag)
        {
            NewTab(uri, propertybag, null, null);
        }


        /// <summary>
        /// opens a new tab avigating at uri, and displaying the image
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="img"></param>
        /// <param name="name"></param>
        public void NewTab(string uri, byte[] img, string name)
        {
            NewTab(uri, null, img, name);
        }


        /// <summary>
        /// opens a new tab avigating at uri, and displaying the image
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="propertyBag"></param>
        /// <param name="img"></param>
        /// <param name="name"></param>
        public void NewTab(string uri, IDictionary propertyBag, byte[] img, string name)
        {
            Tab oldTab = Current;
            Current.Hide();
            Tab tab = new Tab { HorizontalAlignment = HorizontalAlignment.Stretch, TabName = name, Image = img, Uri = uri };
            itemTabs.Add(tab);
            Current = tab;
            try
            {
                if (propertyBag == null) tab.Navigate(uri);
                else tab.Navigate(uri, propertyBag);
                tab.Focus();
            }
            catch (Exception e)
            {
                RoLog.Instance.WriteToLog(e.ToString(), TracedAttribute.ERROR);
                tab.Close();
                Current = oldTab;
                throw;
            }
            Current.Show();
        }

    }
}
