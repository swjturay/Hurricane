﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using Hurricane.Music;
using Hurricane.Music.MusicDatabase.EventArgs;
using Hurricane.Resources.Styles.DragDropListView.ServiceProviders.UI;
using Hurricane.ViewModels;

namespace Hurricane.AppMainWindow.WindowSkins
{
    /// <summary>
    /// Interaction logic for WindowAdvancedView.xaml
    /// </summary>
    public partial class WindowAdvancedView : IWindowSkin
    {
        public WindowAdvancedView()
        {
            InitializeComponent();
            new ListViewDragDropManager<Track>(this.listview) {ShowDragAdorner = true};

            this.Configuration = new WindowSkinConfiguration() { MaxHeight = double.PositiveInfinity, MaxWidth = double.PositiveInfinity, MinHeight = 500, MinWidth = 850, ShowSystemMenuOnRightClick = true, ShowTitleBar = false, ShowWindowControls = true, NeedMovingHelp = true, ShowFullscreenDialogs = true };
            SettingsViewModel.Instance.Load();
        }

        public event EventHandler DragMoveStart;

        public event EventHandler DragMoveStop;

        public event EventHandler CloseRequest
        {
            add { }
            remove { }
        }

        public event EventHandler ToggleWindowState;

        public void EnableWindow()
        {
           spektrum1.RefreshInterval = 20;
        }

        public void DisableWindow()
        {
            spektrum1.RefreshInterval = int.MaxValue;
        }

        public void RegisterSoundPlayer(CSCoreEngine engine)
        {
            spektrum1.RegisterSoundPlayer(engine);
        }

        protected WindowSkinConfiguration configuration;
        public WindowSkinConfiguration Configuration
        {
            get
            {
                return configuration;
            }
            set
            {
                configuration = value;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DragMoveStart != null) DragMoveStart(this, EventArgs.Empty);
            if (e.ClickCount == 2) if (ToggleWindowState != null) ToggleWindowState(this, EventArgs.Empty);
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DragMoveStop != null) DragMoveStop(this, EventArgs.Empty);
        }

        private MusicManager _manager;
        public void MusicManagerEnabled(object manager)
        {
            _manager = (MusicManager)manager;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listview = (ListView)sender;
            listview.ScrollIntoView(listview.SelectedItem);
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Move;
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Effects == DragDropEffects.None)
                return;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                MainViewModel.Instance.DragDropFiles((string[])e.Data.GetData(DataFormats.FileDrop));
            }
        }
    }
}