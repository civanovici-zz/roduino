using System;
using System.Collections;
using Castle.ActiveRecord;
using NHibernate.Linq;
using System.Linq;

namespace RoDuino.SMS.Bll.Bll.Base
{
    public class BaseItem<T> : ActiveRecordValidationBase where T : new()
    {
        #region properties



        [PrimaryKey(PrimaryKeyType.Identity)]
        public int Id { get; set; }

        [Property]
        public string Name { get; set; }


        #endregion

        public static Query<T> Queryable
        {
            get
            {
                var options = new QueryOptions();
                IList myObsj=new  ArrayList();
                var xx = from x in myObsj select x;
                return new Query<T>(new QueryProvider<T>(options), options);
            }
        }

        public static T[] FindAll()
        {
            return FindAll(false);
        }

        public static T[] FindAll(bool showDeleted)
        {
            return FindAll("Id", showDeleted);
        }

        public static T[] FindAll(string orderProperty)
        {
            return FindAll(orderProperty, false);
        }

        public static T[] FindAll(string orderProperty, bool showDeleted)
        {
            return FindAll(orderProperty, false, showDeleted);
        }

        /// <summary>
        /// find ALL order by property
        /// </summary>
        /// <param name="orderProperty"></param>
        /// <returns></returns>
        public static T[] FindAll(string orderProperty, bool isOrderDesc, bool showDeleted)
        {
            T[] result = null;
            var criteria = new ICriterion[] { };

            if (orderProperty.Equals(""))
            {
                result = ActiveRecordBase<T>.FindAll(new Order[1] { Order.Desc("Id") }, criteria);
            }
            else
            {
                result = isOrderDesc
                             ? ActiveRecordBase<T>.FindAll(new Order[1] { Order.Desc(orderProperty) }, criteria)
                             : ActiveRecordBase<T>.FindAll(new Order[1] { Order.Asc(orderProperty) }, criteria);
            }
            return result;
        }

        public static IList<T> Search(IList<ICriterion> queryParam)
        {
            return Search(queryParam, "Name", false, false);
        }

        public static IList<T> Search(IList<ICriterion> queryParam, string orderBy)
        {
            return Search(queryParam, orderBy, false, false);
        }

        public static IList<T> Search(IList<ICriterion> queryParam, string orderBy, bool isOrderDesc)
        {
            return Search(queryParam, orderBy, isOrderDesc, false);
        }

        public static IList<T> Search(IList<ICriterion> queryParam, string orderBy, bool isOrderDesc, bool showDeleted)
        {

            IList<T> records = ActiveRecordBase<T>.FindAll(new Order[1] { isOrderDesc ? Order.Desc(orderBy) : Order.Asc(orderBy) }, queryParam.ToArray());
            return records;
        }

        public static IList<T> FindByProperty(string propertyName, string propertyValue)
        {
            return ActiveRecordBase<T>.FindAll(new Order[1] { Order.Desc("Id") }, new ICriterion[] { Expression.Eq(propertyName, propertyValue) });
        }

        public static T FindOneByProperty(string propertyName, string propertyValue)
        {
            IList<T> list = FindByProperty(propertyName, propertyValue);
            return list.Count > 0 ? list[0] : default(T);
        }

        public static T FindFirst()
        {
            T[] x = (T[])FindAll();
            return x.Length == 0 ? new T() : x[0];
        }
        public static T FindFirst(bool showDeleted)
        {
            T[] x = (T[])FindAll(showDeleted);
            return x.Length == 0 ? new T() : x[0];
        }

        public static T Find(int id)
        {
            return (T)FindByPrimaryKey(typeof(T), id, false);
        }


        //public static IList FindAllLookup()
        //{
        //    return FindAllLookup(false);
        //}

        //public static IList FindAllLookup(bool showDeleted)
        //{

        //    object[] para = new object[] { };

        //    string hql = "select new Lookup(l.Id,l.Name) from " + typeof(T).Name + " l";
        //    para = new object[] { };

        //    hql += " order by l.Name ";

        //    IList lookups =
        //        (IList)ActiveRecordMediator.ExecuteQuery(new SimpleQuery(typeof(T), typeof(Lookup), hql, para));
        //    //            return ActiveRecordBase.FindAllByProperty (typeof(Lookup), property, value);
        //    return lookups;
        //}
    }
}
