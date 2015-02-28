using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Specialized;
using System.Threading;

namespace HDDLED
{
    public partial class InvisibleForm : Form
    {
        #region Global Variables
        NotifyIcon hddNotifyIcon;
        Icon busyIcon;
        Icon idleIcon;
        Thread hddInfoWorkerThread;
        #endregion

        #region Main Form (entry point)
        public InvisibleForm()
        {
            InitializeComponent();

            // Load icons from files into objects
            busyIcon = new Icon("HDD_Busy.ico");
            idleIcon = new Icon("HDD_Idle.ico");
            
            // Create notify icons and assign idle icon and show it
            hddNotifyIcon = new NotifyIcon();
            hddNotifyIcon.Icon = idleIcon;
            hddNotifyIcon.Visible = true;

            // Create all context menu items and add them to notification tray icon
            MenuItem progNameMenuItem = new MenuItem("Hard Drive LED v1.0 BETA by: Barnacules");
            MenuItem breakMenuItem = new MenuItem("-");
            MenuItem quitMenuItem = new MenuItem("Quit");
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(progNameMenuItem);
            contextMenu.MenuItems.Add(breakMenuItem);
            contextMenu.MenuItems.Add(quitMenuItem);
            hddNotifyIcon.ContextMenu = contextMenu;

            // Wire up quit button to close application
            quitMenuItem.Click += quitMenuItem_Click;

            // 
            //  Hide the form because we don't need it, this is a notification tray application
            //
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            // Start worker thread that pulls HDD activity
            hddInfoWorkerThread = new Thread(new ThreadStart(HddActivityThread));
            hddInfoWorkerThread.Start();
        }
        #endregion

        #region Context Menu Event Handlers
        /// <summary>
        /// Close the application on click of 'quit' button on context menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void quitMenuItem_Click(object sender, EventArgs e)
        {
            hddInfoWorkerThread.Abort();
            hddNotifyIcon.Dispose();
            this.Close();
        }
        #endregion

        #region Hard drive activity threads
        /// <summary>
        /// This is the thread that pulls the HDD for activity and updates the notification icon
        /// </summary>
        public void HddActivityThread()
        {
            ManagementClass driveDataClass = new ManagementClass("Win32_PerfFormattedData_PerfDisk_PhysicalDisk");

            try
            {
                // Main loop where all the magic happens
                while (true)
                {
                    // Connect to the drive performance instance 
                    ManagementObjectCollection driveDataClassCollection = driveDataClass.GetInstances();
                    foreach( ManagementObject obj in driveDataClassCollection)
                    {
                        // Only process the _Total instance and ignore all the indevidual instances
                        if( obj["Name"].ToString() == "_Total")
                        {
                            if( Convert.ToUInt64(obj["DiskBytesPersec"]) > 0 )
                            {
                                // Show busy icon
                                hddNotifyIcon.Icon = busyIcon;
                            }
                            else
                            {
                                // Show idle icon
                                hddNotifyIcon.Icon = idleIcon;
                            }
                        }
                    }

                    // Sleep for 10th of millisecond 
                    Thread.Sleep(100);
                }
            } catch( ThreadAbortException tbe )
            {
                driveDataClass.Dispose();
                // Thead was aborted
            }
        }
        #endregion
    }
}
