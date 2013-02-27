using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RoDuino.SMS.Helpers
{
    public class GroupList
    {
        private Hashtable allElements = new Hashtable();
        private string group1 = "";
        private string group2 = "";
        private string group3 = "";
        private string group4 = "";
        private List<ListItem> items = new List<ListItem>();
        private Hashtable levels = new Hashtable();
        private string picture = "";
        private Random random = new Random();

        public Hashtable AllElements
        {
            get { return allElements; }
        }

        public List<ListItem> Items
        {
            get { return items; }
        }


        public void Group(List<ListItem> list, string pict, string g1, string g2, string g3, string g4)
        {
            this.items = list;
            this.picture = pict;
            group1 = g1;
            group2 = g2;
            group3 = g3;
            group4 = g4;

            if (group1 != null) GroupTopLevel(list);
            if (group2 != null) GroupSecondLevel(0, group2);
            if (group3 != null) GroupSecondLevel(1, group3);
            if (group4 != null) GroupSecondLevel(2, group4);
        }

        private void GroupTopLevel(List<ListItem> list)
        {
            Hashtable groups = new Hashtable();
            foreach (ListItem l in list)
            {
                object val = GetValue(l, group1);
                if (groups[val] == null)
                {
                    GroupItem pg = new GroupItem(random.Next(), val.ToString());
                    pg.Picture = (string)GetValue(l, picture);
                    pg.Image = (byte[])GetValue(l, "Image");
                    groups[val] = pg;
                    this.Add(pg);
                }
                GroupItem parent = (GroupItem)groups[val];
                GroupItem child = new GroupItem();
                child.InnerObject = l;
                parent.Children.Add(child);


                //Console.WriteLine("{0}, {1}/{2} and pic:{3} ",l.Group,parent.Path,child.InnerObject.Group,parent.Picture);
            }
        }

        public void GroupSecondLevel(int level, string groupProperty)
        {
            ArrayList groups = GetForLevel(level + "");
            foreach (GroupItem group in groups)
            {
                Hashtable newGroups = new Hashtable();
                SortById comById = new SortById();
                group.Children.Sort(comById);
                int pozition = 0;
                foreach (GroupItem child in group.Children)
                {
                    object val = GetValue(child.InnerObject, groupProperty);
                    if (newGroups[val] == null)
                    {
                        GroupItem newGroup = new GroupItem(random.Next(), val.ToString());
                        newGroup.Parent = group;
                        newGroup.Position = pozition;
                        //newGroup.InnerObject = child.InnerObject;
                        newGroup.Picture = (string)GetValue(child.InnerObject, picture);
                        newGroup.Image = (byte[])GetValue(child.InnerObject, "Image");
                        newGroups[val] = newGroup;
                        //this.Add(newGroup);
                    }
                    GroupItem parent = (GroupItem)newGroups[val];
                    parent.Children.Add(child);
                    pozition++;
                }
                group.Children.Clear();
                ArrayList tmpArray = new ArrayList();
                SortByPozition sortByPozition = new SortByPozition();
                tmpArray.AddRange(newGroups.Values);
                tmpArray.Sort(sortByPozition);
                foreach (GroupItem entry in tmpArray)
                {
                    group.Children.Add(entry);
                    //                    Console.WriteLine("Group: {0}, entry:{1}", group.Path, entry.Path);
                    this.Add(entry);
                }
                //                Console.WriteLine("Group: {0}, total:{1}", group.Path,  group.Children.Count);
            }
        }


        public ArrayList GetForLevel(string level)
        {
            return (ArrayList)levels[level];
        }

        public GroupItem Find(long id)
        {
            return (GroupItem)allElements[id];
        }

        private object GetValue(object l, string group)
        {
            PropertyInfo p = l.GetType().GetProperty(group);
            return p.GetValue(l, null);
        }


        private void Add(GroupItem item)
        {
            if (levels[item.Level + ""] == null)
            {
                levels[item.Level + ""] = new ArrayList();
            }
            ArrayList levelList = (ArrayList)levels[item.Level + ""];
            levelList.Add(item);
            allElements[item.Id] = item;
        }

        /*
                private void Remove(GroupItem item)
                {
                    ArrayList levelList = (ArrayList)levels[item.Level];
                    levelList.Remove(item);
                    allElements.Remove(item);
                }
        */
    }

    public class SortById : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            return ((GroupItem)x).InnerObject.Id.CompareTo(((GroupItem)y).InnerObject.Id);
        }

        #endregion
    }

    public class SortByPozition : IComparer
    {
        #region IComparer Members

        public int Compare(object x, object y)
        {
            return ((GroupItem)x).Position.CompareTo(((GroupItem)y).Position);
        }

        #endregion
    }
}
