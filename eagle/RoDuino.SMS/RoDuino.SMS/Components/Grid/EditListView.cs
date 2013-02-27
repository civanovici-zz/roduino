using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoDuino.SMS.Bll.Bll;

namespace RoDuino.SMS.Components.Grid
{
    class EditListView : FilterSortListView
    {
        /// <summary>
        /// Called for each item in the list. Return true if the item should be in
        /// the current result set, otherwise return false to exclude the item.
        /// </summary>    
        override protected bool FilterCallback(object item)
        {
            Client person = item as Client;
            if (person == null)
                return false;

            // Check for match.
            // Additional filters to search other columns
            if (this.Filter.Matches(person.Name) ||
                this.Filter.Matches(person.Network) ||
                this.Filter.Matches(person.Phone) )
                return true;

            return false;
        }
    }
}
