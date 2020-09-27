using System;
using System.Collections.Generic;
using System.Text;

namespace CrudGen
{
    public class NavLink
    {
        public string Url { get; set; }
        public string Text { get; set; }
    }

    public partial class NavMenuTemplate
    {
        private IEnumerable<NavLink> _navLinks;

        public NavMenuTemplate(IEnumerable<NavLink> navLinks)
        {
            _navLinks = navLinks;
        }
    }
}
