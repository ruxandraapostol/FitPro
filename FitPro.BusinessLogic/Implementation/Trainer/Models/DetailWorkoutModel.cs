using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitPro.BusinessLogic
{
    public class DetailWorkoutModel
    {
        public Guid UserId { get; set; }
        public Guid ProgramId { get; set; }
        public string Name { get; set; }
        public string AuthorTrainer { get; set; }
        public string LastModifiedTrainer { get; set; }
        public string VideoLinkUrl { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public int Time { get; set; }
        public List<string> Categories { get; set; }

        public bool FromSaved { get; set; }
        public bool FromShare { get; set; }
    }
}
