using System;
using System.Collections.Generic;
using System.Linq;
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

        private IEnumerable<string> GetRefFieldNames() => _class.Fields.Where(x => x.IsReference).Select(x => x.References).Distinct();

        private string LookupServiceName(string name)
        {
            return $"{name}LookupService";
        }
        private string LookupServiceInst(string name)
        {
            var lookupServiceName = LookupServiceName(name);
            return Char.ToLower(lookupServiceName[0]) + lookupServiceName.Substring(1);
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
            if (f.IsReference)
            {
                //XXXlookupService
                var selectField = $"SetSelectField(true, o => o.{f.Name}.Display, {LookupServiceInst(f.References)}.GetAllAsync)";
                var result = $"c.Add(o => o.{ f.ReferenceName}, true).Titled(\"{f.Name}\").{selectField};";
                var result2 = $"c.Add(o => o.{f.Name}.Display).Titled(\"{f.Name}\").SetCrudHidden(true);";
                return result + "\r\n                " + result2;
                //c.Add(o => o.Country.Name).Titled("Country").SetCrudHidden(true);
                //c.Add(o => o.CountryId, true).SetSelectField(true, o => o.CountryId.ToString(), countryLookupService.GetAllAsync).Titled("Country");

                // return result;
            }
            else
            {
                var result = $"c.Add(o => o.{f.Name}){(f.Key ? ".SetPrimaryKey(true)" : "")};";
                return result;
            }
        }
    }
}
