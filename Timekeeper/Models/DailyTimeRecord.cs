namespace Timekeeper.Models
{
    public class DailyTimeRecord
    {
        private TimekeepingTransaction _timeIn;
        private TimekeepingTransaction _timeOut;

        public TimekeepingTransaction TimeIn
        {
            get => _timeIn;
            set
            {
                if (_timeIn != null)
                {
                    throw new InvalidOperationException("TimeIn cannot be overriden");
                }
                _timeIn = value;
            }
        }

        public TimekeepingTransaction TimeOut
        {
            get => _timeOut;
            set
            {
                if (_timeOut != null)
                {
                    throw new InvalidOperationException("TimeOut cannot be overriden");
                }
                _timeOut = value;
            }
        }

        public TimeSpan? TimeWorked
        {
            get
            {
                if (_timeOut == null)
                {
                    return null;
                }
                return _timeOut.TransactionDateTime - _timeIn.TransactionDateTime;
            }
        }

        public DateTime Date
        {
            get
            {
                var localTime = _timeIn.TransactionDateTimeLocal;
                return new DateTime(localTime.Year, localTime.Month, localTime.Day, 8, 0, 0, DateTimeKind.Local);
            }
        }

        public AttendanceState Attendance
        {
            get
            {
                bool isOnDuty = _timeIn != null && _timeOut == null;
                bool isLate = Date < _timeIn.TransactionDateTimeLocal;
                bool isUndertime = TimeWorked.HasValue && TimeWorked < new TimeSpan(9, 0, 0);

                if (isLate && isUndertime)
                {
                    return AttendanceState.LateUndertime;
                }

                if (isUndertime)
                {
                    return AttendanceState.Undertime;
                }

                if (isLate)
                {
                    return AttendanceState.Late;
                }

                if (isOnDuty)
                {
                    return AttendanceState.OnDuty;
                }

                return AttendanceState.Present;
            }
        }

        public DailyTimeRecord(TimekeepingTransaction timeIn)
        {
            _timeIn = timeIn;
        }

        public DailyTimeRecord(TimekeepingTransaction timeIn, TimekeepingTransaction timeOut)
        {
            _timeIn = timeIn;
            _timeOut = timeOut;
        }
    }

    public enum AttendanceState
    {
        Present = 0,
        OnDuty = 1,
        Late = 2,
        Undertime = 3,
        LateUndertime = 4
    }
}
