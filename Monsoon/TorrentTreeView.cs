//
// TorrentTreeView.cs
//
// Author:
//   Jared Hendry (buchan@gmail.com)
//
// Copyright (C) 2007 Jared Hendry
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Gtk;
using System;
using System.Text;
using MonoTorrent.Common;
using MonoTorrent.Client;

namespace Monsoon
{
	public class TorrentTreeView : TreeView
	{
		public event EventHandler DeleteTorrent;
		public event EventHandler RemoveTorrent;
		
		public TreeViewColumn nameColumn;
		public TreeViewColumn statusColumn;
		public TreeViewColumn doneColumn;
		public TreeViewColumn seedsColumn;
		public TreeViewColumn peersColumn;
		public TreeViewColumn downSpeedColumn;
		public TreeViewColumn upSpeedColumn;
		public TreeViewColumn ratioColumn;
		public TreeViewColumn sizeColumn;
		public TreeViewColumn etaColumn;
		
		private TorrentController torrentController;
		private TorrentContextMenu menu;
		
		private TargetEntry[] targetEntries;
		private TargetEntry[] sourceEntries;

		public TorrentTreeView(TorrentController torrentController) : base()
		{
			this.torrentController = torrentController;
			
			targetEntries = new TargetEntry[]{
				new TargetEntry("text/uri-list", 0, 0) 
			};
			
			sourceEntries = new TargetEntry[]{
				new TargetEntry("application/x-monotorrent-torrentmanager-objects", 0, 0)
			};
			
			buildColumns();
				
			Reorderable = true;
			HeadersVisible = true;
			HeadersClickable = true;
			Selection.Mode = SelectionMode.Multiple;
			
			EnableModelDragDest(targetEntries, Gdk.DragAction.Copy);
			DragDataReceived += OnTorrentDragDataReceived;
			//this.DragDrop += OnTest;
			
			
			this.EnableModelDragSource(Gdk.ModifierType.Button1Mask, sourceEntries, Gdk.DragAction.Copy);
			DragDataGet += OnTorrentDragDataGet;

			
			menu = new TorrentContextMenu(torrentController);
			menu.DeleteTorrent += MainWindow.WrappedHandler ((EventHandler) delegate {
				if (DeleteTorrent != null)
					DeleteTorrent(this, EventArgs.Empty);
			});
			menu.RemoveTorrent += MainWindow.WrappedHandler ((EventHandler) delegate {
				if (RemoveTorrent != null)
					RemoveTorrent (this, EventArgs.Empty);
			});
		}


		protected override bool	OnButtonPressEvent (Gdk.EventButton e)
		{
			// Call this first so context menu has a selected torrent
			base.OnButtonPressEvent(e);
			
			if(e.Button == 3 && Selection.CountSelectedRows() == 1){
				menu.ShowAll ();
				menu.Popup ();
				return true;
			}
			
			return false;
		}
		
		private void OnTorrentDragDataGet (object o, DragDataGetArgs args)
		{
			// TODO: Support dragging multiple torrents to a label
			TorrentManager manager;
			
			manager = torrentController.GetSelectedTorrent();
			if(manager == null)
				return;
			
			args.SelectionData.Set(Gdk.Atom.Intern("application/x-monotorrent-torrentmanager-objects", false), 8, manager.Torrent.InfoHash);
		}
		
		private void OnTorrentDragDataReceived (object o, DragDataReceivedArgs args) 
		{
			string [] uriList = (Encoding.UTF8.GetString(args.SelectionData.Data).TrimEnd()).Split('\n');
			
			foreach(string s in uriList){
				try
				{
					Uri uri = new Uri(s.TrimEnd());
					if (uri.IsFile && uri.LocalPath.EndsWith(".torrent", StringComparison.OrdinalIgnoreCase))
						torrentController.MainWindow.LoadTorrent(uri.LocalPath);
				}
				catch
				{
				}
			}
		}
			
