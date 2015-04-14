using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Todo.Provider;
using Todo.Provider.Models;

/*
 * Todo List made by Duco Winterwerp in 2015
 * Icon by Boyan Kostov (https://www.iconfinder.com/icons/283038/check_checklist_clip_clipboard_done_exam_memo_organizer_task_tasks_todo_icon#size=128)
*/
namespace Todo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static ITodoProvider Provider { get; set; }

		public App()
		{
			Provider = new FileProvider();
		}

		public static string Resource(string key)
		{
			return Application.Current.FindResource(key).ToString();
		}
	}
}
