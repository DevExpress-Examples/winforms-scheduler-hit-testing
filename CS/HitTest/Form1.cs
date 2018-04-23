#region #usings
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion #usings

namespace HitTest
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            schedulerControl1.DayView.ShowWorkTimeOnly = true;
            schedulerControl1.Start = DateTime.Now.Date;
            schedulerControl1.DayView.GotoTimeInterval(new TimeInterval(schedulerControl1.Start.AddHours(17), new TimeSpan(2, 0, 0)));
            LoadData();
        }

        private void LoadData()
        {
            Appointment apt = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
            apt.Subject = "Presentation";
            apt.Start = schedulerControl1.Start.Date.AddHours(14);
            apt.End = apt.Start.AddHours(3);
            apt.Location = "Globus BookStore";
            apt.Description = "The Sassanid Empire";
            apt.LabelKey = 3;
            schedulerStorage1.Appointments.Add(apt);
        }

        #region #hittest
        private void schedulerControl1_MouseMove(object sender, MouseEventArgs e)
        {
            SchedulerControl scheduler = sender as SchedulerControl;
            if (scheduler == null) return;

            Point pos = new Point(e.X, e.Y);
            SchedulerViewInfoBase viewInfo = schedulerControl1.ActiveView.ViewInfo;
            SchedulerHitInfo hitInfo = viewInfo.CalcHitInfo(pos, false);

            if (hitInfo.HitTest == SchedulerHitTest.AppointmentContent)
            {
                Appointment apt = ((AppointmentViewInfo)hitInfo.ViewInfo).Appointment;
                Text = apt.Subject;
            }
            else if (scheduler.ActiveView.Type == SchedulerViewType.Day && hitInfo.HitTest == SchedulerHitTest.Cell)
            {
                int diff = pos.Y - hitInfo.ViewInfo.Bounds.Y;
                long ticksPerPixel = hitInfo.ViewInfo.Interval.Duration.Ticks /
                    hitInfo.ViewInfo.Bounds.Height;
                long ticksCount = ticksPerPixel * diff;
                DateTime actualTime = hitInfo.ViewInfo.Interval.Start.AddTicks(ticksCount);
                Text = actualTime.ToString();
            }
            else if (hitInfo.HitTest == SchedulerHitTest.None)
            {
                Text = Application.ProductName;
            }
            else Text = string.Empty;
        }

        private void schedulerControl1_DragOver(object sender, DragEventArgs e)
        {
            SchedulerControl scheduler = sender as SchedulerControl;
            if (scheduler == null) return;

            Point p = scheduler.PointToClient(new Point(e.X, e.Y));
            SchedulerHitInfo info = scheduler.DayView.ViewInfo.CalcHitInfo(p, true);
            if (info.HitTest == SchedulerHitTest.AllDayArea)
                e.Effect = DragDropEffects.None;
        }
        #endregion #hittest
    }
}