using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Provider.Models
{
	public class TodoItem
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public Priority Priority { get; set; }
		public bool Done { get; set; }
		public string Category { get; set; }
	}
}