		private void buildColumns()
		{
			nameColumn = new TreeViewColumn();
			statusColumn = new TreeViewColumn();
			doneColumn = new TreeViewColumn();
			seedsColumn = new TreeViewColumn();
			peersColumn = new TreeViewColumn();
			downSpeedColumn = new TreeViewColumn();
			upSpeedColumn = new TreeViewColumn();
			ratioColumn = new TreeViewColumn();
			sizeColumn = new TreeViewColumn();
			etaColumn = new TreeViewColumn();
			
			nameColumn.Title = _("Name");
			statusColumn.Title = _("Status");
			doneColumn.Title = _("Done");
			seedsColumn.Title = _("Seeds");
			peersColumn.Title = _("Peers");
			downSpeedColumn.Title = _("DL Speed");
			upSpeedColumn.Title = _("UP Speed");
			ratioColumn.Title = _("Ratio");
			sizeColumn.Title = _("Size");
			etaColumn.Title = _("ETA");
			
			nameColumn.Resizable = true;
			statusColumn.Resizable = true;
			doneColumn.Resizable = true;
			seedsColumn.Resizable = true;
			peersColumn.Resizable = true;
			downSpeedColumn.Resizable = true;
			upSpeedColumn.Resizable = true;
			ratioColumn.Resizable = true;
			sizeColumn.Resizable = true;
			etaColumn.Resizable = true;
			
			nameColumn.Reorderable = true;
			statusColumn.Reorderable = true;
			doneColumn.Reorderable = true;
			seedsColumn.Reorderable = true;
			peersColumn.Reorderable = true;
			downSpeedColumn.Reorderable = true;
			upSpeedColumn.Reorderable = true;
			ratioColumn.Reorderable = true;
			sizeColumn.Reorderable = true;
			etaColumn.Reorderable = true;
			
			Gtk.CellRendererText torrentNameCell = new Gtk.CellRendererText ();
			Gtk.CellRendererText torrentStatusCell = new Gtk.CellRendererText();
			Gtk.CellRendererProgress torrentDoneCell = new Gtk.CellRendererProgress();
			Gtk.CellRendererText torrentSeedsCell = new Gtk.CellRendererText();
			Gtk.CellRendererText torrentPeersCell = new Gtk.CellRendererText();
			Gtk.CellRendererText torrentDownSpeedCell = new Gtk.CellRendererText();
			Gtk.CellRendererText torrentUpSpeedCell = new Gtk.CellRendererText();
			Gtk.CellRendererText torrentRatioCell = new Gtk.CellRendererText();
			Gtk.CellRendererText torrentSizeCell = new Gtk.CellRendererText();
			Gtk.CellRendererText torrentEtaCell = new Gtk.CellRendererText();
					
			nameColumn.PackStart(torrentNameCell, true);
			statusColumn.PackStart(torrentStatusCell, true);
			doneColumn.PackStart(torrentDoneCell, true);
			seedsColumn.PackStart(torrentSeedsCell, true);
			peersColumn.PackStart(torrentPeersCell, true);
			downSpeedColumn.PackStart(torrentDownSpeedCell, true);
			upSpeedColumn.PackStart(torrentUpSpeedCell, true);
			ratioColumn.PackStart(torrentRatioCell, true);
			sizeColumn.PackStart(torrentSizeCell, true);
			etaColumn.PackStart(torrentEtaCell, true);
							
			nameColumn.SetCellDataFunc (torrentNameCell, new Gtk.TreeCellDataFunc (RenderTorrentName));
			statusColumn.SetCellDataFunc (torrentStatusCell, new Gtk.TreeCellDataFunc (RenderTorrentStatus));
			doneColumn.SetCellDataFunc (torrentDoneCell, new Gtk.TreeCellDataFunc (RenderTorrentDone));
			seedsColumn.SetCellDataFunc (torrentSeedsCell, new Gtk.TreeCellDataFunc (RenderTorrentSeeds));
			peersColumn.SetCellDataFunc (torrentPeersCell, new Gtk.TreeCellDataFunc (RenderTorrentPeers));
			downSpeedColumn.SetCellDataFunc (torrentDownSpeedCell, new Gtk.TreeCellDataFunc (RenderTorrentDownSpeed));
			upSpeedColumn.SetCellDataFunc (torrentUpSpeedCell, new Gtk.TreeCellDataFunc (RenderTorrentUpSpeed));
			ratioColumn.SetCellDataFunc (torrentRatioCell, new Gtk.TreeCellDataFunc (RenderTorrentRatio));
			sizeColumn.SetCellDataFunc (torrentSizeCell, new Gtk.TreeCellDataFunc (RenderTorrentSize));
			etaColumn.SetCellDataFunc(torrentEtaCell, new Gtk.TreeCellDataFunc(RenderTorrentEta));
			
			nameColumn.Sizing = TreeViewColumnSizing.Fixed;
			statusColumn.Sizing = TreeViewColumnSizing.Fixed;
			doneColumn.Sizing = TreeViewColumnSizing.Fixed;
			seedsColumn.Sizing = TreeViewColumnSizing.Fixed;
			peersColumn.Sizing = TreeViewColumnSizing.Fixed;
			downSpeedColumn.Sizing = TreeViewColumnSizing.Fixed;
			upSpeedColumn.Sizing = TreeViewColumnSizing.Fixed;
			ratioColumn.Sizing = TreeViewColumnSizing.Fixed;
			sizeColumn.Sizing = TreeViewColumnSizing.Fixed;
			etaColumn.Sizing = TreeViewColumnSizing.Fixed;
			
			AppendColumn(nameColumn);
			AppendColumn(statusColumn);
			AppendColumn(doneColumn);
			AppendColumn(etaColumn);
			AppendColumn(seedsColumn);
			AppendColumn(peersColumn);
			AppendColumn(downSpeedColumn);
			AppendColumn(upSpeedColumn);
			AppendColumn(ratioColumn);
			AppendColumn(sizeColumn);
		}
		
		
		private void RenderTorrentName (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if (torrent == null)
				(cell as Gtk.CellRendererText).Text = string.Empty;
			else
				(cell as Gtk.CellRendererText).Text = torrent.Torrent.Name;
		}
		
