using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Helpers
{
    public class GroupItem : ListItem
    {
        private ArrayList children = new ArrayList();
        private ListItem innerObject;
        private int level;
        private GroupItem parent;
        private int position;


        public GroupItem()
        {
        }

        public GroupItem(string name)
            : base(0, name)
        {
        }

        public GroupItem(long id, string name)
            : base(id, name)
        {
        }


        public ArrayList Children
        {
            get { return children; }
            set { children = value; }
        }


        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public GroupItem Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                this.level = parent.Level + 1;
            }
        }

        public ListItem InnerObject
        {
            get { return innerObject; }
            set { innerObject = value; }
        }
    }
}
