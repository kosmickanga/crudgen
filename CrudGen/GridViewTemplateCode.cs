using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public partial class GridViewTemplate
    {
        private string _page;
        private string _namespace;
        private string _itemClass;
        private string _gridServiceName;
        private Class _class;
        private string _crudServiceName;

        public GridViewTemplate(string page, string @namespace, string itemClass, string gridServiceName, string crudServiceName, Class @class)
        {
            _page = page;
            _namespace = @namespace;
            _itemClass = itemClass;
            _gridServiceName = gridServiceName;
            _class = @class;
            _crudServiceName = crudServiceName;
        }

        private string GenerateColumn(Field f)
        {
            //                c.Add(o => o.Id).SetPrimaryKey(true);
            //c.Add(o => o.Date, "OrderCustomDate").Format("{0:yyyy-MM-dd}");
            /*
            c.Add(o => o.Id).SetPrimaryKey(true);
            c.Add(o => o.Date, "OrderCustomDate").Format("{0:yyyy-MM-dd}");
            c.Add(o => o.CustomerId, true).SetSelectField(true, o => o.Customer.Name, customerService.GetAllCustomersAsync);
            c.Add(o => o.Customer.Name);
            c.Add(o => o.Customer.Email);
            */
            var result = $"c.Add(o => o.{f.Name}){(f.Key ? ".SetPrimaryKey(true)" : "")};";
            return result;
        }
    }
}
