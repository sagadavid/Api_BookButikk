using System.ComponentModel.DataAnnotations;//to implement model validations

namespace Api_BookButikk.Model
{
    public class BookModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage ="please add a title")]//validation
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