		private void RenderTorrentStatus (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
			
			if (torrent.State == TorrentState.Downloading){
				(cell as Gtk.CellRendererText).Foreground = "darkgreen";
			}else if (torrent.State == TorrentState.Paused){
				(cell as Gtk.CellRendererText).Foreground = "orange";
			}else if (torrent.State == TorrentState.Hashing){
				(cell as Gtk.CellRendererText).Foreground = "purple";
			}else if (torrent.State == TorrentState.Seeding){
				(cell as Gtk.CellRendererText).Foreground = "darkgreen";
			}else if (torrent.State == TorrentState.Stopped && torrent.Complete){
				(cell as Gtk.CellRendererText).Foreground = "blue";
			} else {
				(cell as Gtk.CellRendererText).Foreground = "red";
			}
	
			(cell as Gtk.CellRendererText).Text = GetStatusString (torrent);
		}
		
		private string GetStatusString (TorrentManager manager)
		{
			if (manager == null)
				return "";
			
			switch (manager.State)
			{
			case TorrentState.Stopped:
				return manager.Complete ? "Finished" : "Stopped";
			case TorrentState.Seeding:
				return "Seeding";
			case TorrentState.Downloading:
				return "Downloading";
			case TorrentState.Hashing:
				return "Hashing";
			case TorrentState.Paused:
				return "Paused";
			default:
				return manager.State.ToString ();
			}
		}
		
