using System.ComponentModel.DataAnnotations;

namespace SampleWebProject.Models {

  public abstract class ViewModelBase
    {
        public abstract int? Id { get; set; }
        public abstract string Name { get; set; }
        public static T CreateAllItem<T>(string name = "") where T : ModelBase, new() {
            var result = new T {
                Id = Constants.AllItemId,
                Name = string.IsNullOrWhiteSpace(name) ? Constants.AllItemName : name
            };
            return result;
        }

        public override string ToString()
        {
            return Name ?? string.Empty;
    }
    }

    public class ModelBase : ViewModelBase
    {
        [Required]
        public override int? Id { get; set; }
        public override string Name { get; set; }
    }

    public class UserModel : ModelBase { }
 
}