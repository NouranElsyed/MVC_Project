
using System.Collections.Generic;

namespace Demo.PL.ViewModels
{
    public class UserViewModel
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
