using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoDuino.SMS.Helpers
{
    public class ListItem
    {
        private string group;
        private long id;
        private byte[] image;
        private string name;
        private string picture;
        private object objValue;

        public ListItem()
        {
        }

        public ListItem(long id, string name)
        {
            this.id = id;
            this.name = name;
        }


        public ListItem(long id, string name, string picture)
        {
            this.id = id;
            this.name = name;
            this.picture = picture;
        }


        public ListItem(long id, string name, string picture, string group)
        {
            this.id = id;
            this.name = name;
            this.picture = picture;
            this.group = group;
        }

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public object Value
        {
            get { return objValue; }
            set { objValue=value; }
        }

        public string Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }
    }
}
