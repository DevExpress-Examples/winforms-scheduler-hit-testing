#region #usings
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion #usings

namespace HitTest {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            schedulerControl1.Start = new System.DateTime(2015, 9, 2);
            schedulerControl1.DayView.GotoTimeInterval(new TimeInterval(schedulerControl1.Start.AddHours(17), new TimeSpan(2,0,0)));
            LoadData();
        }
        #region #hittest
        private void schedulerControl1_MouseMove(object sender, MouseEventArgs e) {
            Point pos = new Point(e.X, e.Y);
            SchedulerViewInfoBase viewInfo = schedulerControl1.ActiveView.ViewInfo;
            SchedulerHitInfo hitInfo = viewInfo.CalcHitInfo(pos, false);

            if (hitInfo.HitTest == SchedulerHitTest.AppointmentContent) {
                Appointment apt = ((AppointmentViewInfo)hitInfo.ViewInfo).Appointment;
                Text = apt.Subject;
            }
            else if (schedulerControl1.ActiveView.Type == SchedulerViewType.Day && hitInfo.HitTest == SchedulerHitTest.Cell) {
                int diff = pos.Y - ((SelectableIntervalViewInfo)hitInfo.ViewInfo).Bounds.Y;
                long ticksPerPixel = ((SelectableIntervalViewInfo)hitInfo.ViewInfo).Interval.Duration.Ticks / 
                    ((SelectableIntervalViewInfo)hitInfo.ViewInfo).Bounds.Height;
                long ticksCount = (long) ticksPerPixel * diff;
                DateTime actualTime = ((SelectableIntervalViewInfo)hitInfo.ViewInfo).Interval.Start.AddTicks(ticksCount);
                Text = actualTime.ToString();
            }
            else if (hitInfo.HitTest == SchedulerHitTest.None) {
                Text = Application.ProductName;
            }
            else Text = "";
        }
        
        private void schedulerControl1_DragOver(object sender, DragEventArgs e) {
            SchedulerControl sc = (SchedulerControl)sender;
            Point p = sc.PointToClient(new Point(e.X, e.Y));
            SchedulerHitInfo info = sc.DayView.ViewInfo.CalcHitInfo(p, true);
            if (info.HitTest == SchedulerHitTest.AllDayArea)
                e.Effect = DragDropEffects.None;
        }
        #endregion #hittest

        private void LoadData(){
            Appointment apt = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
            apt.Subject = "Presentation";
            apt.Start = new DateTime(2015, 09, 02, 18,0,0);
            apt.End = apt.Start.AddHours(3);
            apt.Location = "Globus BookStore";
            apt.Description = "The Sassanid Empire";
            apt.LabelId = 3;
            apt.HasReminder = true;
            apt.Reminder.TimeBeforeStart = new TimeSpan(0, 15, 0);
            schedulerStorage1.Appointments.Add(apt);
    
        }

        

    }
}