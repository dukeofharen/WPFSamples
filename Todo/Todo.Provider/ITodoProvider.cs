using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Provider.Models;

namespace Todo.Provider
{
	public interface ITodoProvider
	{
		TodoItem[] GetTodoItems(string category = "", Priority? priority = null, bool onlyOpenItems = false);
		TodoItem GetTodoItem(Guid id);
		string[] GetCategories();
		void AddTodoItem(TodoItem item);
		void UpdateTodoItem(TodoItem item);
		void DeleteTodoItem(Guid id);
		void SetDone(Guid id, bool done);
	}
}
