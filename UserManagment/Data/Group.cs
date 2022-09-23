using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UserManagment.Data
{
    public class Group
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [JsonIgnore]
        public List<User>? Users { get; set; } = new List<User>();
        public List<Group>? ChildGroups { get; set; } = new List<Group>();
        public Group? ParentGroup { get; set; }
        public int? ParentGroupId { get; set; }
    }
}
