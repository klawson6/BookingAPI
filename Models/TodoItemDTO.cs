using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAPI.Models {
    public class TodoItemDTO {
        public long Id { get; set; }
        private string name;
        public string Name {
            get {return name;}
            set {name = value;}
        }
        public bool IsComplete { get; set; }
    }
}