		private void RenderTorrentDone (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
			if(torrent.State == TorrentState.Hashing) {
					(cell as Gtk.CellRendererProgress).Value = (int)torrentController.GetTorrentHashProgress(torrent);
			} else {
				(cell as Gtk.CellRendererProgress).Value = (int)torrent.Progress;
			}
		}
		
		private void RenderTorrentSeeds (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0); 
			
			if(torrent == null)
				return;
			
			(cell as Gtk.CellRendererText).Text = torrent.Peers.Seeds.ToString();
		}
		
		private void RenderTorrentPeers (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
					
			(cell as Gtk.CellRendererText).Text = torrent.Peers.Leechs.ToString()  + " (" + torrent.Peers.Available + ")";
		}
	
		private void RenderTorrentDownSpeed (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
			(cell as Gtk.CellRendererText).Text = ByteConverter.ConvertSpeed (torrent.Monitor.DownloadSpeed);
		}
		
		private void RenderTorrentUpSpeed (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
			
			(cell as Gtk.CellRendererText).Text = ByteConverter.ConvertSpeed (torrent.Monitor.UploadSpeed);
		}
		
		private void RenderTorrentRatio (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
			
			double totalDown;
			double totalUp;
			
			totalDown = torrentController.GetPreviousDownload(torrent) + torrent.Monitor.DataBytesDownloaded;
			totalUp = torrentController.GetPreviousUpload(torrent) + torrent.Monitor.DataBytesUploaded;
			
			if (totalDown > 0 || ((totalDown / 1024f) > torrent.Torrent.Size))
				(cell as Gtk.CellRendererText).Text = (totalUp / (double)totalDown).ToString("0.00");
			else
				(cell as Gtk.CellRendererText).Text = (totalUp / (torrent.Torrent.Size / 1024f)).ToString("0.00");
		
		}
		
		private void RenderTorrentSize (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager) model.GetValue (iter, 0);
			
			if(torrent == null)
				return;
			
			(cell as Gtk.CellRendererText).Text = ByteConverter.ConvertSize (torrent.Torrent.Size);
		}
		
		private void RenderTorrentEta (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			TorrentManager torrent = (TorrentManager)model.GetValue(iter, 0);			
			if (torrent == null)
				return;
			(cell as Gtk.CellRendererText).Text = GetEtaString(torrent);
		}
		
		private string GetEtaString (TorrentManager manager)
		{
			TimeSpan eta;
			if (manager.State == TorrentState.Downloading && (manager.Torrent.Size - (manager.Monitor.DataBytesDownloaded + torrentController.GetPreviousDownload(manager))) > 0)
			{
				int dSpeed = manager.Monitor.DownloadSpeed;
				eta = TimeSpan.FromSeconds(dSpeed > 0 ? ((manager.Torrent.Size - (manager.Monitor.DataBytesDownloaded + torrentController.GetPreviousDownload(manager))) / dSpeed) : -1);
			}
			else if (manager.State == TorrentState.Seeding && (manager.Torrent.Size - (manager.Monitor.DataBytesUploaded + torrentController.GetPreviousUpload(manager))) > 0)
			{
				int uSpeed = manager.Monitor.UploadSpeed;
				eta = TimeSpan.FromSeconds(uSpeed > 0 ? ((manager.Torrent.Size - (manager.Monitor.DataBytesUploaded + torrentController.GetPreviousUpload(manager))) / uSpeed) : -1);
			}
			else
				return string.Empty;
			
			if (eta.Seconds <= 0)
				return "∞";
			if (eta.Days > 0)
				return string.Format("{0}d {1}h", eta.Days, eta.Hours);
			if (eta.Hours > 0)
				return string.Format("{0}h {1}m", eta.Hours, eta.Minutes);					
			if (eta.Minutes > 0)
				return string.Format("{0}m {1}s", eta.Minutes, eta.Seconds);
			
			return string.Format("{0}s", eta.Seconds);
		}
		
		private static string _(string s)
		{
			return Mono.Unix.Catalog.GetString(s);
		}
	}
}
