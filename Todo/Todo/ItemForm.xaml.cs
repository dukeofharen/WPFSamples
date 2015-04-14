using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Todo.Provider.Models;

namespace Todo
{
	/// <summary>
	/// Interaction logic for ItemForm.xaml
	/// </summary>
	public partial class ItemForm : Window
	{
		private MainWindow MainWindow { get; set; }
		private string[] Priorities
		{
			get
			{
				return Enum.GetNames(typeof(Priority));
			}
		}
		private Guid? Id { get; set; }

		public ItemForm(Guid? id = null, MainWindow mainWindow = null)
		{
			this.MainWindow = mainWindow;
			InitializeComponent();
			if (id.HasValue)
			{
				this.Id = id;
			}
			this.InitializeForm();
		}

		private void InitializeForm()
		{
			foreach (string priorityString in this.Priorities)
			{
				this.priority.Items.Add(priorityString);
			}
			this.priority.SelectedIndex = 0;

			if (this.Id.HasValue)
			{
				this.Title = App.Resource("updateItem");
				TodoItem todoItem = App.Provider.GetTodoItem(this.Id.Value);
				if (todoItem != null)
				{
					this.title.Text = todoItem.Title;
					this.description.Text = todoItem.Description;
					this.category.Text = todoItem.Category;
					this.done.IsChecked = todoItem.Done;
					this.priority.SelectedValue = Enum.GetName(typeof(Priority), todoItem.Priority);
				}
			}
			else
			{
				this.Title = App.Resource("addItem");
			}
		}

		private void save_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(this.title.Text))
			{
				this.Error(App.Resource("errorFillInTitle"));
				return;
			}
			if (string.IsNullOrEmpty(this.category.Text))
			{
				this.Error(App.Resource("errorFillInCategory"));
				return;
			}

			Priority priority;
			Enum.TryParse<Priority>(this.priority.SelectedValue.ToString(), out priority);

			TodoItem item = new TodoItem()
			{
				Id = this.Id.HasValue ? this.Id.Value : Guid.NewGuid(),
				Title = this.title.Text,
				Description = this.description.Text,
				CreationDate = DateTime.Now,
				Done = this.done.IsChecked.HasValue ? this.done.IsChecked.Value : false,
				Category = this.category.Text,
				Priority = priority
			};

			if (this.Id.HasValue)
			{
				App.Provider.UpdateTodoItem(item);
			}
			else
			{
				App.Provider.AddTodoItem(item);
			}
			this.RefreshAndClose();
		}

		private void Error(string message)
		{
			MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private void RefreshAndClose()
		{
			if (this.MainWindow != null)
			{
				this.MainWindow.Refresh();
			}
			this.Close();
		}
	}
}
