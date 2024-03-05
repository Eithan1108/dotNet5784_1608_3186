

using BO;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL;

class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertIdToEnable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)value == 0 ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertTaskToVisible : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Task)value == null ? "Hidden" : "Visible";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class ConvertTaskToEnable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Task)value == null ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class ConvertTaskToVisibleNot : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (BO.Task)value == null ? "Visible" : "Hidden";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class ConvertBoolToEnable : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == true ? false : true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class DateToWidthConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.Task task)
        {
            TimeSpan requiredEffortTime = (TimeSpan)task.RequiredEffortTime!; //get the task duration
            TimeSpan allProjectDuration;
            try
            {
                allProjectDuration = (TimeSpan)((DateTime)s_bl.Looz.GetEndDate()! - (DateTime)s_bl.Looz.GetStartDate()!)!; //get the project duration
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;

            }

            return (requiredEffortTime / allProjectDuration) * 1000; //return the task width
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class DateToMarginConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.Task task)
        {
            DateTime taskStartDate = (DateTime)task.ScheduledDate!; //get the task duration

            TimeSpan allProjectDuration;
            DateTime startDate;
            DateTime endDate;

            try
            {
                endDate = (DateTime)s_bl.Looz.GetEndDate()!;
                startDate = (DateTime)s_bl.Looz.GetStartDate()!;
                allProjectDuration = (TimeSpan)(endDate - startDate); //get the project duration
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 0;

            }

            return new Thickness((((TimeSpan)(taskStartDate - startDate) / allProjectDuration) * 1000), 0, 0, 0); //return the task margin
        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


class StatusToColorConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get; //get the Bl instance

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.Task task)
        {
            if (task.Dependencies != null)
            {
                foreach (var item in task.Dependencies)
                {
                    if (task.Status == BO.Status.InJeopardy)
                        return "#e16d70";
                }
            }
            if (task.Status == BO.Status.InJeopardy)
                return "#e16d70";
            if (task.Status == BO.Status.Done)
                return "#bcc771";
            if (task.Status == BO.Status.OnTrack)
                return "#60b0d1";
            if (task.Status == BO.Status.Scheduled)
                return "#ecbe62";
            if (task.Status == BO.Status.Unsheduled)
                return "#cfbc79";

        }
        return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

class GantbtnEnableConverter : IValueConverter
{
    //if we click on set schedule btn, enable this button
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value == true ? true : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}
