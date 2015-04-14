using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Provider.Models;

namespace Todo.Provider
{
	public class FileProvider : ITodoProvider
	{
		private List<TodoItem> Items { get; set; }

		public FileProvider()
		{
			this.Items = new List<TodoItem>();
			if (!Directory.Exists("items"))
			{
				Directory.CreateDirectory("items");
			}
			if (this.Items.Count == 0)
			{
				string[] files = Directory.GetFiles("items", "*.json");
				foreach (string file in files)
				{
					string contents = File.ReadAllText(file);
					if (contents.Length > 0)
					{
						TodoItem item = Helpers.JsonDeserialize<TodoItem>(contents);
						if (item != null)
						{
							this.Items.Add(item);
						}
					}
				}
			}
		}

		public TodoItem[] GetTodoItems(string category = "", Priority? priority = null, bool onlyOpenItems = false)
		{
			var query = this.Items.AsQueryable();
			if (!string.IsNullOrEmpty(category))
			{
				query = query.Where(i => i.Category == category);
			}
			if (priority.HasValue)
			{
				query = query.Where(i => i.Priority == priority.Value);
			}
			if (onlyOpenItems)
			{
				query = query.Where(i => !i.Done);
			}
			return query.OrderBy(i => i.Done).ToArray();
		}

		public TodoItem GetTodoItem(Guid id)
		{
			return this.Items.Where(i => i.Id == id).FirstOrDefault();
		}


		public void AddTodoItem(TodoItem item)
		{
			this.Items.Add(item);
			File.WriteAllText(string.Format("items{0}{1}.json", Path.DirectorySeparatorChar, item.Id.ToString()), Helpers.JsonSerialize(item));
		}


		public void UpdateTodoItem(TodoItem item)
		{
			TodoItem existingItem = this.GetTodoItem(item.Id);
			if (existingItem != null)
			{
				this.Items.Remove(existingItem);
			}
			this.Items.Add(item);
			File.WriteAllText(string.Format("items{0}{1}.json", Path.DirectorySeparatorChar, item.Id.ToString()), Helpers.JsonSerialize(item));
		}


		public void DeleteTodoItem(Guid id)
		{
			TodoItem currentItem = this.GetTodoItem(id);
			if (currentItem != null)
			{
				this.Items.Remove(currentItem);
				File.Delete(string.Format("items{0}{1}.json", Path.DirectorySeparatorChar, currentItem.Id.ToString()));
			}
		}


		public string[] GetCategories()
		{
			string[] categories = (from i in this.Items
					   select i.Category)
					   .Distinct()
					   .ToArray();
			return categories;
		}


		public void SetDone(Guid id, bool done)
		{
			TodoItem item = this.GetTodoItem(id);
			if (item != null)
			{
				item.Done = done;
				this.UpdateTodoItem(item);
			}
		}
	}
}
