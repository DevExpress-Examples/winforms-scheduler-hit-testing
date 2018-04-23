using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraScheduler.Drawing;
using DevExpress.XtraScheduler;

namespace HitTest {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            LoadData();
        }

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
                long ticksPerPixel = ((SelectableIntervalViewInfo)hitInfo.ViewInfo).Interval.Duration.Ticks / ((SelectableIntervalViewInfo)hitInfo.ViewInfo).Bounds.Height;
                long ticksCount = ticksPerPixel * diff;
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

        
   
        
        private void LoadData(){
            Appointment apt = schedulerStorage1.CreateAppointment(AppointmentType.Normal);
            apt.Subject = "Book Presentation";
            apt.Start = new DateTime(2008, 09, 02, 18,0,0);
            apt.End = apt.Start.AddHours(3);
            apt.Location = "Biblio-Globus BookStore";
            apt.Description = "The Sassanid Empire by Sergey Dashkov";
            apt.LabelId = 4;
            apt.HasReminder = true;
            apt.Reminder.TimeBeforeStart = new TimeSpan(0, 15, 0);
            schedulerStorage1.Appointments.Add(apt);
    
        }

        

    }
}