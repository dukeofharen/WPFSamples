using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Todo.Provider.Models;

namespace Todo
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			string[] priorities = Enum.GetNames(typeof(Priority));
			this.priorities.Items.Add(App.Resource("priority"));
			foreach (string priority in priorities)
			{
				this.priorities.Items.Add(priority);
			}
			this.priorities.SelectedIndex = 0;

			this.Refresh();
		}

		public void Refresh(bool refreshCategories = true)
		{
			this.todoItems.Children.Clear();
			CreateRow(null, true);
			foreach (TodoItem item in this.GetItems())
			{
				CreateRow(item);
			}

			if (refreshCategories)
			{
				this.categories.Items.Clear();
				this.categories.Items.Add(App.Resource("category"));
				foreach (string category in App.Provider.GetCategories())
				{
					this.categories.Items.Add(category);
				}
				this.categories.SelectedIndex = 0;
			}
		}

		private void CreateRow(TodoItem item, bool addColumnDescriptions = false)
		{
			RowDefinition newRow = new RowDefinition();
			newRow.Height = new GridLength(0, GridUnitType.Auto);
			this.todoItems.RowDefinitions.Insert(this.todoItems.RowDefinitions.Count - 1, newRow);

			int rowIndex = this.todoItems.RowDefinitions.Count - 2;

			if (addColumnDescriptions)
			{
				Func<string, TextBlock> NewTextBlock = (text) =>
				{
					return new TextBlock() { Text = text, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 5) };
				};

				TextBlock block = NewTextBlock(App.Resource("done"));
				Grid.SetRow(block, rowIndex);
				Grid.SetColumn(block, 0);
				this.todoItems.Children.Add(block);

				block = NewTextBlock(App.Resource("title"));
				Grid.SetRow(block, rowIndex);
				Grid.SetColumn(block, 1);
				this.todoItems.Children.Add(block);

				block = NewTextBlock(App.Resource("categoryHeader"));
				Grid.SetRow(block, rowIndex);
				Grid.SetColumn(block, 2);
				this.todoItems.Children.Add(block);

				block = NewTextBlock(App.Resource("priorityHeader"));
				Grid.SetRow(block, rowIndex);
				Grid.SetColumn(block, 3);
				this.todoItems.Children.Add(block);

				block = NewTextBlock(App.Resource("datetime"));
				Grid.SetRow(block, rowIndex);
				Grid.SetColumn(block, 4);
				this.todoItems.Children.Add(block);

				block = NewTextBlock(App.Resource("actions"));
				block.HorizontalAlignment = HorizontalAlignment.Right;
				Grid.SetRow(block, rowIndex);
				Grid.SetColumn(block, 5);
				this.todoItems.Children.Add(block);

				return;
			}

			CheckBox done = new CheckBox() { IsChecked = item.Done };
			done.Tag = item.Id;
			done.Click += done_Click;
			Grid.SetRow(done, rowIndex);
			Grid.SetColumn(done, 0);
			this.todoItems.Children.Add(done);

			TextBlock title = new TextBlock() { Text = item.Title };
			title.TextTrimming = TextTrimming.CharacterEllipsis;
			Grid.SetRow(title, rowIndex);
			Grid.SetColumn(title, 1);
			this.todoItems.Children.Add(title);

			TextBlock category = new TextBlock() { Text = item.Category };
			Grid.SetRow(category, rowIndex);
			Grid.SetColumn(category, 2);
			this.todoItems.Children.Add(category);

			TextBlock priority = new TextBlock() { Text = Enum.GetName(typeof(Priority), item.Priority) };
			Grid.SetRow(priority, rowIndex);
			Grid.SetColumn(priority, 3);
			this.todoItems.Children.Add(priority);

			TextBlock dateTime = new TextBlock() { Text = item.CreationDate.ToString("yyyy-MM-dd HH:mm") };
			Grid.SetRow(dateTime, rowIndex);
			Grid.SetColumn(dateTime, 4);
			this.todoItems.Children.Add(dateTime);

			WrapPanel wrap = new WrapPanel();
			wrap.HorizontalAlignment = HorizontalAlignment.Right;

			Button edit = new Button();
			edit.Margin = new Thickness(0, 0, 10, 0);
			edit.Padding = new Thickness(5);
			edit.Content = App.Resource("updateItem");
			edit.Tag = item.Id;
			wrap.Children.Add(edit);
			edit.Click += edit_Click;

			Button delete = new Button();
			delete.Content = App.Resource("deleteItem");
			wrap.Children.Add(delete);
			delete.Padding = new Thickness(5);
			delete.Tag = item.Id;
			delete.Click += delete_Click;

			Grid.SetRow(wrap, rowIndex);
			Grid.SetColumn(wrap, 5);

			this.todoItems.Children.Add(wrap);
		}

		void done_Click(object sender, RoutedEventArgs e)
		{
			CheckBox checkBox = (CheckBox)e.Source;
			Guid id = (Guid)checkBox.Tag;
			App.Provider.SetDone(id, checkBox.IsChecked.HasValue ? checkBox.IsChecked.Value : false);
			this.Refresh(false);
		}

		private void edit_Click(object sender, RoutedEventArgs e)
		{
			Guid id = (Guid)((Button)e.Source).Tag;
			ItemForm form = new ItemForm(id, this);
			form.Show();
		}

		private void delete_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show(App.Resource("msgAreYouSure"), string.Empty, MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				Guid id = (Guid)((Button)e.Source).Tag;
				App.Provider.DeleteTodoItem(id);
				this.Refresh();
			}
		}

		private void addItem_Click(object sender, RoutedEventArgs e)
		{
			ItemForm form = new ItemForm(null, this);
			form.Show();
		}

		private void categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				this.Refresh(false);
			}
		}

		private void priorities_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 1)
			{
				this.Refresh(false);
			}
		}

		private void openItems_Click(object sender, RoutedEventArgs e)
		{
			this.Refresh(false);
		}

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			Environment.Exit(0);
		}

		private TodoItem[] GetItems()
		{
			string category = string.Empty;
			if (this.categories.SelectedValue != null && this.categories.SelectedIndex != 0)
			{
				category = this.categories.SelectedValue.ToString();
			}

			Priority? priority = null;
			if (this.priorities.SelectedValue != null && this.priorities.SelectedIndex != 0)
			{
				Priority tempPriority;
				Enum.TryParse<Priority>(this.priorities.SelectedValue.ToString(), out tempPriority);
				priority = tempPriority;
			}

			bool onlyOpenItems = false;
			if (this.openItems.IsChecked.HasValue && this.openItems.IsChecked.Value)
			{
				onlyOpenItems = true;
			}

			return App.Provider.GetTodoItems(category, priority, onlyOpenItems);
		}
	}
}