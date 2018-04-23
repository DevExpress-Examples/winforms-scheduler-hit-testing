#Region "#usings"
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports System
Imports System.Drawing
Imports System.Windows.Forms
#End Region ' #usings

Namespace HitTest
    Partial Public Class Form1
        Inherits DevExpress.XtraEditors.XtraForm

        Public Sub New()
            InitializeComponent()
            schedulerControl1.DayView.ShowWorkTimeOnly = True
            schedulerControl1.Start = Date.Now.Date
            schedulerControl1.DayView.GotoTimeInterval(New TimeInterval(schedulerControl1.Start.AddHours(17), New TimeSpan(2, 0, 0)))
            LoadData()
        End Sub

        Private Sub LoadData()
            Dim apt As Appointment = schedulerStorage1.CreateAppointment(AppointmentType.Normal)
            apt.Subject = "Presentation"
            apt.Start = schedulerControl1.Start.Date.AddHours(14)
            apt.End = apt.Start.AddHours(3)
            apt.Location = "Globus BookStore"
            apt.Description = "The Sassanid Empire"
            apt.LabelKey = 3
            schedulerStorage1.Appointments.Add(apt)
        End Sub

        #Region "#hittest"
        Private Sub schedulerControl1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles schedulerControl1.MouseMove
            Dim scheduler As SchedulerControl = TryCast(sender, SchedulerControl)
            If scheduler Is Nothing Then
                Return
            End If

            Dim pos As New Point(e.X, e.Y)
            Dim viewInfo As SchedulerViewInfoBase = schedulerControl1.ActiveView.ViewInfo
            Dim hitInfo As SchedulerHitInfo = viewInfo.CalcHitInfo(pos, False)

            If hitInfo.HitTest = SchedulerHitTest.AppointmentContent Then
                Dim apt As Appointment = CType(hitInfo.ViewInfo, AppointmentViewInfo).Appointment
                Text = apt.Subject
            ElseIf scheduler.ActiveView.Type = SchedulerViewType.Day AndAlso hitInfo.HitTest = SchedulerHitTest.Cell Then
                Dim diff As Integer = pos.Y - hitInfo.ViewInfo.Bounds.Y
                Dim ticksPerPixel = hitInfo.ViewInfo.Interval.Duration.Ticks / hitInfo.ViewInfo.Bounds.Height
                Dim ticksCount = CType(ticksPerPixel * diff, Long)
                Dim actualTime As Date = hitInfo.ViewInfo.Interval.Start.AddTicks(ticksCount)
                Text = actualTime.ToString()
            ElseIf hitInfo.HitTest = SchedulerHitTest.None Then
                Text = Application.ProductName
            Else
                Text = String.Empty
            End If
        End Sub

        Private Sub schedulerControl1_DragOver(ByVal sender As Object, ByVal e As DragEventArgs) Handles schedulerControl1.DragOver
            Dim scheduler As SchedulerControl = TryCast(sender, SchedulerControl)
            If scheduler Is Nothing Then
                Return
            End If

            Dim p As Point = scheduler.PointToClient(New Point(e.X, e.Y))
            Dim info As SchedulerHitInfo = scheduler.DayView.ViewInfo.CalcHitInfo(p, True)
            If info.HitTest = SchedulerHitTest.AllDayArea Then
                e.Effect = DragDropEffects.None
            End If
        End Sub
        #End Region ' #hittest
    End Class
End Namespace