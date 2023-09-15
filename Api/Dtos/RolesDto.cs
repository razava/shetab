using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class RoleDto
    {
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
        public bool IsInRole { get; set; }
    }
}
