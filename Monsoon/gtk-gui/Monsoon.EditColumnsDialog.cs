
// This file has been generated by the GUI designer. Do not modify.
namespace Monsoon
{
	public partial class EditColumnsDialog
	{
		private global::Gtk.Frame frame1;
		
		private global::Gtk.Alignment GtkAlignment;
		
		private global::Gtk.Table table;
		
		private global::Gtk.Label GtkLabel10;
		
		private global::Gtk.Button button21;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Monsoon.EditColumnsDialog
			this.Name = "Monsoon.EditColumnsDialog";
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Internal child Monsoon.EditColumnsDialog.VBox
			global::Gtk.VBox w1 = this.VBox;
			w1.Name = "dialog1_VBox";
			w1.BorderWidth = ((uint)(2));
			// Container child dialog1_VBox.Gtk.Box+BoxChild
			this.frame1 = new global::Gtk.Frame ();
			this.frame1.Name = "frame1";
			this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
			// Container child frame1.Gtk.Container+ContainerChild
			this.GtkAlignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
			this.GtkAlignment.Name = "GtkAlignment";
			this.GtkAlignment.LeftPadding = ((uint)(12));
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			this.table = new global::Gtk.Table (((uint)(3)), ((uint)(2)), false);
			this.table.Name = "table";
			this.table.RowSpacing = ((uint)(6));
			this.table.ColumnSpacing = ((uint)(6));
			this.GtkAlignment.Add (this.table);
			this.frame1.Add (this.GtkAlignment);
			this.GtkLabel10 = new global::Gtk.Label ();
			this.GtkLabel10.Name = "GtkLabel10";
			this.GtkLabel10.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Visible columns</b>");
			this.GtkLabel10.UseMarkup = true;
			this.frame1.LabelWidget = this.GtkLabel10;
			w1.Add (this.frame1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(w1 [this.frame1]));
			w4.Position = 0;
			// Internal child Monsoon.EditColumnsDialog.ActionArea
			global::Gtk.HButtonBox w5 = this.ActionArea;
			w5.Name = "dialog1_ActionArea";
			w5.Spacing = 6;
			w5.BorderWidth = ((uint)(5));
			w5.LayoutStyle = ((global::Gtk.ButtonBoxStyle)(4));
			// Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
			this.button21 = new global::Gtk.Button ();
			this.button21.CanDefault = true;
			this.button21.CanFocus = true;
			this.button21.Name = "button21";
			this.button21.UseUnderline = true;
			this.button21.Label = global::Mono.Unix.Catalog.GetString ("Close");
			global::Gtk.Image w6 = new global::Gtk.Image ();
			w6.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-close", global::Gtk.IconSize.Menu);
			this.button21.Image = w6;
			this.AddActionWidget (this.button21, 0);
			global::Gtk.ButtonBox.ButtonBoxChild w7 = ((global::Gtk.ButtonBox.ButtonBoxChild)(w5 [this.button21]));
			w7.Expand = false;
			w7.Fill = false;
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 207;
			this.DefaultHeight = 242;
			this.Show ();
		}
	}
}
