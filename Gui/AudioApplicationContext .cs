using AudioChanger.AudioApi;
using Gui.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gui
{
    class AudioApplicationContext : ApplicationContext
    {
        private readonly IAudioApi m_audio;
        private readonly NotifyIcon m_trayIcon;
        private readonly ContextMenuStrip m_menu;

        public AudioApplicationContext()
        {
            m_audio = AudioFactory.Create();
            m_menu = new ContextMenuStrip();

            // Initialize Tray Icon
            m_trayIcon = new NotifyIcon()
            {
                Icon = Resources.Speaker,
                Visible = true,
                ContextMenuStrip = m_menu
            };
            m_trayIcon.MouseDown += TrayIcon_MouseDown;

        }

        private void TrayIcon_MouseDown(object sender, MouseEventArgs e)
        {
            RefreshMenuItems();
        }

        private void RefreshMenuItems()
        {
            m_menu.Items.Clear();

            // Audio menu items
            IEnumerable<Device> devices = GetDevices();
            ToolStripMenuItem[] menuItems = CreateAudioMenuItems(devices);
            m_menu.Items.AddRange(menuItems);

            // Exit
            ToolStripMenuItem exitMenuItem = CreateExitMenuItem();
            m_menu.Items.Add(exitMenuItem);
        }

        private IEnumerable<Device> GetDevices()
        {
            IEnumerable<Device> devices;
            try
            {
                devices = m_audio.GetDevices(AudioType.Output);
                return devices;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private ToolStripMenuItem CreateExitMenuItem()
        {
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem();
            exitMenuItem.Text = "Exit";
            exitMenuItem.Click += ExitApplication;
            return exitMenuItem;
        }

        private ToolStripMenuItem[] CreateAudioMenuItems(IEnumerable<Device> devices)
        {
            if(devices == null)
            {
                return new ToolStripMenuItem[0];
            }

            var menuItems = new List<ToolStripMenuItem>();
            foreach (Device device in devices)
            {
                var menuItem = new ToolStripMenuItem();
                menuItem.Tag = device;
                menuItem.Text = $"{device.Name} ({device.Description})";
                menuItem.Click += (s, e) => SetDefaultOutput((ToolStripMenuItem)s);
                if (device.IsDefaultOutput)
                {
                    menuItem.Image = Resources.Check16;
                }
                menuItems.Add(menuItem);
            }

            return menuItems.ToArray();
        }

        private void SetDefaultOutput(ToolStripMenuItem menuItem)
        {
            try
            {
                Device device = (Device)menuItem.Tag;
                m_audio.SetDefaultOutputDevice(device.ID);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        void ExitApplication(object sender, EventArgs e)
        {
            m_trayIcon.Visible = false;
            Application.Exit();
        }
    }

}
